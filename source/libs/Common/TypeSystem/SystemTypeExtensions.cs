using System;

namespace DataDynamics.PageFX.Common.TypeSystem
{
	public static class SystemTypeExtensions
	{
		public static TypeCode GetTypeCode(this IType type)
		{
			if (type == null) return TypeCode.Empty;
			var st = type.SystemType();
			return st != null ? st.TypeCode : TypeCode.Empty;
		}

		public static bool IsSystemType(this IType type)
		{
			return type.SystemType() != null;
		}

		public static SystemType SystemType(this IType type)
		{
			return type != null ? SystemTypes.FindByFullName(type.FullName) : null;
		}

		public static IType SystemType(this IType type, SystemTypeCode typeCode)
		{
			return type != null ? type.Assembly.SystemTypes[typeCode] : null;
		}

		public static bool Is(this IType type, SystemTypeCode typeCode)
		{
			var st = type.SystemType();
			return st != null && st.Code == typeCode;
		}

		public static bool IsNumeric(this IType type)
		{
			if (type == null) return false;
			var st = type.SystemType();
			if (st == null) return false;
			return st.IsNumeric;
		}

		public static bool IsIntegral(this IType type)
		{
			if (type == null) return false;
			var st = type.SystemType();
			return st != null && st.IsIntegral;
		}

		public static bool IsSigned(this IType type)
		{
			if (type == null) return false;
			var st = type.SystemType();
			return st != null && st.IsSigned;
		}

		public static bool IsUnsigned(this IType type)
		{
			if (type == null) return false;
			var st = type.SystemType();
			return st != null && st.IsUnsigned;
		}

		public static IType ToUnsigned(this IType type)
		{
			if (type == null) return null;
			var st = type.SystemType();
			if (st == null) return type;
			switch (st.Code)
			{
				case SystemTypeCode.Int8:
					return type.SystemType(SystemTypeCode.UInt8);
				case SystemTypeCode.Int16:
					return type.SystemType(SystemTypeCode.UInt16);
				case SystemTypeCode.Int32:
					return type.SystemType(SystemTypeCode.UInt32);
				case SystemTypeCode.Int64:
					return type.SystemType(SystemTypeCode.UInt64);
				default:
					return type;
			}
		}
	}
}