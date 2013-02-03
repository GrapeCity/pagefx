using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using DataDynamics.PageFX.Common.Services;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Common.Utilities;
using DataDynamics.PageFX.FlashLand.Avm;
using DataDynamics.PageFX.FlashLand.Core;
using DataDynamics.PageFX.FlashLand.Core.CodeGeneration;
using DataDynamics.PageFX.FlashLand.IL;
using DataDynamics.PageFX.FlashLand.Swc;
using DataDynamics.PageFX.FlashLand.Swf;

namespace DataDynamics.PageFX.FlashLand.Abc
{
    #region enum AbcClassFlags
    [Flags]
    public enum AbcClassFlags
    {
        /// <summary>
        /// The class is sealed: properties can not be dynamically added
        /// to instances of the class.
        /// </summary>
        Sealed = 0x01,

        /// <summary>
        /// The class is final: it cannot be a base class for any other
        /// class.
        /// </summary>
        Final = 0x02,

        /// <summary>
        /// The class is an interface.
        /// </summary>
        Interface = 0x04,

        /// <summary>
        /// The class uses its protected namespace and the
        /// protectedNs field is present in the interface_info
        /// structure.
        /// </summary>
        ProtectedNamespace = 0x08,

        //NOTE: This flag was found in source of flex compiler.
        //We may investigate this, may be AVM already supports some kind of value types
        NonNullable = 0x10,

        SealedProtectedNamespace = Sealed | ProtectedNamespace,
        FinalSealed = Final | Sealed,
    }
    #endregion

	/// <summary>
    /// Contains traits for non-static members of user defined type.
    /// </summary>
    public sealed class AbcInstance : ISupportXmlDump, ISwfIndexedAtom, IAbcTraitProvider, ITypeData
    {
		public AbcInstance()
        {
            _traits = new AbcTraitCollection(this);
        }

		public AbcInstance(bool withClass)
            : this()
        {
            if (withClass)
                Class = new AbcClass();
        }

		/// <summary>
        /// Gets or sets index of the <see cref="AbcInstance"/> within collection of <see cref="AbcInstance"/> in ABC file.
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Gets or sets type QName.
        /// </summary>
        public AbcMultiname Name { get; set; }

		public bool IsDefined(AbcFile abc)
		{
			return abc.IsDefined(this);
		}

		ITypeData ITypeData.Import(AbcFile abc)
		{
			return abc.ImportInstance(this);
		}

		/// <summary>
        /// Gets namespace name
        /// </summary>
        public string NamespaceString
        {
            get
            {
                if (Name != null)
                    return Name.NamespaceString;
                return "";
            }
        }

        /// <summary>
        /// Gets type name
        /// </summary>
        public string NameString
        {
            get { return Name != null ? Name.NameString : ""; }
        }

        public string FullName
        {
            get { return Name != null ? Name.FullName : ""; }
        }

        public string Id
        {
            get
            {
	            string ns = NamespaceString;
	            return string.IsNullOrEmpty(ns) ? NameString : ns + ":" + NameString;
            }
        }

        public Visibility Visibility
        {
            get
            {
                if (Name == null) return Visibility.Private;
                return Name.Visibility;
            }
        }

        /// <summary>
        /// Gets or sets the name of base type.
        /// </summary>
        public AbcMultiname BaseTypeName { get; set; }

        internal AbcInstance BaseInstance
        {
            get
            {
                if (_baseInstance == null)
                {
	                var type = Type;
                    if (type != null)
                    {
						if (IsError)
						{
							// TODO: use Native.Object
							return null;
						}

                        var baseType = type.BaseType;
                        if (baseType != null)
                        {
                            var instance = baseType.AbcInstance();
                            if (instance != null)
                                return instance;
                        }
                    }
                }
                return _baseInstance;
            }
            set 
            {
                if (_baseInstance != null)
                {
					int i = _baseInstance.Subclasses.IndexOf(this);
                    if (i >= 0)
                        _baseInstance.Subclasses.RemoveAt(i);
                }
                _baseInstance = value;
                if (value != null)
                    _baseInstance.Subclasses.Add(this);
            }
        }
        private AbcInstance _baseInstance;

        /// <summary>
        /// Gets or sets type assotiated with this <see cref="AbcInstance"/>.
        /// </summary>
        public IType Type { get; set; }

        /// <summary>
        /// Gets or sets class flags.
        /// </summary>
        public AbcClassFlags Flags { get; set; }

        public bool IsInterface
        {
            get { return (Flags & AbcClassFlags.Interface) != 0; }
            set
            {
                if (value) Flags |= AbcClassFlags.Interface;
                else Flags &= ~AbcClassFlags.Interface;
            }
        }

        public bool IsNative { get; set; }

        internal AbcInstance ImportedFrom { get; set; }

        /// <summary>
        /// Imported <see cref="AbcInstance"/>.
        /// </summary>
        internal AbcInstance ImportedInstance { get; set; }

        /// <summary>
        /// Returns true if this <see cref="AbcInstance"/> was imported from other <see cref="AbcInstance"/>.
        /// </summary>
        public bool IsImported
        {
            get { return ImportedFrom != null; }
        }

        public bool IsForeign
        {
            get { return IsNative || IsImported; }
        }
        
        public bool HasGlobalName(string name)
        {
            if (!IsGlobal) return false;
            return NameString == name;
        }

        /// <summary>
        /// Returns true if this instance is defined in global package.
        /// </summary>
        public bool IsGlobal
        {
            get
            {
                var mn = Name;
                if (mn == null) return false;
                var ns = mn.Namespace;
                if (ns == null) return false;
                return ns.IsGlobalPackage;
            }
        }

        public bool IsObject
        {
            get { return HasGlobalName(Const.AvmGlobalTypes.Object); }
        }

        public bool IsError
        {
            get { return HasGlobalName(Const.AvmGlobalTypes.Error); }
        }

	    public AbcFile Abc { get; set; }

		internal AbcGenerator Generator
		{
			get { return Abc != null ? Abc.Generator : null; }
		}

	    public AbcNamespace ProtectedNamespace { get; set; }

	    public bool HasProtectedNamespace
        {
            get { return (Flags & AbcClassFlags.ProtectedNamespace) != 0; }
            set
            {
                if (value) Flags |= AbcClassFlags.ProtectedNamespace;
                else Flags &= ~AbcClassFlags.ProtectedNamespace;
            }
        }

        public AbcMethod Initializer
        {
            get { return _initializer; }
            set
            {
                if (value != _initializer)
                {
                    _initializer = value;
                    if (value != null)
                    {
                        value.IsInitializer = true;
                        value.Instance = this;
                    }
                }
            }
        }
        private AbcMethod _initializer;

        internal AbcMethod StaticCtor;
        internal AbcTrait StaticCtorFlag;
        internal AbcMethod StaticCtorCaller;
        
        public List<AbcMultiname> Interfaces
        {
            get { return _interfaces; }
        }
        private readonly List<AbcMultiname> _interfaces = new List<AbcMultiname>();

        public bool HasInterface(AbcInstance iface)
        {
        	return Interfaces.Any(mn => ReferenceEquals(mn, iface.Name));
        }

        public AbcTraitCollection Traits
        {
            get { return _traits; }
        }
        private readonly AbcTraitCollection _traits;

        public IEnumerable<AbcTrait> GetAllTraits()
        {
	        return _class != null ? _traits.Concat(_class.Traits) : _traits;
        }

	    public AbcTraitCollection GetTraits(bool isStatic)
	    {
		    return isStatic ? Class.Traits : Traits;
	    }

	    public AbcClass Class
        {
            get { return _class; }
            set
            {
                if (value != _class)
                {
                    if (_class != null)
                        _class.Instance = null;
                    _class = value;
                    if (_class != null)
                        _class.Instance = this;
                }
            }
        }
        private AbcClass _class;

        internal bool InSwc
        {
            get { return Abc != null && Abc.InSwc; }
        }

        internal SwcFile Swc
        {
            get { return Abc != null ? Abc.Swc : null; }
        }

        internal bool UseExternalLinking
        {
            get
            {
                if (Abc != null)
                    return Abc.UseExternalLinking;
                return false;
            }
        }

        internal bool IsLinkedExternally { get; set; }

	    internal Embed Embed { get; set; }

	    internal bool IsEmbeddedAsset
        {
            get { return Embed != null; }
        }

        internal string Locale { get; set; }

        internal string ResourceBundleName { get; set; }

        internal bool IsResourceBundle
        {
            get { return !string.IsNullOrEmpty(ResourceBundleName); }
        }

        internal bool IsMixin { get; set; }

        internal bool IsStyleMixin { get; set; }

        internal bool IsFlexInitMixin { get; set; }

        internal bool HasStyles { get; set; }

        internal bool IsApp { get; set; }

        internal float FlashVersion
        {
            get { return _flashVersion; }
            set { _flashVersion = value; }
        }
        private float _flashVersion = -1;

        internal bool Ordered;

		/// <summary>
        /// Adds trait to instance or class trait collection.
        /// </summary>
        /// <param name="trait">trait to add</param>
        /// <param name="isStatic">true to add to class trais; false - to instance traits</param>
        public void AddTrait(AbcTrait trait, bool isStatic)
        {
            if (trait == null)
                throw new ArgumentNullException("trait");
            //Note: traits for static members are contained within ABC file in array of AbcClass
            if (isStatic)
                _class.Traits.Add(trait);
            else
                _traits.Add(trait);
        }

        public AbcNamespace GetPrivateNamespace()
        {
            return Abc.DefinePrivateNamespace(this);
        }

        public AbcMultiname DefinePrivateName(string name)
        {
            var ns = GetPrivateNamespace();
            return Abc.DefineQName(ns, name);
        }

        public AbcTrait GetStaticSlot(string name)
        {
            return _class.Traits.Find(name);
        }

        #region CreateSlot
        public AbcTrait CreateSlot(object name, object type, bool isStatic)
        {
            if (name == null)
                throw new ArgumentNullException("name");
            if (type == null)
                throw new ArgumentNullException("type");

            var traitName = Abc.DefineName(name);
            var typeName = Abc.DefineTypeNameSafe(type);

            var trait = AbcTrait.CreateSlot(typeName, traitName);

            AddTrait(trait, isStatic);
            
            return trait;
        }

        public AbcTrait CreateSlot(object name, object type)
        {
            return CreateSlot(name, type, false);
        }

        public AbcTrait CreateStaticSlot(object name, object type)
        {
            return CreateSlot(name, type, true);
        }

        public AbcTrait CreatePrivateSlot(string name, object type)
        {
            var mn = DefinePrivateName(name);
            return CreateSlot(mn, type);
        }

        public AbcTrait CreatePrivateStaticSlot(string name, object type)
        {
            var mn = DefinePrivateName(name);
            return CreateStaticSlot(mn, type);
        }
        #endregion

        #region DefineSlot
        public AbcTrait DefineSlot(object name, object type, bool isStatic)
        {
            if (name == null)
                throw new ArgumentNullException("name");
            if (type == null)
                throw new ArgumentNullException("type");

            var traitName = Abc.DefineName(name);

            var traits = isStatic ? Class.Traits : Traits;
            var trait = traits.Find(traitName, AbcTraitKind.Slot);
            if (trait != null) return trait;

            var typeName = Abc.DefineTypeNameSafe(type);

            trait = AbcTrait.CreateSlot(typeName, traitName);
            traits.Add(trait);

            return trait;
        }

        public AbcTrait DefineSlot(object name, object type)
        {
            return DefineSlot(name, type, false);
        }

        public AbcTrait DefineStaticSlot(object name, object type)
        {
            return DefineSlot(name, type, true);
        }

        public AbcTrait DefinePrivateSlot(string name, object type)
        {
            var mn = DefinePrivateName(name);
            return DefineSlot(mn, type);
        }

        public AbcTrait DefinePrivateStaticSlot(string name, object type)
        {
            var mn = DefinePrivateName(name);
            return DefineStaticSlot(mn, type);
        }
        #endregion

        #region DefineMethod

		internal AbcMethod DefineMethod(Sig sig, AbcCoder coder, Action<AbcMethod> complete)
		{
			if (sig == null)
				throw new ArgumentNullException("sig");

			if (!sig.IsInitilizer && sig.Name == null)
				throw new InvalidOperationException();

			var klass = Class;
			if (klass == null)
				throw new InvalidOperationException(string.Format("Class is not defined yet for Instance {0}", FullName));

			AbcMultiname traitName = null;
			AbcTrait trait;

			bool isStatic = (sig.Semantics & MethodSemantics.Static) != 0;
			var traits = isStatic ? klass.Traits : Traits;

			if (sig.IsInitilizer)
			{
				if (Initializer != null)
					throw new InvalidOperationException();
			}
			else
			{
				traitName = Abc.DefineName(sig.Name);
				trait = traits.Find(traitName, sig.Kind);
				if (trait != null)
				{
					return trait.Method;
				}
			}

			var method = new AbcMethod
				{
					Method = sig.Source
				};

			var generator = Abc.Generator;
			if (sig.Source != null)
			{
				generator.SetData(sig.Source, method);
			}

			AbcMethodBody body = null;
			if (sig.IsAbstract)
			{
				if (coder != null)
					throw new InvalidOperationException();
			}
			else
			{
				body = new AbcMethodBody(method);
			}

			Abc.AddMethod(method);

			if (sig.IsInitilizer)
			{
				Initializer = method;
			}
			else
			{
				//for non initializer method we must define trait and return type
				method.ReturnType = BuildReturnType(sig.ReturnType, method);

				trait = AbcTrait.CreateMethod(method, traitName);
				trait.Kind = sig.Kind;

				if (!isStatic)
				{
					trait.IsVirtual = (sig.Semantics & MethodSemantics.Virtual) != 0;
					trait.IsOverride = (sig.Semantics & MethodSemantics.Override) != 0;
				}

				traits.Add(trait);
			}

			if (sig.Args != null)
			{
				if (sig.Args.Length == 1 && sig.Args[0] is IMethod)
				{
					var m = (IMethod)sig.Args[0];
					if (generator == null)
						throw new InvalidOperationException();
					generator.MethodBuilder.BuildParameters(method, m);
				}
				else
				{
					Abc.AddParameters(method.Parameters, sig.Args);
				}
			}

			if (body != null && coder != null)
			{
				var code = new AbcCode(Abc);
				coder(code);
				body.Finish(code);
			}

			if (complete != null)
			{
				complete(method);
			}

			return method;
		}

		internal AbcMethod DefineMethod(Sig sig, AbcCoder coder)
		{
			return DefineMethod(sig, coder, null);
		}

		internal AbcMethod DefineNotImplementedMethod(IMethod method)
		{
			var generator = Abc.Generator;
			return DefineMethod(
				generator.MethodBuilder.SigOf(method),
				code =>
					{
						var exceptionType = generator.Corlib.GetType(CorlibTypeId.NotImplementedException);
						code.ThrowException(exceptionType);

						//TODO: Is it needed???
						if (method.IsVoid())
						{
							code.ReturnVoid();
						}
						else
						{
							code.PushNull();
							code.ReturnValue();
						}
					});
		}

		private AbcMultiname BuildReturnType(object returnType, AbcMethod method)
		{
			if (returnType == null)
			{
				return Abc.DefineTypeName(AvmTypeCode.Void);
			}

			var generator = Abc.Generator;

			var source = returnType as IMethod;
			if (source != null)
			{
				return generator.MethodBuilder.BuildReturnType(method, source);
			}

			var type = returnType as IType;
			if (type != null)
			{
				return generator.TypeBuilder.BuildReturnType(type);
			}

			return Abc.DefineTypeNameSafe(returnType);
		}

		#endregion

		public void Read(SwfReader reader)
        {
            Name = reader.ReadMultiname();
            BaseTypeName = reader.ReadMultiname();
            Flags = (AbcClassFlags)reader.ReadUInt8();

            if ((Flags & AbcClassFlags.ProtectedNamespace) != 0)
            {
                ProtectedNamespace = reader.ReadAbcNamespace();
            }

            int ifaceCount = (int)reader.ReadUIntEncoded();
            for (int i = 0; i < ifaceCount; ++i)
            {
                var iface = reader.ReadMultiname();
                _interfaces.Add(iface);
            }

            Initializer = reader.ReadAbcMethod();

            _traits.Read(reader);
        }

        public void Write(SwfWriter writer)
        {
            writer.WriteUIntEncoded((uint)Name.Index);

            if (BaseTypeName == null) writer.WriteUInt8(0);
            else writer.WriteUIntEncoded((uint)BaseTypeName.Index);

            writer.WriteUInt8((byte)Flags);

            if ((Flags & AbcClassFlags.ProtectedNamespace) != 0)
            {
                writer.WriteUIntEncoded((uint)ProtectedNamespace.Index);
            }

            int n = _interfaces.Count;
            writer.WriteUIntEncoded((uint)n);
            for (int i = 0; i < n; ++i)
            {
                var iface = _interfaces[i];
                writer.WriteUIntEncoded((uint)iface.Index);
            }

            writer.WriteUIntEncoded((uint)_initializer.Index);
            _traits.Write(writer);
        }

		internal bool IsDumped;

        public void DumpXml(XmlWriter writer)
        {
            if (AbcDumpService.FilterClass(this)) return;
            IsDumped = true;

            writer.WriteStartElement("instance");
            writer.WriteAttributeString("index", Index.ToString());

            if (Name != null)
                writer.WriteAttributeString("name", Name.FullName);

            //if (_superName != null)
            //    writer.WriteAttributeString("supername", _superName.FullName);
            //if (_name != null)
            //    writer.WriteElementString("name", _name.ToString());

            if (BaseTypeName != null)
                writer.WriteElementString("super", BaseTypeName.ToString());

            writer.WriteElementString("flags", Flags.ToString());

            if ((Flags & AbcClassFlags.ProtectedNamespace) != 0 && ProtectedNamespace != null)
            {
                writer.WriteStartElement("protectedNamespace");
                writer.WriteAttributeString("name", ProtectedNamespace.Name.Value);
                writer.WriteAttributeString("kind", ProtectedNamespace.Kind.ToString());
                writer.WriteEndElement();
            }

            if (_interfaces.Count > 0)
            {
                writer.WriteStartElement("interfaces");
                foreach (var i in _interfaces)
                {
                    writer.WriteStartElement("interface");
                    writer.WriteAttributeString("name", i.ToString());
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
            }

            if (_initializer != null)
            {
                if (AbcDumpService.DumpInitializerCode)
                {
                    bool old = AbcDumpService.DumpCode;
                    AbcDumpService.DumpCode = true;
                    _initializer.DumpXml(writer, "iinit");
                    AbcDumpService.DumpCode = old;
                }
                else
                {
                    writer.WriteElementString("iinit", _initializer.ToString());
                }
            }

            _traits.DumpXml(writer);

            if (_class != null)
                _class.DumpXml(writer);

            writer.WriteEndElement();
        }

		public void DumpDirectory(string dir)
        {
            string name = NameString;
            string ns = NamespaceString;
            if (!string.IsNullOrEmpty(ns))
            {
                dir = Path.Combine(dir, ns);
                Directory.CreateDirectory(dir);
            }
            string path = Path.Combine(dir, name + ".cs");
            using (var writer = new StreamWriter(path))
                Dump(writer, true);
        }

        private static void DumpMembers(TextWriter writer, AbcTraitCollection traits, string tab, bool isStatic)
        {
            string prefix = isStatic ? "Static" : "Instance";

            bool eol = false;
            var fields = traits.GetFields();
            if (fields != null && fields.Length > 0)
            {
                writer.WriteLine("{0}#region {1} Fields", tab, prefix);
                for (int i = 0; i < fields.Length; ++i)
                {
                    if (i > 0) writer.WriteLine();
                    fields[i].DumpField(writer, tab, isStatic);
                }
                writer.WriteLine("{0}#endregion", tab);
                eol = true;
            }

            var props = traits.GetProperties();
            if (props.Count > 0)
            {
                if (eol) writer.WriteLine();
                writer.WriteLine("{0}#region {1} Properties", tab, prefix);
                props.Dump(writer, tab, isStatic);
                writer.WriteLine("{0}#endregion", tab);
                eol = true;
            }

            var methods = traits.GetMethods();
            if (methods != null && methods.Length > 0)
            {
                if (eol) writer.WriteLine();
                writer.WriteLine("{0}#region {1} Methods", tab, prefix);
                int n = methods.Length;
                for (int i = 0; i < n; ++i)
                {
                    if (i > 0) writer.WriteLine();
                    methods[i].Dump(writer, tab, isStatic);
                }
                writer.WriteLine("{0}#endregion", tab);
            }
        }

        public void Dump(TextWriter writer, bool withNamespace)
        {
            var tab = new Indent();
            if (withNamespace)
            {
                writer.WriteLine("namespace {0}", NamespaceString);
                writer.WriteLine("{");
                tab++;
            }

            #region def
            writer.Write(tab.Value);
            writer.Write("public ");
            if ((Flags & AbcClassFlags.Final) != 0)
                writer.Write("final ");
            if ((Flags & AbcClassFlags.Sealed) != 0)
                writer.Write("sealed ");

            writer.Write("{0} {1}", IsInterface ? "interface" : "class", NameString);

            int ifaceNum = _interfaces.Count;
            if (BaseTypeName != null || ifaceNum > 0)
                writer.Write(" : ");

            if (BaseTypeName != null)
            {
                writer.Write(BaseTypeName.NameString);
                if (ifaceNum > 0)
                    writer.Write(", ");
            }
            for (int i = 0; i < ifaceNum; ++i)
            {
                if (i > 0) writer.Write(", ");
                writer.Write(_interfaces[i].NameString);
            }
            writer.WriteLine();
            #endregion

            writer.WriteLine("{0}{{", tab.Value);

            tab++;
            DumpMembers(writer, _traits, tab, false);
            if (_class != null)
                DumpMembers(writer, _class.Traits, tab, true);
            --tab;

            writer.WriteLine("{0}}}", tab);

            if (withNamespace)
                writer.WriteLine("}");
        }

		public override string ToString()
        {
            if (Name != null)
                return Name.ToString();
            return base.ToString();
        }

		#region Internal Properties
        internal List<AbcInstance> Implementations
        {
            get { return _impls ?? (_impls = new List<AbcInstance>()); }
        }
        private List<AbcInstance> _impls;

        internal List<AbcInstance> Implements
        {
            get { return _ifaces ?? (_ifaces = new List<AbcInstance>()); }
        }
        private List<AbcInstance> _ifaces;

        internal List<AbcInstance> Subclasses
        {
            get { return _subclasses ?? (_subclasses = new List<AbcInstance>()); }
        }
        private List<AbcInstance> _subclasses;

        internal List<AbcFile> ImportAbcFiles
        {
            get { return _importAbcFiles ?? (_importAbcFiles = new List<AbcFile>()); }
        }
        private List<AbcFile> _importAbcFiles;

        public AbcScript Script
        {
            get
            {
                if (_class != null)
                {
                    var t = _class.Trait;
                    if (t != null)
                        return t.Owner as AbcScript;
                }
                return null;
            }
        }
        #endregion

        #region Trait Cache
        private void CacheTraits()
        {
            if (_traitCache != null) return;
            _traitCache = new AbcTraitCache();
            _traitCache.AddRange(GetAllTraits());
        }
        private AbcTraitCache _traitCache;

        internal AbcTrait FindTrait(ITypeMember member)
        {
            CacheTraits();
            return _traitCache.Find(member);
        }
        #endregion

        #region Utils
        internal bool IsInheritedFrom(AbcMultiname typename)
        {
            if (ReferenceEquals(BaseTypeName, typename)) return true;
        	return _interfaces.Any(iface => ReferenceEquals(iface, typename));
        }

        internal bool IsTypeUsed(AbcMultiname typename)
        {
            foreach (var t in GetAllTraits())
            {
                switch (t.Kind)
                {
                    case AbcTraitKind.Function:
                    case AbcTraitKind.Getter:
                    case AbcTraitKind.Setter:
                    case AbcTraitKind.Method:
                        {
                            var m = t.Method;
                            if (m.IsTypeUsed(typename))
                                return true;
                        }
                        break;

                    case AbcTraitKind.Const:
                    case AbcTraitKind.Slot:
                        {
                            if (ReferenceEquals(t.SlotType, typename))
                                return true;
                        }
                        break;
                }
            }
            return false;
        }

        public AbcTrait FindSuperTrait(AbcMultiname name, AbcTraitKind kind)
        {
            var st = BaseInstance;
            while (st != null)
            {
                var t = st.Traits.Find(name, kind);
                if (t != null) return t;
                st = st.BaseInstance;
            }
            return null;
        }
        #endregion
    }
}