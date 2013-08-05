using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml;
using DataDynamics.PageFX.Common.Collections;
using DataDynamics.PageFX.Common.CompilerServices;
using DataDynamics.PageFX.Common.Extensions;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Flash.Core.CodeGeneration;
using DataDynamics.PageFX.Flash.Core.SwfGeneration;
using DataDynamics.PageFX.Flash.IL;
using DataDynamics.PageFX.Flash.Swc;
using DataDynamics.PageFX.Flash.Swf;

namespace DataDynamics.PageFX.Flash.Abc
{
	/// <summary>
    /// Represents ABC file that can be played by AVM+
    /// </summary>
    public partial class AbcFile : ISupportXmlDump
    {
        #region Constructors
        /// <summary>
        /// Initializes new instance of <see cref="AbcFile"/> class.
        /// </summary>
        public AbcFile()
        {
			IntPool = new AbcConstPool<int>();
			UIntPool = new AbcConstPool<uint>();
			DoublePool = new AbcConstPool<double>();
	        StringPool = new AbcConstPool<string>();
			Namespaces = new AbcNamespacePool(this);
			NamespaceSets = new AbcNssetPool(this);
			Multinames = new AbcMultinamePool(this);

			Methods = new AbcMethodCollection(this);
			Metadata = new AbcMetadata();
			Classes = new AbcClassCollection();
			Instances = new AbcInstanceCollection(this);
			Scripts = new AbcScriptCollection(this);
	        MethodBodies = new AbcMethodBodyCollection();
        }

        /// <summary>
        /// Instantiates abc file and reads it using given <see cref="SwfReader"/>.
        /// </summary>
        /// <param name="reader"><see cref="SwfReader"/> used to read the ABC file</param>
        public AbcFile(SwfReader reader) : this()
        {
            Read(reader);
        }

        /// <summary>
        /// Instantiates abc file and reads it from given input stream.
        /// </summary>
        /// <param name="input">input stream to parse</param>
        public AbcFile(Stream input)
            : this(new SwfReader(input))
        {
        }

        /// <summary>
        /// Instantiates abc file and reads it from given buffer.
        /// </summary>
        /// <param name="data">buffer to parse</param>
        public AbcFile(byte[] data) 
            : this(new SwfReader(data))
        {
        }

        /// <summary>
        /// Instantiates abc file and reads it from specified file.
        /// </summary>
        /// <param name="path">path to file which you want to load</param>
        public AbcFile(string path) : this()
        {
            Load(path);
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets name of the ABC file.
        /// </summary>
        public string Name { get; set; }

        public int Index { get; set; }

        /// <summary>
        /// Gets or sets the version of the ABC format.
        /// </summary>
        public Version Version
        {
            get { return _version ?? new Version(CurrentMajor, CurrentMinor); }
	        set { _version = value; }
        }
        private Version _version;

        internal const int CurrentMajor = 46;
        internal const int CurrentMinor = 16;

        #region ConstPools

		/// <summary>
		/// Gets the pool of signed integer constants.
		/// </summary>
		public AbcConstPool<int> IntPool { get; private set; }

		/// <summary>
		/// Gets the pool of unsigned integer constants.
		/// </summary>
		public AbcConstPool<uint> UIntPool { get; private set; }

		/// <summary>
		/// Gets the pool of double constants.
		/// </summary>
		public AbcConstPool<double> DoublePool { get; private set; }

		/// <summary>
		/// Gets the pool string constants.
		/// </summary>
		public AbcConstPool<string> StringPool { get; private set; }

		/// <summary>
		/// Gets the pool of namespaces used in the ABC file.
		/// </summary>
		public AbcNamespacePool Namespaces { get; private set; }

		public AbcNssetPool NamespaceSets { get; private set; }

		public AbcMultinamePool Multinames { get; private set; }

		#endregion

        #region ABC Elements

		/// <summary>
		/// Gets the methods defined in this abc file.
		/// </summary>
		public AbcMethodCollection Methods { get; private set; }

		/// <summary>
		/// Gets the metadata entries.
		/// </summary>
		public AbcMetadata Metadata { get; private set; }

		/// <summary>
		/// 
		/// </summary>
		public AbcInstanceCollection Instances { get; private set; }

		/// <summary>
		/// 
		/// </summary>
		public AbcClassCollection Classes { get; private set; }

		/// <summary>
		/// 
		/// </summary>
		public AbcScriptCollection Scripts { get; private set; }

		/// <summary>
		/// 
		/// </summary>
		public AbcMethodBodyCollection MethodBodies { get; private set; }

		#endregion

		public AbcInstance FirstInstance
        {
            get 
            {
                if (Instances.Count > 0)
                    return Instances[0];
                return null;
            }
        }

        /// <summary>
        /// Gets the last script
        /// </summary>
        public AbcScript LastScript
        {
            get
            {
                int i = Scripts.Count - 1;
                if (i >= 0) return Scripts[i];
                return null;
            }
        }

        /// <summary>
        /// Adds new instance to the ABC file.
        /// </summary>
        /// <param name="instance"><see cref="AbcInstance"/> to add</param>
        public void AddInstance(AbcInstance instance)
        {
            if (instance == null)
                throw new ArgumentNullException("instance");
            if (instance.Class == null)
                throw new ArgumentException("instance has no class");
            Instances.Add(instance);
            Classes.Add(instance.Class);
        }

        public void AddMethod(AbcMethod method)
        {
            if (method == null)
                throw new ArgumentNullException("method");
            Methods.Add(method);
            var body = method.Body;
            if (body != null)
                MethodBodies.Add(body);
        }

        /// <summary>
        /// Gets the number of interface defined in this ABC file.
        /// </summary>
        public int InterfaceCount
        {
            get { return Instances.Count(i => i.IsInterface); }
        }

        public IAssembly Assembly { get; set; }

        /// <summary>
        /// Gets or sets swf movie where this ABC was defined.
        /// </summary>
        internal SwfMovie Swf { get; set; }

        internal SwcFile Swc { get; set; }

        internal XmlElement SwcElement { get; set; }

        /// <summary>
        /// Gets or sets the flag indicating whether this ABC file is from SWC file.
        /// </summary>
        internal bool InSwc
        {
            get { return Swc != null; }
        }

        internal bool UseExternalLinking
        {
            get
            {
                if (Swc != null)
                    return Swc.RSL != null;
                return false;
            }
        }

        internal bool IsLinkedExternally { get; set; }
        #endregion

        #region Internal Properties
        internal AbcInstance Def;

        internal AbcGenerator Generator;

        public IAssembly ApplicationAssembly
        {
            get
            {
                if (Generator != null)
                    return Generator.AppAssembly;
				return SwfCompiler != null ? SwfCompiler.AppAssembly : null;
            }
        }

        internal SwfCompiler SwfCompiler
        {
            get
            {
                if (Generator != null)
                    return Generator.SwfCompiler;
                return _sfc;
            }
            set { _sfc = value; }
        }
        private SwfCompiler _sfc;

        internal bool IsSwf
        {
            get { return SwfCompiler != null; }
        }

        internal AbcFile PrevFrame { get; set; }

        internal AbcInstance MainInstance
        {
            get
            {
                if (Generator != null)
                    return Generator.MainInstance;
                return null;
            }
        }

        internal IEnumerable<AbcFile> PrevFrames
        {
            get
            {
                var f = PrevFrame;
                while (f != null)
                {
                    yield return f;
                    f = f.PrevFrame;
                }
            }
        }

        internal IEnumerable<AbcFile> AllFrames
        {
            get
            {
                if (_sfc != null)
                {
                    bool yieldThis = true;
                    foreach (var abc in _sfc.AbcFrames)
                    {
                        if (yieldThis && abc == this)
                            yieldThis = false;
                        yield return abc;
                    }
                    if (yieldThis)
                        yield return this;
                }
                else
                {
                    yield return this;
                }
            }
        }
        #endregion

        #region UndefinedValue
        private class UndefinedValue
        {
            public override string ToString()
            {
                return "undefined";
            }
        }
        public static readonly object Undefined = new UndefinedValue();

        public static bool IsUndefined(object value)
        {
            return ReferenceEquals(value, Undefined);
        }
        #endregion

        #region GetConstant
        public object GetConstant(AbcConstKind kind, int index)
        {
            switch (kind)
            {
                case AbcConstKind.Int:
                    return IntPool[index];

                case AbcConstKind.UInt:
                    return UIntPool[index];

                case AbcConstKind.Double:
                    return DoublePool[index];

                case AbcConstKind.String:
                    return StringPool[index];

                case AbcConstKind.True:
                    return true;

                case AbcConstKind.False:
                    return false;

                case AbcConstKind.Null:
                    return null;

                case AbcConstKind.Undefined:
                    return Undefined;

                case AbcConstKind.PublicNamespace:
                case AbcConstKind.PackageNamespace:
                case AbcConstKind.InternalNamespace:
                case AbcConstKind.ProtectedNamespace:
                case AbcConstKind.ExplicitNamespace:
                case AbcConstKind.StaticProtectedNamespace:
                case AbcConstKind.PrivateNamespace:
                    return Namespaces[index];

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        #endregion
        
        #region IO

        #region Read
        public string Path
        {
            get { return _path; }
        }
        private string _path;

        public void Load(string path)
        {
            _path = path;
            using (var reader = new SwfReader(File.ReadAllBytes(path)))
                Read(reader);
        }

        public void Read(SwfReader reader)
        {
            reader.ABC = this;
            int minor = reader.ReadUInt16();
            int major = reader.ReadUInt16();
            _version = new Version(major, minor);
            if (minor == CurrentMinor && major == CurrentMajor)
            {
                //NOTE: The "0" entry of each constant pool is not used.  If the count for a given pool says there are
                //NOTE: "n" entries in the pool, there are "n-1" entries in the file, corresponding to indices 1..(n-1).
                IntPool.Read(reader);
                UIntPool.Read(reader);

                SwfReader.CheckU30 = true;
                DoublePool.Read(reader);
                StringPool.Read(reader);
                Namespaces.Read(reader);
                NamespaceSets.Read(reader);
                Multinames.Read(reader);
                Methods.Read(reader);
                Metadata.Read(reader);

                int n = (int)reader.ReadUIntEncoded();
                Instances.Read(n, reader);
                Classes.Read(n, reader);
                for (int i = 0; i < n; ++i)
                {
                    var klass = Classes[i];
                    var instance = Instances[i];
                    instance.Class = klass;
                    klass.Instance = instance;
                    klass.Initializer.Instance = instance;
                }

                Scripts.Read(reader);
                MethodBodies.Read(reader);
                SwfReader.CheckU30 = false;
            }
            else
            {
                throw new NotImplementedException();
            }
        }
        #endregion

        #region Write

    	public bool AutoComplete { get; set; }

    	public bool ReduceSize { get; set; }

    	public void Write(SwfWriter writer)
        {
            writer.ABC = this;
            var ver = Version;
            writer.WriteUInt16((ushort)ver.Minor);
            writer.WriteUInt16((ushort)ver.Major);
            if (ver.Major == CurrentMajor && ver.Minor == CurrentMinor)
            {
                IntPool.Write(writer);
                UIntPool.Write(writer);
                DoublePool.Write(writer);
                StringPool.Write(writer);
                Namespaces.Write(writer);
                NamespaceSets.Write(writer);
                Multinames.Write(writer);

                Methods.Write(writer);
                Metadata.Write(writer);

                int n = Instances.Count;
                writer.WriteUIntEncoded((uint)n);
                Instances.Write(writer);
                Classes.Write(writer);

                Scripts.Write(writer);
                MethodBodies.Write(writer);
            }
            else
            {
                throw new NotImplementedException();
            }
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

        #region ToByteArray
        public byte[] ToByteArray()
        {
            var ms = new MemoryStream();
            using (var writer = new SwfWriter(ms))
            {
                Write(writer);
                ms.Flush();
                ms.Close();
                return ms.ToArray();
            }
        }
        #endregion

        #endregion

        #region Dump
        public void DumpXml(XmlWriter writer)
        {
            writer.WriteStartElement("abc");
            writer.WriteAttributeString("version", Version.ToString());
            if (!string.IsNullOrEmpty(Name))
                writer.WriteAttributeString("name", Name);

            //constant pool
            if (AbcDumpService.DumpConstPool)
            {
                writer.WriteStartElement("constants");
                IntPool.DumpXml(writer);
                UIntPool.DumpXml(writer);
                DoublePool.DumpXml(writer);
                StringPool.DumpXml(writer);
                Namespaces.DumpXml(writer);
                NamespaceSets.DumpXml(writer);
                Multinames.DumpXml(writer);
                writer.WriteEndElement();
            }

            Methods.DumpXml(writer);
            //NOTE: metadata will be dumped with traits.
            //_metadata.DumpXml(writer);
            Instances.DumpXml(writer);
            //NOTE: classes are dumped with instances
            //_classes.DumpXml(writer);
            Scripts.DumpXml(writer);
            //NOTE: bodies are dumped with methods
            //_methodBodies.Dump(writer);
            writer.WriteEndElement();
        }

        public void DumpXml(string path)
        {
        	var xws = new XmlWriterSettings {Indent = true, IndentChars = "  "};
        	using (var writer = XmlWriter.Create(path, xws))
            {
                DumpXml(writer);
            }
        }

        public void DumpDirectory(string dir)
        {
            Instances.DumpDirectory(dir);
        }

        public void DumpFile(string path)
        {
            using (var writer = new StreamWriter(path))
            {
	            var namespaces = Instances.GroupBy(x => x.NamespaceString);

				bool eol = false;
	            foreach (var ns in namespaces)
	            {
					if (eol) writer.WriteLine();

		            writer.WriteLine("#region namespace {0}", ns.Key);
					writer.WriteLine("namespace {0}", ns.Key);
					writer.WriteLine("{");

					DumpInstances(ns, writer);

		            writer.WriteLine("}");
					writer.WriteLine("#endregion");

					eol = true;
	            }

                if (Instances.Count > 0)
                    writer.WriteLine();

                Scripts.Dump(writer);
            }
        }

		private static void DumpInstances(IEnumerable<AbcInstance> seq, TextWriter writer)
		{
			bool eol = false;
			foreach (var instance in seq)
			{
				if (eol) writer.WriteLine();
				instance.Dump(writer, false);
				eol = true;
			}
		}

		#endregion

        #region Finish
        public void Finish()
        {
            EnsureImport();
            FixOrder();
        }

        #region EnsureImport
        private void EnsureImport()
        {
            foreach (var instance in Instances)
            {
                if (instance.Initializer == null)
                    throw new BadImageFormatException();
                if (instance.Class.Initializer == null)
                    throw new BadImageFormatException();
                if (!IsDefined(instance.Name))
                    throw Errors.ABC.BadFormat.CreateException();

                if (instance.BaseTypeName != null)
                    instance.BaseTypeName = ImportConst(instance.BaseTypeName);

                EnsureInterfaces(instance);
                EnsureTraits(instance);

                //CheckInheritance(instance);
            }

            foreach (var klass in Classes)
            {
                if (klass.Initializer == null)
                    throw new BadImageFormatException();
                EnsureTraits(klass);
            }

            foreach (var script in Scripts)
            {
                if (script.Initializer == null)
                    throw new BadImageFormatException();
                EnsureTraits(script);
            }

            foreach (var method in Methods)
            {
                method.ReturnType = ImportConst(method.ReturnType);
                foreach (var p in method.Parameters)
                {
                    p.Type = ImportConst(p.Type);
                    p.Name = ImportConst(p.Name);
                }
            }

            foreach (var body in MethodBodies)
            {
                EnsureTraits(body);
                //CheckIL(body);
            }
        }

        static void CheckInheritance(AbcInstance instance)
        {
            if (instance.IsInterface)
            {
                if (instance.Name.Kind != AbcConstKind.QName)
                    Debugger.Break();
            }

            if (instance.BaseTypeName != null
                && !instance.BaseTypeName.IsAny
                && instance.BaseTypeName.Kind != AbcConstKind.QName)
                Debugger.Break();

            foreach (var mn in instance.Interfaces)
                CheckQName(mn);
        }

        static void CheckQName(AbcMultiname mn)
        {
            if (mn.Kind != AbcConstKind.QName)
                Debugger.Break();
        }

        void EnsureInterfaces(AbcInstance instance)
        {
            var ifaces = instance.Interfaces;
            int n = ifaces.Count;
            for (int i = 0; i < n; ++i)
            {
                var mn = ifaces[i];
                ifaces[i] = ImportConst(mn);
            }
        }

        void EnsureTraits(IAbcTraitProvider tp)
        {
            foreach (var trait in tp.Traits)
            {
                if (!IsDefined(trait.Name))
                    throw Errors.ABC.BadFormat.CreateException();
                //CheckQName(trait.Name);
                if (trait.IsField)
                    trait.SlotType = ImportConst(trait.SlotType);
            }
        }

        void CheckIL(AbcMethodBody body)
        {
            foreach (var instr in body.IL)
                CheckInstruction(instr);
        }

        void CheckInstruction(Instruction instr)
        {
            if (!instr.HasOperands) return;
            foreach (var op in instr.Operands)
            {
                var c = op.Value as IAbcConst;
                if (c != null)
                {
                    if (!IsDefined(c))
                        throw new InvalidOperationException();
                    if (c.Index == 0)
                        throw new InvalidOperationException();
                    continue;
                }

                var klass = op.Value as AbcClass;
                if (klass != null)
                {
                    if (klass.Abc != this)
                        throw new InvalidOperationException();
                }

                var f = op.Value as AbcMethod;
                if (f != null)
                {
                    if (f.Abc != this)
                        throw new InvalidOperationException();
                }
            }
        }
        #endregion

        #region ScriptComparer
        static int CompareScriptInstances(AbcScript s1, AbcScript s2)
        {
            var i1 = s1.SingleInstance;
            var i2 = s2.SingleInstance;
            if (i1 == null)
            {
                if (i2 != null) return -1;
                return s1.Index - s2.Index;
            }

            if (i2 == null)
                return -1;

            return i1.Index - i2.Index;
        }

        class SimpleScriptComparer : IComparer<AbcScript>
        {
            public int Compare(AbcScript s1, AbcScript s2)
            {
                if (s1 == s2) return 0;
                //main should be last
                if (s1.IsMain) return 1;
                if (s2.IsMain) return -1;
                return CompareScriptInstances(s1, s2);
            }
        }

        class SwfScriptComparer : IComparer<AbcScript>
        {
            public int Compare(AbcScript s1, AbcScript s2)
            {
                if (s1 == s2) return 0;
                return CompareScriptInstances(s1, s2);
            }
        }

        IComparer<AbcScript> CreateScriptComparer()
        {
            if (IsSwf) return new SwfScriptComparer();
            return new SimpleScriptComparer();
        }
        #endregion

        #region FixOrder
        private void FixOrder()
        {
            OrderInstancesByInheritance();
        }

    	private bool IsFlexApp
        {
            get { return _sfc != null && _sfc.IsFlexApplication; }
        }

        private void OrderInstancesByInheritance()
        {
            _order = new List<AbcInstance>();

            if (IsFlexApp && Generator != null)
            {
                var app = Generator.MainInstance;
                OrderByInheritance(app);
            }

            foreach (var instance in Instances)
            {
                OrderByInheritance(instance);
            }

            Debug.Assert(_order.Count == Instances.Count);

            Instances.Clear();
            Classes.Clear();

            int n = _order.Count;
            for (int i = 0; i < n; ++i)
            {
                var instance = _order[i];
                var klass = instance.Class;
                Instances.Add(instance);
                Classes.Add(klass);
				Debug.Assert(instance.Index == i);
				Debug.Assert(klass.Index == i);
            }

            Scripts.Sort(CreateScriptComparer());
        }

        List<AbcInstance> _order;

        void OrderByInheritance(AbcInstance instance)
        {
            if (instance == null) return;
            if (instance.Ordered) return;

            foreach (var other in Instances)
            {
                if (other == instance) continue;
                if (other.Ordered) continue;
                if (instance.IsInheritedFrom(other.Name))
                    OrderByInheritance(other);
            }

            instance.Ordered = true;
            _order.Add(instance);
        }
        #endregion
        #endregion

        #region Utils
        public bool ContainsType(string ns, string name)
        {
            string fn = ns.MakeFullName(name);
            return Instances[fn] != null;
        }

        public bool IsCore
        {
            get
            {
                if (!_isCore.HasValue)
                    _isCore = ContainsType("", "Object");
                return (bool)_isCore;
            }
            set 
            {
                _isCore = value;
            }
        }
        private bool? _isCore;

        public IEnumerable<AbcTrait> GetTraits(AbcTraitOwner owners)
        {
	        var allTraits = Enumerable.Empty<AbcTrait>();
            if ((owners & AbcTraitOwner.Instance) != 0)
            {
	            var traits = Instances.SelectMany(x => x.Traits.Concat(x.Class.Traits));
	            allTraits = allTraits.Concat(traits);
            }
            if ((owners & AbcTraitOwner.Script) != 0)
            {
	            var traits = Scripts.SelectMany(x => x.Traits);
				allTraits = allTraits.Concat(traits);
            }
            if ((owners & AbcTraitOwner.MethodBody) != 0)
            {
	            var traits = MethodBodies.SelectMany(x => x.Traits);
				allTraits = allTraits.Concat(traits);
            }
	        return allTraits;
        }

        public AbcInstance FindInstance(Func<AbcInstance, bool> predicate)
        {
            return Instances.FirstOrDefault(predicate);
        }

        public AbcInstance FindInstance(string fullname)
        {
            return Instances.Find(fullname);
        }

		public AbcInstance FindInstance(AbcMultiname multiname)
		{
			return Instances.Find(multiname);
		}

		public static AbcInstance FindInstance(IEnumerable<AbcFile> files, string fullname)
    	{
    		return files.Select(abc => abc.FindInstance(fullname)).FirstOrDefault(instance => instance != null);
    	}

    	#endregion

		public override string ToString()
        {
            if (!string.IsNullOrEmpty(Name))
                return Name;
            return base.ToString();
        }

		#region Refs
        internal ImportStrategy ImportStrategy;

        internal List<AbcFile> FileRefs
        {
            get { return _fileRefs; }
        }
        private readonly List<AbcFile> _fileRefs = new List<AbcFile>();

        internal List<AbcInstance> InstanceRefs
        {
            get { return _instanceRefs; }
        }
        private readonly List<AbcInstance> _instanceRefs = new List<AbcInstance>();

        internal void AddNsRef(string ns)
        {
            if (_nsrefs == null)
                _nsrefs = new HashList<string, string>(s => s);
            if (!_nsrefs.Contains(ns))
                _nsrefs.Add(ns);
        }
        private HashList<string, string> _nsrefs;

        internal IEnumerable<string> GetNsRefs()
        {
            if (_nsrefs != null) return _nsrefs;
            return null;
        }
        #endregion
    }

    internal enum ImportStrategy
    {
        Names,
        Refs
    }
}