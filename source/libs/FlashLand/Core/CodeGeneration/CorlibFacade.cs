using DataDynamics.PageFX.Common.Services;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Common.Utilities;
using DataDynamics.PageFX.FlashLand.Abc;
using DataDynamics.PageFX.FlashLand.Core.CodeGeneration.Corlib;
using TypeImpl = DataDynamics.PageFX.FlashLand.Core.CodeGeneration.Corlib.TypeImpl;

namespace DataDynamics.PageFX.FlashLand.Core.CodeGeneration
{
	//TODO: make as independent part
	internal sealed class CorlibFacade
	{
		private readonly AbcGenerator _generator;
		private CorlibTypes _types;

		public CorlibFacade(AbcGenerator generator)
		{
			_generator = generator;
		}

		private SystemTypes SystemTypes
		{
			get { return _generator.SystemTypes; }
		}

		private TypeFactory TypeFactory
		{
			get { return _generator.TypeFactory; }
		}

		private CorlibTypes Types
		{
			get { return _types ?? (_types = new CorlibTypes(_generator.AppAssembly)); }
		}

		public IType GetType(CorlibTypeId id)
		{
			return Types[id];
		}

		public IGenericType GetType(GenericTypeId id)
		{
			return Types[id];
		}

		public AbcInstance GetInstance(CorlibTypeId id)
		{
			return _generator.TypeBuilder.BuildInstance(GetType(id));
		}

		public IType MakeInstance(GenericTypeId typeId, IType arg)
		{
			var type = GetType(typeId);
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

		public ObjectTypeImpl Object
		{
			get { return _objectType ?? (_objectType = new ObjectTypeImpl(_generator)); }
		}
		private ObjectTypeImpl _objectType;

		public AbcMethod GetMethod(ObjectMethodId id)
		{
			return Object[id];
		}

		internal TypeImpl SystemType
		{
			get { return _systemType ?? (_systemType = new TypeImpl(_generator)); }
		}
		private TypeImpl _systemType;

		public AbcMethod GetMethod(TypeMethodId id)
		{
			return SystemType[id];
		}

		public EnvironmentTypeImpl Environment
		{
			get { return _environmentType ?? (_environmentType = new EnvironmentTypeImpl(_generator)); }
		}
		private EnvironmentTypeImpl _environmentType;

		public AbcMethod GetMethod(EnvironmentMethodId id)
        {
			return Environment[id];
        }

		public ArrayTypeImpl Array
		{
			get { return _arrayType ?? (_arrayType = new ArrayTypeImpl(_generator)); }
		}
		private ArrayTypeImpl _arrayType;

		public AbcMethod GetMethod(ArrayMethodId id)
        {
            return Array[id];
        }

		internal AssemblyTypeImpl Assembly
		{
			get { return _assemblyType ?? (_assemblyType = new AssemblyTypeImpl(_generator)); }
		}
		private AssemblyTypeImpl _assemblyType;

		public AbcMethod GetMethod(AssemblyMethodId id)
        {
            return Assembly[id];
        }

		private ConsoleTypeImpl Console
		{
			get { return _consoleType ?? (_consoleType = new ConsoleTypeImpl(_generator)); }
		}
		private ConsoleTypeImpl _consoleType;

		public AbcMethod GetMethod(ConsoleMethodId id)
        {
            return Console[id];
        }

		internal ConvertTypeImpl Convert
		{
			get { return _convertType ?? (_convertType = new ConvertTypeImpl(_generator)); }
		}
		private ConvertTypeImpl _convertType;

        public AbcMethod GetMethod(ConvertMethodId id)
        {
			return Convert[id];
        }

		/// <summary>
		/// PageFX CompilerUtils Methods inside corlib
		/// </summary>
		private CompilerTypeImpl Compiler
		{
			get { return _compilerType ?? (_compilerType = new CompilerTypeImpl(_generator)); }
		}
		private CompilerTypeImpl _compilerType;

        public AbcMethod GetMethod(CompilerMethodId id)
        {
            return Compiler[id];
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
    internal enum FieldId
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