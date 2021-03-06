using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Xml;
using DataDynamics.PageFX.Common.Collections;
using DataDynamics.PageFX.Common.CompilerServices;
using DataDynamics.PageFX.Common.IO;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Common.Utilities;
using DataDynamics.PageFX.Flash.Abc;
using DataDynamics.PageFX.Flash.Core;
using DataDynamics.PageFX.Flash.Core.ResourceBundles;
using DataDynamics.PageFX.Flash.Swf;
using Ionic.Zip;

namespace DataDynamics.PageFX.Flash.Swc
{
    public sealed class SwcFile
    {
        public const string LIBRARY_SWF = "library.swf";
        public const string CATALOG_XML = "catalog.xml";

        #region ctors
        private readonly ZipFile _zip;

        public SwcFile(Stream input)
        {
            _zip = ZipFile.Read(input);
            _libs = new HashList<string, SwfMovie>(lib => lib.Name);
        }

        public SwcFile(byte[] data)
            : this(new MemoryStream(data))
        {
        }

        public SwcFile(string path)
            : this(File.ReadAllBytes(path))
        {
            Name = Path.GetFileName(path);
        }
        #endregion

        #region Properties
        public string Name { get; set; }

        public SwfTagDecodeOptions TagDecodeOptions
        {
            get { return _tagDecodeOptions; }
            set { _tagDecodeOptions = value; }
        }
        private SwfTagDecodeOptions _tagDecodeOptions = SwfTagDecodeOptions.DonotDecodeImages;

        public XmlDocument Catalog
        {
            get
            {
                if (_catalog != null) return _catalog;
                var s = _zip.ExtractEntry(CATALOG_XML);
                if (s == null)
                {
                    return null;
                }
                _catalog = new XmlDocument();
                _catalog.Load(s);
                return _catalog;
            }
        }
        private XmlDocument _catalog;

        public IAssembly Assembly { get; set; }

        public RslItem RSL { get; set; }
        #endregion

        #region Libs
        public SwfMovie GetLibrary(string name)
        {
            InitLibElems();

            if (string.IsNullOrEmpty(name))
                name = LIBRARY_SWF;

            var libElem = _libElems[name];
            if (libElem == null)
            {
                name = name + ".swf";
                libElem = _libElems[name];
            }

            if (libElem == null)
                throw new ArgumentException(string.Format("Unable to find library {0}", name));

            var lib = _libs[name];
            if (lib != null) return lib;
            var stream = _zip.ExtractEntry(name);
            if (stream == null) return null;

            lib = new SwfMovie();
            lib.Load(stream, _tagDecodeOptions);
            lib.Index = _libs.Count;
            lib.Name = name;
            lib.Swc = this;
            lib.SwcElement = libElem;
            _libs.Add(lib);

            //CacheScripts(lib);

            return lib;
        }

        public SwfMovie GetLibrary(int index)
        {
            InitLibs();
            return _libs[index];
        }

        void CacheScripts(SwfMovie lib)
        {
            if (lib.SwcScriptsCached) return;
            lib.SwcScriptsCached = true;

            var libElem = lib.SwcElement;
            var scriptElems = GetElements(libElem, "script");
            foreach (var scriptElem in scriptElems)
            {
                string scriptName = scriptElem.GetAttribute("name");
                var abc = lib.FindAbc(scriptName);
                if (abc == null)
                    throw new BadImageFormatException();

                string defID = GetDefID(scriptElem);
                string fn = ToFullName(defID);
                abc.Def = abc.FindInstance(fn);

                abc.SwcElement = scriptElem;
                _defCache[defID] = abc;
            }
        }

        static string ToFullName(string defID)
        {
            return defID.Replace(':', '.');
        }

        static string GetLibName(XmlElement libElem)
        {
            return libElem.GetAttribute("path");
        }

        public IList<SwfMovie> GetLibraries()
        {
            InitLibs();
            return _libs;
        }

        public List<AbcFile> GetAbcFiles()
        {
            if (_abclist == null)
            {
                _abclist = new List<AbcFile>();
                foreach (var swf in GetLibraries())
                    _abclist.AddRange(swf.GetAbcFiles());
            }
            return _abclist;
        }

        List<AbcFile> _abclist;

        void InitLibs()
        {
        	InitLibElems();

        	if (_libElems.Select(libElem => GetLibName(libElem)).Select(name => GetLibrary(name)).Any(lib => lib == null))
        	{
        		throw new BadImageFormatException();
        	}
        }

    	void InitLibElems()
        {
            if (_libElems != null) return;

            _libElems = new HashList<string, XmlElement>(GetLibName);

            var cat = Catalog;
            if (cat == null)
                throw new InvalidOperationException("No catalog.xml");

            var libs = GetLibElements(cat);
            foreach (var libElem in libs)
                _libElems.Add(libElem);
        }

        readonly HashList<string, SwfMovie> _libs;
        HashList<string, XmlElement> _libElems;
        #endregion

        #region Digests
        /// <summary>
        /// Gets digest for given library within this SWC file.
        /// </summary>
        /// <param name="libName"></param>
        /// <param name="hashType"></param>
        /// <param name="isSigned"></param>
        /// <returns></returns>
        public string GetLibraryDigest(string libName, string hashType, bool isSigned)
        {
            if (string.IsNullOrEmpty(hashType))
                hashType = HashExtensions.TypeSHA256;

        	if (string.IsNullOrEmpty(libName))
                libName = LIBRARY_SWF;

            var lib = GetLibrary(libName);
            if (lib == null)
            {
                libName = libName + ".swf";
                lib = GetLibrary(libName);
            }

            if (lib == null)
                throw new ArgumentException(string.Format("Unable to find library {0}", libName));

            InitDigests();

            string key = MakeDigestKey(libName, hashType, isSigned);

            return _digests[key] as string;
        }

        private Hashtable _digests;

        private void InitDigests()
        {
            if (_digests != null) return;

            _digests = new Hashtable();

            var cat = Catalog;

            var libs = GetLibElements(cat);
            foreach (var libElem in libs)
            {
                string libName = GetLibName(libElem);
                var digestsElem = GetElement(libElem, "digests");
                if (digestsElem != null)
                {
                    foreach (var digElem in GetElements(digestsElem, "digest"))
                    {
                        bool isSigned = digElem.GetAttribute("signed") == "true";
                        string hashType = digElem.GetAttribute("type");
                        string value = digElem.GetAttribute("value");
                        string key = MakeDigestKey(libName, hashType, isSigned);
                        _digests[key] = value;
                    }
                }
            }
        }

        private static string MakeDigestKey(string libName, string hashType, bool isSigned)
        {
            return libName + "_" + hashType + "_" + (isSigned ? "S" : "U");
        }
        #endregion

        #region XmlUtils
        static XmlElement GetElement(XmlNode parent, string localName)
        {
        	return (from XmlNode kid in parent.ChildNodes
					select kid as XmlElement)
					.FirstOrDefault(e => e != null && e.LocalName == localName);
        }

    	static IEnumerable<XmlElement> GetElements(XmlNode parent, string localName)
    	{
    		return (from XmlNode kid in parent.ChildNodes
					select kid as XmlElement into e
					where e != null && e.LocalName == localName select e).ToList();
    	}

    	static IEnumerable<XmlElement> GetLibElements(XmlDocument cat)
        {
            var root = cat.DocumentElement;
            var libs = GetElement(root, "libraries");
            if (libs == null) return null;
            return GetElements(libs, "library");
        }
        #endregion

        #region Dependency Resolving
        //NOTE: SWC dep types:
        //i - inheritance (supertype or interface)
        //n - namespace constant like AS3, mx_internal
        //e - external
        //s - members or code??

        private ISwcLinker _linker;
        private readonly Hashtable _defCache = new Hashtable();
        private readonly Hashtable _externalRefs = new Hashtable();
        internal readonly AbcCache AbcCache = new AbcCache();
        private SwcDepFile _deps;

        public void ResolveDependencies(ISwcLinker linker, SwcDepFile deps)
        {
#if PERF
            int start = Environment.TickCount;
#endif
            _linker = linker;
            _deps = deps;

            LoadAbcFiles();

            if (_deps != null)
                ResolveDeps();
            else
                ResolveXmlDeps();

#if PERF
            Console.WriteLine("SwcFile.ResolveDependencies: {0}", Environment.TickCount - start);
#endif
        }

        #region Loading ABC files

        private void LoadAbcFiles()
        {
            var libs = GetLibraries();
            foreach (var lib in libs)
            {
                foreach (var abc in lib.GetAbcFiles())
                {
					if (_linker != null)
					{
						abc.Assembly = _linker.Assembly;
					}

                	ProcessTraits(lib, abc);

                    AbcCache.Add(abc);

					if (_deps != null)
					{
						abc.ImportStrategy = ImportStrategy.Refs;
					}
                }
            }
        }

        #endregion

        #region ResolveDeps - Fast Version
        void ResolveDeps()
        {
            foreach (var f in _deps.Files)
                ResolveDeps(f);
        }

        AbcFile GetAbcFile(int lib, int file)
        {
            var l = _libs[lib];
            return l.GetAbcFiles()[file];
        }

        void ResolveDeps(SwcDepFile.File f)
        {
            var abc = GetAbcFile(f.LibIndex, f.FileIndex);
            foreach (var dep in f.Deps)
                ResolveDep(abc, dep);
        }

        void ResolveDep(AbcFile abc, SwcDepFile.Dep dep)
        {
            switch (dep.Type)
            {
                case SwcDepFile.DepType.FileRef:
                    {
                        var r = (SwcDepFile.FileRef)dep;
                        var abc2 = GetAbcFile(r.LibIndex, r.FileIndex);
                        abc.FileRefs.Add(abc2);
                    }
                    break;

                case SwcDepFile.DepType.TypeRef:
                    {
                        if (_linker != null)
                        {
                            var tr = (SwcDepFile.TypeRef)dep;
                            var rv = _linker.ResolveExternalReference(tr.FullName);

                            var depAbc = rv as AbcFile;
                            if (depAbc != null)
                            {
                                abc.FileRefs.Add(depAbc);
                                return;
                            }

                            var depInstance = rv as AbcInstance;
                            if (depInstance != null)
                            {
                                if (!depInstance.IsForeign)
                                    abc.InstanceRefs.Add(depInstance);
                            }
                        }
                    }
                    break;

                case SwcDepFile.DepType.NamespaceRef:
                    {
                        var r = (SwcDepFile.NamespaceRef)dep;
                        string ns = _deps.Namespaces[r.Index];
                        var depAbc = AbcCache.Namespaces.Find(ns);
                        if (depAbc != null)
                            abc.FileRefs.Add(depAbc);
                    }
                    break;
            }
        }
        #endregion

        #region ResolveXmlDeps
        void ResolveXmlDeps()
        {
            var libs = GetLibraries();
            foreach (var lib in libs)
                ResolveXmlDeps(lib);
        }

        void ResolveXmlDeps(SwfMovie lib)
        {
            CacheScripts(lib);

            var abcFiles = lib.GetAbcFiles();
            foreach (var abc in abcFiles)
            {
                AbcInstance defInstance = abc.Def;
                if (defInstance == null) continue;

                var scriptElem = abc.SwcElement;

                //if (_linker != null)
                //    _linker.LinkType(defInstance);

                var depElems = GetElements(scriptElem, "dep");
                foreach (var depElem in depElems)
                {
                    string depID = depElem.GetAttribute("id");
                    string type = depElem.GetAttribute("type");
                    if (type == "n")
                    {
                        ResolveNamespace(defInstance, depID);
                    }
                    else
                    {
                        ResolveDefLink(defInstance, depID);
                    }
                }
            }
        }
        #endregion

        #region Utils
        public struct Dep
        {
            public string ID;
            public string Type;

            public bool IsNamespace
            {
                get { return Type == "n"; }
            }
        }

        public IEnumerable<Dep> GetDeps(AbcFile abc)
        {
            var scriptElem = abc.SwcElement;
            if (scriptElem != null)
            {
                var depElems = GetElements(scriptElem, "dep");
                foreach (var depElem in depElems)
                {
                    string depID = depElem.GetAttribute("id");
                    string type = depElem.GetAttribute("type");
                    yield return new Dep { ID = depID, Type = type };
                }
            }
        }

        public IEnumerable<string> GetNamespaceRefs(AbcFile abc)
        {
        	return from dep in GetDeps(abc)
				   where dep.IsNamespace
				   select dep.ID;
        }

    	#endregion

        #region ResolveNamespace
        internal bool AddNsRefs;

        void ResolveNamespace(AbcInstance defInstance, string ns)
        {
            ns = ns.Replace(':', '.');
            var depAbc = AbcCache.Namespaces.Find(ns);
            if (depAbc != null)
            {
                if (!depAbc.IsCore)
                {
                    defInstance.ImportAbcFiles.Add(depAbc);
                    if (AddNsRefs)
                        defInstance.Abc.AddNsRef(ns);
                }
                return;
            }

            if (_linker != null)
            {
                
            }
        }
        #endregion

        #region ResolveDefLink
        void ResolveDefLink(AbcInstance defInstance, string depID)
        {
            if (defInstance == null)
                return;

            var depAbc = (AbcFile)_defCache[depID];
            if (depAbc != null)
            {
                defInstance.ImportAbcFiles.Add(depAbc);
                return;
            }

            if (_linker != null)
            {
                var r = _externalRefs[depID];
                if (r == null)
                {
                    r = _linker.ResolveExternalReference(depID);
                    if (r != null)
                        _externalRefs[depID] = r;
                }

                if (r != null)
                {
                    depAbc = r as AbcFile;
                    if (depAbc != null)
                    {
                        if (!depAbc.IsCore)
                            defInstance.ImportAbcFiles.Add(depAbc);
                        return;
                    }

                    var depInstance = r as AbcInstance;
                    if (depInstance != null)
                    {
                        //TODO:
                    }
                }
            }
        }
        #endregion
        #endregion

        #region ProcessTraits

		private void ProcessTraits(SwfMovie lib, AbcFile abc)
        {
			foreach (var instance in abc.Instances)
        	{
				if (IsMixin(instance))
				{
					RegisterMixin(instance);
				}
        	}

			foreach (var trait in abc.GetTraits(AbcTraitOwner.Script))
            {
				if (trait.HasMetadata)
				{
					foreach (var e in trait.Metadata)
						ProcessMeta(lib, trait, e);
				}

				if (trait.Kind == AbcTraitKind.Class && trait.Embed == null)
				{
					ResolveEmbed(lib, trait.Class.Instance, trait);
				}
            }
        }

	    private static void ResolveEmbed(SwfMovie lib, AbcInstance instance, AbcTrait trait)
	    {
			if (instance.IsInterface) return;

		    var superName = instance.BaseTypeName.FullName;
		    if (!superName.EndsWith("Asset") || IsAssetClass(instance)) return;

			string className = instance.FullName;
		    var asset = lib.FindAsset(className);
		    if (asset == null)
		    {
			    CompilerReport.Add(Warnings.UnableFindSwfAsset, className);
			    return;
		    }

		    Embed.Apply(trait, asset, lib);
	    }

		//TODO: move this to external file
		private static readonly HashSet<string> AssetClasses = new HashSet<string>(
			new[]
				{
					"mx.core.BitmapAsset",
					"mx.core.ButtonAsset",
					"mx.core.ByteArrayAsset",
					"mx.core.FontAsset",
					"mx.core.MovieClipAsset",
					"mx.core.MovieClipLoaderAsset",
					"mx.core.SoundAsset",
					"mx.core.SpriteAsset",
					"mx.core.TextFieldAsset",
					"mx.skins.halo.DefaultDragImage"
				});

	    private static bool IsAssetClass(AbcInstance instance)
	    {
		    return AssetClasses.Contains(instance.FullName);
	    }

	    private void ProcessMeta(SwfMovie lib, AbcTrait trait, AbcMetaEntry e)
        {
            string name = e.NameString;
            if (name == MetadataTags.Embed)
            {
                Embed.Resolve(trait, e, lib);
                return;
            }

            if (name == MetadataTags.Mixin)
            {
                var klass = trait.Class;
                if (klass == null)
                    throw new InvalidOperationException("Mixin can be applied to class trait only");

                var instance = klass.Instance;
                RegisterMixin(instance);
            }
        }

    	private void RegisterMixin(AbcInstance instance)
    	{
    		instance.IsMixin = true;
    		AbcCache.Mixins.Add(instance);
    	}

    	private static readonly Dictionary<string, bool> MixinInterfaces =
    		new Dictionary<string, bool>
    			{
					{"mx.binding.IWatcherSetupUtil", true},
					{"mx.binding.IWatcherSetupUtil2", true}
    			};

		private static bool IsMixin(AbcInstance instance)
		{
			return instance.Interfaces.Any(name => MixinInterfaces.ContainsKey(name.FullName));
		}

    	#endregion

        #region DevUtils
        public IEnumerable<SwfMovie> ExtractSwfs()
        {
        	return (from ZipEntry e in _zip
        	        where e.FileName.EndsWith(".swf", StringComparison.OrdinalIgnoreCase)
        	        let stream = e.OpenReader().ToMemoryStream()
        	        select new SwfMovie(stream) {Name = e.FileName}).ToArray();
        }

    	#endregion

        #region Utils
        static string GetDefID(XmlElement scriptElem)
        {
            var defElem = GetElement(scriptElem, "def");
            if (defElem == null)
                throw new BadImageFormatException();

            string defID = defElem.GetAttribute("id");
            if (string.IsNullOrEmpty(defID))
                throw new BadImageFormatException();
            return defID;
        }
        #endregion

        #region Resource Bundles
        bool _rbloaded;

        internal void LoadResourceBundles()
        {
            if (_rbloaded) return;
            _rbloaded = true;
            ResourceBundles.LoadZipFile(_zip, null);
        }

        readonly Hashtable _imageCache = new Hashtable();

        internal Image ResolveImage(string path)
        {
            path = path.ToLower();
            var image = _imageCache[path] as Image;
            if (image != null)
                return image;

            foreach (var e in _zip)
            {
                string name = e.FileName;
                if (string.Compare(name, path, true) == 0)
                {
                    var ms = e.OpenReader().ToMemoryStream();
                    image = Image.FromStream(ms);
                    _imageCache[path] = image;
                    return image;
                }
            }

            return null;
        }
        #endregion
    }
}