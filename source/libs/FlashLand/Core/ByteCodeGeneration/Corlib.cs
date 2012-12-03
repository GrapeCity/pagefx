using System.Collections.Generic;
using DataDynamics.PageFX.Common.Services;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Common.Utilities;
using DataDynamics.PageFX.FlashLand.Abc;

namespace DataDynamics.PageFX.FlashLand.Core.ByteCodeGeneration
{
	internal partial class AbcGenerator
	{
		public readonly CorlibTypeCache CorlibTypes = new CorlibTypeCache();

		public IType GetType(CorlibTypeId id)
		{
			return CorlibTypes[id];
		}

		public AbcInstance GetInstance(CorlibTypeId id)
		{
			return DefineAbcInstance(GetType(id));
		}

		public IType MakeInstance(GenericTypeId type, IType arg)
		{
			return TypeFactory.MakeGenericType(CorlibTypes[type], arg);
		}

		public IType MakeNullable(IType arg)
		{
			return MakeInstance(GenericTypeId.NullableT, arg);
		}

		public IType MakeIEnumerable(IType arg)
		{
			return MakeInstance(GenericTypeId.IEnumerableT, arg);
		}

		#region Object Type

		private sealed class ObjectTypeImpl
		{
			private readonly AbcGenerator _generator;

			public ObjectTypeImpl(AbcGenerator generator)
			{
				_generator = generator;
			}

			public AbcInstance Instance
			{
				get { return _generator.DefineAbcInstance(SystemTypes.Object); }
			}

			public AbcMethod this[ObjectMethodId id]
			{
				get { return Methods[(int)id].Value; }
			}

			private LazyValue<AbcMethod>[] Methods
			{
				get
				{
					return _methods ?? (_methods
					                    = new[]
					                      	{
					                      		_generator.LazyMethod(SystemTypes.Object, "GetType"),
					                      		_generator.LazyMethod(SystemTypes.Object, "Equals", 1),
					                      		_generator.LazyMethod(SystemTypes.Object, "GetTypeId"),
					                      		_generator.LazyMethod(SystemTypes.Object, "NewHashCode")
					                      	});
				}
			}

			private LazyValue<AbcMethod>[] _methods;
		}

		private ObjectTypeImpl ObjectType
		{
			get { return _objectType ?? (_objectType = new ObjectTypeImpl(this)); }
		}

		private ObjectTypeImpl _objectType;

		public AbcInstance GetObjectInstance()
		{
			return ObjectType.Instance;
		}

		public AbcMethod GetMethod(ObjectMethodId id)
		{
			return ObjectType[id];
		}

		#endregion

		#region System Type

		private sealed class SystemTypeImpl
		{
			private readonly AbcGenerator _generator;
			private LazyValue<AbcMethod>[] _methods;

			public SystemTypeImpl(AbcGenerator generator)
			{
				_generator = generator;
			}

			public AbcInstance Instance
			{
				get { return _generator.DefineAbcInstance(SystemTypes.Type); }
			}

			public AbcMethod this[TypeMethodId id]
			{
				get { return Methods[(int)id].Value; }
			}

			private LazyValue<AbcMethod>[] Methods
			{
				get
				{
					return _methods ?? (_methods =
					                    new[]
					                    	{
					                    		_generator.LazyMethod(SystemTypes.Type, "get_ValueTypeKind")
					                    	});
				}
			}
		}

		private SystemTypeImpl SystemType
		{
			get { return _systemType ?? (_systemType = new SystemTypeImpl(this)); }
		}

		private SystemTypeImpl _systemType;

		public AbcInstance GetTypeInstance()
		{
			return SystemType.Instance;
		}

		public AbcMethod GetMethod(TypeMethodId id)
		{
			return SystemType[id];
		}

		#endregion

		#region Environment Type

		private sealed class EnvironmentTypeImpl
		{
			private readonly AbcGenerator _generator;
			private LazyValue<AbcMethod>[] _methods;

			public EnvironmentTypeImpl(AbcGenerator generator)
			{
				_generator = generator;
			}

			public AbcMethod this[EnvironmentMethodId id]
			{
				get { return EnvironmentMethods[(int)id].Value; }
			}

			private LazyValue<AbcMethod>[] EnvironmentMethods
			{
				get
				{
					return _methods ?? (_methods =
					                    new[]
					                    	{
					                    		_generator.LazyMethod(Type, "get_StackTrace")
					                    	});
				}
			}

			private IType Type
			{
				get { return _generator.CorlibTypes[CorlibTypeId.Environment]; }
			}
		}

		private EnvironmentTypeImpl EnvironmentType
		{
			get { return _environmentType ?? (_environmentType = new EnvironmentTypeImpl(this)); }
		}
		private EnvironmentTypeImpl _environmentType;

		public AbcMethod GetMethod(EnvironmentMethodId id)
        {
			return EnvironmentType[id];
        }
        
        #endregion

        #region Array Type

		private sealed class ArrayTypeImpl
		{
			private readonly AbcGenerator _generator;
			private LazyValue<AbcMethod>[] _methods;

			public ArrayTypeImpl(AbcGenerator generator)
			{
				_generator = generator;
			}

			public AbcInstance Instance
			{
				get { return _generator.DefineAbcInstance(SystemTypes.Array); }
			}

			public AbcMethod this[ArrayMethodId id]
			{
				get { return ArrayMethods[(int)id].Value; }
			}

			private LazyValue<AbcMethod>[] ArrayMethods
			{
				get
				{
					return _methods ?? (_methods =
					                    new[]
					                    	{
					                    		_generator.LazyMethod(SystemTypes.Array, "get_Length"),
					                    		_generator.LazyMethod(SystemTypes.Array, "GetElem", 1),
					                    		_generator.LazyMethod(SystemTypes.Array, "SetElem", 2),
					                    		_generator.LazyMethod(SystemTypes.Array, "GetItem", 1),
					                    		_generator.LazyMethod(SystemTypes.Array, "SetItem", 2),
					                    		_generator.LazyMethod(SystemTypes.Array, "ToFlatIndex", t => !t.IsArray),
					                    		_generator.LazyMethod(SystemTypes.Array, "IsCharArray"),
					                    		_generator.LazyMethod(SystemTypes.Array, "CastTo", 1),
					                    		_generator.LazyMethod(SystemTypes.Array, "HasElemType", 2),
					                    		_generator.LazyMethod(SystemTypes.Array, "Clear"),
					                    		_generator.LazyMethod(SystemTypes.Array, "IndexOf", 1),
					                    		_generator.LazyMethod(SystemTypes.Array, "Contains", 1),
					                    		_generator.LazyMethod(SystemTypes.Array, "CopyTo", 2)
					                    	});
				}
			}
		}

		private ArrayTypeImpl ArrayType
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

        #endregion

        #region Assembly Type

		private sealed class AssemblyTypeImpl
		{
			private readonly AbcGenerator _generator;
			private LazyValue<AbcMethod>[] _methods;

			public AssemblyTypeImpl(AbcGenerator generator)
			{
				_generator = generator;
			}

			public AbcInstance Instance
			{
				get { return _generator.GetInstance(CorlibTypeId.Assembly); }
			}

			public AbcMethod this[AssemblyMethodId id]
			{
				get { return AssemblyMethods[(int)id].Value; }
			}

			private LazyValue<AbcMethod>[] AssemblyMethods
			{
				get
				{
					return _methods ?? (_methods =
					                    new[]
					                    	{
					                    		_generator.LazyMethod(Type, "GetType", 1)
					                    	});
				}
			}

			private IType Type
			{
				get { return _generator.CorlibTypes[CorlibTypeId.Assembly]; }
			}
		}

		private AssemblyTypeImpl AssemblyType
		{
			get { return _assemblyType ?? (_assemblyType = new AssemblyTypeImpl(this)); }
		}
		private AssemblyTypeImpl _assemblyType;

		public AbcInstance GetAssemblyInstance()
		{
			return AssemblyType.Instance;
        }

        public AbcMethod GetMethod(AssemblyMethodId id)
        {
            return AssemblyType[id];
        }

        #endregion

        #region Console Type

		private sealed class ConsoleTypeImpl
		{
			private readonly AbcGenerator _generator;
			private LazyValue<AbcMethod>[] _methods;

			public ConsoleTypeImpl(AbcGenerator generator)
			{
				_generator = generator;
			}

			public AbcMethod this[ConsoleMethodId id]
			{
				get { return ConsoleMethods[(int)id].Value; }
			}

			private LazyValue<AbcMethod>[] ConsoleMethods
			{
				get
				{
					return _methods ?? (_methods =
					                    new[]
					                    	{
					                    		_generator.LazyMethod(Type, "WriteLine", 0),
					                    		_generator.LazyMethod(Type, "WriteLine", SystemTypes.String),
					                    		_generator.LazyMethod(Type, "OpenSW", 0),
					                    		_generator.LazyMethod(Type, "CloseSW", 0)
					                    	});
				}
			}

			private IType Type
			{
				get { return _generator.GetType(CorlibTypeId.Console); }
			}
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
    	
        #endregion

        #region Convert Methods

        public AbcMethod GetMethod(ConvertMethodId id)
        {
            return ConvertMethods[(int)id].Value;
        }

        private LazyValue<AbcMethod>[] ConvertMethods
        {
            get
            {
                if (_methodsConvert != null)
                    return _methodsConvert;

                var list = new List<LazyValue<AbcMethod>>();
                AddConvertMethods(list, "ToBoolean");
                AddConvertMethods(list, "ToByte");
                AddConvertMethods(list, "ToSByte");
                AddConvertMethods(list, "ToChar");
                AddConvertMethods(list, "ToInt16");
                AddConvertMethods(list, "ToUInt16");
                AddConvertMethods(list, "ToInt32");
                AddConvertMethods(list, "ToUInt32");
                AddConvertMethods(list, "ToInt64");
                AddConvertMethods(list, "ToUInt64");
                AddConvertMethods(list, "ToSingle");
                AddConvertMethods(list, "ToDouble");
                AddConvertMethods(list, "ToDecimal");
                
                _methodsConvert = list.ToArray();
                return _methodsConvert;
            }
        }
        private LazyValue<AbcMethod>[] _methodsConvert;

        private void AddConvertMethods(ICollection<LazyValue<AbcMethod>> list, string name)
        {
            var type = GetType(CorlibTypeId.Convert);
            list.Add(LazyMethod(type, name, SystemTypes.Boolean));
            list.Add(LazyMethod(type, name, SystemTypes.Byte));
            list.Add(LazyMethod(type, name, SystemTypes.SByte));
            list.Add(LazyMethod(type, name, SystemTypes.Char));
            list.Add(LazyMethod(type, name, SystemTypes.Int16));
            list.Add(LazyMethod(type, name, SystemTypes.UInt16));
            list.Add(LazyMethod(type, name, SystemTypes.Int32));
            list.Add(LazyMethod(type, name, SystemTypes.UInt32));
            list.Add(LazyMethod(type, name, SystemTypes.Int64));
            list.Add(LazyMethod(type, name, SystemTypes.UInt64));
            list.Add(LazyMethod(type, name, SystemTypes.Single));
            list.Add(LazyMethod(type, name, SystemTypes.Double));
            list.Add(LazyMethod(type, name, SystemTypes.Decimal));
            list.Add(LazyMethod(type, name, SystemTypes.String));
            list.Add(LazyMethod(type, name, SystemTypes.Object));
            list.Add(LazyMethod(type, name, SystemTypes.DateTime));
        }

        #endregion

        #region PageFX CompilerUtils Methods inside corlib

		private sealed class CompilerTypeImpl
		{
			private readonly AbcGenerator _generator;
			private LazyValue<AbcMethod>[] _methods;

			public CompilerTypeImpl(AbcGenerator generator)
			{
				_generator = generator;
			}

			public AbcMethod this[CompilerMethodId id]
			{
				get { return CompilerMethods[(int)id].Value; }
			}

			private LazyValue<AbcMethod>[] CompilerMethods
			{
				get
				{
					return _methods ?? (_methods =
					                    new[]
					                    	{
					                    		_generator.LazyMethod(Type, "ToArray", 1)
					                    	});
				}
			}

			private IType Type
			{
				get { return _generator.GetType(CorlibTypeId.CompilerUtils); }
			}
		}

		private CompilerTypeImpl CompilerType
		{
			get { return _compilerType ?? (_compilerType = new CompilerTypeImpl(this)); }
		}
		private CompilerTypeImpl _compilerType;

        public AbcMethod GetMethod(CompilerMethodId id)
        {
            return CompilerType[id];
        }
    	
        #endregion

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

    #region Method IDs

    #region ArrayMethodId
    enum ArrayMethodId
    {
        GetLength,
        GetElem,
        SetElem,
        GetItem,
        SetItem,
        ToFlatIndex,
        IsCharArray,
        CastTo,
        HasElemType,
        Clear,
        IndexOf,
        Contains,
        CopyTo,
    }
    #endregion

    #region ObjectMethodId
    enum ObjectMethodId
    {
        GetType,
        Equals,
        GetTypeId,
        NewHashCode
    }
    #endregion

    #region TypeMethodId
    enum TypeMethodId
    {
        ValueTypeKind,
    }
    #endregion

    #region EnvironmentMethodId
    enum EnvironmentMethodId
    {
        StackTrace,
    }
    #endregion

    #region AssemblyMethodId
    enum AssemblyMethodId
    {
        GetTypeById
    }
    #endregion

    #region ConsoleMethodId
    enum ConsoleMethodId
    {
        WriteLine,
        WriteLine_String,
        OpenSW,
        CloseSW
    }
    #endregion

    #region ConvertMethodId
    enum ConvertMethodId
    {
        ToBool_bool,
        ToBool_byte,
        ToBool_sbyte,
        ToBool_char,
        ToBool_short,
        ToBool_ushort,
        ToBool_int,
        ToBool_uint,
        ToBool_long,
        ToBool_ulong,
        ToBool_float,
        ToBool_double,
        ToBool_decimal,
        ToBool_string,
        ToBool_object,
        ToBool_DateTime,

        ToByte_bool,
        ToByte_byte,
        ToByte_sbyte,
        ToByte_char,
        ToByte_short,
        ToByte_ushort,
        ToByte_int,
        ToByte_uint,
        ToByte_long,
        ToByte_ulong,
        ToByte_float,
        ToByte_double,
        ToByte_decimal,
        ToByte_string,
        ToByte_object,
        ToByte_DateTime,

        ToSByte_bool,
        ToSByte_byte,
        ToSByte_sbyte,
        ToSByte_char,
        ToSByte_short,
        ToSByte_ushort,
        ToSByte_int,
        ToSByte_uint,
        ToSByte_long,
        ToSByte_ulong,
        ToSByte_float,
        ToSByte_double,
        ToSByte_decimal,
        ToSByte_string,
        ToSByte_object,
        ToSByte_DateTime,

        ToChar_bool,
        ToChar_byte,
        ToChar_sbyte,
        ToChar_char,
        ToChar_short,
        ToChar_ushort,
        ToChar_int,
        ToChar_uint,
        ToChar_long,
        ToChar_ulong,
        ToChar_float,
        ToChar_double,
        ToChar_decimal,
        ToChar_string,
        ToChar_object,
        ToChar_DateTime,

        ToInt16_bool,
        ToInt16_byte,
        ToInt16_sbyte,
        ToInt16_char,
        ToInt16_short,
        ToInt16_ushort,
        ToInt16_int,
        ToInt16_uint,
        ToInt16_long,
        ToInt16_ulong,
        ToInt16_float,
        ToInt16_double,
        ToInt16_decimal,
        ToInt16_string,
        ToInt16_object,
        ToInt16_DateTime,

        ToUInt16_bool,
        ToUInt16_byte,
        ToUInt16_sbyte,
        ToUInt16_char,
        ToUInt16_short,
        ToUInt16_ushort,
        ToUInt16_int,
        ToUInt16_uint,
        ToUInt16_long,
        ToUInt16_ulong,
        ToUInt16_float,
        ToUInt16_double,
        ToUInt16_decimal,
        ToUInt16_string,
        ToUInt16_object,
        ToUInt16_DateTime,

        ToInt32_bool,
        ToInt32_byte,
        ToInt32_sbyte,
        ToInt32_char,
        ToInt32_short,
        ToInt32_ushort,
        ToInt32_int,
        ToInt32_uint,
        ToInt32_long,
        ToInt32_ulong,
        ToInt32_float,
        ToInt32_double,
        ToInt32_decimal,
        ToInt32_string,
        ToInt32_object,
        ToInt32_DateTime,

        ToUInt32_bool,
        ToUInt32_byte,
        ToUInt32_sbyte,
        ToUInt32_char,
        ToUInt32_short,
        ToUInt32_ushort,
        ToUInt32_int,
        ToUInt32_uint,
        ToUInt32_long,
        ToUInt32_ulong,
        ToUInt32_float,
        ToUInt32_double,
        ToUInt32_decimal,
        ToUInt32_string,
        ToUInt32_object,
        ToUInt32_DateTime,

        ToInt64_bool,
        ToInt64_byte,
        ToInt64_sbyte,
        ToInt64_char,
        ToInt64_short,
        ToInt64_ushort,
        ToInt64_int,
        ToInt64_uint,
        ToInt64_long,
        ToInt64_ulong,
        ToInt64_float,
        ToInt64_double,
        ToInt64_decimal,
        ToInt64_string,
        ToInt64_object,
        ToInt64_DateTime,

        ToUInt64_bool,
        ToUInt64_byte,
        ToUInt64_sbyte,
        ToUInt64_char,
        ToUInt64_short,
        ToUInt64_ushort,
        ToUInt64_int,
        ToUInt64_uint,
        ToUInt64_long,
        ToUInt64_ulong,
        ToUInt64_float,
        ToUInt64_double,
        ToUInt64_decimal,
        ToUInt64_string,
        ToUInt64_object,
        ToUInt64_DateTime,

        ToSingle_bool,
        ToSingle_byte,
        ToSingle_sbyte,
        ToSingle_char,
        ToSingle_short,
        ToSingle_ushort,
        ToSingle_int,
        ToSingle_uint,
        ToSingle_long,
        ToSingle_ulong,
        ToSingle_float,
        ToSingle_double,
        ToSingle_decimal,
        ToSingle_string,
        ToSingle_object,
        ToSingle_DateTime,

        ToDouble_bool,
        ToDouble_byte,
        ToDouble_sbyte,
        ToDouble_char,
        ToDouble_short,
        ToDouble_ushort,
        ToDouble_int,
        ToDouble_uint,
        ToDouble_long,
        ToDouble_ulong,
        ToDouble_float,
        ToDouble_double,
        ToDouble_decimal,
        ToDouble_string,
        ToDouble_object,
        ToDouble_DateTime,

        ToDecimal_bool,
        ToDecimal_byte,
        ToDecimal_sbyte,
        ToDecimal_char,
        ToDecimal_short,
        ToDecimal_ushort,
        ToDecimal_int,
        ToDecimal_uint,
        ToDecimal_long,
        ToDecimal_ulong,
        ToDecimal_float,
        ToDecimal_double,
        ToDecimal_decimal,
        ToDecimal_string,
        ToDecimal_object,
        ToDecimal_DateTime,

        Unknown,
    }
    #endregion

    #region CompilerMethodId
    enum CompilerMethodId
    {
        ToArray,
    }
    #endregion

    #endregion

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