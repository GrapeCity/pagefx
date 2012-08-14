using System;
using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.JavaScript
{
	internal static class JsTypeExtensions
	{
		/// <summary>
		/// Returns true if given type should not be compiled.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static bool IsExcluded(this IType type)
		{
			if (type.IsModuleType())
				return true;

			if (type.IsSpecialName || type.IsRuntimeSpecialName)
				return true;

			if (type.IsArrayInitializer())
				return true;

			if (type.IsPrivateImplementationDetails())
			{
				return type.Fields.All(field => field.IsArrayInitializer());
			}

			return false;
		}

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

		public static IEnumerable<IType> GetFullTypeHierarchy(this IType type)
		{
			yield return type;

			if (type.BaseType != null)
			{
				foreach (var t in type.BaseType.GetFullTypeHierarchy())
				{
					yield return t;
				}
			}

			foreach (var iface in type.Interfaces)
			{
				foreach (var t in iface.GetFullTypeHierarchy())
				{
					yield return t;
				}
			}
		}
	}
}