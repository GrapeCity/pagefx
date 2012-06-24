using System;

namespace DataDynamics.PageFX.CLI.Execution
{
	internal static class ObjectExtensions
	{
		public static bool IsNumeric(this object x)
		{
			switch (Type.GetTypeCode(x.GetType()))
			{
				case TypeCode.Boolean:
				case TypeCode.Char:
				case TypeCode.SByte:
				case TypeCode.Byte:
				case TypeCode.Int16:
				case TypeCode.UInt16:
				case TypeCode.Int32:
				case TypeCode.UInt32:
				case TypeCode.Int64:
				case TypeCode.UInt64:
				case TypeCode.Single:
				case TypeCode.Double:
				case TypeCode.Decimal:
					return true;
				default:
					return false;
			}
		}

		public static object ToUnsigned(this object x)
		{
			switch (Type.GetTypeCode(x.GetType()))
			{
				case TypeCode.Boolean:
					return (bool)x ? 1u : 0u;
				case TypeCode.Char:
					return (uint)(char)x;
				case TypeCode.SByte:
					return (byte)(sbyte)x;
				case TypeCode.Int16:
					return (UInt16)(Int16)x;
				case TypeCode.Int32:
					return (UInt32)(Int32)x;
				case TypeCode.Int64:
					return (UInt64)(Int64)x;
				case TypeCode.Single:
					//TODO: roundtrip
					return x;
				case TypeCode.Double:
					//TODO: roundtrip
					return x;
				default:
					return x;
			}
		}

		public static bool IsTrue(this object value)
		{
			if (value == null) return false;
			switch (Type.GetTypeCode(value.GetType()))
			{
				case TypeCode.Empty:
				case TypeCode.DBNull:
					return false;
				case TypeCode.Object:
				case TypeCode.DateTime:
				case TypeCode.String:
					return true;
				case TypeCode.Boolean:
					return (bool)value;
				case TypeCode.Char:
					return (char)value != 0;
				case TypeCode.SByte:
					return (sbyte)value != 0;
				case TypeCode.Byte:
					return (byte)value != 0;
				case TypeCode.Int16:
					return (Int16)value != 0;
				case TypeCode.UInt16:
					return (UInt16)value != 0;
				case TypeCode.Int32:
					return (Int32)value != 0;
				case TypeCode.UInt32:
					return (UInt32)value != 0;
				case TypeCode.Int64:
					return (Int64)value != 0;
				case TypeCode.UInt64:
					return (UInt64)value != 0;
				case TypeCode.Single:
					return (float)value != 0;
				case TypeCode.Double:
					return (double)value != 0;
				case TypeCode.Decimal:
					return (decimal)value != 0;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		public static object Copy(this object value)
		{
			var instance = value as Instance;
			if (instance != null && instance.IsValueType)
			{
				return instance.Copy();
			}
			return value;
		}
	}
}
