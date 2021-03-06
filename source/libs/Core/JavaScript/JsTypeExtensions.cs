using System;
using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.Common.TypeSystem;

namespace DataDynamics.PageFX.Core.JavaScript
{
	internal static class JsTypeExtensions
	{
		private const string AvmStringClassName = "Avm.String";

		private static readonly Dictionary<TypeCode, string> TypeCodeNames =
			new Dictionary<TypeCode, string>
				{
					{TypeCode.Empty, "Empty"},
					{TypeCode.Object, "o"},
					{TypeCode.DBNull, "DBNull"},
					{TypeCode.Boolean, "b"},
					{TypeCode.Char, "c"},
					{TypeCode.SByte, "i1"},
					{TypeCode.Byte, "u1"},
					{TypeCode.Int16, "i2"},
					{TypeCode.UInt16, "u2"},
					{TypeCode.Int32, "i4"},
					{TypeCode.UInt32, "u4"},
					{TypeCode.Int64, "i8"},
					{TypeCode.UInt64, "u8"},
					{TypeCode.Single, "r4"},
					{TypeCode.Double, "r8"},
					{TypeCode.Decimal, "d"},
					{TypeCode.DateTime, "DateTime"},
					{TypeCode.String, "s"},
				};

		public static object JsTypeCode(this IType type)
		{
			var key = type.GetTypeCode();
			string name;
			if (TypeCodeNames.TryGetValue(key, out name))
			{
				return ("$tc." + name).Id();
			}
			return key;
		}

		public static bool IsString(this IType type)
		{
			return type.IsSystemString() || type.IsAvmString();
		}

		private static bool IsSystemString(this IType type)
		{
			return type.Is(SystemTypeCode.String);
		}

		public static bool IsAvmString(this IType type)
		{
			if (type == null) return false;
			return type.FullName == AvmStringClassName;
		}

		public static string JsFullName(this IType type)
		{
			string ns = type.Namespace;
			if (string.IsNullOrEmpty(ns))
			{
				ns = "$global";
			}
			return new JsPropertyRef(ns.Id(), type.Name).ToString();
		}

		public static string JsFullName(this IType type, IMethod method)
		{
			if (type.IsString())
			{
				if (!method.IsStatic)
				{
					return type.Name;
				}
				//TODO: provide attributes to specify type name of native/internal/inline calls
				if (method.Name == "fromCharCode")
				{
					return type.Name;
				}
			}
			if (type.FullName == "Avm.Array")
			{
				if (!method.IsStatic)
				{
					return type.Name;
				}
			}
			return type.JsFullName();
		}

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
			switch (type.GetTypeCode())
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
				case TypeCode.Single:
				case TypeCode.Double:
					return 0;

				case TypeCode.Decimal:
				case TypeCode.Int64:
				case TypeCode.UInt64:
					return type.New();
				
				default:
					if (type.TypeKind == TypeKind.Struct)
					{
						var st = type.SystemType();
						if (st != null && (st.Code == SystemTypeCode.ValueType || st.Code == SystemTypeCode.Enum))
							return null;
						return type.New();
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

		public static IEnumerable<IField> GetFields(this IType type, bool isStatic)
		{
			if (isStatic)
			{
				return type.Fields.Where(x => x.IsStatic && !x.IsConstant && !x.IsArrayInitializer());
			}
			return type.Fields.Where(f => !f.IsStatic && !f.IsConstant && !f.IsArrayInitializer());
		}
	}
}