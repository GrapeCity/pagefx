using System.Linq;
using System.Collections.Generic;
using DataDynamics.PageFX.Common.Services;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Common.Utilities;
using DataDynamics.PageFX.FlashLand.Abc;

namespace DataDynamics.PageFX.FlashLand.Core.ByteCodeGeneration.Corlib
{
	internal sealed class ConvertTypeImpl
	{
		private readonly AbcGenerator _generator;
		private LazyValue<AbcMethod>[] _methods;

		public ConvertTypeImpl(AbcGenerator generator)
		{
			_generator = generator;
		}

		public IType Type
		{
			get { return _generator.GetType(CorlibTypeId.Convert); }
		}

		public AbcInstance Instance
		{
			get { return _generator.GetInstance(CorlibTypeId.Convert); }
		}

		private SystemTypes SystemTypes
		{
			get { return _generator.SystemTypes; }
		}

		public AbcMethod this[ConvertMethodId id]
		{
			get { return Methods[(int)id].Value; }
		}

		private LazyValue<AbcMethod>[] Methods
		{
			get
			{
				if (_methods == null)
				{
					string[] methods =
						{
							"ToBoolean",
							"ToByte",
							"ToSByte",
							"ToChar",
							"ToInt16",
							"ToUInt16",
							"ToInt32",
							"ToUInt32",
							"ToInt64",
							"ToUInt64",
							"ToSingle",
							"ToDouble",
							"ToDecimal"
						};

					_methods = methods.SelectMany(name => GetMethods(name)).ToArray();
				}

				return _methods;
			}
		}
		
		private IEnumerable<LazyValue<AbcMethod>> GetMethods(string name)
		{
			yield return GetMethod(name, SystemTypes.Boolean);
			yield return GetMethod(name, SystemTypes.UInt8);
			yield return GetMethod(name, SystemTypes.Int8);
			yield return GetMethod(name, SystemTypes.Char);
			yield return GetMethod(name, SystemTypes.Int16);
			yield return GetMethod(name, SystemTypes.UInt16);
			yield return GetMethod(name, SystemTypes.Int32);
			yield return GetMethod(name, SystemTypes.UInt32);
			yield return GetMethod(name, SystemTypes.Int64);
			yield return GetMethod(name, SystemTypes.UInt64);
			yield return GetMethod(name, SystemTypes.Single);
			yield return GetMethod(name, SystemTypes.Double);
			yield return GetMethod(name, SystemTypes.Decimal);
			yield return GetMethod(name, SystemTypes.String);
			yield return GetMethod(name, SystemTypes.Object);
			yield return GetMethod(name, SystemTypes.DateTime);
		}

		private LazyValue<AbcMethod> GetMethod(string name, IType arg)
		{
			return _generator.LazyMethod(Type, name, arg);
		}
	}

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
}
