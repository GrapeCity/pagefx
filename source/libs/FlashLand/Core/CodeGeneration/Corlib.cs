using DataDynamics.PageFX.Common.Services;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Common.Utilities;
using DataDynamics.PageFX.FlashLand.Abc;
using DataDynamics.PageFX.FlashLand.Core.CodeGeneration.Corlib;
using TypeImpl = DataDynamics.PageFX.FlashLand.Core.CodeGeneration.Corlib.TypeImpl;

namespace DataDynamics.PageFX.FlashLand.Core.CodeGeneration
{
	//TODO: make as independent part
	internal partial class AbcGenerator
	{
		public IType GetType(CorlibTypeId id)
		{
			return CorlibTypes[id];
		}

		public AbcInstance GetInstance(CorlibTypeId id)
		{
			return DefineAbcInstance(GetType(id));
		}

		public IType MakeInstance(GenericTypeId typeId, IType arg)
		{
			var type = CorlibTypes[typeId];
			return TypeFactory.MakeGenericType(type, arg);
		}

		public IType MakeNullable(IType arg)
		{
			return MakeInstance(GenericTypeId.NullableT, arg);
		}

		public IType MakeIEnumerable(IType arg)
		{
			return MakeInstance(GenericTypeId.IEnumerableT, arg);
		}

		private ObjectTypeImpl ObjectType
		{
			get { return _objectType ?? (_objectType = new ObjectTypeImpl(this)); }
		}
		private ObjectTypeImpl _objectType;

		public AbcMethod GetMethod(ObjectMethodId id)
		{
			return ObjectType[id];
		}

		private TypeImpl SystemType
		{
			get { return _systemType ?? (_systemType = new TypeImpl(this)); }
		}
		private TypeImpl _systemType;

		public AbcMethod GetMethod(TypeMethodId id)
		{
			return SystemType[id];
		}

		public EnvironmentTypeImpl EnvironmentType
		{
			get { return _environmentType ?? (_environmentType = new EnvironmentTypeImpl(this)); }
		}
		private EnvironmentTypeImpl _environmentType;

		public AbcMethod GetMethod(EnvironmentMethodId id)
        {
			return EnvironmentType[id];
        }

		public ArrayTypeImpl ArrayType
		{
			get { return _arrayType ?? (_arrayType = new ArrayTypeImpl(this)); }
		}
		private ArrayTypeImpl _arrayType;

		public AbcInstance GetArrayInstance()
		{
			return ArrayType.Instance;
        }

        public AbcMethod GetMethod(ArrayMethodId id)
        {
            return ArrayType[id];
        }

		internal AssemblyTypeImpl AssemblyType
		{
			get { return _assemblyType ?? (_assemblyType = new AssemblyTypeImpl(this)); }
		}
		private AssemblyTypeImpl _assemblyType;

		public AbcMethod GetMethod(AssemblyMethodId id)
        {
            return AssemblyType[id];
        }

		private ConsoleTypeImpl ConsoleType
		{
			get { return _consoleType ?? (_consoleType = new ConsoleTypeImpl(this)); }
		}
		private ConsoleTypeImpl _consoleType;

		public AbcMethod GetMethod(ConsoleMethodId id)
        {
            return ConsoleType[id];
        }

		private ConvertTypeImpl ConvertType
		{
			get { return _convertType ?? (_convertType = new ConvertTypeImpl(this)); }
		}
		private ConvertTypeImpl _convertType;

        public AbcMethod GetMethod(ConvertMethodId id)
        {
			return ConvertType[id];
        }

		/// <summary>
		/// PageFX CompilerUtils Methods inside corlib
		/// </summary>
		private CompilerTypeImpl CompilerType
		{
			get { return _compilerType ?? (_compilerType = new CompilerTypeImpl(this)); }
		}
		private CompilerTypeImpl _compilerType;

        public AbcMethod GetMethod(CompilerMethodId id)
        {
            return CompilerType[id];
        }

		#region Fields

        public IField GetField(FieldId id)
        {
            return LazyFields[(int)id].Value;
        }

        private LazyValue<IField>[] LazyFields
        {
            get 
            {
                if (_lazyFields != null)
                    return _lazyFields;

	            _lazyFields = new[]
		            {
			            //Delegate Fields
			            NewLazyField(SystemTypes.Delegate, Const.Delegate.Target),
			            NewLazyField(SystemTypes.Delegate, Const.Delegate.Function),
			            NewLazyField(SystemTypes.MulticastDelegate, Const.Delegate.Prev),

			            NewLazyField(GetType(CorlibTypeId.ParameterInfo), Const.ParameterInfo.ClassImpl),
			            NewLazyField(GetType(CorlibTypeId.ParameterInfo), Const.ParameterInfo.NameImpl),
			            NewLazyField(GetType(CorlibTypeId.ParameterInfo), Const.ParameterInfo.MemberImpl),

			            NewLazyField(GetType(CorlibTypeId.MethodBase), Const.MethodBase.Name),
			            NewLazyField(GetType(CorlibTypeId.MethodBase), Const.MethodBase.Function),
			            NewLazyField(GetType(CorlibTypeId.MethodBase), Const.MethodBase.Attributes),
			            NewLazyField(GetType(CorlibTypeId.MethodBase), Const.MethodBase.Parameters),

			            NewLazyField(GetType(CorlibTypeId.ConstructorInfo), Const.ConstructorInfo.CreateFunction),

			            NewLazyField(GetType(CorlibTypeId.PropertyInfo), "m_name"),
			            NewLazyField(GetType(CorlibTypeId.PropertyInfo), "m_propType"),
			            NewLazyField(GetType(CorlibTypeId.PropertyInfo), "m_getMethod"),
			            NewLazyField(GetType(CorlibTypeId.PropertyInfo), "m_setMethod")
		            };

                return _lazyFields;
            }
        }
        private LazyValue<IField>[] _lazyFields;

        private static LazyValue<IField> NewLazyField(IType type, string name)
        {
            return new LazyValue<IField>(() => type.FindField(name, true));
        }

        #endregion
    }

	#region enum FieldId
    enum FieldId
    {
        Delegate_Target,
        Delegate_Function,
        Delegate_Prev,

        ParameterInfo_ClassImpl,
        ParameterInfo_NameImpl,
        ParameterInfo_MemberImpl,

        MethodBase_Name,
        MethodBase_Function,
        MethodBase_Attributes,
        MethodBase_Parameters,

        ConstructorInfo_CreateFunction,

        PropertyInfo_Name,
        PropertyInfo_Type,
        PropertyInfo_Getter,
        PropertyInfo_Setter,
    }
    #endregion
}