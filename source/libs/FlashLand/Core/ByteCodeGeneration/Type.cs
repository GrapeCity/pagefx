using System;
using System.Collections.Generic;
using System.Diagnostics;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.FlashLand.Abc;
using DataDynamics.PageFX.FlashLand.Core.SpecialTypes;
using DataDynamics.PageFX.FlashLand.Core.Tools;

namespace DataDynamics.PageFX.FlashLand.Core.ByteCodeGeneration
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

        #region DefineType
        /// <summary>
        /// Defines given type in generated ABC file
        /// </summary>
        /// <param name="type">the type to define</param>
        /// <returns>tag associated with given type</returns>
        public object DefineType(IType type)
        {
            if (type == null) return null;

            var abcSubject = type.Tag as IAbcFileSubject;
            if (abcSubject != null)
            {
                abcSubject.ByteCode = _abc;
                return abcSubject;
            }

            if (_abc.IsDefined(type))
                return type.Tag;

            var tag = ImportType(type);

            bool isImported = false;
            if (tag == null)
            {
                if (type.Tag != null)
                    throw new InvalidOperationException();
                tag = DefineTypeCore(type);
            }
            else
            {
                isImported = true;
            }

            if (tag != null)
            {
                type.Tag = tag;
                RegisterType(type);

                if (!isImported)
                    DefineMembers(type);
            }

            abcSubject = type.Tag as IAbcFileSubject;
            if (abcSubject != null)
                abcSubject.ByteCode = _abc;

            return type.Tag;
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

            GetTypeId(type);
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
            var tag = type.Tag;
            if (tag != null)
            {
                var instance = tag as AbcInstance;
                if (instance != null)
                {
                    DefineBaseTypes(type);
                    return _abc.ImportInstance(instance);
                }

                var name = tag as AbcMultiname;
                if (name != null)
                    return _abc.ImportConst(name);
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
                        return _abc.BuiltinTypes.Object;
                    }
            }

            return null;
        }
        #endregion

        #region DefineUserType
        object DefineUserType(IType type)
        {
            if (LinkVectorInstance(type))
                return type.Tag;

            //NOTE: can be used only in typeof operations
            if (type is IGenericType)
                return null;

            if (GenericType.HasGenericParams(type))
                throw new InvalidOperationException();

            AbcMultiname superName;
            AbcInstance superType;
            DefineSuperType(type, out superName, out superType);

            //NOTE: Fix for enums.
            if (_abc.IsDefined(type))
                return type.Tag;

#if DEBUG
            DebugService.DoCancel();
            DebugService.LogInfo("DefineUserType started for {0}", type.FullName);
#endif
            var ifaceNames = DefineInterfaces(type);

            if (_abc.IsDefined(type))
                return type.Tag as AbcInstance;

            var name = DefineInstanceName(type);

            var instance = new AbcInstance(true)
                               {
                                   Type = type,
                                   Name = name,
                                   SuperName = superName,
                                   SuperType = superType
                               };

            type.Tag = instance;
            SetFlags(instance, type);
            AddInterfaces(instance, type, ifaceNames);

            AddInstance(instance);

            if (IsRootSprite(type))
                _rootSprite = instance;

            DefineDebugInfo(type, instance);

#if DEBUG
            DebugService.DoCancel();
            DebugService.LogInfo("DefineUserType succeeded for {0}", type.FullName);
#endif

            return instance;
        }

        void AddInstance(AbcInstance instance)
        {
            _abc.AddInstance(instance);
        }

        bool IsRootSprite(IType type)
        {
            if (sfc != null && !string.IsNullOrEmpty(sfc.RootSprite))
                return type.FullName == sfc.RootSprite;
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
                    instance.ProtectedNamespace = _abc.DefineProtectedNamespace(instance.NameString);
                }
            }
        }

        //Defines qname for AbcInstance for given type
        AbcMultiname DefineInstanceName(IType type)
        {
            var ns = DefineTypeNamespace(type);
            string name = InternalTypeExtensions.GetPartialTypeName(type);
            return _abc.DefineQName(ns, name);
        }

        AbcNamespace DefineTypeNamespace(IType type)
        {
            string ns = type.GetTypeNamespace(RootNamespace);
            return _abc.DefineNamespace(AbcConstKind.PackageNamespace, ns);
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
                superName = _abc.BuiltinTypes.Error;
                return;
            }

            var baseType = type.BaseType;
            if (baseType != null)
            {
                //NOTE: This fix explicit usage of Avm.Object as base class.
                //In fact .NET developer will never use this class, or no need to use this class.
                if (baseType.IsAvmObject())
                    baseType = SysTypes.Object;

                superName = DefineTypeName(baseType);
                superType = baseType.Tag as AbcInstance;
				return;
            }

	        if (type.Is(SystemTypeCode.Object))
	        {
		        superName = _abc.BuiltinTypes.Object;
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
                               SuperName = super.Name,
                               SuperType = super,
                               Initializer = DefineDefaultInitializer(null)
                           };
            AddInstance(instance);

            instance.Class.Initializer = _abc.DefineEmptyMethod();

            _enumSuperTypes[index] = instance;

            instance.CreateSlot(_abc.DefineGlobalQName(Const.Boxing.Value), DefineMemberType(vtype));

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

                var ifaceInstance = iface.Tag as AbcInstance;
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
            var arr = DefineType(SysTypes.Array);
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
            for (int i = 0; i < _abc.Instances.Count; ++i)
            {
#if DEBUG
                DebugService.DoCancel();
#endif
                var instance = _abc.Instances[i];
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
            var instance = type.Tag as AbcInstance;
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

        bool MustDefine(IField field)
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

        void DefineFields(IType type)
        {
            foreach (var field in type.Fields)
            {
#if DEBUG
                DebugService.DoCancel();
#endif
                if (MustDefine(field))
                    DefineField(field);
            }
        }

        void DefineExposedMethods(IType type)
        {
            foreach (var method in new List<IMethod>(type.Methods))
            {
                if (method.IsInternalCall) continue;
                if (GenericType.IsGenericContext(method)) continue;
                if (Mode == AbcGenMode.Full || method.IsExposed())
                    DefineMethod(method);
            }
        }

        void DefineMember(ITypeMember member)
        {
            if (member == null)
                throw new ArgumentNullException("member");

            switch (member.MemberType)
            {
                case MemberType.Constructor:
                case MemberType.Method:
                    DefineMethod((IMethod)member);
                    break;

                case MemberType.Field:
                    DefineField((IField)member);
                    break;

                case MemberType.Type:
                    DefineType((IType)member);
                    break;
            }
        }
        #endregion

        #region DefineCompiledMethods
        void DefineCompiledMethods(AbcInstance instance)
        {
            var type = instance.Type;
            if (type != null)
                DefineCompiledMethods(type);
        }

        void DefineCompiledMethods(IType type)
        {
            var instance = type.Tag as AbcInstance;
            if (instance == null) return;

            ImplementArrayInterface(type);

            //Define Compiled Interface Methods
            foreach (var iface in instance.Implements)
            {
#if DEBUG
                DebugService.DoCancel();
#endif
                DefineCompiledMethods(type, iface);
            }

            //Define Override Methods for already Compiled Base Methods
            var super = instance.SuperType;
            while (super != null)
            {
#if DEBUG
                DebugService.DoCancel();
#endif
                DefineCompiledMethods(type, super);
                super = super.SuperType;
            }
        }

        private void DefineCompiledMethods(IType type, AbcInstance super)
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
                var m = abcMethod.SourceMethod;
                if (m == null) continue;
                if (m.IsStatic) continue;
                if (m.IsConstructor) continue;

                if (super.IsInterface)
                {
                    DefineImplementation(type, m);
                }
                else
                {
                    if (m.IsVirtual || m.IsAbstract)
                        DefineOverrideMethod(type, m);
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

            return instance.DefineStaticMethod(
                thisCtor.TraitName.NameString + "__static",
                instance.Name,
                code =>
                    {
                        code.PushThisScope();
                        code.CreateInstance(instance);
                        code.Dup();
                        code.LoadArguments(ctor);
                        code.Call(thisCtor);
                        code.ReturnValue();
                    },
                thisCtor);
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

        bool LinkVectorInstance(IType type)
        {
            var instance = type as IGenericInstance;
            if (instance == null) return false;

            if (!instance.Type.HasAttribute(Attrs.GenericVector))
                return false;

            DefineTypes(instance.GenericArguments);

            var v = new VectorInstance(type);
#if DEBUG
            Debug.Assert(type.Tag == v);
#endif

            return true;
        }
        #endregion
    }
}