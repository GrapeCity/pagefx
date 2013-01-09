using System;
using System.Collections.Generic;
using System.Diagnostics;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.FlashLand.Abc;
using DataDynamics.PageFX.FlashLand.Core.SpecialTypes;
using DataDynamics.PageFX.FlashLand.Core.Tools;

namespace DataDynamics.PageFX.FlashLand.Core.CodeGeneration
{
    //contains creation of AbcInstance for given IType.
    //key methods: DefineType
    partial class AbcGenerator
    {
        public AbcMultiname DefineTypeName(IType type)
        {
            if (type == null) return null;
            var obj = DefineType(type);
            return obj.ToMultiname();
        }

		public AbcMultiname DefineMemberType(IType type)
		{
			DefineType(type);
			if (type.Data == null)
				throw new InvalidOperationException(string.Format("Type {0} is not defined", type.FullName));
			return Abc.GetTypeName(type, true);
		}

        #region DefineType
        /// <summary>
        /// Defines given type in generated ABC file
        /// </summary>
        /// <param name="type">the type to define</param>
        /// <returns>tag associated with given type</returns>
        public object DefineType(IType type)
        {
            if (type == null) return null;

            var abcSubject = type.Data as IAbcFileSubject;
            if (abcSubject != null)
            {
                abcSubject.ByteCode = Abc;
                return abcSubject;
            }

            if (Abc.IsDefined(type))
                return type.Data;

            var tag = ImportType(type);

            bool isImported = false;
            if (tag == null)
            {
                if (type.Data != null)
                    throw new InvalidOperationException();
                tag = DefineTypeCore(type);
            }
            else
            {
                isImported = true;
            }

            if (tag != null)
            {
                SetData(type, tag);

                RegisterType(type);

                if (!isImported)
                    DefineMembers(type);
            }

            abcSubject = type.Data as IAbcFileSubject;
            if (abcSubject != null)
                abcSubject.ByteCode = Abc;

            return type.Data;
        }
        #endregion

        #region RegisterType
        void RegisterType(IType type)
        {
            if (type.IsModuleType())
                return;

            switch (type.TypeKind)
            {
                case TypeKind.Pointer:
                    throw new NotSupportedException();

                case TypeKind.Reference:
                    return;
            }

            Reflection.GetTypeId(type);
        }
        #endregion

        #region DefineAbcInstance
        /// <summary>
        /// Defines <see cref="AbcInstance"/> for given <see cref="IType"/>.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public AbcInstance DefineAbcInstance(IType type)
        {
            if (type == null)
                throw new ArgumentNullException("type");
            var res = DefineType(type) as AbcInstance;
            if (res == null)
                throw new InvalidOperationException(string.Format("Unable to define AbcInstance for type: {0}", type));
            return res;
        }
        #endregion

        #region DefineBaseTypes
        void DefineBaseTypes(IType type)
        {
            if (type == null) return;

            DefineType(type.BaseType);

            if (type.Interfaces != null)
            {
                foreach (var ifaceType in type.Interfaces)
                    DefineType(ifaceType);
            }
        }
        #endregion

        #region ImportType
        object ImportType(IType type)
        {
            var tag = type.Data;
            if (tag != null)
            {
                var instance = tag as AbcInstance;
                if (instance != null)
                {
                    DefineBaseTypes(type);
                    return Abc.ImportInstance(instance);
                }

                var name = tag as AbcMultiname;
                if (name != null)
                    return Abc.ImportConst(name);
            }
            return null;
        }
        #endregion

        #region DefineTypeCore
        object DefineTypeCore(IType type)
        {
            if (type.CanExclude())
                return null;

            switch (type.TypeKind)
            {
                case TypeKind.Interface:
                case TypeKind.Class:
                case TypeKind.Struct:
                case TypeKind.Primitive:
                case TypeKind.Delegate:
                    return DefineUserType(type);

                case TypeKind.GenericParameter:
                    return null;

                case TypeKind.Enum:
                    DefineType(type.ValueType);
                    return DefineUserType(type);

                case TypeKind.Array:
                    return DefineArrayType((IArrayType)type);

                case TypeKind.Pointer:
                    throw new NotSupportedException();

                case TypeKind.Reference:
                    {
                        var ctype = (ICompoundType)type;
                        DefineType(ctype.ElementType);
                        return Abc.BuiltinTypes.Object;
                    }
            }

            return null;
        }
        #endregion

        #region DefineUserType
        object DefineUserType(IType type)
        {
            if (LinkVectorInstance(type))
                return type.Data;

            //NOTE: can be used only in typeof operations
            if (type is IGenericType)
                return null;

            if (GenericType.HasGenericParams(type))
                throw new InvalidOperationException();

            AbcMultiname superName;
            AbcInstance superType;
            DefineSuperType(type, out superName, out superType);

            //NOTE: Fix for enums.
            if (Abc.IsDefined(type))
                return type.Data;

#if DEBUG
            DebugService.DoCancel();
            DebugService.LogInfo("DefineUserType started for {0}", type.FullName);
#endif
            var ifaceNames = DefineInterfaces(type);

            if (Abc.IsDefined(type))
                return type.AbcInstance();

            var name = DefineInstanceName(type);

            var instance = new AbcInstance(true)
                               {
                                   Type = type,
                                   Name = name,
                                   BaseTypeName = superName,
                                   BaseInstance = superType
                               };

            SetData(type, instance);
			SetFlags(instance, type);
            AddInterfaces(instance, type, ifaceNames);

	        Abc.AddInstance(instance);

	        if (IsRootSprite(type))
                RootSprite.Instance = instance;

            DefineDebugInfo(type, instance);

#if DEBUG
            DebugService.DoCancel();
            DebugService.LogInfo("DefineUserType succeeded for {0}", type.FullName);
#endif

            return instance;
        }

	    bool IsRootSprite(IType type)
        {
            if (SwfCompiler != null && !string.IsNullOrEmpty(SwfCompiler.RootSprite))
                return type.FullName == SwfCompiler.RootSprite;
            return type.IsRootSprite();
        }

        void SetFlags(AbcInstance instance, IType type)
        {
            if (type.TypeKind == TypeKind.Interface)
            {
                instance.Flags |= AbcClassFlags.Sealed | AbcClassFlags.Interface;
                //instance.Flags |= AbcClassFlags.Interface;
            }
            else
            {
                if (type.IsSealed)
                    instance.Flags |= AbcClassFlags.Final;
                instance.Flags |= AbcClassFlags.Sealed;

                if (type.HasProtectedNamespace())
                {
                    instance.Flags |= AbcClassFlags.ProtectedNamespace;
                    instance.ProtectedNamespace = Abc.DefineProtectedNamespace(instance.NameString);
                }
            }
        }

        //Defines qname for AbcInstance for given type
        AbcMultiname DefineInstanceName(IType type)
        {
            var ns = DefineTypeNamespace(type);
            string name = InternalTypeExtensions.GetPartialTypeName(type);
            return Abc.DefineQName(ns, name);
        }

        AbcNamespace DefineTypeNamespace(IType type)
        {
            string ns = type.GetTypeNamespace(RootNamespace);
            return Abc.DefineNamespace(AbcConstKind.PackageNamespace, ns);
        }
        #endregion

        #region DefineSuperType
		//TODO: avoid usage of out parameters
        private void DefineSuperType(IType type, out AbcMultiname superName, out AbcInstance superType)
        {
            superName = null;
            superType = null;

            if (type.IsInterface) return;

            if (type.IsEnum)
            {
                superType = DefineEnumSuperType(type);
                superName = superType.Name;
                return;
            }

            if (type.Is(SystemTypeCode.Exception))
            {
                superType = type.Assembly.Corlib().CustomData().ErrorInstance;
                superName = Abc.BuiltinTypes.Error;
                return;
            }

            var baseType = type.BaseType;
            if (baseType != null)
            {
                //NOTE: This fix explicit usage of Avm.Object as base class.
                //In fact .NET developer will never use this class, or no need to use this class.
                if (baseType.IsAvmObject())
                    baseType = SystemTypes.Object;

                superName = DefineTypeName(baseType);
                superType = baseType.AbcInstance();
				return;
            }

	        if (type.Is(SystemTypeCode.Object))
	        {
		        superName = Abc.BuiltinTypes.Object;
	        }
        }

        readonly AbcInstance[] _enumSuperTypes = new AbcInstance[8];

        static int GetEnumIndex(SystemType st)
        {
            switch (st.Code)
            {
                case SystemTypeCode.Int8: return 0;
                case SystemTypeCode.UInt8: return 1;
                case SystemTypeCode.Int16: return 2;
                case SystemTypeCode.UInt16: return 3;
                case SystemTypeCode.Int32: return 4;
                case SystemTypeCode.UInt32: return 5;
                case SystemTypeCode.Int64: return 6;
                case SystemTypeCode.UInt64: return 7;
                default:
                    throw new ArgumentOutOfRangeException("st", "invalid enum value type");
            }
        }

        static string GetEnumName(SystemType st)
        {
            switch (st.Code)
            {
                case SystemTypeCode.Int8:
                    return "EnumI1";
                case SystemTypeCode.UInt8:
                    return "EnumU1";
                case SystemTypeCode.Int16:
                    return "EnumI2";
                case SystemTypeCode.UInt16:
                    return "EnumU2";
                case SystemTypeCode.Int32:
                    return "EnumI4";
                case SystemTypeCode.UInt32:
                    return "EnumU4";
                case SystemTypeCode.Int64:
                    return "EnumI8";
                case SystemTypeCode.UInt64:
                    return "EnumU8";
                default:
                    throw new ArgumentOutOfRangeException("st", "invalid enum value type");
            }
        }

        private AbcInstance DefineEnumSuperType(IType type)
        {
            if (!type.IsEnum)
                throw new InvalidOperationException("type is not enum");
            var vtype = type.ValueType;
            var st = vtype.SystemType();
            if (st == null)
                throw new InvalidOperationException(string.Format("invalid enum type {0}", type.FullName));
            int index = GetEnumIndex(st);

            var instance = _enumSuperTypes[index];
            if (instance != null) return instance;

            var super = DefineAbcInstance(type.BaseType);

            instance = _enumSuperTypes[index];
            if (instance != null) return instance;

            instance = new AbcInstance(true)
                           {
                               Name = DefinePfxName(GetEnumName(st), false),
                               BaseTypeName = super.Name,
                               BaseInstance = super,
                               Initializer = DefineDefaultInitializer(null)
                           };
	        Abc.AddInstance(instance);

	        instance.Class.Initializer = Abc.DefineEmptyMethod();

            _enumSuperTypes[index] = instance;

            instance.CreateSlot(Abc.DefineGlobalQName(Const.Boxing.Value), DefineMemberType(vtype));

            //SetFlags(instance, type);

            return instance;
        }
        #endregion

        #region DefineInterfaces
        private static bool OnlyDeclareInterfaces(IType type)
        {
            if (type.Is(SystemTypeCode.String))
                return true;
            return false;
        }

        private IList<AbcMultiname> DefineInterfaces(IType type)
        {
            if (type.Interfaces == null) return null;
            int n = type.Interfaces.Count;
            if (n <= 0) return null;

            bool onlyDecl = OnlyDeclareInterfaces(type);

            List<AbcMultiname> list = null;
            if (!onlyDecl)
                list = new List<AbcMultiname>();

            for (int i = 0; i < n; ++i)
            {
                var iface = type.Interfaces[i];
#if DEBUG
                DebugService.DoCancel();
#endif
                var ifaceName = DefineTypeName(iface);
                if (ifaceName == null)
                    throw new InvalidOperationException(string.Format("Unable to define interface {0}", iface.FullName));

                if (!onlyDecl)
                    list.Add(ifaceName);
            }

            return list;
        }

        static void AddInterfaces(AbcInstance instance, IType type, IList<AbcMultiname> ifaceNames)
        {
            if (ifaceNames == null) return;
            if (type.Interfaces == null) return;
            int n = type.Interfaces.Count;
            for (int i = 0; i < n; ++i)
            {
#if DEBUG
                DebugService.DoCancel();
#endif
                var iface = type.Interfaces[i];
                var ifaceName = ifaceNames[i];

                var ifaceInstance = iface.AbcInstance();
                if (ifaceInstance != null)
                {
                    ifaceInstance.Implementations.Add(instance);
                    instance.Implements.Add(ifaceInstance);
                }
                instance.Interfaces.Add(ifaceName);
            }
        }
        #endregion

        #region DefineArrayType
        private object DefineArrayType(IArrayType type)
        {
            var arr = DefineType(SystemTypes.Array);
            var elemType = type.ElementType;
            DefineType(elemType);
            return arr;
        }
        #endregion

        #region FinishTypes, FinishType
        void FinishType(AbcInstance instance)
        {
            //TODO: Comment when copying of value types will be implemented using Reflection
            DefineCopyMethod(instance);

            DefineCompiledMethods(instance);
        }

        void FinishTypes()
        {
#if PERF
            int start = Environment.TickCount;
#endif
            for (int i = 0; i < Abc.Instances.Count; ++i)
            {
#if DEBUG
                DebugService.DoCancel();
#endif
                var instance = Abc.Instances[i];
                FinishType(instance);
            }
#if PERF
            Console.WriteLine("AbcGen.FinishTypes: {0}", Environment.TickCount - start);
#endif
        }
        #endregion

        #region DefineMembers
        private void DefineMembers(IType type)
        {
            var instance = type.AbcInstance();
            if (instance == null) return;
            if (instance.IsForeign) return;
            if (type.IsArray) return;

            //NOTE:
            //For array types we define only System.Array.
            //Therefore in order to correctly define memebers
            //we should use System.Array instead of SomeType[].
            if (!ReferenceEquals(instance.Type, type))
                type = instance.Type;

            if (!type.IsInterface)
            {
                DefineFlexAppMembers(instance);
                DefineFields(type);
            }

            EnshureInitializers(instance);

            //DefineCompiledMethods(type);
            DefineExposedMethods(type);
        }

        private bool MustDefine(IField field)
        {
            //Note: constants and static fields will be defined by demand
            var declType = field.DeclaringType;
            if (declType.IsEnum) return true;
            if (field.IsConstant) return false;
            if (field.HasEmbedAttribute()) return true;
            if (GenericType.HasGenericParams(field.Type)) return false;
            if (field.IsExposed()) return true;
            if (Mode == AbcGenMode.Full) return true;
            if (field.IsStatic) return false;
            return true;
        }

        private void DefineFields(IType type)
        {
			//TODO: PERF do this only once
            foreach (var field in type.Fields)
            {
#if DEBUG
                DebugService.DoCancel();
#endif
                if (MustDefine(field))
                    FieldBuilder.Build(field);
            }
        }

        private void DefineExposedMethods(IType type)
        {
            foreach (var method in new List<IMethod>(type.Methods))
            {
                if (method.IsInternalCall) continue;
                if (GenericType.IsGenericContext(method)) continue;
                if (Mode == AbcGenMode.Full || method.IsExposed())
                    DefineMethod(method);
            }
        }

	    #endregion

        #region DefineCompiledMethods
        private void DefineCompiledMethods(AbcInstance instance)
        {
            var type = instance.Type;
            if (type != null)
                DefineCompiledMethods(instance, type);
        }

        private void DefineCompiledMethods(AbcInstance instance, IType type)
        {
            if (instance == null)
				throw new ArgumentNullException("instance");
	        if (type == null)
				throw new ArgumentNullException("type");

	        ImplementArrayInterface(type);

            //Define Compiled Interface Methods
            foreach (var iface in instance.Implements)
            {
#if DEBUG
                DebugService.DoCancel();
#endif
                DefineCompiledMethods(instance, type, iface);
            }

            //Define Override Methods for already Compiled Base Methods
            var super = instance.BaseInstance;
            while (super != null)
            {
#if DEBUG
                DebugService.DoCancel();
#endif
                DefineCompiledMethods(instance, type, super);
                super = super.BaseInstance;
            }
        }

		/// <summary>
		/// Compiles override methods of specified base/interface type.
		/// </summary>
		/// <param name="instance"></param>
		/// <param name="type"></param>
		/// <param name="super">Base type or interface.</param>
        private void DefineCompiledMethods(AbcInstance instance, IType type, AbcInstance super)
        {
            if (type.Is(SystemTypeCode.Exception)) return;

            //NOTE: super.Traits.Count can be changed during execution
            for (int i = 0; i < super.Traits.Count; ++i)
            {
#if DEBUG
                DebugService.DoCancel();
#endif
                var trait = super.Traits[i];
                if (!trait.IsMethod) continue;

                var abcMethod = trait.Method;
                var method = abcMethod.SourceMethod;
                if (method == null) continue;
                if (method.IsStatic) continue;
                if (method.IsConstructor) continue;

                if (super.IsInterface)
                {
                    DefineImplementation(instance, type, method, abcMethod);
                }
                else
                {
                    if (method.IsVirtual || method.IsAbstract)
                        DefineOverrideMethod(type, method);
                }
            }
        }
        #endregion

        #region DefineCtorStaticCall
        /// <summary>
        /// Defines static method to call this ctor.
        /// </summary>
        /// <param name="ctor">this ctor</param>
        /// <returns></returns>
        public AbcMethod DefineCtorStaticCall(IMethod ctor)
        {
            Debug.Assert(ctor.IsConstructor);
            Debug.Assert(!ctor.IsStatic);

            var thisCtor = DefineAbcMethod(ctor);
            if (thisCtor.IsInitializer)
                throw new InvalidOperationException("ctor is initializer");

            var declType = ctor.DeclaringType;
            var instance = DefineAbcInstance(declType);

            return instance.DefineMethod(
                Sig.@static(thisCtor.TraitName.NameString + "__static", instance.Name, thisCtor),
                code =>
                    {
                        code.PushThisScope();
                        code.CreateInstance(instance);
                        code.Dup();
                        code.LoadArguments(ctor);
                        code.Call(thisCtor);
                        code.ReturnValue();
                    });
        }
        #endregion

        #region Utils
        void DefineTypes(IEnumerable<IType> types)
        {
            if (types == null) return;
            foreach (var type in types)
            {
#if DEBUG
                DebugService.DoCancel();
#endif
                DefineType(type);
            }
        }

        private bool LinkVectorInstance(IType type)
        {
            var instance = type as IGenericInstance;
            if (instance == null) return false;

            if (!instance.Type.HasAttribute(Attrs.GenericVector))
                return false;

            DefineTypes(instance.GenericArguments);

            SetData(type, new VectorInstance(type));

            return true;
        }
        #endregion
    }
}