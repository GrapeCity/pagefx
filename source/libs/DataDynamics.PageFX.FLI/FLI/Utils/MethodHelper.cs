using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.FLI
{
    static class MethodHelper
    {
        #region GetConvertMethodName
        public static string GetConvertMethodName(IType target)
        {
            var st = target.SystemType;
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
        #endregion

        #region GetConvertMethodId
        public static ConvertMethodId GetConvertMethodId(IType source, IType target)
        {
            var t = target.SystemType;
            if (t == null) return ConvertMethodId.Unknown;
            var s = source.SystemType;
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
        #endregion

        public static IMethod FindImplementation(IType type, IMethod method, bool inherited)
        {
            if (method.IsGenericInstance)
                method = method.InstanceOf;

            while (type != null)
            {
            	var impl = (from candidate in type.Methods
            	            where candidate.ImplementedMethods != null &&
            	                  candidate.ImplementedMethods.Any(x => x == method || x.ProxyOf == method)
            	            select candidate).FirstOrDefault();
				if (impl != null)
				{
					return impl;
				}
                if (!inherited) break;
                type = type.BaseType;
            }

            return null;
        }

        public static IMethod FindImplementation(IType type, IMethod method)
        {
            return FindImplementation(type, method, true);
        }

        public static bool HasBaseExplicitImpl(IMethod method)
        {
            if (method == null) return false;
            var ifaceMethod = GetInterfaceOfExplicitImpl(method);
            if (ifaceMethod == null) return false;
            var declType = method.DeclaringType;
            if (FindImplementation(declType.BaseType, ifaceMethod, true) != null)
                return true;
            return false;
        }

        public static IMethod GetInterfaceOfExplicitImpl(IMethod method)
        {
            if (method == null) return null;
            if (!method.IsExplicitImplementation) return null;
            var impl = method.ImplementedMethods;
            if (impl == null || impl.Length != 1)
                throw new InvalidOperationException("bad explicit implementation");
            return impl[0];
        }

        public static bool IsNew(IMethod method)
        {
            if (method == null) return false;
            if (method.IsStatic) return false;
            if (method.IsConstructor) return false;
            if (method.IsVirtual)
            {
                if (method.IsNewSlot)
                {
                    if (method.IsExplicitImplementation)
                        return false;
                    return HasBaseDef(method);
                }
                return false;
            }
            return HasBaseDef(method);
        }

        static bool HasBaseDef(IMethod method)
        {
            if (method.BaseMethod != null)
                return true;
            if (method.Parameters.Count == 0)
            {
                switch (method.Visibility)
                {
                        case Visibility.Private:
                        case Visibility.NestedPrivate:
                            return false;
                }
                string name = method.Name;
                var bt = method.DeclaringType.BaseType;
                while (bt != null)
                {
                    var f = bt.Fields[name];
                    if (f != null)
                        return true;
                    bt = bt.BaseType;
                }
            }
            return false;
        }

        public static bool IsOverride(IMethod method)
        {
            if (method.IsStatic) return false;
            if (method.IsConstructor) return false;
            if (method.IsAbstract) return false;

            if (method.IsVirtual)
            {
                if (method.IsNewSlot)
                    return HasBaseExplicitImpl(method);

                return HasSameBaseMethod(method);
                //return true;
            }

            return false;
        }

        static bool IsProtected(Visibility x)
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

        static bool AreEquals(Visibility x, Visibility y)
        {
            if (x == y) return true;
            if (IsProtected(x)) return IsProtected(y);
            return false;
        }

        static bool HasSameBaseMethod(IMethod method)
        {
            var bm = method.BaseMethod;
            if (bm == null) return false;
            if (bm.Type != method.Type) return false;
            return AreEquals(bm.Visibility, method.Visibility);
        }

        public static bool IsObjectOverrideMethod(IMethod method)
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

        public static bool IsImplemented(IMethod method)
        {
            if (method.IsInternalCall) return false;
            switch (method.CodeType)
            {
                case MethodCodeType.Native:
                case MethodCodeType.Runtime:
                    return false;
            }
            return true;
        }

        public static bool IsStatic(IMethod m)
        {
            if (m.IsStatic)
                return true;
            if (AsStaticCall(m))
                return true;
            return false;
        }

        public static bool AsStaticCall(IMethod m)
        {
            if (m.IsStatic) return false;
            if (AbcGenConfig.UseAvmString)
            {
                if (m.DeclaringType == SystemTypes.String)
                {
                    if (m.IsInternalCall)
                        return false;
                    return true;
                }
            }
            return false;
        }

        public static bool IsVoid(IMethod m)
        {
            if (m == null) return false;
            if (AsStaticCall(m) && m.IsConstructor)
                return false;
            return TypeService.IsVoid(m);
        }

        public static bool IsInstanceInitializer(IMethod method)
        {
            if (method.IsStatic) return false;
            if (!method.IsConstructor) return false;
            var declType = method.DeclaringType;
            if (method.Parameters.Count > 0
                && !TypeHelper.AllowNonParameterlessInitializer(declType))
                return false;
            return TypeHelper.HasSingleConstructor(declType);
        }

        public static bool IsGetter(IMethod method)
        {
            var prop = method.Association as IProperty;
            if (prop == null) return false;
            if (prop.Getter != method) return false;
            if (prop.Parameters.Count > 0) return false;
            return true;
        }

        public static bool IsSetter(IMethod method)
        {
            var prop = method.Association as IProperty;
            if (prop == null) return false;
            if (prop.Setter != method) return false;
            if (prop.Parameters.Count > 0) return false;
            return true;
        }

        public static bool IsAccessor(IMethod method)
        {
            var prop = method.Association as IProperty;
            if (prop == null) return false;
            if (method.Association != prop) return false;
            if (prop.Parameters.Count > 0) return false;
            return true;
        }

        #region Find
        public static IMethod Find(IType type, string name)
        {
            var set = type.Methods[name];
			return set == null ? null : set.FirstOrDefault();
        }

        public static IMethod Find(IType type, string name, IType arg1)
        {
            return type.Methods[name, arg1];
        }

        public static IMethod Find(IType type, string name, IType arg1, IType arg2)
        {
            return type.Methods[name, arg1, arg2];
        }

        public static IMethod Find(IType type, string name, IType arg1, IType arg2, IType arg3)
        {
            return type.Methods[name, arg1, arg2, arg3];
        }

        public static IMethod Find(IType type, string name, int argc)
        {
            return type.Methods[name, argc];
        }

        public static IMethod Find(IType type, string name, int argc, Predicate<IParameterCollection> args)
        {
            return type.Methods[name,
                                m =>
                                    {
                                        var p= m.Parameters;
                                        if (p.Count != argc) return false;
                                        return args(p);
                                    }];
        }
        #endregion

        public static IMethod FindImplementedMethod(IMethod method)
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
                var bm = Method.FindMethod(bt, method, false);
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

        public static bool HasPseudoThis(IMethod m)
        {
            return AsStaticCall(m) && !m.IsConstructor;
        }

        public static bool HasABCOrQNameAttribute(ICustomAttributeProvider m)
        {
            if (m == null) return false;
            return m.CustomAttributes.Any(attr =>
                                          	{
                                          		string type = attr.TypeName;
                                          		return type == Attrs.ABC || type == Attrs.QName;
                                          	});
        }

        public static bool IsAbcMethod(IMethod m)
        {
            if (m == null) return false;

            if (m.IsConstructor && m.IsInternalCall)
            {
                if (HasABCOrQNameAttribute(m.DeclaringType))
                    return true;
            }

            return HasABCOrQNameAttribute(m);
        }

        public static bool IsManaged(IMethod m)
        {
            if (IsAbcMethod(m))
                return false;
            return m.IsManaged;
        }

        public static int GetPlayerVersion(ITypeMember m)
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
                            if (int.TryParse(s, out v))
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