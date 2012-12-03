using System;
using DataDynamics.PageFX.Common.TypeSystem;

namespace DataDynamics.PageFX.Ecma335.Execution
{
	internal static class InternalTypeExtensions
	{
		public static object GetDefaultValue(this IType type)
		{
			switch (type.GetTypeCode())
			{
				case TypeCode.DBNull:
					return DBNull.Value;
				case TypeCode.Boolean:
					return default(bool);
				case TypeCode.Char:
					return default(char);
				case TypeCode.SByte:
					return default(sbyte);
				case TypeCode.Byte:
					return default(byte);
				case TypeCode.Int16:
					return default(Int16);
				case TypeCode.UInt16:
					return default(UInt16);
				case TypeCode.Int32:
					return default(Int32);
				case TypeCode.UInt32:
					return default(UInt32);
				case TypeCode.Int64:
					return default(Int64);
				case TypeCode.UInt64:
					return default(UInt64);
				case TypeCode.Single:
					return default(Single);
				case TypeCode.Double:
					return default(Double);
				case TypeCode.Decimal:
					return default(Decimal);
				case TypeCode.DateTime:
					return default(DateTime);
				default:
					return null;
			}
		}
	}
}
