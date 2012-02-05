using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using DataDynamics.PageFX.FLI.SWC;
using DataDynamics.PageFX.FLI.SWF;

namespace DataDynamics.PageFX.FLI.ABC
{
    public class AblFile
    {
        #region ctors
        public AblFile()
        {
        }

        public AblFile(SwfReader reader)
        {
            Read(reader);
        }

        public AblFile(Stream input)
            : this(new SwfReader(input))
        {
        }

        public AblFile(string path)
        {
            Load(path);
        }
        #endregion

        #region Properties
        public string Path
        {
            get { return _path; }
        }
        private string _path;

        public int Version 
        {
            get { return _version; }
            set 
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException("value");
                _version = value;
            }
        }
        private int _version = 1;

        public AbcFileList Files
        {
            get { return _files; }
        }
        private readonly AbcFileList _files = new AbcFileList();

        public AbcNamespaceList Namespaces
        {
            get { return _namespaces; }
        }
        private readonly AbcNamespaceList _namespaces = new AbcNamespaceList();

        public SwfMovie SwfAssets
        {
            get { return _swfassets; }
        }
        private readonly SwfMovie _swfassets = new SwfMovie();

        public AblAssets Assets
        {
            get { return _assets; }
        }
        private readonly AblAssets _assets = new AblAssets();
        #endregion

        #region IO
        #region Load
        public void Load(string path)
        {
            _path = path;
            using (var reader = new SwfReader(File.ReadAllBytes(path)))
                Read(reader);
        }

        public void Load(Stream input)
        {
            using (var writer = new SwfReader(input))
                Read(writer);
        }
        #endregion

        #region Save
        public void Save(string path)
        {
            using (var writer = new SwfWriter(path))
                Write(writer);
        }

        public void Save(Stream output)
        {
            using (var writer = new SwfWriter(output))
                Write(writer);
        }
        #endregion

        #region Read/Write
        public void Read(SwfReader reader)
        {
            ReadHeader(reader);
            _files.Read(reader);
            _namespaces.Read(reader);

            int size = reader.ReadInt32();
            byte[] swf = reader.ReadBlock(size);
            _swfassets.Load(new MemoryStream(swf), SwfTagDecodeOptions.DonotDecodeImages);

            _assets.Read(this, reader);
            ReadDeps(reader);
        }

        public void Write(SwfWriter writer)
        {
            WriteHeader(writer);
            _files.Write(writer);
            _namespaces.Write(writer);

            byte[] swf = _swfassets.ToByteArray();
            writer.WriteInt32(swf.Length);
            writer.Write(swf);
            
            _assets.Write(writer);
            WriteDeps(writer);
        }

        private const string SIG = "ABL";
        private static readonly byte[] BSIG = Encoding.ASCII.GetBytes(SIG);

        private void ReadHeader(SwfReader reader)
        {
            if (reader.ReadASCII(3) != SIG)
                throw new BadFormatException("Bad file signature. not " + SIG);
            _version = reader.ReadByte();
        }

        private void WriteHeader(SwfWriter writer)
        {
            writer.Write(BSIG);
            writer.WriteByte((byte)_version);
        }

        private void ReadDeps(SwfReader reader)
        {
            int n = (int)reader.ReadUIntEncoded();
            for (int i = 0; i < n; ++i)
            {
                int file = (int)reader.ReadUIntEncoded();
                var abc = _files[file];
                var deps = new DepList(this);
                deps.Read(reader);
                abc.Deps = deps;
            }
        }

        private void WriteDeps(SwfWriter writer)
        {
            int n = Logic.CountOf(_files, f => f.HasDeps);
            writer.WriteUIntEncoded(n);
            for (int i = 0; i < _files.Count; ++i)
            {
                var abc = _files[i];
                if (abc.HasDeps)
                {
                    writer.WriteUIntEncoded(i);
                    abc.Deps.Write(writer);
                }
            }
        }
        #endregion
        #endregion

        #region FromSwc
        Hashtable _typecache;

        void CacheTypes()
        {
            _typecache = new Hashtable();
            foreach (var file in _files)
            {
                var t = file.FirstInstance;
                _typecache[t.FullName] = t;
            }
        }

        AbcInstance Find(string fullname)
        {
            return (AbcInstance)_typecache[fullname];
        }

        public static AblFile FromSwc(string path)
        {
            var swc = new SwcFile(path);
            return FromSwc(swc);
        }

        public static AblFile FromSwc(SwcFile swc)
        {
            var abl = new AblFile();

            foreach (var f in swc.GetAbcFiles())
            {
                if (f.Scripts.Count == 1)
                {
                    var script = f.Scripts[0];
                    if (script.Traits.Count == 1)
                    {
                        var t = script.Traits[0];
                        switch (t.Kind)
                        {
                            case AbcTraitKind.Class:
                                abl.Files.Add(f);
                                break;

                            case AbcTraitKind.Const:
                                {
                                    var ns = t.SlotValue as AbcNamespace;
                                    if (ns != null)
                                        abl.Namespaces.Add(t.NameString, ns);
                                }
                                break;
                        }
                    }
                }
            }

            abl.CacheTypes();

            foreach (var f in abl.Files)
                BuildDeps(abl, swc, f.FirstInstance);

            return abl;
        }

        static void BuildDeps(AblFile abl, SwcFile swc, AbcInstance instance)
        {
            var abc = instance.ABC;

            BuildTypeRefs(abl, swc, abc, instance);
            BuildNamespaceRefs(abl, abc, swc);
            BuildMetaRefs(abl, abc, instance);
        }

        static void BuildTypeRefs(AblFile abl, SwcFile swc, AbcFile abc, AbcInstance instance)
        {
            foreach (var tr in TypeRef.GetRefs(abc))
            {
                var type = abl.Find(tr.FullName);
                if (type != null) //file ref
                {
                    if (type == instance) continue;
                    if (type.ABC == abc) continue;

                    var dep = new DepFileRef(type.ABC.Index) { Kind = tr.DepKind };
                    abc.AddDep(dep);
                }
                else //external type ref
                {
                    //TODO: check existance of type ref
                    var dep = new DepTypeRef(tr.FullName) { Kind = tr.DepKind };
                    abc.AddDep(dep);
                }
            }
        }

        static void BuildNamespaceRefs(AblFile abl, AbcFile abc, SwcFile swc)
        {
            foreach (var nsname in swc.GetNamespaceRefs(abc))
            {
                int i = abl.Namespaces.IndexOf(nsname);
                if (i >= 0)
                {
                    abc.AddDep(new DepNamespaceRef(i));
                }
                else
                {

                }
            }
        }

        static void BuildMetaRefs(AblFile abl, AbcFile abc, AbcInstance instance)
        {
            var ct = instance.Class.Trait;
            if (!ct.HasMetadata) return;
            foreach (var e in ct.Metadata)
                BuildMetaRef(abl, abc, ct, e);
        }

        static void BuildMetaRef(AblFile abl, AbcFile abc, AbcTrait trait, AbcMetaEntry e)
        {
            switch (e.NameString)
            {
                case MDTags.ResourceBundle:
                    {
                        string name = RBUtil.GetName(e);
                        if (string.IsNullOrEmpty(name))
                            throw Errors.RBC.BadMetaEntry.CreateException();
                        abc.AddDep(new DepResourceBundle(name));
                    }
                    return;

                case MDTags.Embed:
                    {
                        Embed.Resolve(trait, e, abc.SWF);
                        var embed = trait.Embed;
                    }
                    return;

                case MDTags.Mixin:
                    abc.AddDep(new DepMixin());
                    return;

                case MDTags.RemoteClass:
                    {
                        string alias = e["alias"];
                        abc.AddDep(new DepRemoteClass(alias));
                    }
                    return;

                case MDTags.Effect:
                    {
                        string effectName = e["name"];
                        string effectEvent = e["event"];
                        abc.AddDep(new DepEffectTrigger(effectName, effectEvent));
                    }
                    return;
            }
        }
        #endregion
    }

    #region class AbcFileList
    public class AbcFileList : List<AbcFile>
    {
        public new void Add(AbcFile f)
        {
            f.Index = Count;
            base.Add(f);
        }

        public void Read(SwfReader reader)
        {
            int n = (int)reader.ReadUIntEncoded();
            for (int i = 0; i < n; ++i)
            {
                var abc = new AbcFile();
                abc.Read(reader);
                Add(abc);
            }
        }

        public void Write(SwfWriter writer)
        {
            int n = Count;
            writer.WriteUIntEncoded(n);
            for (int i = 0; i < n; ++i)
            {
                var abc = this[i];
                abc.Write(writer);
            }
        }
    }
    #endregion
}