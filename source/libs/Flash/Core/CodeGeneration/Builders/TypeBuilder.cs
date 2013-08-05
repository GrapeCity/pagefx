using System;
using System.Collections.Generic;
using System.Diagnostics;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Flash.Abc;
using DataDynamics.PageFX.Flash.Core.SpecialTypes;
using DataDynamics.PageFX.Flash.Core.Tools;

namespace DataDynamics.PageFX.Flash.Core.CodeGeneration.Builders
{
	//TODO: make static

	/// <summary>
	/// Implements creation of <see cref="AbcInstance"/> for given <see cref="IType"/>.
	/// </summary>
    internal sealed class TypeBuilder
    {
	    private readonly AbcGenerator _generator;

	    public TypeBuilder(AbcGenerator generator)
		{
			_generator = generator;
		}

	    private AbcFile Abc
	    {
			get { return _generator.Abc; }
	    }

	    private SystemTypes SystemTypes
	    {
			get { return _generator.SystemTypes; }
	    }

	    public AbcMultiname DefineTypeName(IType type)
        {
            if (type == null) return null;
            Build(type);
            return type.GetMultiname();
        }

		public AbcMultiname BuildMemberType(IType type)
		{
			Build(type);
			if (type.Data == null)
				throw new InvalidOperationException(string.Format("Type {0} is not defined", type.FullName));
			return Abc.GetTypeName(type, true);
		}

		public AbcMultiname BuildReturnType(IType type)
		{
			if (type == null)
				return Abc.BuiltinTypes.Void;
			var name = BuildMemberType(type);
			if (name == null)
				throw new InvalidOperationException("Unable to define return type for method");
			return name;
		}

		/// <summary>
        /// Defines given type in generated ABC file
        /// </summary>
        /// <param name="type">the type to define</param>
        /// <returns>tag associated with given type</returns>
        public object Build(IType type)
        {
            if (type == null) return null;

			if (Abc.IsDefined(type))
			{
				return type.Data;
			}

			var tag = ImportType(type);

			bool isImported = false;
			if (tag == null)
			{
				if (type.Data != null)
					throw new InvalidOperationException();
				tag = BuildCore(type);
			}
			else
			{
				isImported = true;
			}

			if (tag != null)
			{
				_generator.SetData(type, tag);

				RegisterType(type);

				if (!isImported)
					BuildMembers(type);
			}

			return type.Data;
        }

		#region RegisterType
        private void RegisterType(IType type)
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

            _generator.Reflection.GetTypeId(type);
        }
        #endregion

		#region BuildInstance
		/// <summary>
        /// Defines <see cref="AbcInstance"/> for given <see cref="IType"/>.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public AbcInstance BuildInstance(IType type)
        {
            if (type == null)
                throw new ArgumentNullException("type");
            var res = Build(type) as AbcInstance;
            if (res == null)
                throw new InvalidOperationException(string.Format("Unable to define AbcInstance for type: {0}", type));
            return res;
        }
        #endregion

		#region BuildBaseTypes
		private void BuildBaseTypes(IType type)
        {
            if (type == null) return;

            Build(type.BaseType);

            if (type.Interfaces != null)
            {
                foreach (var ifaceType in type.Interfaces)
                    Build(ifaceType);
            }
        }
        #endregion

        #region ImportType
        private object ImportType(IType type)
        {
	        var tag = type.Data;
	        if (tag == null) return null;

	        var instance = tag as AbcInstance;
	        if (instance != null)
	        {
		        BuildBaseTypes(type);
		        return Abc.ImportInstance(instance);
	        }

	        var data = tag as ITypeData;
			if (data != null)
			{
				return data.Import(Abc);
			}
			
	        //TODO: avoid direct usage of AbcMultiname as tag for types, wrap to ITypeAgent impl always
	        var name = tag as AbcMultiname;
	        if (name != null)
	        {
		        return Abc.ImportConst(name);
	        }

	        return null;
        }

		#endregion

		#region BuildCore
		private object BuildCore(IType type)
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
                    return BuildUserType(type);

                case TypeKind.GenericParameter:
                    return null;

                case TypeKind.Enum:
                    Build(type.ValueType);
                    return BuildUserType(type);

                case TypeKind.Array:
                    return BuildArrayType(type);

                case TypeKind.Pointer:
                    throw new NotSupportedException();

                case TypeKind.Reference:
                    {
	                    Build(type.ElementType);
                        return Abc.BuiltinTypes.Object;
                    }
            }

            return null;
        }
        #endregion

		#region BuildUserType
		private object BuildUserType(IType type)
        {
            if (LinkVectorInstance(type))
                return type.Data;

            //NOTE: can be used only in typeof operations
            if (type.IsGeneric())
                return null;

            if (type.HasGenericParams())
                throw new InvalidOperationException();

            AbcMultiname superName;
            AbcInstance superType;
            DefineSuperType(type, out superName, out superType);

            //NOTE: Fix for enums.
            if (Abc.IsDefined(type))
                return type.Data;

#if DEBUG
			DebugService.LogInfo("DefineUserType started for {0}", type.FullName);
#endif
            var ifaceNames = BuildInterfaces(type);

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

            _generator.SetData(type, instance);
			SetFlags(instance, type);
            AddInterfaces(instance, type, ifaceNames);

	        Abc.AddInstance(instance);

	        if (_generator.IsRootSprite(type))
				_generator.RootSprite.Instance = instance;

            DebugInfoBuilder.Build(_generator, type, instance);

#if DEBUG
			DebugService.LogInfo("DefineUserType succeeded for {0}", type.FullName);
#endif

            return instance;
        }

	    private void SetFlags(AbcInstance instance, IType type)
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
		private AbcMultiname DefineInstanceName(IType type)
        {
			string ns = type.GetTypeNamespace(_generator.RootNamespace);
            string name = type.NestedName;
            return Abc.DefineName(QName.Package(ns, name));
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
				// TODO: use Native.Error
                // superType = typeof(Error);
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

        private readonly AbcInstance[] _enumSuperTypes = new AbcInstance[8];

		private static int GetEnumIndex(SystemType st)
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

		private static string GetEnumName(SystemType st)
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

            var super = BuildInstance(type.BaseType);

            instance = _enumSuperTypes[index];
            if (instance != null) return instance;

            instance = new AbcInstance(true)
                           {
                               Name = Abc.DefineName(QName.PfxPackage(GetEnumName(st))),
                               BaseTypeName = super.Name,
                               BaseInstance = super,
							   Initializer = Initializers.BuildDefaultInitializer(Abc, null)
                           };
	        Abc.AddInstance(instance);

	        instance.Class.Initializer = Abc.DefineEmptyMethod();

            _enumSuperTypes[index] = instance;

	        string name = Const.Boxing.Value;
	        instance.CreateSlot(Abc.DefineName(QName.Global(name)), BuildMemberType(vtype));

            //SetFlags(instance, type);

            return instance;
        }
        #endregion

		private static bool OnlyDeclareInterfaces(IType type)
        {
            if (type.Is(SystemTypeCode.String))
                return true;
            return false;
        }

        private IList<AbcMultiname> BuildInterfaces(IType type)
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
                var ifaceName = DefineTypeName(iface);
                if (ifaceName == null)
                    throw new InvalidOperationException(string.Format("Unable to define interface {0}", iface.FullName));

                if (!onlyDecl)
                    list.Add(ifaceName);
            }

            return list;
        }

        private static void AddInterfaces(AbcInstance instance, IType type, IList<AbcMultiname> ifaceNames)
        {
            if (ifaceNames == null) return;
            if (type.Interfaces == null) return;
            int n = type.Interfaces.Count;
            for (int i = 0; i < n; ++i)
            {
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

		private object BuildArrayType(IType type)
        {
            var arr = Build(SystemTypes.Array);
            var elemType = type.ElementType;
            Build(elemType);
            return arr;
        }

		#region FinishTypes, FinishType

	    public void FinishTypes()
        {
#if PERF
            int start = Environment.TickCount;
#endif
            for (int i = 0; i < Abc.Instances.Count; ++i)
            {
                var instance = Abc.Instances[i];
                FinishType(instance);
            }
#if PERF
            Console.WriteLine("AbcGen.FinishTypes: {0}", Environment.TickCount - start);
#endif
        }

	    private void FinishType(AbcInstance instance)
	    {
		    //TODO: Comment when copying of value types will be implemented using Reflection
		    CopyImpl.Copy(instance);

		    BuildCompiledMethods(instance);
	    }

	    #endregion

		#region BuildMembers
		private void BuildMembers(IType type)
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
                _generator.FlexAppBuilder.DefineMembers(instance);
                DefineFields(type);
            }

			Initializers.EnshureInitializers(instance);

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
            if (field.Type.HasGenericParams()) return false;
            if (field.IsExposed()) return true;
			if (_generator.Mode == AbcGenerationMode.Full) return true;
            if (field.IsStatic) return false;
            return true;
        }

        internal void DefineFields(IType type)
        {
			//TODO: PERF do this only once
            foreach (var field in type.Fields)
            {
                if (MustDefine(field))
					_generator.FieldBuilder.Build(field);
            }
        }

        private void DefineExposedMethods(IType type)
        {
            foreach (var method in new List<IMethod>(type.Methods))
            {
                if (method.IsInternalCall) continue;
                if (method.IsGenericContext()) continue;
				if (_generator.Mode == AbcGenerationMode.Full || method.IsExposed())
					_generator.MethodBuilder.Build(method);
            }
        }

	    #endregion

		#region BuildCompiledMethods
		private void BuildCompiledMethods(AbcInstance instance)
        {
            var type = instance.Type;
            if (type != null)
                BuildCompiledMethods(instance, type);
        }

        private void BuildCompiledMethods(AbcInstance instance, IType type)
        {
            if (instance == null)
				throw new ArgumentNullException("instance");
	        if (type == null)
				throw new ArgumentNullException("type");

			_generator.ArrayImpl.ImplementInterface(type);

            //Define Compiled Interface Methods
            foreach (var iface in instance.Implements)
            {
                BuildCompiledMethods(instance, type, iface);
            }

            //Define Override Methods for already Compiled Base Methods
            var super = instance.BaseInstance;
            while (super != null)
            {
                BuildCompiledMethods(instance, type, super);
                super = super.BaseInstance;
            }
        }

		/// <summary>
		/// Compiles override methods of specified base/interface type.
		/// </summary>
		/// <param name="instance"></param>
		/// <param name="type"></param>
		/// <param name="super">Base type or interface.</param>
        private void BuildCompiledMethods(AbcInstance instance, IType type, AbcInstance super)
        {
            if (type.Is(SystemTypeCode.Exception)) return;

            //NOTE: super.Traits.Count can be changed during execution
            for (int i = 0; i < super.Traits.Count; ++i)
            {
                var trait = super.Traits[i];
                if (!trait.IsMethod) continue;

                var abcMethod = trait.Method;
                var method = abcMethod.Method;
                if (method == null) continue;
                if (method.IsStatic) continue;
                if (method.IsConstructor) continue;

                if (super.IsInterface)
                {
					_generator.MethodBuilder.BuildImplementation(instance, type, method, abcMethod);
                }
                else
                {
                    if (method.IsVirtual || method.IsAbstract)
						_generator.MethodBuilder.BuildOverrideMethod(type, method);
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

			var thisCtor = _generator.MethodBuilder.BuildAbcMethod(ctor);
            if (thisCtor.IsInitializer)
                throw new InvalidOperationException("ctor is initializer");

            var declType = ctor.DeclaringType;
            var instance = BuildInstance(declType);

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

	    private void BuildRange(IEnumerable<IType> types)
        {
            if (types == null) return;
            foreach (var type in types)
            {
                Build(type);
            }
        }

        private bool LinkVectorInstance(IType type)
        {
	        if (!type.IsGenericInstance()) return false;

            if (!type.Type.HasAttribute(Attrs.GenericVector))
                return false;

            BuildRange(type.GenericArguments);

            _generator.SetData(type, new VectorInstance(Abc, type));

            return true;
        }
    }
}