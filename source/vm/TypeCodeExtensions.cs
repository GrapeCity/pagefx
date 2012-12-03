using System;

namespace DataDynamics.PageFX
{
	internal static class TypeCodeExtensions
	{
		public static bool IsSigned(this TypeCode value)
		{
			switch (value)
			{
				case TypeCode.SByte:
				case TypeCode.Int16:
				case TypeCode.Int32:
				case TypeCode.Int64:
				case TypeCode.Single:
				case TypeCode.Boolean:
				case TypeCode.Char:
					return true;
				default:
					return false;
			}
		}

		public static bool IsUnsigned(this TypeCode value)
		{
			switch (value)
			{
				case TypeCode.Byte:
				case TypeCode.UInt16:
				case TypeCode.UInt32:
				case TypeCode.UInt64:
					return true;
				default:
					return false;
			}
		}
	}
}