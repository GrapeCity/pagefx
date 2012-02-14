using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.CodeModel;
using DataDynamics.PageFX.FLI.IL;

namespace DataDynamics.PageFX.FLI.ABC
{
    using AbcString = AbcConst<string>;

    //contains Define API
    public partial class AbcFile
    {
        #region IsDefined
        /// <summary>
        /// Determines whether the given <see cref="AbcInstance"/> is defined in this ABC file.
        /// </summary>
        /// <param name="instance"><see cref="AbcInstance"/> to check</param>
        /// <returns></returns>
        public bool IsDefined(AbcInstance instance)
        {
        	return instance.IsNative || AllFrames.Any(abc => instance.ABC == abc);
        }

    	/// <summary>
        /// Determines whether the given <see cref="AbcMultiname"/> is defined in this ABC file.
        /// </summary>
        /// <param name="name">name to check</param>
        /// <returns></returns>
        public bool IsDefined(AbcMultiname name)
        {
            int index = name.Index;
            if (index < 0 || index >= _multinames.Count)
                return false;
            if (_multinames[index] != name)
                return false;
            return true;
        }

        /// <summary>
        /// Determines whether the given <see cref="AbcMethod"/> is defined in this ABC file.
        /// </summary>
        /// <param name="method">method to check</param>
        /// <returns></returns>
        public bool IsDefined(AbcMethod method)
        {
            int index = method.Index;
            if (index < 0 || index >= _methods.Count)
                return false;
            if (_methods[index] != method)
                return false;
            return true;
        }

        /// <summary>
        /// Determines whether the given <see cref="AbcClass"/> is defined in this ABC file.
        /// </summary>
        /// <param name="klass">class to check</param>
        /// <returns></returns>
        public bool IsDefined(AbcClass klass)
        {
            return _classes.IsDefined(klass);
        }

        /// <summary>
        /// Determines whether the given <see cref="AbcScript"/> is defined in this ABC file.
        /// </summary>
        /// <param name="script">script to check</param>
        /// <returns></returns>
        public bool IsDefined(AbcScript script)
        {
            return _scripts.IsDefined(script);
        }

        public bool IsDefined(AbcTrait trait)
        {
            var instance = trait.Owner as AbcInstance;
            if (instance != null)
                return IsDefined(instance);
            var klass = trait.Owner as AbcClass;
            if (klass != null)
                return IsDefined(klass);
            var script = trait.Owner as AbcScript;
            if (script != null)
                return IsDefined(script);
            return false;
        }

        //public bool IsDefined(AbcMethodBody body)
        //{
        //    int index = body.Index;
        //    if (index < 0 || index >= _methods.Count)
        //        return false;
        //    if (_methodBodies[index] != body)
        //        return false;
        //    return true;
        //}

        /// <summary>
        /// Determines whether the given type is defined in this ABC file.
        /// </summary>
        /// <param name="type">type to check</param>
        /// <returns></returns>
        public bool IsDefined(IType type)
        {
            var tag = type.Tag;
            if (tag == null) return false;
            if (tag is ISpecialType) return true;
            var instance = tag as AbcInstance;
            if (instance != null)
                return IsDefined(instance);
            var mname = tag as AbcMultiname;
            if (mname != null)
                return IsDefined(mname);
            return false;
        }

        /// <summary>
        /// Determines whether the given method is defined in this ABC file.
        /// </summary>
        /// <param name="method">method to check</param>
        /// <returns></returns>
        public bool IsDefined(IMethod method)
        {
            var tag = method.Tag;
            if (tag == null) return false;

            var code = tag as AbcCode;
            if (code != null)
                //return code.abc == this;
                return true;

            var abcMethod = tag as AbcMethod;
            if (abcMethod != null)
                return IsDefined(abcMethod);

            var name = tag as AbcMultiname;
            if (name != null)
                return IsDefined(name);

            var mn = tag as AbcMemberName;
            if (mn != null)
            {
                if (!IsDefined(mn.Type) || !IsDefined(mn.Name))
                {
                    method.Tag = new AbcMemberName(ImportConst(mn.Type), ImportConst(mn.Name));
                }
                return true;
            }

            return false;
        }

        public bool IsDefined(IField field)
        {
            var tag = field.Tag;
            if (tag == null) return false;
            var trait = tag as AbcTrait;
            if (trait != null)
                return IsDefined(trait);
            var name = tag as AbcMultiname;
            if (name != null)
                return IsDefined(name);
            return false;
        }
        #endregion

        #region Constants
        /// <summary>
        /// Defines 32-bit signed integer constant.
        /// </summary>
        /// <param name="value">value of constant to define.</param>
        /// <returns></returns>
        public AbcConst<int> DefineInt(int value)
        {
            return _intPool.Define(value);
        }

        /// <summary>
        /// Defines 32-bit unsigned integer constant.
        /// </summary>
        /// <param name="value">value of constant to define.</param>
        /// <returns></returns>
        public AbcConst<uint> DefineUInt(uint value)
        {
            return _uintPool.Define(value);
        }

        /// <summary>
        /// Defines double constant.
        /// </summary>
        /// <param name="value">value of constant to define.</param>
        /// <returns></returns>
        public AbcConst<double > DefineDouble(double value)
        {
            return _doublePool.Define(value);
        }

        public AbcConst<double> DefineSingle(float value)
        {
            double v = AbcHelper.ToDouble(value);
            return _doublePool.Define(v);
        }

        /// <summary>
        /// Defines string constant.
        /// </summary>
        /// <param name="value">value of constant to define.</param>
        /// <returns></returns>
        public AbcString DefineString(string value)
        {
            if (value == null)
                throw new ArgumentNullException("value");
            return _stringPool.Define(value);
        }

        public AbcString EmptyString
        {
            get { return _emptyString ?? (_emptyString = DefineString("")); }
        }
        private AbcString _emptyString;

        AbcString DefineString2(string value)
        {
            AbcString c = null;
            if (!string.IsNullOrEmpty(value))
                c = DefineString(value);
            return c;
        }
        #endregion

        #region Frequently Used Names in ABC generation
        public AbcMultiname RuntimeQName
        {
            get { return _mnRuntimeQName ?? (_mnRuntimeQName = DefineMultiname(AbcConstKind.RTQNameL)); }
        }
        private AbcMultiname _mnRuntimeQName;

        public AbcMultiname NameArrayIndexer
        {
            get
            {
                if (_mnArrayIndexer == null)
                {
                    var nss = DefineNamespaceSet(GlobalPackage);
                    _mnArrayIndexer = DefineMultiname(AbcConstKind.MultinameL, nss);
                }
                return _mnArrayIndexer;
            }
        }
        private AbcMultiname _mnArrayIndexer;
        #endregion

        #region Known Namespaces
        LazyValue<AbcNamespace>[] KnownNamespaces
        {
            get 
            {
                if (_knownNamespaces != null)
                    return _knownNamespaces;
                return _knownNamespaces = new[]
                {
                    new LazyValue<AbcNamespace>(() => DefinePackage(EmptyString)), //Global
                    new LazyValue<AbcNamespace>(() => DefineInternalNamespace(EmptyString)), //Internal
                    new LazyValue<AbcNamespace>(() => DefineInternalNamespace(EmptyString)), //BodyTrait
                    new LazyValue<AbcNamespace>(() => DefinePublicNamespace(AS3.NS2006)), //AS3
                    new LazyValue<AbcNamespace>(() => DefinePublicNamespace(MX.NamespaceInternal2006)), //MXInternal
                    new LazyValue<AbcNamespace>(() => DefinePackage(Const.Namespaces.PFX)), //PfxPackage
                    new LazyValue<AbcNamespace>(() => DefinePublicNamespace(Const.Namespaces.PFX)), //PfxPublic
                };
            }
        }
        LazyValue<AbcNamespace>[] _knownNamespaces;

        internal AbcNamespace DefineNamespace(KnownNamespace id)
        {
            return KnownNamespaces[(int)id].Value;
        }

        /// <summary>
        /// Returns global package namespace.
        /// </summary>
        public AbcNamespace GlobalPackage
        {
            get { return DefineNamespace(KnownNamespace.Global); }
        }

        /// <summary>
        /// Gets empty internal namespace
        /// </summary>
        public AbcNamespace InternalNamespace
        {
            get { return DefineNamespace(KnownNamespace.Internal); }
        }

        /// <summary>
        /// Gets namespace that is used for method body activation traits.
        /// </summary>
        public AbcNamespace BodyTraitNamespace
        {
            get { return DefineNamespace(KnownNamespace.BodyTrait); }
        }

        /// <summary>
        /// Returns MX internal namespace.
        /// </summary>
        public AbcNamespace NamespaceMxInternal
        {
            get { return DefineNamespace(KnownNamespace.MxInternal); }
        }

        public AbcNamespace NamespaceAS3
        {
            get { return DefineNamespace(KnownNamespace.AS3); }
        }

        public AbcNamespace PfxPackage
        {
            get { return DefineNamespace(KnownNamespace.PfxPackage); }
        }

        public AbcNamespace PfxPublicNamespace
        {
            get { return DefineNamespace(KnownNamespace.PfxPublic); }
        }
        #endregion

        #region Namespaces
        /// <summary>
        /// Defines namespace with given name.
        /// </summary>
        /// <param name="kind">kind of namespace to define.</param>
        /// <param name="name">name of namespace to define.</param>
        /// <returns></returns>
        public AbcNamespace DefineNamespace(AbcConstKind kind, AbcString name)
        {
            string key = AbcNamespace.KeyOf(name, kind);
            var ns = _nspool[key];
            if (ns != null) return ns;
            ns = new AbcNamespace(name, kind, key);
            _nspool.Add(ns);
            return ns;
        }

        /// <summary>
        /// Defines namespace with given name.
        /// </summary>
        /// <param name="kind"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public AbcNamespace DefineNamespace(AbcConstKind kind, string name)
        {
            var c = _stringPool.Define(name);
            return DefineNamespace(kind, c);
        }

        /// <summary>
        /// Defines public namespace with given name.
        /// </summary>
        /// <param name="name">name of namespace to define.</param>
        /// <returns></returns>
        public AbcNamespace DefinePublicNamespace(string name)
        {
            return DefineNamespace(AbcConstKind.PublicNamespace, name);
        }

        /// <summary>
        /// Defines public namespace with given name.
        /// </summary>
        /// <param name="name">name of namespace to define.</param>
        /// <returns></returns>
        public AbcNamespace DefinePublicNamespace(AbcString name)
        {
            return DefineNamespace(AbcConstKind.PublicNamespace, name);
        }

        /// <summary>
        /// Defines protected namespace with given name.
        /// </summary>
        /// <param name="name">name of namespace to define.</param>
        /// <returns></returns>
        public AbcNamespace DefineProtectedNamespace(string name)
        {
            return DefineNamespace(AbcConstKind.ProtectedNamespace, name);
        }

        /// <summary>
        /// Defines protected namespace with given name.
        /// </summary>
        /// <param name="name">name of namespace to define.</param>
        /// <returns></returns>
        public AbcNamespace DefineProtectedNamespace(AbcString name)
        {
            return DefineNamespace(AbcConstKind.ProtectedNamespace, name);
        }

        /// <summary>
        /// Defines internal namespace with given name.
        /// </summary>
        /// <param name="name">name of namespace to define.</param>
        /// <returns></returns>
        public AbcNamespace DefineInternalNamespace(string name)
        {
            return DefineNamespace(AbcConstKind.InternalNamespace, name);
        }

        /// <summary>
        /// Defines internal namespace with given name.
        /// </summary>
        /// <param name="name">name of namespace to define.</param>
        /// <returns></returns>
        public AbcNamespace DefineInternalNamespace(AbcString name)
        {
            return DefineNamespace(AbcConstKind.InternalNamespace, name);
        }

        /// <summary>
        /// Defines namespace with given name.
        /// </summary>
        /// <param name="name">name of namespace to define.</param>
        /// <returns></returns>
        public AbcNamespace DefinePrivateNamespace(string name)
        {
            return DefineNamespace(AbcConstKind.PrivateNamespace, name);
        }

        public AbcNamespace DefinePrivateNamespace(AbcInstance instance)
        {
            string ns = TypeHelper.GetNamespaceForMembers(instance);
            return DefinePrivateNamespace(ns);
        }

        /// <summary>
        /// Defines package namespace with given name.
        /// </summary>
        /// <param name="name">name of namespace to define.</param>
        /// <returns></returns>
        public AbcNamespace DefinePackage(string name)
        {
            return DefineNamespace(AbcConstKind.PackageNamespace, name);
        }

        /// <summary>
        /// Defines package namespace with given name.
        /// </summary>
        /// <param name="name">name of namespace to define.</param>
        /// <returns></returns>
        public AbcNamespace DefinePackage(AbcString name)
        {
            return DefineNamespace(AbcConstKind.PackageNamespace, name);
        }

        /// <summary>
        /// Defines namespace.
        /// </summary>
        /// <param name="ns">name of namespace to define.</param>
        /// <param name="v"></param>
        /// <param name="isStatic"></param>
        /// <returns></returns>
        public AbcNamespace DefineNamespace(string ns, Visibility v, bool isStatic)
        {
            switch (v)
            {
                case Visibility.NestedPrivate:
                case Visibility.Private:
                case Visibility.PrivateScope:
                    return DefineNamespace(AbcConstKind.PrivateNamespace, ns);

                case Visibility.NestedProtected:
                case Visibility.Protected:
                    if (isStatic)
                        return DefineNamespace(AbcConstKind.StaticProtectedNamespace, ns);
                    return DefineNamespace(AbcConstKind.ProtectedNamespace, ns);

                case Visibility.NestedProtectedInternal:
                case Visibility.ProtectedInternal:
                    return DefineNamespace(AbcConstKind.ProtectedNamespace, ns);

                case Visibility.NestedInternal:
                case Visibility.Internal:
                    return DefineNamespace(AbcConstKind.InternalNamespace, ns);

                default:
                    return GlobalPackage;
            }
        }

        public AbcNamespace DefineNamespace(string ns, Visibility v)
        {
            return DefineNamespace(ns, v, false);
        }

    	static string GetNsName(Visibility v)
        {
            switch (v)
            {
                case Visibility.PrivateScope:
                case Visibility.NestedPrivate:
                case Visibility.Private:
                    return "private";

                case Visibility.NestedProtected:
                case Visibility.NestedProtectedInternal:
                case Visibility.Protected:
                case Visibility.ProtectedInternal:
                    return "protected";

                case Visibility.NestedInternal:
                case Visibility.Internal:
                    return "internal";

                default:
                    return "";
            }
        }

        public AbcNamespace DefineNamespace(IType type, Visibility v, bool isStatic)
        {
            if (type == null)
                throw new ArgumentNullException();

            if (type.IsInterface)
                return GlobalPackage;

            //FIX: For internal override methods.
            string ns = GetNsName(v);
            //string ns = "";
            //if (!IsInternal(v))
            //    ns = TypeHelper.GetNamespaceForMembers(type);

            return DefineNamespace(ns, v, isStatic);
        }

        public AbcNamespace DefineNamespace(IType type, Visibility v)
        {
            return DefineNamespace(type, v, false);
        }

        /// <summary>
        /// Defines namespace for given type member.
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public AbcNamespace DefineNamespace(ITypeMember m)
        {
            return DefineNamespace(m.DeclaringType, m.Visibility, m.IsStatic);
        }
        #endregion

        #region Namespace Sets
        public AbcNamespaceSet DefineNamespaceSet(params AbcNamespace[] list)
        {
            string key = AbcNamespaceSet.KeyOf(list);
            var nss = _nssets[key];
            if (nss != null) return nss;
            nss = new AbcNamespaceSet(list, key);
            _nssets.Add(nss);
            return nss;
        }

    	public AbcMultiname DefineMultiname(AbcConstKind kind, AbcNamespace ns, AbcString name)
        {
            string key = AbcMultiname.KeyOf(kind, ns, name);
            var mn = _multinames[key];
            if (mn != null) return mn;
            mn = new AbcMultiname(kind, ns, name) {key = key};
            _multinames.Add(mn);
            return mn;
        }
        #endregion

        #region Multinames
        public AbcMultiname DefineMultiname(AbcConstKind kind, AbcNamespace ns, string name)
        {
            var s = DefineString(name);
            return DefineMultiname(kind, ns, s);
        }

        public AbcMultiname DefineMultiname(AbcConstKind kind, AbcNamespaceSet nss, AbcString name)
        {
            string key = AbcMultiname.KeyOf(kind, nss, name);
            var mn = _multinames[key];
            if (mn != null) return mn;
            mn = new AbcMultiname(kind, nss, name) {key = key};
            _multinames.Add(mn);
            return mn;
        }

        public AbcMultiname DefineMultiname(AbcConstKind kind, AbcNamespaceSet nss, string name)
        {
            var c = DefineString(name);
            return DefineMultiname(kind, nss, c);
        }

        public AbcMultiname DefineMultiname(AbcConstKind kind, AbcNamespaceSet nss)
        {
            string key = AbcMultiname.KeyOf(kind, nss);
            var mn = _multinames[key];
            if (mn != null) return mn;
            mn = new AbcMultiname(kind, nss) {key = key};
            _multinames.Add(mn);
            return mn;
        }

        public AbcMultiname DefineMultiname(AbcConstKind kind)
        {
            return _multinames.Define(kind);
        }
        #endregion

        #region QNames
        /// <summary>
        /// Defines qname.
        /// </summary>
        /// <param name="ns">namespace of qname to define</param>
        /// <param name="name">name of qname to define</param>
        /// <returns></returns>
        public AbcMultiname DefineQName(AbcNamespace ns, string name)
        {
            var s = DefineString(name);
            return DefineMultiname(AbcConstKind.QName, ns, s);
        }

        public AbcMultiname DefineQName(string ns, string name)
        {
            if (ns == null)
                throw new ArgumentNullException("ns");
            if (name == null)
                throw new ArgumentNullException("name");
            if (name.Length == 0)
                throw new ArgumentException("name cannot be empty");
            return DefineQName(DefinePackage(ns), name);
        }

        public AbcMultiname DefineQName(ITypeMember m, string name)
        {
            var ns = DefineNamespace(m);
            return DefineQName(ns, name);
        }

        public AbcMultiname DefineQName(IType type, string name, Visibility v, bool isStatic)
        {
            var ns = DefineNamespace(type, v, isStatic);
            return DefineQName(ns, name);
        }

        public AbcMultiname DefineQName(AbcConstKind nskind, string ns, string name)
        {
            var n = DefineNamespace(nskind, ns);
            return DefineQName(n, name);
        }

        /// <summary>
        /// Defines QName with public namespace
        /// </summary>
        /// <param name="ns">QName public namespace to define</param>
        /// <param name="name">QName name to define</param>
        /// <returns></returns>
        public AbcMultiname DefinePublicQName(string ns, string name)
        {
            return DefineQName(AbcConstKind.PublicNamespace, ns, name);
        }

        public AbcMultiname DefinePackageQName(string ns, string name)
        {
            return DefineQName(AbcConstKind.PackageNamespace, ns, name);
        }

        public AbcMultiname DefineQName(AbcConstKind nskind, string fullname)
        {
            int i = fullname.LastIndexOf('.');
            if (i >= 0)
            {
                string ns = fullname.Substring(0, i);
                string name = fullname.Substring(i + 1);
                return DefineQName(nskind, ns, name);
            }
            return DefineQName(nskind, "", fullname);
        }

        /// <summary>
        /// Defines qname with global package namespace
        /// </summary>
        /// <param name="name">name of qname to define</param>
        /// <returns></returns>
        public AbcMultiname DefineGlobalQName(string name)
        {
            return DefineQName(GlobalPackage, name);
        }

        public AbcMultiname DefineMxInternalName(string name)
        {
            return DefineQName(NamespaceMxInternal, name);
        }

        public AbcMultiname DefineAS3Name(string name)
        {
            return DefineQName(NamespaceAS3, name);
        }
        #endregion

        #region DefineName
        public AbcMultiname DefineName(object name)
        {
            var mn = name as AbcMultiname;
            if (mn != null)
                return ImportConst(mn);

            var s = name as string;
            if (s != null)
                return DefineGlobalQName(s);

            var kqn = name as KnownQName;
            if (kqn != null)
                return kqn.Define(this);

            throw new NotSupportedException("invalid name object");
        }
        #endregion

        #region Vector
        public AbcMultiname VectorTypeName
        {
            get
            {
                if (_nameVector == null)
                {
                    var ns = DefineNamespace(AbcConstKind.PackageNamespace, AS3.Vector.Namespace);
                    _nameVector = DefineQName(ns, AS3.Vector.Name);
                }
                return _nameVector;
            }
        }
        AbcMultiname _nameVector;

        public AbcMultiname DefineVectorTypeName(AbcMultiname param)
        {
            var mn = new AbcMultiname(VectorTypeName, param);
            return ImportConst(mn);
        }
        #endregion

        #region Builtin Types
        public AbcMultiname this[AvmTypeCode type]
        {
            get
            {
                return BuiltinTypes[type];
            }
        }

        public AvmBuiltinTypes BuiltinTypes
        {
            get { return _avmBuiltinTypes ?? (_avmBuiltinTypes = new AvmBuiltinTypes(this)); }
        }
        AvmBuiltinTypes _avmBuiltinTypes;

        public AbcMultiname GetTypeName(IType type, bool native)
        {
            if (type.TypeKind == TypeKind.Reference)
                return BuiltinTypes.Object;

            var st = type.SystemType;
            if (st != null)
            {
                switch (st.Code)
                {
                    case SystemTypeCode.Void:
                        return BuiltinTypes.Void;

                    case SystemTypeCode.String:
                        if (AbcGenConfig.UseAvmString)
                            return BuiltinTypes.String;
                        break;
                }
            }
            
            if (native)
            {
                if (type.TypeKind == TypeKind.Enum)
                {
                    if (type.ValueType == null)
                        throw new InvalidOperationException("Invalid enum type");
                    return GetTypeName(type.ValueType, native);
                }

                if (st != null)
                {
                    switch (st.Code)
                    {
                        case SystemTypeCode.Boolean:
                            return BuiltinTypes.RealBoolean;

                        case SystemTypeCode.Int8:
                            if (AvmConfig.SupportSmallIntegers)
                                return BuiltinTypes.Int8;
                            return BuiltinTypes.Int32;

                        case SystemTypeCode.Int16:
                            if (AvmConfig.SupportSmallIntegers)
                                return BuiltinTypes.Int16;
                            return BuiltinTypes.Int32;

                        case SystemTypeCode.Int32:
                        case SystemTypeCode.IntPtr:
                            return BuiltinTypes.Int32;

                        case SystemTypeCode.UInt8:
                            if (AvmConfig.SupportSmallIntegers)
                                return BuiltinTypes.UInt8;
                            return BuiltinTypes.UInt32;

                        case SystemTypeCode.UInt16:
                            if (AvmConfig.SupportSmallIntegers)
                                return BuiltinTypes.UInt16;
                            return BuiltinTypes.UInt32;

                        case SystemTypeCode.Char:
                            if (AvmConfig.SupportSmallIntegers)
                                return BuiltinTypes.UInt16;
                            return BuiltinTypes.UInt32;

                        case SystemTypeCode.UInt32:
                        case SystemTypeCode.UIntPtr:
                            return BuiltinTypes.UInt32;

                        case SystemTypeCode.Int64:
                            if (AvmConfig.SupportNativeInt64)
                                return BuiltinTypes.Int64;
                            break;
                        
                        case SystemTypeCode.UInt64:
                            if (AvmConfig.SupportNativeInt64)
                                return BuiltinTypes.UInt64;
                            break;
                        
                        case SystemTypeCode.Single:
                        case SystemTypeCode.Double:
                            return BuiltinTypes.Number;

                        case SystemTypeCode.Object:
                            return BuiltinTypes.Object;
                    }
                }

                if (TypeHelper.UseNativeObject(type))
                    return BuiltinTypes.Object;
            }

            var mn = TypeHelper.GetTypeMultiname(type);
            if (mn == null)
                throw new InvalidOperationException("Type is not defined yet");

            return mn;
        }
        #endregion

        #region PageFX Names
        public AbcMultiname DefinePfxName(string name)
        {
            return DefineQName(PfxPublicNamespace, name);
        }

        public AbcMultiname PtrValueName
        {
            get
            {
                if (_mnPtrValue != null) return _mnPtrValue;
                _mnPtrValue = DefinePackageQName("$ptr", "value");
                return _mnPtrValue;
            }
        }
        AbcMultiname _mnPtrValue;
        #endregion

        #region Method Definitions
        #region DefineEmptyMethod
        public AbcMethod DefineEmptyMethod(bool pushThisScope)
        {
            var method = new AbcMethod();
            var body = new AbcMethodBody(method);
            var code = new AbcCode(this);
            if (pushThisScope)
                code.PushThisScope();
            code.ReturnVoid();
            body.Finish(code);
            AddMethod(method);
            return method;
        }

        public AbcMethod DefineEmptyMethod()
        {
            return DefineEmptyMethod(false);
        }
        #endregion

        #region DefineEmptyVoidMethod
        public AbcMethod DefineEmptyVoidMethod(AbcString name, bool pushScope)
        {
            var method = new AbcMethod
                             {
                                 ReturnType = BuiltinTypes.Void,
                                 Name = name
                             };

            var body = new AbcMethodBody(method);
            var code = new AbcCode(this);
            if (pushScope)
                code.PushThisScope();
            code.ReturnVoid();
            body.Finish(code);

            AddMethod(method);

            return method;
        }

        public AbcMethod DefineEmptyVoidMethod()
        {
            return DefineEmptyVoidMethod((AbcString)null, false);
        }

        public AbcMethod DefineEmptyVoidMethod(string name, bool pushScope)
        {
            return DefineEmptyVoidMethod(DefineString2(name), pushScope);
        }

        public AbcMethod DefineEmptyVoidMethod(AbcString name)
        {
            return DefineEmptyVoidMethod(name, false);
        }

        public AbcMethod DefineEmptyVoidMethod(string name)
        {
            return DefineEmptyVoidMethod(DefineString2(name), false);
        }
        #endregion

        #region DefineEmptyConstructor
        public AbcMethod DefineEmptyConstructor(AbcString name, bool pushScope)
        {
            var method = new AbcMethod
                             {
                                 ReturnType = BuiltinTypes.Void,
                                 Name = name
                             };

            var body = new AbcMethodBody(method);
            var code = new AbcCode(this);

            if (pushScope)
                code.PushThisScope();
            code.ConstructSuper();
            code.ReturnVoid();
            body.Finish(code);

            AddMethod(method);

            return method;
        }

        public AbcMethod DefineEmptyConstructor(string name, bool pushScope)
        {
            return DefineEmptyConstructor(DefineString2(name), pushScope);
        }

        public AbcMethod DefineEmptyConstructor(AbcString name)
        {
            return DefineEmptyConstructor(name, false);
        }

        public AbcMethod DefineEmptyConstructor(string name)
        {
            return DefineEmptyConstructor(DefineString2(name), false);
        }

        public AbcMethod DefineEmptyConstructor()
        {
            return DefineEmptyConstructor((AbcString)null, false);
        }
        #endregion

        #region DefineEmptyAbstractMethod
        //Used to define static initializer for interfaces
        public AbcMethod DefineEmptyAbstractMethod()
        {
            var method = new AbcMethod();
            _methods.Add(method);
            return method;
        }
        #endregion      

        #region DefineMethod
        internal AbcMethod DefineMethod(object retType, AbcCoder coder, params object[] args)
        {
            AbcMultiname returnType = null;
            
            if (retType != null)
                returnType = DefineTypeNameStrict(retType);

            var method = new AbcMethod { ReturnType = returnType };

            if (args != null)
            {
                DefineParams(method.Parameters, args);
            }

            var body = new AbcMethodBody(method);

            AddMethod(method);

            if (coder != null)
            {
                var code = new AbcCode(this);
                coder(code);
                body.Finish(code);
            }

            return method;
        }

        internal AbcMethod DefineMethod(AbcMethod sig, AbcCoder coder)
        {
            return DefineMethod(sig.ReturnType, coder, sig);
        }

        internal AbcMethod DefineVoidMethod(AbcCoder coder, params object[] args)
        {
            return DefineMethod(AvmTypeCode.Void, coder, args);
        }

        internal AbcMethod DefineInitializer(AbcCoder coder, params object[] args)
        {
            return DefineMethod(null, coder, args);
        }
        #endregion

        #region DefineTraitsInitializer
        /// <summary>
        /// Defines instance initializer which init given traits.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public AbcMethod DefineTraitsInitializer(params object[] args)
        {
            if (args == null)
                throw new ArgumentNullException("args");

            var method = new AbcMethod
                             {
                                 ReturnType = this[AvmTypeCode.Object]
                             };

            var traits = new List<AbcTrait>();
            int n = args.Length;
            for (int i = 0; i < n; ++i)
            {
                var t = args[i] as AbcTrait;
                if (t == null)
                    throw new InvalidOperationException();
                if (!t.IsSlot)
                    throw new InvalidOperationException();

                AbcConst<string> name = null;
                if (i + 1 < n)
                {
                    string s = args[i + 1] as string;
                    if (s != null)
                    {
                        name = DefineString(s);
                        ++i;
                    }
                }

                method.AddParam(t.SlotType, name);

                traits.Add(t);
            }

            var body = new AbcMethodBody(method);

            AddMethod(method);

            var code = new AbcCode(this);

            code.PushThisScope();

            code.ConstructSuper();

            n = traits.Count;
            for (int i = 0; i < n; ++i)
            {
                code.LoadThis();
                code.GetLocal(i + 1);
                code.SetProperty(traits[i]);
            }

            code.ReturnVoid();
            body.Finish(code);

            return method;
        }
        #endregion
        #endregion

        #region DefineScript
        public void DefineScriptInit(AbcScript script)
        {
            script.Initializer = DefineMethod(
                AvmTypeCode.Void,
                delegate(AbcCode code)
                    {
                        code.PushThisScope();
                        code.InitClassProperties(script);
                        code.ReturnVoid();
                    }
                );
        }

        public void DefineScript(IEnumerable<AbcInstance> instances, Predicate<AbcInstance> filter)
        {
            var script = new AbcScript();
            _scripts.Add(script);
            script.DefineClassTraits(instances, filter);
            DefineScriptInit(script);
        }

        public void DefineScript(params AbcInstance[] instances)
        {
            DefineScript(instances, null);
        }

        public void DefineEmptyScript()
        {
            var script = new AbcScript {Initializer = DefineEmptyMethod()};
            _scripts.Add(script);
        }

        public void DefineScripts(IEnumerable<AbcInstance> instances)
        {
            var arr = new AbcInstance[1];
            foreach (var instance in instances)
            {
                arr[0] = instance;
                DefineScript(arr);
            }
        }
        #endregion

        #region DefineParam
        public AbcParameter DefineParam(AbcMultiname type, string name)
        {
            return new AbcParameter(type, DefineString2(name));
        }

        public AbcParameter DefineParam(AbcInstance type, string name)
        {
            return DefineParam(type.Name, name);
        }

        public AbcParameter DefineParam(AvmTypeCode type, string name)
        {
            return new AbcParameter(this[type], DefineString2(name));
        }

        public AbcParameter DefineParam(AbcMultiname type)
        {
            return new AbcParameter(type);
        }

        public AbcParameter DefineParam(AbcInstance type)
        {
            return DefineParam(type.Name);
        }

        public AbcParameter DefineParam(AvmTypeCode type)
        {
            return new AbcParameter(this[type]);
        }

        AbcConst<string> GetParamName(object[] args, ref int i)
        {
            if (i + 1 < args.Length)
            {
                var arg = args[i + 1];
                string s = arg as string;
                if (s != null)
                {
                    if (s.Length == 0)
                        throw new InvalidOperationException("empty arg name");
                    ++i;
                    return DefineString(s);
                }
                var c = arg as AbcConst<string>;
                if (c != null)
                {
                    ++i;
                    return c;
                }
            }
            return null;
        }

        public AbcMultiname DefineTypeName(object type)
        {
            var typeName = type as AbcMultiname;
            if (typeName != null)
                return typeName;

            if (type is AvmTypeCode)
            {
                return this[(AvmTypeCode)type];
            }

            var instance = type as AbcInstance;
            if (instance != null)
                return instance.Name;

            var t = type as IType;
            if (t != null)
            {
                typeName = generator.DefineMemberType(t);
                return typeName;
            }

            var trait = type as AbcTrait;
            if (trait != null)
            {
                if (trait.IsField)
                    return trait.SlotType;
                if (trait.IsMethod)
                {
                    var m = trait.Method;
                    if (m != null)
                        return m.ReturnType;
                }
            }

            return null;
        }

        public AbcMultiname DefineTypeNameStrict(object type)
        {
            var mn = DefineTypeName(type);
            if (mn == null)
                throw new InvalidOperationException("invalid type object");
            return mn;
        }

        public void DefineParams(AbcParameterList list, params object[] args)
        {
            if (args == null) return;
            int n = args.Length;
            for (int i = 0; i < n; ++i)
            {
                var arg = args[i];

                var typeName = DefineTypeName(arg);
                if (typeName != null)
                {
                    var name = GetParamName(args, ref i);
                    list.Add(new AbcParameter(typeName, name));
                    continue;
                }

                var m = arg as AbcMethod;
                if (m != null)
                {
                    list.CopyFrom(m);
                    continue;
                }

                var pl = arg as AbcParameterList;
                if (pl != null)
                {
                    list.CopyFrom(pl);
                    continue;
                }

                var p = arg as AbcParameter;
                if (p != null)
                {
                    list.Add(ImportParam(p));
                    continue;
                }

                throw new NotImplementedException();
            }
        }

        public AbcParameter ImportParam(AbcParameter p)
        {
            var type = ImportConst(p.Type);
            var name = ImportConst(p.Name);
            var np = new AbcParameter(type, name);
            if (p.IsOptional)
            {
                np.IsOptional = true;
                np.Value = ImportValue(p.Value);
            }
            return np;
        }

        public AbcParametersHandler DefineParams(params object[] args)
        {
            if (args == null) return null;
            return pl => DefineParams(pl, args);
        }
        #endregion

        public AbcTrait CreateSlot(object type, object name)
        {
            var typeName = DefineTypeNameStrict(type);
            var mn = DefineName(name);
            return AbcTrait.CreateSlot(typeName, mn);
        }

        #region DefineEmptyInstance
        public AbcInstance DefineEmptyInstance(object name, bool emptyCtor)
        {
            var instance = new AbcInstance(true)
            {
                Name = DefineName(name),
                SuperName = this[AvmTypeCode.Object],
                Flags = AbcClassFlags.Final | AbcClassFlags.Sealed
            };

            if (emptyCtor)
                instance.Initializer = DefineEmptyConstructor();

            instance.Class.Initializer = DefineEmptyMethod();

            AddInstance(instance);

            return instance;
        }
        #endregion

        #region DefineNamespaceScript
        readonly Hashtable _nsscripts = new Hashtable();

        public AbcScript DefineNamespaceScript(AbcNamespace ns, string name)
        {
            if (ns == null)
                throw new ArgumentNullException("ns");
            var script = _nsscripts[name] as AbcScript;
            if (script != null) return script;
            ns = ImportConst(ns);

            var traitName = DefineGlobalQName(name);
            script = new AbcScript
                         {
                             Initializer = DefineEmptyMethod()
                         };
            _nsscripts[name] = script;

            script.Traits.AddConst(this[AvmTypeCode.Namespace], traitName, ns);

            return script;
        }
        #endregion
    }
}