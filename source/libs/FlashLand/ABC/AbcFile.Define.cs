using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.Common.Extensions;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.FlashLand.Avm;
using DataDynamics.PageFX.FlashLand.Core;
using DataDynamics.PageFX.FlashLand.Core.SpecialTypes;
using DataDynamics.PageFX.FlashLand.IL;

namespace DataDynamics.PageFX.FlashLand.Abc
{
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
        	return instance.IsNative || AllFrames.Any(abc => instance.Abc == abc);
        }

    	/// <summary>
        /// Determines whether the given <see cref="AbcMultiname"/> is defined in this ABC file.
        /// </summary>
        /// <param name="name">name to check</param>
        /// <returns></returns>
        public bool IsDefined(AbcMultiname name)
    	{
    		if (name == null) return false;
            int index = name.Index;
            if (index < 0 || index >= Multinames.Count) return false;
            return ReferenceEquals(Multinames[index], name);
        }

        /// <summary>
        /// Determines whether the given <see cref="AbcMethod"/> is defined in this ABC file.
        /// </summary>
        /// <param name="method">method to check</param>
        /// <returns></returns>
        public bool IsDefined(AbcMethod method)
        {
            int index = method.Index;
            if (index < 0 || index >= Methods.Count) return false;
            return ReferenceEquals(Methods[index], method);
        }

        /// <summary>
        /// Determines whether the given <see cref="AbcClass"/> is defined in this ABC file.
        /// </summary>
        /// <param name="klass">class to check</param>
        /// <returns></returns>
        public bool IsDefined(AbcClass klass)
        {
            return Classes.IsDefined(klass);
        }

        /// <summary>
        /// Determines whether the given <see cref="AbcScript"/> is defined in this ABC file.
        /// </summary>
        /// <param name="script">script to check</param>
        /// <returns></returns>
        public bool IsDefined(AbcScript script)
        {
            return Scripts.IsDefined(script);
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

        /// <summary>
        /// Determines whether the given type is defined in this ABC file.
        /// </summary>
        /// <param name="type">type to check</param>
        /// <returns></returns>
        public bool IsDefined(IType type)
        {
            var tag = type.Data;
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
            var tag = method.Data;
            if (tag == null) return false;
	        if (tag is InlineCall) return true;

            var abcMethod = tag as AbcMethod;
	        if (abcMethod != null)
	        {
		        return IsDefined(abcMethod);
	        }

	        var call = tag as IMethodCall;
			return call != null ? IsDefined(call.Name) : IsDefined(tag as AbcMultiname);
        }

        public bool IsDefined(IField field)
        {
            var tag = field.Data;
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
            return IntPool.Define(value);
        }

        /// <summary>
        /// Defines 32-bit unsigned integer constant.
        /// </summary>
        /// <param name="value">value of constant to define.</param>
        /// <returns></returns>
        public AbcConst<uint> DefineUInt(uint value)
        {
            return UIntPool.Define(value);
        }

        /// <summary>
        /// Defines double constant.
        /// </summary>
        /// <param name="value">value of constant to define.</param>
        /// <returns></returns>
        public AbcConst<double > DefineDouble(double value)
        {
            return DoublePool.Define(value);
        }

        public AbcConst<double> DefineSingle(float value)
        {
            double v = value.ToDoublePrecisely();
            return DoublePool.Define(v);
        }

        /// <summary>
        /// Defines string constant.
        /// </summary>
        /// <param name="value">value of constant to define.</param>
        /// <returns></returns>
        public AbcConst<string> DefineString(string value)
        {
            if (value == null)
                throw new ArgumentNullException("value");
            return StringPool.Define(value);
        }

        public AbcConst<string> EmptyString
        {
            get { return _emptyString ?? (_emptyString = DefineString("")); }
        }
        private AbcConst<string> _emptyString;

        private AbcConst<string> DefineStringOrNull(string value)
        {
            AbcConst<string> c = null;
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
                    var nss = DefineNamespaceSet(KnownNamespaces.GlobalPackage);
                    _mnArrayIndexer = DefineMultiname(AbcConstKind.MultinameL, nss);
                }
                return _mnArrayIndexer;
            }
        }
        private AbcMultiname _mnArrayIndexer;
        #endregion

        #region Known Namespaces

		internal KnownNamespaces KnownNamespaces
        {
            get 
            {
                if (_knownNamespaces == null)
					_knownNamespaces = new KnownNamespaces(this);
                return _knownNamespaces;
            }
        }
		private KnownNamespaces _knownNamespaces;

	    #endregion

        #region Namespaces
        /// <summary>
        /// Defines namespace with given name.
        /// </summary>
        /// <param name="kind">kind of namespace to define.</param>
        /// <param name="name">name of namespace to define.</param>
        /// <returns></returns>
        public AbcNamespace DefineNamespace(AbcConstKind kind, AbcConst<string> name)
        {
            string key = name.MakeKey(kind);
            var ns = Namespaces[key];
            if (ns != null) return ns;
            ns = new AbcNamespace(name, kind, key);
            Namespaces.Add(ns);
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
            var c = StringPool.Define(name);
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
        public AbcNamespace DefinePublicNamespace(AbcConst<string> name)
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
        public AbcNamespace DefineProtectedNamespace(AbcConst<string> name)
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
        public AbcNamespace DefineInternalNamespace(AbcConst<string> name)
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
            string ns = InternalTypeExtensions.GetNamespaceForMembers(instance);
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
        public AbcNamespace DefinePackage(AbcConst<string> name)
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
                    return KnownNamespaces.GlobalPackage;
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
                return KnownNamespaces.GlobalPackage;

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
            var nss = NamespaceSets[key];
            if (nss != null) return nss;
            nss = new AbcNamespaceSet(list, key);
            NamespaceSets.Add(nss);
            return nss;
        }

    	public AbcMultiname DefineMultiname(AbcConstKind kind, AbcNamespace ns, AbcConst<string> name)
        {
            string key = AbcMultiname.KeyOf(kind, ns, name);
            var mn = Multinames[key];
            if (mn != null) return mn;
            mn = new AbcMultiname(kind, ns, name) {Key = key};
            Multinames.Add(mn);
            return mn;
        }
        #endregion

        #region Multinames
        public AbcMultiname DefineMultiname(AbcConstKind kind, AbcNamespace ns, string name)
        {
            var s = DefineString(name);
            return DefineMultiname(kind, ns, s);
        }

        public AbcMultiname DefineMultiname(AbcConstKind kind, AbcNamespaceSet nss, AbcConst<string> name)
        {
            string key = AbcMultiname.KeyOf(kind, nss, name);
            var mn = Multinames[key];
            if (mn != null) return mn;
            mn = new AbcMultiname(kind, nss, name) {Key = key};
            Multinames.Add(mn);
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
            var mn = Multinames[key];
            if (mn != null) return mn;
            mn = new AbcMultiname(kind, nss) {Key = key};
            Multinames.Add(mn);
            return mn;
        }

        public AbcMultiname DefineMultiname(AbcConstKind kind)
        {
            return Multinames.Define(kind);
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

	    public AbcMultiname DefineQName(ITypeMember m, string name)
        {
            var ns = DefineNamespace(m);
            return DefineQName(ns, name);
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
            {
				return QName.Global(s).Define(this);
            }

            var qn = name as QName;
	        if (qn != null)
	        {
		        return qn.Define(this);
	        }

	        throw new NotSupportedException("invalid name object");
        }
        #endregion

        #region Vector

	    public AbcMultiname DefineVectorTypeName(AbcMultiname param)
        {
            var mn = new AbcMultiname(BuiltinTypes.Vector, param);
            return ImportConst(mn);
        }

        #endregion

        #region Builtin Types

	    public BuiltinTypes BuiltinTypes
        {
            get { return _builtinTypes ?? (_builtinTypes = new BuiltinTypes(this)); }
        }
        private BuiltinTypes _builtinTypes;

        public AbcMultiname GetTypeName(IType type, bool native)
        {
            if (type.TypeKind == TypeKind.Reference)
                return BuiltinTypes.Object;

            var st = type.SystemType();
            if (st != null)
            {
                switch (st.Code)
                {
                    case SystemTypeCode.Void:
                        return BuiltinTypes.Void;

                    case SystemTypeCode.String:
                        return BuiltinTypes.String;
                }
            }
            
            if (native)
            {
                if (type.TypeKind == TypeKind.Enum)
                {
                    if (type.ValueType == null)
                        throw new InvalidOperationException("Invalid enum type");
                    return GetTypeName(type.ValueType, true);
                }

                if (st != null)
                {
                    switch (st.Code)
                    {
                        case SystemTypeCode.Boolean:
                            return BuiltinTypes.RealBoolean;

                        case SystemTypeCode.Int8:
                            return AvmConfig.SupportSmallIntegers ? BuiltinTypes.Int8 : BuiltinTypes.Int32;

	                    case SystemTypeCode.Int16:
                            return AvmConfig.SupportSmallIntegers ? BuiltinTypes.Int16 : BuiltinTypes.Int32;

	                    case SystemTypeCode.Int32:
                        case SystemTypeCode.IntPtr:
                            return BuiltinTypes.Int32;

                        case SystemTypeCode.UInt8:
                            return AvmConfig.SupportSmallIntegers ? BuiltinTypes.UInt8 : BuiltinTypes.UInt32;

	                    case SystemTypeCode.UInt16:
                            return AvmConfig.SupportSmallIntegers ? BuiltinTypes.UInt16 : BuiltinTypes.UInt32;

	                    case SystemTypeCode.Char:
                            return AvmConfig.SupportSmallIntegers ? BuiltinTypes.UInt16 : BuiltinTypes.UInt32;

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

                if (type.UseNativeObject())
                    return BuiltinTypes.Object;
            }

            var mn = type.GetMultiname();
            if (mn == null)
                throw new InvalidOperationException("Type is not defined yet");

            return mn;
        }
        #endregion

	    #region Method Definitions

	    //Used to define static initializer for interfaces
        public AbcMethod DefineEmptyAbstractMethod()
        {
            var method = new AbcMethod();
            Methods.Add(method);
            return method;
        }

	    internal AbcMethod DefineMethod(Sig sig, AbcCoder coder)
	    {
		    var method = new AbcMethod
			    {
				    ReturnType = sig.ReturnType != null
					                 ? DefineTypeNameSafe(sig.ReturnType)
					                 : null
			    };

		    if (sig.Args != null)
		    {
			    AddParameters(method.Parameters, sig.Args);
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

		public AbcMethod DefineEmptyMethod()
		{
			return DefineMethod(
				Sig.global(null),
				code => code.ReturnVoid());
		}

	    /// <summary>
        /// Defines instance initializer which init given traits.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public AbcMethod DefineTraitsInitializer(params object[] args)
        {
            var pairs = args
				.Pairwise(x => x is string ? 1 : 0)
				.Select(x =>
				{
					var t = x.First as AbcTrait;
					if (t == null)
						throw new InvalidOperationException();
					if (!t.IsSlot)
						throw new InvalidOperationException();
					return new KeyValuePair<AbcTrait, string>(t, x.Second as string);
				})
				.ToList();

			var traits = pairs.Select(x => x.Key).ToList();

	        return DefineMethod(
		        Sig.global(AvmTypeCode.Void, args),
		        code =>
			        {
				        code.PushThisScope();

				        code.ConstructSuper();

						for (int i = 0; i < traits.Count; ++i)
				        {
					        code.LoadThis();
					        code.GetLocal(i + 1);
					        code.SetProperty(traits[i]);
				        }

				        code.ReturnVoid();
			        }
		        );
        }

	    #endregion

        #region DefineScript
        public void DefineScriptInit(AbcScript script)
        {
	        script.Initializer = DefineMethod(
		        Sig.@void(),
		        code =>
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
            Scripts.Add(script);
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
            Scripts.Add(script);
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

        #region Parameters

	    public AbcParameter CreateParameter(object typeDef, string name)
        {
	        if (typeDef == null)
				throw new ArgumentNullException("typeDef");

	        var typeName = DefineTypeName(typeDef);

	        return new AbcParameter(typeName, DefineStringOrNull(name));
        }

	    private AbcConst<string> GetParamName(object[] args, ref int i)
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

        public AbcMultiname DefineTypeName(object typeDef)
        {
            var name = typeDef as AbcMultiname;
            if (name != null)
                return name;

            if (typeDef is AvmTypeCode)
            {
                return BuiltinTypes[(AvmTypeCode)typeDef];
            }

            var instance = typeDef as AbcInstance;
            if (instance != null)
                return instance.Name;

            var type = typeDef as IType;
            if (type != null)
            {
                name = Generator.TypeBuilder.BuildMemberType(type);
                return name;
            }

            var trait = typeDef as AbcTrait;
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

        public AbcMultiname DefineTypeNameSafe(object type)
        {
            var mn = DefineTypeName(type);
            if (mn == null)
                throw new InvalidOperationException("invalid type object");
            return mn;
        }

        public void AddParameters(AbcParameterList list, params object[] args)
        {
            if (args == null) return;
            int n = args.Length;
            for (int i = 0; i < n; ++i)
            {
                var arg = args[i];

				var p = arg as AbcParameter;
				if (p != null)
				{
					list.Add(ImportParam(p));
					continue;
				}

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

	    #endregion

        public AbcTrait CreateSlot(object type, object name)
        {
            var typeName = DefineTypeNameSafe(type);
            var mn = DefineName(name);
            return AbcTrait.CreateSlot(typeName, mn);
        }

        #region DefineEmptyInstance
        public AbcInstance DefineEmptyInstance(object name, bool emptyCtor)
        {
	        var instance = new AbcInstance(true)
		        {
			        Name = DefineName(name),
			        BaseTypeName = BuiltinTypes.Object,
			        Flags = AbcClassFlags.Final | AbcClassFlags.Sealed
		        };

            if (emptyCtor)
            {
	            instance.Initializer = DefineMethod(
		            Sig.@void(),
		            code =>
			            {
				            code.ConstructSuper();
				            code.ReturnVoid();
			            });
            }

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

            var traitName = DefineName(QName.Global(name));
            script = new AbcScript
                         {
                             Initializer = DefineEmptyMethod()
                         };
            _nsscripts[name] = script;

            script.Traits.AddConst(BuiltinTypes.Namespace, traitName, ns);

            return script;
        }
        #endregion
    }
}