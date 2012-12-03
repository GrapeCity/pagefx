using System;
using System.Linq;
using DataDynamics.PageFX.Common;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.FlashLand.Abc;

namespace DataDynamics.PageFX.FLI
{
	internal static class InternalMethodExtensions
	{
		public static bool IsInitializer(this IMethod method)
		{
			if (method == null) return false;
			if (!method.IsConstructor) return false;
			var m = method.Tag as AbcMethod;
			if (m == null) return false;
			return m.IsInitializer;
		}

		public static bool IsFlashEventMethod(this IMethod method)
		{
			if (method == null) return false;
			var e = method.Association as IEvent;
			if (e == null) return false;
			return e.HasAttribute("EventAttribute");
		}

		public static string GetConvertMethodName(this IType target)
		{
			var st = target.SystemType();
			if (st == null) return "";
			switch (st.Code)
			{
				case SystemTypeCode.Boolean:
					return "ToBoolean";
				case SystemTypeCode.Char:
					return "ToChar";
				case SystemTypeCode.Decimal:
					return "ToDecimal";
				case SystemTypeCode.Double:
					return "ToDouble";
				case SystemTypeCode.DateTime:
					return "ToDateTime";
				case SystemTypeCode.Int16:
					return "ToInt16";
				case SystemTypeCode.Int32:
					return "ToInt32";
				case SystemTypeCode.Int64:
					return "ToInt64";
				case SystemTypeCode.UInt16:
					return "ToUInt16";
				case SystemTypeCode.UInt32:
					return "ToUInt32";
				case SystemTypeCode.UInt64:
					return "ToUInt64";
				case SystemTypeCode.Int8:
					return "ToSByte";
				case SystemTypeCode.UInt8:
					return "ToByte";
			}
			return "";
		}

		public static ConvertMethodId GetConvertMethodId(this IType source, IType target)
		{
			var t = target.SystemType();
			if (t == null) return ConvertMethodId.Unknown;
			var s = source.SystemType();
			if (s == null) return ConvertMethodId.Unknown;
			switch (t.Code)
			{
				case SystemTypeCode.Boolean:
					switch (s.Code)
					{
						case SystemTypeCode.Boolean:
							return ConvertMethodId.ToBool_bool;
						case SystemTypeCode.Char:
							return ConvertMethodId.ToBool_char;
						case SystemTypeCode.Int8:
							return ConvertMethodId.ToBool_sbyte;
						case SystemTypeCode.UInt8:
							return ConvertMethodId.ToBool_byte;
						case SystemTypeCode.Int16:
							return ConvertMethodId.ToBool_short;
						case SystemTypeCode.UInt16:
							return ConvertMethodId.ToBool_ushort;
						case SystemTypeCode.Int32:
							return ConvertMethodId.ToBool_int;
						case SystemTypeCode.UInt32:
							return ConvertMethodId.ToBool_uint;
						case SystemTypeCode.Int64:
							return ConvertMethodId.ToBool_long;
						case SystemTypeCode.UInt64:
							return ConvertMethodId.ToBool_ulong;
						case SystemTypeCode.Single:
							return ConvertMethodId.ToBool_float;
						case SystemTypeCode.Double:
							return ConvertMethodId.ToBool_double;
						case SystemTypeCode.Decimal:
							return ConvertMethodId.ToBool_decimal;
						case SystemTypeCode.String:
							return ConvertMethodId.ToBool_string;
						case SystemTypeCode.Object:
							return ConvertMethodId.ToBool_object;
						case SystemTypeCode.DateTime:
							return ConvertMethodId.ToBool_DateTime;
					}
					break;

				case SystemTypeCode.Char:
					switch (s.Code)
					{
						case SystemTypeCode.Boolean:
							return ConvertMethodId.ToChar_bool;
						case SystemTypeCode.Char:
							return ConvertMethodId.ToChar_char;
						case SystemTypeCode.Int8:
							return ConvertMethodId.ToChar_sbyte;
						case SystemTypeCode.UInt8:
							return ConvertMethodId.ToChar_byte;
						case SystemTypeCode.Int16:
							return ConvertMethodId.ToChar_short;
						case SystemTypeCode.UInt16:
							return ConvertMethodId.ToChar_ushort;
						case SystemTypeCode.Int32:
							return ConvertMethodId.ToChar_int;
						case SystemTypeCode.UInt32:
							return ConvertMethodId.ToChar_uint;
						case SystemTypeCode.Int64:
							return ConvertMethodId.ToChar_long;
						case SystemTypeCode.UInt64:
							return ConvertMethodId.ToChar_ulong;
						case SystemTypeCode.Single:
							return ConvertMethodId.ToChar_float;
						case SystemTypeCode.Double:
							return ConvertMethodId.ToChar_double;
						case SystemTypeCode.Decimal:
							return ConvertMethodId.ToChar_decimal;
						case SystemTypeCode.String:
							return ConvertMethodId.ToChar_string;
						case SystemTypeCode.Object:
							return ConvertMethodId.ToChar_object;
						case SystemTypeCode.DateTime:
							return ConvertMethodId.ToChar_DateTime;
					}
					break;

				case SystemTypeCode.Int8:
					switch (s.Code)
					{
						case SystemTypeCode.Boolean:
							return ConvertMethodId.ToSByte_bool;
						case SystemTypeCode.Char:
							return ConvertMethodId.ToSByte_char;
						case SystemTypeCode.Int8:
							return ConvertMethodId.ToSByte_sbyte;
						case SystemTypeCode.UInt8:
							return ConvertMethodId.ToSByte_byte;
						case SystemTypeCode.Int16:
							return ConvertMethodId.ToSByte_short;
						case SystemTypeCode.UInt16:
							return ConvertMethodId.ToSByte_ushort;
						case SystemTypeCode.Int32:
							return ConvertMethodId.ToSByte_int;
						case SystemTypeCode.UInt32:
							return ConvertMethodId.ToSByte_uint;
						case SystemTypeCode.Int64:
							return ConvertMethodId.ToSByte_long;
						case SystemTypeCode.UInt64:
							return ConvertMethodId.ToSByte_ulong;
						case SystemTypeCode.Single:
							return ConvertMethodId.ToSByte_float;
						case SystemTypeCode.Double:
							return ConvertMethodId.ToSByte_double;
						case SystemTypeCode.Decimal:
							return ConvertMethodId.ToSByte_decimal;
						case SystemTypeCode.String:
							return ConvertMethodId.ToSByte_string;
						case SystemTypeCode.Object:
							return ConvertMethodId.ToSByte_object;
						case SystemTypeCode.DateTime:
							return ConvertMethodId.ToSByte_DateTime;
					}
					break;

				case SystemTypeCode.UInt8:
					switch (s.Code)
					{
						case SystemTypeCode.Boolean:
							return ConvertMethodId.ToByte_bool;
						case SystemTypeCode.Char:
							return ConvertMethodId.ToByte_char;
						case SystemTypeCode.Int8:
							return ConvertMethodId.ToByte_sbyte;
						case SystemTypeCode.UInt8:
							return ConvertMethodId.ToByte_byte;
						case SystemTypeCode.Int16:
							return ConvertMethodId.ToByte_short;
						case SystemTypeCode.UInt16:
							return ConvertMethodId.ToByte_ushort;
						case SystemTypeCode.Int32:
							return ConvertMethodId.ToByte_int;
						case SystemTypeCode.UInt32:
							return ConvertMethodId.ToByte_uint;
						case SystemTypeCode.Int64:
							return ConvertMethodId.ToByte_long;
						case SystemTypeCode.UInt64:
							return ConvertMethodId.ToByte_ulong;
						case SystemTypeCode.Single:
							return ConvertMethodId.ToByte_float;
						case SystemTypeCode.Double:
							return ConvertMethodId.ToByte_double;
						case SystemTypeCode.Decimal:
							return ConvertMethodId.ToByte_decimal;
						case SystemTypeCode.String:
							return ConvertMethodId.ToByte_string;
						case SystemTypeCode.Object:
							return ConvertMethodId.ToByte_object;
						case SystemTypeCode.DateTime:
							return ConvertMethodId.ToByte_DateTime;
					}
					break;

				case SystemTypeCode.Int16:
					switch (s.Code)
					{
						case SystemTypeCode.Boolean:
							return ConvertMethodId.ToInt16_bool;
						case SystemTypeCode.Char:
							return ConvertMethodId.ToInt16_char;
						case SystemTypeCode.Int8:
							return ConvertMethodId.ToInt16_sbyte;
						case SystemTypeCode.UInt8:
							return ConvertMethodId.ToInt16_byte;
						case SystemTypeCode.Int16:
							return ConvertMethodId.ToInt16_short;
						case SystemTypeCode.UInt16:
							return ConvertMethodId.ToInt16_ushort;
						case SystemTypeCode.Int32:
							return ConvertMethodId.ToInt16_int;
						case SystemTypeCode.UInt32:
							return ConvertMethodId.ToInt16_uint;
						case SystemTypeCode.Int64:
							return ConvertMethodId.ToInt16_long;
						case SystemTypeCode.UInt64:
							return ConvertMethodId.ToInt16_ulong;
						case SystemTypeCode.Single:
							return ConvertMethodId.ToInt16_float;
						case SystemTypeCode.Double:
							return ConvertMethodId.ToInt16_double;
						case SystemTypeCode.Decimal:
							return ConvertMethodId.ToInt16_decimal;
						case SystemTypeCode.String:
							return ConvertMethodId.ToInt16_string;
						case SystemTypeCode.Object:
							return ConvertMethodId.ToInt16_object;
						case SystemTypeCode.DateTime:
							return ConvertMethodId.ToInt16_DateTime;
					}
					break;

				case SystemTypeCode.UInt16:
					switch (s.Code)
					{
						case SystemTypeCode.Boolean:
							return ConvertMethodId.ToUInt16_bool;
						case SystemTypeCode.Char:
							return ConvertMethodId.ToUInt16_char;
						case SystemTypeCode.Int8:
							return ConvertMethodId.ToUInt16_sbyte;
						case SystemTypeCode.UInt8:
							return ConvertMethodId.ToUInt16_byte;
						case SystemTypeCode.Int16:
							return ConvertMethodId.ToUInt16_short;
						case SystemTypeCode.UInt16:
							return ConvertMethodId.ToUInt16_ushort;
						case SystemTypeCode.Int32:
							return ConvertMethodId.ToUInt16_int;
						case SystemTypeCode.UInt32:
							return ConvertMethodId.ToUInt16_uint;
						case SystemTypeCode.Int64:
							return ConvertMethodId.ToUInt16_long;
						case SystemTypeCode.UInt64:
							return ConvertMethodId.ToUInt16_ulong;
						case SystemTypeCode.Single:
							return ConvertMethodId.ToUInt16_float;
						case SystemTypeCode.Double:
							return ConvertMethodId.ToUInt16_double;
						case SystemTypeCode.Decimal:
							return ConvertMethodId.ToUInt16_decimal;
						case SystemTypeCode.String:
							return ConvertMethodId.ToUInt16_string;
						case SystemTypeCode.Object:
							return ConvertMethodId.ToUInt16_object;
						case SystemTypeCode.DateTime:
							return ConvertMethodId.ToUInt16_DateTime;
					}
					break;

				case SystemTypeCode.Int32:
					switch (s.Code)
					{
						case SystemTypeCode.Boolean:
							return ConvertMethodId.ToInt32_bool;
						case SystemTypeCode.Char:
							return ConvertMethodId.ToInt32_char;
						case SystemTypeCode.Int8:
							return ConvertMethodId.ToInt32_sbyte;
						case SystemTypeCode.UInt8:
							return ConvertMethodId.ToInt32_byte;
						case SystemTypeCode.Int16:
							return ConvertMethodId.ToInt32_short;
						case SystemTypeCode.UInt16:
							return ConvertMethodId.ToInt32_ushort;
						case SystemTypeCode.Int32:
							return ConvertMethodId.ToInt32_int;
						case SystemTypeCode.UInt32:
							return ConvertMethodId.ToInt32_uint;
						case SystemTypeCode.Int64:
							return ConvertMethodId.ToInt32_long;
						case SystemTypeCode.UInt64:
							return ConvertMethodId.ToInt32_ulong;
						case SystemTypeCode.Single:
							return ConvertMethodId.ToInt32_float;
						case SystemTypeCode.Double:
							return ConvertMethodId.ToInt32_double;
						case SystemTypeCode.Decimal:
							return ConvertMethodId.ToInt32_decimal;
						case SystemTypeCode.String:
							return ConvertMethodId.ToInt32_string;
						case SystemTypeCode.Object:
							return ConvertMethodId.ToInt32_object;
						case SystemTypeCode.DateTime:
							return ConvertMethodId.ToInt32_DateTime;
					}
					break;

				case SystemTypeCode.UInt32:
					switch (s.Code)
					{
						case SystemTypeCode.Boolean:
							return ConvertMethodId.ToUInt32_bool;
						case SystemTypeCode.Char:
							return ConvertMethodId.ToUInt32_char;
						case SystemTypeCode.Int8:
							return ConvertMethodId.ToUInt32_sbyte;
						case SystemTypeCode.UInt8:
							return ConvertMethodId.ToUInt32_byte;
						case SystemTypeCode.Int16:
							return ConvertMethodId.ToUInt32_short;
						case SystemTypeCode.UInt16:
							return ConvertMethodId.ToUInt32_ushort;
						case SystemTypeCode.Int32:
							return ConvertMethodId.ToUInt32_int;
						case SystemTypeCode.UInt32:
							return ConvertMethodId.ToUInt32_uint;
						case SystemTypeCode.Int64:
							return ConvertMethodId.ToUInt32_long;
						case SystemTypeCode.UInt64:
							return ConvertMethodId.ToUInt32_ulong;
						case SystemTypeCode.Single:
							return ConvertMethodId.ToUInt32_float;
						case SystemTypeCode.Double:
							return ConvertMethodId.ToUInt32_double;
						case SystemTypeCode.Decimal:
							return ConvertMethodId.ToUInt32_decimal;
						case SystemTypeCode.String:
							return ConvertMethodId.ToUInt32_string;
						case SystemTypeCode.Object:
							return ConvertMethodId.ToUInt32_object;
						case SystemTypeCode.DateTime:
							return ConvertMethodId.ToUInt32_DateTime;
					}
					break;

				case SystemTypeCode.Int64:
					switch (s.Code)
					{
						case SystemTypeCode.Boolean:
							return ConvertMethodId.ToInt64_bool;
						case SystemTypeCode.Char:
							return ConvertMethodId.ToInt64_char;
						case SystemTypeCode.Int8:
							return ConvertMethodId.ToInt64_sbyte;
						case SystemTypeCode.UInt8:
							return ConvertMethodId.ToInt64_byte;
						case SystemTypeCode.Int16:
							return ConvertMethodId.ToInt64_short;
						case SystemTypeCode.UInt16:
							return ConvertMethodId.ToInt64_ushort;
						case SystemTypeCode.Int32:
							return ConvertMethodId.ToInt64_int;
						case SystemTypeCode.UInt32:
							return ConvertMethodId.ToInt64_uint;
						case SystemTypeCode.Int64:
							return ConvertMethodId.ToInt64_long;
						case SystemTypeCode.UInt64:
							return ConvertMethodId.ToInt64_ulong;
						case SystemTypeCode.Single:
							return ConvertMethodId.ToInt64_float;
						case SystemTypeCode.Double:
							return ConvertMethodId.ToInt64_double;
						case SystemTypeCode.Decimal:
							return ConvertMethodId.ToInt64_decimal;
						case SystemTypeCode.String:
							return ConvertMethodId.ToInt64_string;
						case SystemTypeCode.Object:
							return ConvertMethodId.ToInt64_object;
						case SystemTypeCode.DateTime:
							return ConvertMethodId.ToInt64_DateTime;
					}
					break;

				case SystemTypeCode.UInt64:
					switch (s.Code)
					{
						case SystemTypeCode.Boolean:
							return ConvertMethodId.ToUInt64_bool;
						case SystemTypeCode.Char:
							return ConvertMethodId.ToUInt64_char;
						case SystemTypeCode.Int8:
							return ConvertMethodId.ToUInt64_sbyte;
						case SystemTypeCode.UInt8:
							return ConvertMethodId.ToUInt64_byte;
						case SystemTypeCode.Int16:
							return ConvertMethodId.ToUInt64_short;
						case SystemTypeCode.UInt16:
							return ConvertMethodId.ToUInt64_ushort;
						case SystemTypeCode.Int32:
							return ConvertMethodId.ToUInt64_int;
						case SystemTypeCode.UInt32:
							return ConvertMethodId.ToUInt64_uint;
						case SystemTypeCode.Int64:
							return ConvertMethodId.ToUInt64_long;
						case SystemTypeCode.UInt64:
							return ConvertMethodId.ToUInt64_ulong;
						case SystemTypeCode.Single:
							return ConvertMethodId.ToUInt64_float;
						case SystemTypeCode.Double:
							return ConvertMethodId.ToUInt64_double;
						case SystemTypeCode.Decimal:
							return ConvertMethodId.ToUInt64_decimal;
						case SystemTypeCode.String:
							return ConvertMethodId.ToUInt64_string;
						case SystemTypeCode.Object:
							return ConvertMethodId.ToUInt64_object;
						case SystemTypeCode.DateTime:
							return ConvertMethodId.ToUInt64_DateTime;
					}
					break;

				case SystemTypeCode.Single:
					switch (s.Code)
					{
						case SystemTypeCode.Boolean:
							return ConvertMethodId.ToSingle_bool;
						case SystemTypeCode.Char:
							return ConvertMethodId.ToSingle_char;
						case SystemTypeCode.Int8:
							return ConvertMethodId.ToSingle_sbyte;
						case SystemTypeCode.UInt8:
							return ConvertMethodId.ToSingle_byte;
						case SystemTypeCode.Int16:
							return ConvertMethodId.ToSingle_short;
						case SystemTypeCode.UInt16:
							return ConvertMethodId.ToSingle_ushort;
						case SystemTypeCode.Int32:
							return ConvertMethodId.ToSingle_int;
						case SystemTypeCode.UInt32:
							return ConvertMethodId.ToSingle_uint;
						case SystemTypeCode.Int64:
							return ConvertMethodId.ToSingle_long;
						case SystemTypeCode.UInt64:
							return ConvertMethodId.ToSingle_ulong;
						case SystemTypeCode.Single:
							return ConvertMethodId.ToSingle_float;
						case SystemTypeCode.Double:
							return ConvertMethodId.ToSingle_double;
						case SystemTypeCode.Decimal:
							return ConvertMethodId.ToSingle_decimal;
						case SystemTypeCode.String:
							return ConvertMethodId.ToSingle_string;
						case SystemTypeCode.Object:
							return ConvertMethodId.ToSingle_object;
						case SystemTypeCode.DateTime:
							return ConvertMethodId.ToSingle_DateTime;
					}
					break;

				case SystemTypeCode.Double:
					switch (s.Code)
					{
						case SystemTypeCode.Boolean:
							return ConvertMethodId.ToDouble_bool;
						case SystemTypeCode.Char:
							return ConvertMethodId.ToDouble_char;
						case SystemTypeCode.Int8:
							return ConvertMethodId.ToDouble_sbyte;
						case SystemTypeCode.UInt8:
							return ConvertMethodId.ToDouble_byte;
						case SystemTypeCode.Int16:
							return ConvertMethodId.ToDouble_short;
						case SystemTypeCode.UInt16:
							return ConvertMethodId.ToDouble_ushort;
						case SystemTypeCode.Int32:
							return ConvertMethodId.ToDouble_int;
						case SystemTypeCode.UInt32:
							return ConvertMethodId.ToDouble_uint;
						case SystemTypeCode.Int64:
							return ConvertMethodId.ToDouble_long;
						case SystemTypeCode.UInt64:
							return ConvertMethodId.ToDouble_ulong;
						case SystemTypeCode.Single:
							return ConvertMethodId.ToDouble_float;
						case SystemTypeCode.Double:
							return ConvertMethodId.ToDouble_double;
						case SystemTypeCode.Decimal:
							return ConvertMethodId.ToDouble_decimal;
						case SystemTypeCode.String:
							return ConvertMethodId.ToDouble_string;
						case SystemTypeCode.Object:
							return ConvertMethodId.ToDouble_object;
						case SystemTypeCode.DateTime:
							return ConvertMethodId.ToDouble_DateTime;
					}
					break;

				case SystemTypeCode.Decimal:
					switch (s.Code)
					{
						case SystemTypeCode.Boolean:
							return ConvertMethodId.ToDecimal_bool;
						case SystemTypeCode.Char:
							return ConvertMethodId.ToDecimal_char;
						case SystemTypeCode.Int8:
							return ConvertMethodId.ToDecimal_sbyte;
						case SystemTypeCode.UInt8:
							return ConvertMethodId.ToDecimal_byte;
						case SystemTypeCode.Int16:
							return ConvertMethodId.ToDecimal_short;
						case SystemTypeCode.UInt16:
							return ConvertMethodId.ToDecimal_ushort;
						case SystemTypeCode.Int32:
							return ConvertMethodId.ToDecimal_int;
						case SystemTypeCode.UInt32:
							return ConvertMethodId.ToDecimal_uint;
						case SystemTypeCode.Int64:
							return ConvertMethodId.ToDecimal_long;
						case SystemTypeCode.UInt64:
							return ConvertMethodId.ToDecimal_ulong;
						case SystemTypeCode.Single:
							return ConvertMethodId.ToDecimal_float;
						case SystemTypeCode.Double:
							return ConvertMethodId.ToDecimal_double;
						case SystemTypeCode.Decimal:
							return ConvertMethodId.ToDecimal_decimal;
						case SystemTypeCode.String:
							return ConvertMethodId.ToDecimal_string;
						case SystemTypeCode.Object:
							return ConvertMethodId.ToDecimal_object;
						case SystemTypeCode.DateTime:
							return ConvertMethodId.ToDecimal_DateTime;
					}
					break;
			}
			return ConvertMethodId.Unknown;
		}

		private static bool HasBaseExplicitImpl(this IMethod method)
		{
			if (method == null) return false;
			var ifaceMethod = method.GetExplicitImpl();
			if (ifaceMethod == null) return false;
			var declType = method.DeclaringType;
			if (declType.BaseType.FindImplementation(ifaceMethod, true) != null)
				return true;
			return false;
		}

		public static bool IsOverride(this IMethod method)
		{
			if (method.IsStatic) return false;
			if (method.IsConstructor) return false;
			if (method.IsAbstract) return false;

			if (method.IsVirtual)
			{
				if (method.IsNewSlot)
					return method.HasBaseExplicitImpl();

				return method.HasSameBaseMethod();
				//return true;
			}

			return false;
		}

		private static bool IsProtected(this Visibility x)
		{
			switch (x)
			{
				case Visibility.NestedProtected:
				case Visibility.NestedProtectedInternal:
				case Visibility.Protected:
				case Visibility.ProtectedInternal:
					return true;
			}
			return false;
		}

		private static bool AreEquals(Visibility x, Visibility y)
		{
			if (x == y) return true;
			if (x.IsProtected()) return y.IsProtected();
			return false;
		}

		private static bool HasSameBaseMethod(this IMethod method)
		{
			var bm = method.BaseMethod;
			return bm != null && ReferenceEquals(bm.Type, method.Type)
			       && AreEquals(bm.Visibility, method.Visibility);
		}

		public static bool IsObjectOverrideMethod(this IMethod method)
		{
			if (method.IsStatic) return false;
			switch (method.Name)
			{
				case Const.Object.MethodGetType:
				case Const.Object.MethodToString:
				case Const.Object.MethodGetHashCode:
					return method.Parameters.Count == 0;

				case Const.Object.MethodEquals:
					return method.Parameters.Count == 1;
			}
			return false;
		}

		public static bool IsStaticCall(this IMethod method)
		{
			return method.IsStatic || method.AsStaticCall();
		}

		public static bool AsStaticCall(this IMethod method)
		{
			if (method.IsStatic) return false;
			if (method.DeclaringType.Is(SystemTypeCode.String))
			{
				return !method.IsInternalCall;
			}
			return false;
		}

		public static bool ReturnsVoid(this IMethod m)
		{
			if (m == null) return false;
			if (m.AsStaticCall() && m.IsConstructor)
				return false;
			return m.IsVoid();
		}

		public static bool IsInstanceInitializer(this IMethod method)
		{
			if (method.IsStatic) return false;
			if (!method.IsConstructor) return false;
			var declType = method.DeclaringType;
			if (method.Parameters.Count > 0
			    && !InternalTypeExtensions.AllowNonParameterlessInitializer(declType))
				return false;
			return declType.HasSingleConstructor();
		}

		public static AbcTraitKind ResolveTraitKind(this IMethod method)
		{
			var prop = method.Association as IProperty;
			if (prop != null && prop.Parameters.Count == 0)
			{
				if (method == prop.Getter)
					return AbcTraitKind.Getter;
				if (method == prop.Setter)
					return AbcTraitKind.Setter;
			}
			return AbcTraitKind.Method;
		}

		public static IMethod FindImplementedMethod(this IMethod method)
		{
			var impl = method.ImplementedMethods;
			if (impl != null && impl.Length == 1)
				return impl[0];

			var bt = method.DeclaringType.BaseType;
			while (bt != null)
			{
#if DEBUG
				DebugService.DoCancel();
#endif
				var bm = bt.FindSameMethod(method, false);
				if (bm != null)
				{
					impl = bm.ImplementedMethods;
					if (impl != null && impl.Length == 1)
						return impl[0];
				}
				bt = bt.BaseType;
			}
			return null;
		}

		public static bool HasPseudoThis(this IMethod m)
		{
			return m.AsStaticCall() && !m.IsConstructor;
		}

		public static bool HasABCOrQNameAttribute(this ICustomAttributeProvider m)
		{
			if (m == null) return false;
			return m.CustomAttributes.Any(attr =>
				{
					string type = attr.TypeName;
					return type == Attrs.ABC || type == Attrs.QName;
				});
		}

		public static bool IsAbcMethod(this IMethod m)
		{
			if (m == null) return false;

			if (m.IsConstructor && m.IsInternalCall)
			{
				if (m.DeclaringType.HasABCOrQNameAttribute())
					return true;
			}

			return m.HasABCOrQNameAttribute();
		}

		public static bool IsManaged(this IMethod m)
		{
			return !m.IsAbcMethod() && m.IsManaged;
		}

		public static int GetPlayerVersion(this ITypeMember m)
		{
			if (m == null) return -1;

			var type = m.DeclaringType;
			if (type.Tag is IVectorType)
				return 10;

			foreach (var attr in m.CustomAttributes)
			{
				string name = attr.TypeName;
				switch (name)
				{
					case Attrs.FP:
						{
							string s = attr.Arguments[0].Value.ToString();
							int v;
							if (Int32.TryParse(s, out v))
								return v;
							return -1;
						}

					case Attrs.FP9: return 9;
					case Attrs.FP10: return 10;
				}
			}

			return -1;
		}
	}
}