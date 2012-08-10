using System;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.JavaScript
{
	internal static class JsTypeExtensions
	{
		public static object InitialValue(this IType type)
		{
			switch (SystemTypes.GetTypeCode(type))
			{
				case TypeCode.Boolean:
					return false;
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
					return 0;
					//case TypeCode.DateTime:
				default:
					if (type.TypeKind == TypeKind.Struct)
					{
						return new JsNewobj(type);
					}
					return null;
			}
		}
	}
}