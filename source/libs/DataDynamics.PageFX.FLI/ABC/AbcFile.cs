using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml;
using DataDynamics.PageFX.CodeModel;
using DataDynamics.PageFX.FLI.IL;
using DataDynamics.PageFX.FLI.SWC;
using DataDynamics.PageFX.FLI.SWF;

namespace DataDynamics.PageFX.FLI.ABC
{
    using AbcString = AbcConst<string>;

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
            _nspool = new AbcNamespacePool(this);
            _nssets = new AbcNssetPool(this);
            _multinames = new AbcMultinamePool(this);
            _instances = new AbcInstanceCollection(this);
            _scripts = new AbcScriptCollection(this);
            _methods = new AbcMethodCollection(this);
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
            get
            {
                if (_version == null)
                    return new Version(CurrentMajor, CurrentMinor);
                return _version;
            }
            set { _version = value; }
        }
        Version _version;

        internal const int CurrentMajor = 46;
        internal const int CurrentMinor = 16;

        #region ConstPools
        /// <summary>
        /// Gets the pool of signed integer constants.
        /// </summary>
        public AbcConstPool<int> IntPool
        {
            get { return _intPool; }
        }
        private readonly AbcConstPool<int> _intPool = new AbcConstPool<int>();

        /// <summary>
        /// Gets the pool of unsigned integer constants.
        /// </summary>
        public AbcConstPool<uint> UIntPool
        {
            get { return _uintPool; }
        }
        private readonly AbcConstPool<uint> _uintPool = new AbcConstPool<uint>();

        /// <summary>
        /// Gets the pool of double constants.
        /// </summary>
        public AbcConstPool<double> DoublePool
        {
            get { return _doublePool; }
        }
        private readonly AbcConstPool<double> _doublePool = new AbcConstPool<double>();

        /// <summary>
        /// Gets the pool string constants.
        /// </summary>
        public AbcConstPool<string> StringPool
        {
            get { return _stringPool; }
        }
        readonly AbcConstPool<string> _stringPool = new AbcConstPool<string>();

        /// <summary>
        /// Gets the pool of namespaces used in the ABC file.
        /// </summary>
        public AbcNamespacePool Namespaces
        {
            get { return _nspool; }
        }
        readonly AbcNamespacePool _nspool;

        public AbcNssetPool NamespaceSets
        {
            get { return _nssets; }
        }
        private readonly AbcNssetPool _nssets;

        public AbcMultinamePool Multinames
        {
            get { return _multinames; }
        }
        private readonly AbcMultinamePool _multinames;
        #endregion

        #region ABC Elements
        /// <summary>
        /// Gets the methods defined in this abc file.
        /// </summary>
        public AbcMethodCollection Methods
        {
            get { return _methods; }
        }
        readonly AbcMethodCollection _methods;

        /// <summary>
        /// Gets the metadata entries.
        /// </summary>
        public AbcMetadata Metadata
        {
            get { return _metadata; }
        }
        readonly AbcMetadata _metadata = new AbcMetadata();

        /// <summary>
        /// 
        /// </summary>
        public AbcInstanceCollection Instances
        {
            get { return _instances; }
        }
        readonly AbcInstanceCollection _instances;

        /// <summary>
        /// 
        /// </summary>
        public AbcClassCollection Classes
        {
            get { return _classes; }
        }
        readonly AbcClassCollection _classes = new AbcClassCollection();

        /// <summary>
        /// 
        /// </summary>
        public AbcScriptCollection Scripts
        {
            get { return _scripts; }
        }
        readonly AbcScriptCollection _scripts;
        
        /// <summary>
        /// 
        /// </summary>
        public AbcMethodBodyCollection MethodBodies
        {
            get { return _methodBodies; }
        }
        readonly AbcMethodBodyCollection _methodBodies = new AbcMethodBodyCollection();
        #endregion

        #region ABL Deps
        public DepList Deps { get; set; }

        internal void AddDep(Dep dep)
        {
            if (Deps == null)
                Deps = new DepList();
            Deps.Add(dep);
        }

        public bool HasDeps
        {
            get { return Deps != null && Deps.Count > 0; }
        }
        #endregion

        public AbcInstance FirstInstance
        {
            get 
            {
                if (_instances.Count > 0)
                    return _instances[0];
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
                int i = _scripts.Count - 1;
                if (i >= 0) return _scripts[i];
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
            _instances.Add(instance);
            _classes.Add(instance.Class);
        }

        public void AddMethod(AbcMethod method)
        {
            if (method == null)
                throw new ArgumentNullException("method");
            _methods.Add(method);
            var body = method.Body;
            if (body != null)
                _methodBodies.Add(body);
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
        internal SwfMovie SWF { get; set; }

        internal SwcFile SWC { get; set; }

        internal XmlElement SwcElement { get; set; }

        /// <summary>
        /// Gets or sets the flag indicating whether this ABC file is from SWC file.
        /// </summary>
        internal bool InSwc
        {
            get { return SWC != null; }
        }

        internal bool UseExternalLinking
        {
            get
            {
                if (SWC != null)
                    return SWC.RSL != null;
                return false;
            }
        }

        internal bool IsLinkedExternally { get; set; }
        #endregion

        #region Internal Properties
        internal AbcInstance Def;

        internal AbcGenerator generator;

        public IAssembly ApplicationAssembly
        {
            get
            {
                if (generator != null)
                    return generator.ApplicationAssembly;
                if (_sfc != null)
                    return _sfc.ApplicationAssembly;
                return null;
            }
        }

        internal SwfCompiler SwfCompiler
        {
            get
            {
                if (generator != null)
                    return generator.sfc;
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
                if (generator != null)
                    return generator.MainInstance;
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
                _intPool.Read(reader);
                _uintPool.Read(reader);

                SwfReader.CheckU30 = true;
                _doublePool.Read(reader);
                _stringPool.Read(reader);
                _nspool.Read(reader);
                _nssets.Read(reader);
                _multinames.Read(reader);
                _methods.Read(reader);
                _metadata.Read(reader);

                int n = (int)reader.ReadUIntEncoded();
                _instances.Read(n, reader);
                _classes.Read(n, reader);
                for (int i = 0; i < n; ++i)
                {
                    var klass = _classes[i];
                    var instance = _instances[i];
                    instance.Class = klass;
                    klass.Instance = instance;
                    klass.Initializer.Instance = instance;
                }

                _scripts.Read(reader);
                _methodBodies.Read(reader);
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
                _intPool.Write(writer);
                _uintPool.Write(writer);
                _doublePool.Write(writer);
                _stringPool.Write(writer);
                _nspool.Write(writer);
                _nssets.Write(writer);
                _multinames.Write(writer);

                _methods.Write(writer);
                _metadata.Write(writer);

                int n = _instances.Count;
                writer.WriteUIntEncoded((uint)n);
                _instances.Write(writer);
                _classes.Write(writer);

                _scripts.Write(writer);
                _methodBodies.Write(writer);
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

        #region FormatOffset
        public string FormatOffset(int offset)
        {
            string s = _intPool.FormatOffset(offset);
            if (!string.IsNullOrEmpty(s)) return s;

            s = _uintPool.FormatOffset(offset);
            if (!string.IsNullOrEmpty(s)) return s;

            s = _doublePool.FormatOffset(offset);
            if (!string.IsNullOrEmpty(s)) return s;

            s = _stringPool.FormatOffset(offset);
            if (!string.IsNullOrEmpty(s)) return s;

            s = _nspool.FormatOffset(this, offset);
            if (!string.IsNullOrEmpty(s)) return s;

            s = _nssets.FormatOffset(this, offset);
            if (!string.IsNullOrEmpty(s)) return s;

            s = _multinames.FormatOffset(this, offset);
            if (!string.IsNullOrEmpty(s)) return s;

            s = _methods.FormatOffset(offset);
            if (!string.IsNullOrEmpty(s)) return s;

            s = _metadata.FormatOffset(this, offset);
            if (!string.IsNullOrEmpty(s)) return s;

            s = _instances.FormatOffset(this, offset);
            if (!string.IsNullOrEmpty(s)) return s;

            s = _classes.FormatOffset(this, offset);
            if (!string.IsNullOrEmpty(s)) return s;

            s = _scripts.FormatOffset(this, offset);
            if (!string.IsNullOrEmpty(s)) return s;

            s = _methodBodies.FormatOffset(offset);
            if (!string.IsNullOrEmpty(s)) return s;

            return null;
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
                _intPool.DumpXml(writer);
                _uintPool.DumpXml(writer);
                _doublePool.DumpXml(writer);
                _stringPool.DumpXml(writer);
                _nspool.DumpXml(writer);
                _nssets.DumpXml(writer);
                _multinames.DumpXml(writer);
                writer.WriteEndElement();
            }

            _methods.DumpXml(writer);
            //NOTE: metadata will be dumped with traits.
            //_metadata.DumpXml(writer);
            _instances.DumpXml(writer);
            //NOTE: classes are dumped with instances
            //_classes.DumpXml(writer);
            _scripts.DumpXml(writer);
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
            _instances.DumpDirectory(dir);
        }

        public void DumpFile(string path)
        {
            using (var writer = new StreamWriter(path))
            {
                _instances.GroupByNamespace().Dump(writer);

                if (_instances.Count > 0)
                    writer.WriteLine();

                _scripts.Dump(writer);
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
        void EnsureImport()
        {
            foreach (var instance in _instances)
            {
                if (instance.Initializer == null)
                    throw new BadFormatException();
                if (instance.Class.Initializer == null)
                    throw new BadFormatException();
                if (!IsDefined(instance.Name))
                    throw Errors.ABC.BadFormat.CreateException();

                if (instance.SuperName != null)
                    instance.SuperName = ImportConst(instance.SuperName);

                EnsureInterfaces(instance);
                EnsureTraits(instance);

                //CheckInheritance(instance);
            }

            foreach (var klass in _classes)
            {
                if (klass.Initializer == null)
                    throw new BadFormatException();
                EnsureTraits(klass);
            }

            foreach (var script in _scripts)
            {
                if (script.Initializer == null)
                    throw new BadFormatException();
                EnsureTraits(script);
            }

            foreach (var method in _methods)
            {
                method.ReturnType = ImportConst(method.ReturnType);
                foreach (var p in method.Parameters)
                {
                    p.Type = ImportConst(p.Type);
                    p.Name = ImportConst(p.Name);
                }
            }

            foreach (var body in _methodBodies)
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

            if (instance.SuperName != null
                && !instance.SuperName.IsAny
                && instance.SuperName.Kind != AbcConstKind.QName)
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
                    if (klass.ABC != this)
                        throw new InvalidOperationException();
                }

                var f = op.Value as AbcMethod;
                if (f != null)
                {
                    if (f.ABC != this)
                        throw new InvalidOperationException();
                }
            }
        }
        #endregion

        #region InstanceComparer
        class InstanceComparer : IComparer<AbcInstance>
        {
            public InstanceComparer(AbcInstance app)
            {
                _app = app;
            }
            readonly AbcInstance _app;

            public int Compare(AbcInstance x, AbcInstance y)
            {
                if (x == y) return 0;
                if (x == null) return -1;
                if (y == null) return 1;

                //if (x.IsFlexInitMixin)
                //{
                //    if (Compare(y, _app) <= 0)
                //        return 1;
                //    return -1;
                //}

                //if (y.IsFlexInitMixin)
                //{
                //    if (Compare(x, _app) <= 0)
                //        return -1;
                //    return 1;
                //}

                //if (x.IsStyleMixin)
                //{
                //    if (y.IsStyleMixin)
                //        return x.Index - y.Index;
                //    if (Compare(y, _app) <= 0)
                //        return 1;
                //    return -1;
                //}

                //if (y.IsStyleMixin)
                //{
                //    if (Compare(x, _app) <= 0)
                //        return -1;
                //    return 1;
                //}

                int n = x.Index - y.Index;
                if (n < 0)
                {
                    if (x.IsInheritedFrom(y.Name))
                        return 1;
                }
                else
                {
                    if (y.IsInheritedFrom(x.Name))
                        return -1;
                }

                ////x is base of y
                //if (x.Name == y.SuperName)
                //    return -1;

                ////y is base of x
                //if (y.Name == x.SuperName)
                //    return 1;

                ////y implements x
                //if (x.IsInterface)
                //{
                //    if (y.HasInterface(x))
                //        return -1;
                //}

                ////x implements y
                //if (y.IsInterface)
                //{
                //    if (x.HasInterface(y))
                //        return 1;
                //}

                //if (n < 0) //x declared before y
                //{
                //    if (x.IsTypeUsed(y.Name))
                //        return 1;
                //}
                //else
                //{
                //    if (x.IsTypeUsed(y.Name))
                //        return -1;
                //}

                return n;
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
        void FixOrder()
        {
            //FixOrder1();
            FixOrder2();
        }

        void FixOrder1()
        {
            var app = MainInstance;
            _instances.Sort(new InstanceComparer(app));

            _classes.Clear();

            int n = _instances.Count;
            for (int i = 0; i < n; ++i)
            {
                var instance = _instances[i];
                var klass = instance.Class;
                instance.Index = i;
                klass.Index = i;
                _classes.AddInternal(klass);
            }

            //_scripts.Sort(CreateScriptComparer());
        }

        bool IsMxApp
        {
            get 
            {
                return _sfc != null && _sfc.IsFlexApplication;
            }
        }

        void FixOrder2()
        {
            _order = new List<AbcInstance>();

            if (IsMxApp && generator != null)
            {
                var app = generator.MainInstance;
                Order(app);
            }

            foreach (var instance in _instances)
            {
                Order(instance);
            }

            Debug.Assert(_order.Count == _instances.Count);

            _instances.Clear();
            _classes.Clear();

            int n = _order.Count;
            for (int i = 0; i < n; ++i)
            {
                var instance = _order[i];
                var klass = instance.Class;
                instance.Index = i;
                klass.Index = i;
                _instances.AddInternal(instance);
                _classes.AddInternal(klass);
            }

            _scripts.Sort(CreateScriptComparer());
        }

        List<AbcInstance> _order;

        void Order(AbcInstance instance)
        {
            if (instance == null) return;
            if (instance.Ordered) return;

            foreach (var other in _instances)
            {
                if (other == instance) continue;
                if (other.Ordered) continue;
                if (instance.IsInheritedFrom(other.Name))
                    Order(other);
            }

            instance.Ordered = true;
            _order.Add(instance);
        }
        #endregion
        #endregion

        #region Utils
        public bool ContainsType(string ns, string name)
        {
            string fn = NameHelper.MakeFullName(ns, name);
            return Instances[fn] != null;
        }

        public bool IsCoreAPI
        {
            get
            {
                if (!_isCoreAPI.HasValue)
                    _isCoreAPI = ContainsType("", "Object");
                return (bool)_isCoreAPI;
            }
            set 
            {
                _isCoreAPI = value;
            }
        }
        private bool? _isCoreAPI;

        public IEnumerable<AbcTrait> GetTraits(AbcTraitOwner owners)
        {
            if ((owners & AbcTraitOwner.Instance) != 0)
            {
                foreach (var instance in _instances)
                {
                    foreach (var trait in instance.Traits)
                        yield return trait;
                    foreach (var trait in instance.Class.Traits)
                        yield return trait;
                }
            }
            if ((owners & AbcTraitOwner.Script) != 0)
            {
                foreach (var script in _scripts)
                {
                    foreach (var trait in script.Traits)
                        yield return trait;
                }
            }
            if ((owners & AbcTraitOwner.MethodBody) != 0)
            {
                foreach (var body in _methodBodies)
                {
                    foreach (var trait in body.Traits)
                        yield return trait;
                }
            }
        }

        public AbcInstance FindInstance(System.Func<AbcInstance, bool> predicate)
        {
            return Instances.FirstOrDefault(predicate);
        }

        public AbcInstance FindInstance(string name)
        {
            return FindInstance(i => i.FullName == name);
        }

        public static AbcInstance FindInstance(IEnumerable<AbcFile> files, System.Func<AbcInstance,bool> predicate)
        {
        	return files.Select(abc => abc.FindInstance(predicate)).FirstOrDefault(instance => instance != null);
        }

    	public static AbcInstance FindInstance(IEnumerable<AbcFile> files, string name)
    	{
    		return files.Select(abc => abc.FindInstance(name)).FirstOrDefault(instance => instance != null);
    	}

    	#endregion

        #region Object Overrides
        public override string ToString()
        {
            if (!string.IsNullOrEmpty(Name))
                return Name;
            return base.ToString();
        }
        #endregion

        #region Refs
        internal ImportStrategy ImportStrategy;

        internal List<AbcFile> FileRefs
        {
            get { return _fileRefs; }
        }
        readonly List<AbcFile> _fileRefs = new List<AbcFile>();

        internal List<AbcInstance> InstanceRefs
        {
            get { return _instanceRefs; }
        }
        readonly List<AbcInstance> _instanceRefs = new List<AbcInstance>();

        internal void AddNsRef(string ns)
        {
            if (_nsrefs == null)
                _nsrefs = new HashedList<string, string>(s => s);
            if (!_nsrefs.Contains(ns))
                _nsrefs.Add(ns);
        }
        HashedList<string, string> _nsrefs;

        internal IEnumerable<string> GetNsRefs()
        {
            if (_nsrefs != null) return _nsrefs;
            return null;
        }
        #endregion
    }

    internal enum ImportStrategy
    {
        Multinames,
        Refs
    }
}