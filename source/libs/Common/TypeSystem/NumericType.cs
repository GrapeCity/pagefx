using System.Collections.Generic;
using System.Linq;

namespace DataDynamics.PageFX.Common.TypeSystem
{
	public static class NumericType
	{
		private static IEnumerable<IType> GetDescendingOrder(IType type)
		{
			yield return type.SystemType(SystemTypeCode.Decimal);
			yield return type.SystemType(SystemTypeCode.Double);
			yield return type.SystemType(SystemTypeCode.Single);
			yield return type.SystemType(SystemTypeCode.Int64);
			yield return type.SystemType(SystemTypeCode.UInt64);
			yield return type.SystemType(SystemTypeCode.Int32);
			yield return type.SystemType(SystemTypeCode.UInt32);
			yield return type.SystemType(SystemTypeCode.Int16);
			yield return type.SystemType(SystemTypeCode.UInt16);
			yield return type.SystemType(SystemTypeCode.Int8);
			yield return type.SystemType(SystemTypeCode.UInt8);
		}

		public static IType GetCommonType(IType a, IType b)
		{
			if (a == null && b == null) return null;
			return GetDescendingOrder(a ?? b).FirstOrDefault(type => ReferenceEquals(a, type) || ReferenceEquals(b, type));
		}

		public static IType UInt32OR64(IType type)
		{
			if (type == null) return null;
			var st = type.SystemType();
			if (st == null) return null;
			switch (st.Code)
			{
				case SystemTypeCode.Int8:
				case SystemTypeCode.Int16:
				case SystemTypeCode.Int32:
				case SystemTypeCode.UInt8:
				case SystemTypeCode.UInt16:
				case SystemTypeCode.UInt32:
					return type.SystemType(SystemTypeCode.UInt32);
				case SystemTypeCode.Int64:
				case SystemTypeCode.UInt64:
					return type.SystemType(SystemTypeCode.UInt64);
			}
			return null;
		}

		public static IType UInt32OR64(IType a, IType b)
		{
			a = UInt32OR64(a);
			b = UInt32OR64(b);
			if (a == null) return b;
			if (b == null) return null;
			if (a.Is(SystemTypeCode.UInt64) || b.Is(SystemTypeCode.UInt64))
				return a.SystemType(SystemTypeCode.UInt64);
			return a.SystemType(SystemTypeCode.UInt32);
		}
	}
}
