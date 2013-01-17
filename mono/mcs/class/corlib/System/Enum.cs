//CHANGED
//
// System.Enum.cs
//
// Authors:
//   Miguel de Icaza (miguel@ximian.com)
//   Nick Drochak (ndrochak@gol.com)
//   Gonzalo Paniagua Javier (gonzalo@ximian.com)
//
// (C) Ximian, Inc.  http://www.ximian.com
//
//

//
// Copyright (C) 2004 Novell, Inc (http://www.novell.com)
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System.Collections;
using System.Globalization;
using Native;

namespace System
{
    internal class EnumInfo
    {
        public bool flags;
        public object[] values;
        public string[] names;
    }

    [Serializable]
    public abstract class Enum : ValueType, IComparable, IConvertible, IFormattable
    {
        #region IConvertible Methods
        public TypeCode GetTypeCode()
        {
            return Type.GetTypeCode(GetValueType());
        }

        bool IConvertible.ToBoolean(IFormatProvider provider)
        {
            return Convert.ToBoolean(GetValue(), provider);
        }

        byte IConvertible.ToByte(IFormatProvider provider)
        {
            return Convert.ToByte(GetValue(), provider);
        }

        char IConvertible.ToChar(IFormatProvider provider)
        {
            return Convert.ToChar(GetValue(), provider);
        }

        DateTime IConvertible.ToDateTime(IFormatProvider provider)
        {
            return Convert.ToDateTime(GetValue(), provider);
        }

        decimal IConvertible.ToDecimal(IFormatProvider provider)
        {
            return Convert.ToDecimal(GetValue(), provider);
        }

        double IConvertible.ToDouble(IFormatProvider provider)
        {
            return Convert.ToDouble(GetValue(), provider);
        }

        short IConvertible.ToInt16(IFormatProvider provider)
        {
            return Convert.ToInt16(GetValue(), provider);
        }

        int IConvertible.ToInt32(IFormatProvider provider)
        {
            return Convert.ToInt32(GetValue(), provider);
        }

        long IConvertible.ToInt64(IFormatProvider provider)
        {
            return Convert.ToInt64(GetValue(), provider);
        }

        sbyte IConvertible.ToSByte(IFormatProvider provider)
        {
            return Convert.ToSByte(GetValue(), provider);
        }

        float IConvertible.ToSingle(IFormatProvider provider)
        {
            return Convert.ToSingle(GetValue(), provider);
        }

        object IConvertible.ToType(Type conversionType, IFormatProvider provider)
        {
            return Convert.ToType(GetValue(), conversionType, provider);
        }

        ushort IConvertible.ToUInt16(IFormatProvider provider)
        {
            return Convert.ToUInt16(GetValue(), provider);
        }

        uint IConvertible.ToUInt32(IFormatProvider provider)
        {
            return Convert.ToUInt32(GetValue(), provider);
        }

        ulong IConvertible.ToUInt64(IFormatProvider provider)
        {
            return Convert.ToUInt64(GetValue(), provider);
        }
        #endregion

        internal Type GetValueType()
        {
            return GetType().UnderlyingSystemType;
        }

        #region GetValue/SetValue

		internal object GetValue()
		{
			object v = avm.get_m_value(this);
			Type vtype = GetValueType();
			Function f = vtype.m_box;
			if (f != null)
				return f.call(null, v);
			f = vtype.m_copy; //int64
			return f.call(null, v);
		}

        internal void SetValue(object value)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            Enum e = value as Enum;
            if (e != null)
                value = e.GetValue();

            Type vtype = GetValueType();
            try
            {
                Function f = vtype.m_unbox;
                if (f != null)
                {
                    object v = f.call(null, value);
                    avm.set_m_value(this, v);
                    return;
                }

                avm.set_m_value(this, value);
            }
            catch (Exception)
            {
                throw new InvalidCastException("Value type is " + value.GetType()
                                               + ", but enum value type is " + vtype);
            }
        }

		#endregion

        internal int ValueTypeKind
        {
            get { return GetValueType().kind; }
        }

        internal static Array GetValues(Type enumType)
        {
            if (enumType == null)
                throw new ArgumentNullException("enumType");

            if (!enumType.IsEnum)
                throw new ArgumentException("enumType is not an Enum type.");

            EnumInfo info = enumType.EnumInfo;
            if (info == null)
                throw new ArgumentException("enumInfo is null");

            return info.values;
        }

        internal static string[] GetNames(Type enumType)
        {
            if (enumType == null)
                throw new ArgumentNullException("enumType");

            if (!enumType.IsEnum)
                throw new ArgumentException("enumType is not an Enum type.");

            EnumInfo info = enumType.EnumInfo;
            if (info == null)
                throw new ArgumentException("enumInfo is null");

            return info.names;
        }

        public static string GetName(Type enumType, object value)
        {
            if (enumType == null)
                throw new ArgumentNullException("enumType");
            if (value == null)
                throw new ArgumentNullException("value");
            if (!enumType.IsEnum)
                throw new ArgumentException("enumType is not an Enum type.");

            Type vType = value.GetType();
            if (vType != enumType && vType != enumType.UnderlyingSystemType)
                throw new ArgumentException("value is not a value of given enum");

            EnumInfo info = enumType.EnumInfo;
            if (info == null)
                throw new ArgumentException("enumInfo is null");

            int n = info.values.Length;
            for (int i = 0; i < n; ++i)
            {
                if (Equals(info.values[i], value))
                    return info.names[i];
            }

            return null;
        }

        public static bool IsDefined(Type enumType, object value)
        {
            if (enumType == null)
                throw new ArgumentNullException("enumType");
            if (value == null)
                throw new ArgumentNullException("value");

            if (!enumType.IsEnum)
                throw new ArgumentException("enumType is not an Enum type.");

            EnumInfo info = enumType.EnumInfo;
            if (info == null)
                throw new ArgumentException("enumInfo is null");

            Type vType = value.GetType();
            if (vType == typeof(String))
            {
                return ((IList)(info.names)).Contains(value);
            }
            if ((vType.index == enumType.utype) || (vType == enumType))
            {
                value = ToObject(enumType, value);
                int n = info.values.Length;
                for (int i = 0; i < n; ++i)
                {
                    if (Equals(info.values[i], value))
                        return true;
                }
                return false;
            }
            else
            {
                throw new ArgumentException("The value parameter is not the correct type."
                    + "It must be type String or the same type as the underlying type"
                    + "of the Enum.");
            }
        }

        public static Type GetUnderlyingType(Type enumType)
        {
            if (enumType == null)
                throw new ArgumentNullException("enumType");

            if (!enumType.IsEnum)
                throw new ArgumentException("enumType is not an Enum type.");

            return enumType.UnderlyingSystemType;
        }

        //NOTE: Used in System.Data
        public static object Parse(Type enumType, string value)
        {
            // Note: Parameters are checked in the other overload
            return Parse(enumType, value, false);
        }

        private static int FindName(string[] names, string name, bool ignoreCase)
        {
            for (int i = 0; i < names.Length; ++i)
            {
                if (String.Compare(name, names[i], ignoreCase, CultureInfo.InvariantCulture) == 0)
                    return i;
            }
            return -1;
        }

        // Helper function for dealing with [Flags]-style enums.
        private static ulong GetValue(object value, TypeCode typeCode)
        {
            switch (typeCode)
            {
                case TypeCode.Byte:
                    return (byte)value;
                case TypeCode.SByte:
                    return (ulong)((sbyte)value);
                case TypeCode.Int16:
                    return (ulong)((short)value);
                case TypeCode.UInt16:
                    return (ulong)value;
                case TypeCode.Int32:
                    return (ulong)((int)value);
                case TypeCode.UInt32:
                    return (ulong)value;
                case TypeCode.Int64:
                    return (ulong)((long)value);
                case TypeCode.UInt64:
                    return (ulong)value;
            }
            throw new ArgumentException("typeCode is not a valid type code for an Enum");
        }

        private static readonly char[] split_char = { ',' };

        private static TypeCode GetVTypeCode(Type enumType)
        {
            return Type.GetTypeCode(enumType.UnderlyingSystemType);
        }

        public static object Parse(Type enumType, string value, bool ignoreCase)
        {
            if (enumType == null)
                throw new ArgumentNullException("enumType");

            if (value == null)
                throw new ArgumentNullException("value");

            if (!enumType.IsEnum)
                throw new ArgumentException("not an Enum type", "enumType");

            EnumInfo info = enumType.EnumInfo;
            if (info == null)
                throw new ArgumentException("EnumInfo is null");

            value = value.Trim();
            if (value.Length == 0)
                throw new ArgumentException("cannot be an empty string", "value");

            // is 'value' a named constant?
            int loc = FindName(info.names, value, ignoreCase);
            if (loc >= 0)
                return info.values[loc];

            TypeCode typeCode = GetVTypeCode(enumType);

            // is 'value' a list of named constants?
            if (value.IndexOf(',') != -1)
            {
                string[] names = value.Split(split_char);
                ulong num = 0;
                int n = names.Length;
                for (int i = 0; i < n; ++i)
                {
                    loc = FindName(info.names, names[i].Trim(), ignoreCase);
                    if (loc < 0)
                        throw new ArgumentException("The requested value was not found.");
                    num |= GetValue(info.values[loc], typeCode);
                }
                return ToObject(enumType, FixNumber(num, typeCode));
            }

            // is 'value' a number?
            try
            {
                return ToObject(enumType, ParseValue(value, typeCode));
            }
            catch (Exception e)
            {
                throw new ArgumentException(String.Format("The requested value `{0}' was not found", value), e);
            }
        }

        private static object FixNumber(ulong value, TypeCode typeCode)
        {
            switch (typeCode)
            {
                case TypeCode.Byte:
                    return (byte)value;
                case TypeCode.SByte:
                    return (sbyte)value;
                case TypeCode.Int16:
                    return (short)value;
                case TypeCode.UInt16:
                    return (ushort)value;
                case TypeCode.Int32:
                    return (int)value;
                case TypeCode.UInt32:
                    return (uint)value;
                case TypeCode.Int64:
                    return (long)value;
                case TypeCode.UInt64:
                    return value;
            }
            throw new ArgumentException("typeCode is not a valid type code for an Enum");
        }

        private static object ParseValue(string s, TypeCode typeCode)
        {
            switch (typeCode)
            {
                case TypeCode.Byte:
                    return byte.Parse(s);
                case TypeCode.SByte:
                    return sbyte.Parse(s);
                case TypeCode.Int16:
                    return short.Parse(s);
                case TypeCode.UInt16:
                    return ushort.Parse(s);
                case TypeCode.Int32:
                    return int.Parse(s);
                case TypeCode.UInt32:
                    return uint.Parse(s);
                case TypeCode.Int64:
                    return long.Parse(s);
                case TypeCode.UInt64:
                    return ulong.Parse(s);
            }
            throw new ArgumentException("typeCode is not a valid type code for an Enum");
        }

        /// <summary>
        ///   Compares the enum value with another enum value of the same type.
        /// </summary>
        ///
        /// <remarks/>
        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;

            Type thisType = GetType();
            Type objType = obj.GetType();
            if (objType != thisType)
            {
                throw new ArgumentException(
                    "Object must be the same type as the "
                    + "enum. The type passed in was "
                    + objType
                    + "; the enum type was "
                    + thisType + ".");
            }

            object value1 = GetValue();
            object value2 = ((Enum)obj).GetValue();

            return ((IComparable)value1).CompareTo(value2);
        }

        #region ToString
        public override string ToString()
        {
            return ToString("G");
        }

#if NET_2_0
	[Obsolete("Provider is ignored, just use ToString")]
#endif
        public string ToString(IFormatProvider provider)
        {
            return ToString("G", provider);
        }

        public string ToString(String format)
        {
            if (string.IsNullOrEmpty(format))
                format = "G";

            return Format(GetType(), GetValue(), format);
        }

#if NET_2_0
		[Obsolete("Provider is ignored, just use ToString")]
#endif
        public string ToString(String format, IFormatProvider provider)
        {
            // provider is not used for Enums
            if (string.IsNullOrEmpty(format))
                format = "G";
            return Format(GetType(), GetValue(), format);
        }
        #endregion

        #region ToObject
        internal static object ToObject(Type enumType, byte value)
        {
            return ToObject(enumType, (object)value);
        }

        internal static object ToObject(Type enumType, short value)
        {
            return ToObject(enumType, (object)value);
        }

        internal static object ToObject(Type enumType, int value)
        {
            return ToObject(enumType, (object)value);
        }

        internal static object ToObject(Type enumType, long value)
        {
            return ToObject(enumType, (object)value);
        }

        public static object ToObject(Type enumType, object value)
        {
            if (enumType == null)
                throw new ArgumentNullException("enumType");
            if (!enumType.IsEnum)
                throw new ArgumentException("Argument must be enum", "enumType");
            Enum e = (Enum)enumType.CreateInstance();
            if (e == null)
                throw new InvalidOperationException();
            e.SetValue(value);
            return e;
        }

        [CLSCompliant(false)]
        internal static object ToObject(Type enumType, sbyte value)
        {
            return ToObject(enumType, (object)value);
        }

        [CLSCompliant(false)]
        internal static object ToObject(Type enumType, ushort value)
        {
            return ToObject(enumType, (object)value);
        }

        [CLSCompliant(false)]
        internal static object ToObject(Type enumType, uint value)
        {
            return ToObject(enumType, (object)value);
        }

        [CLSCompliant(false)]
        internal static object ToObject(Type enumType, ulong value)
        {
            return ToObject(enumType, (object)value);
        }
        #endregion

        public override int GetHashCode()
        {
            object v = GetValue();
            return v.GetHashCode();
        }

        public override bool Equals(object o)
        {
            object v = GetValue();
            if (Equals(v, o)) return true;
            Enum e = o as Enum;
            if (e == null) return false;
            if (e.GetType() != GetType()) return false;
            return Equals(v, e.GetValue());
        }

        #region Format
        private static string FormatHex(Type enumType, object value, bool upper)
        {
            switch (GetVTypeCode(enumType))
            {
                case TypeCode.SByte:
                    return ((sbyte)value).ToString(upper ? "X2" : "x2");
                case TypeCode.Byte:
                    return ((byte)value).ToString(upper ? "X2" : "x2");
                case TypeCode.Int16:
                    return ((short)value).ToString(upper ? "X4" : "x4");
                case TypeCode.UInt16:
                    return ((ushort)value).ToString(upper ? "X4" : "x4");
                case TypeCode.Int32:
                    return ((int)value).ToString(upper ? "X8" : "x8");
                case TypeCode.UInt32:
                    return ((uint)value).ToString(upper ? "X8" : "x8");
                case TypeCode.Int64:
                    return ((long)value).ToString(upper ? "X16" : "x16");
                case TypeCode.UInt64:
                    return ((ulong)value).ToString(upper ? "X16" : "x16");
                default:
                    throw new Exception("Invalid type code for enumeration.");
            }
        }

        static string FormatFlags(Type enumType, object value)
        {
            EnumInfo info = enumType.EnumInfo;
            if (info == null)
                throw new ArgumentException("enumInfo is null");

            string asString = value.ToString();
            if (asString == "0")
            {
                string name = GetName(enumType, value);
                if (name == null)
                    return asString;
                return name;
            }

            string retVal = "";
            int n = info.values.Length;

            TypeCode typeCode = Type.GetTypeCode(enumType.UnderlyingSystemType);

            // This is ugly, yes.  We need to handle the different integer
            // types for enums.  If someone else has a better idea, be my guest.
            switch (typeCode)
            {
                case TypeCode.SByte:
                    {
                        sbyte flags = (sbyte)value;
                        for (int i = n - 1; i >= 0; i--)
                        {
                            sbyte enumValue = (sbyte)info.values[i];
                            if (enumValue == 0) continue;

                            if ((flags & enumValue) == enumValue)
                            {
                                if (retVal.Length > 0)
                                    retVal = info.names[i] + ", " + retVal;
                                else
                                    retVal = info.names[i];
                                flags -= enumValue;
                            }
                        }
                        if (flags != 0) return asString;
                    }
                    break;

                case TypeCode.Byte:
                    {
                        byte flags = (byte)value;
                        for (int i = n - 1; i >= 0; i--)
                        {
                            byte enumValue = (byte)info.values[i];
                            if (enumValue == 0) continue;

                            if ((flags & enumValue) == enumValue)
                            {
                                if (retVal.Length > 0)
                                    retVal = info.names[i] + ", " + retVal;
                                else
                                    retVal = info.names[i];
                                flags -= enumValue;
                            }
                        }
                        if (flags != 0) return asString;
                    }
                    break;

                case TypeCode.Int16:
                    {
                        short flags = (short)value;
                        for (int i = n - 1; i >= 0; i--)
                        {
                            short enumValue = (short)info.values[i];
                            if (enumValue == 0) continue;

                            if ((flags & enumValue) == enumValue)
                            {
                                if (retVal.Length > 0)
                                    retVal = info.names[i] + ", " + retVal;
                                else
                                    retVal = info.names[i];
                                flags -= enumValue;
                            }
                        }
                        if (flags != 0) return asString;
                    }
                    break;

                case TypeCode.Int32:
                    {
                        int flags = (int)value;
                        for (int i = n - 1; i >= 0; i--)
                        {
                            int enumValue = (int)info.values[i];
                            if (enumValue == 0) continue;

                            if ((flags & enumValue) == enumValue)
                            {
                                if (retVal.Length > 0)
                                    retVal = info.names[i] + ", " + retVal;
                                else
                                    retVal = info.names[i];
                                flags -= enumValue;
                            }
                        }
                        if (flags != 0) return asString;
                    }
                    break;

                case TypeCode.UInt16:
                    {
                        ushort flags = (ushort)value;
                        for (int i = n - 1; i >= 0; i--)
                        {
                            ushort enumValue = (ushort)info.values[i];
                            if (enumValue == 0) continue;

                            if ((flags & enumValue) == enumValue)
                            {
                                if (retVal.Length > 0)
                                    retVal = info.names[i] + ", " + retVal;
                                else
                                    retVal = info.names[i];
                                flags -= enumValue;
                            }
                        }
                        if (flags != 0) return asString;
                    }
                    break;

                case TypeCode.UInt32:
                    {
                        uint flags = (uint)value;
                        for (int i = n - 1; i >= 0; i--)
                        {
                            uint enumValue = (uint)info.values[i];
                            if (enumValue == 0) continue;

                            if ((flags & enumValue) == enumValue)
                            {
                                if (retVal.Length > 0)
                                    retVal = info.names[i] + ", " + retVal;
                                else
                                    retVal = info.names[i];
                                flags -= enumValue;
                            }
                        }
                        if (flags != 0) return asString;
                    }
                    break;

                case TypeCode.Int64:
                    {
                        long flags = (long)value;
                        for (int i = n - 1; i >= 0; i--)
                        {
                            long enumValue = (long)info.values[i];
                            if (enumValue == 0) continue;

                            if ((flags & enumValue) == enumValue)
                            {
                                if (retVal.Length > 0)
                                    retVal = info.names[i] + ", " + retVal;
                                else
                                    retVal = info.names[i];
                                flags -= enumValue;
                            }
                        }
                        if (flags != 0) return asString;
                    }
                    break;

                case TypeCode.UInt64:
                    {
                        ulong flags = (ulong)value;
                        for (int i = n - 1; i >= 0; i--)
                        {
                            ulong enumValue = (ulong)info.values[i];
                            if (enumValue == 0) continue;
                            
                            if ((flags & enumValue) == enumValue)
                            {
                                if (retVal.Length > 0)
                                    retVal = info.names[i] + ", " + retVal;
                                else
                                    retVal = info.names[i];
                                flags -= enumValue;
                            }
                        }
                        if (flags != 0) return asString;
                    }
                    break;
            }

            if (retVal == "")
                return asString;

            return retVal;
        }

        internal static string Format(Type enumType, object value, string format)
        {
            if (enumType == null)
                throw new ArgumentNullException("enumType");
            if (value == null)
                throw new ArgumentNullException("value");
            if (format == null)
                throw new ArgumentNullException("format");

            if (!enumType.IsEnum)
                throw new ArgumentException("Type provided must be an Enum.");

            Type vType = value.GetType();
            Type underlyingType = GetUnderlyingType(enumType);
            if (vType.IsEnum)
            {
                if (vType != enumType)
                    throw new ArgumentException(string.Format("Object must be the same type as the enum. The type" +
                                                              " passed in was {0}; the enum type was {1}.",
                                                              vType.FullName, enumType.FullName));
            }
            else if (vType != underlyingType)
            {
                throw new ArgumentException(string.Format("Enum underlying type and the object must be the same type" +
                                                          " or object. Type passed in was {0}; the enum underlying" +
                                                          " type was {1}.", vType.FullName, underlyingType.FullName));
            }

            if (format.Length != 1)
                throw new FormatException("Format String can be only \"G\",\"g\",\"X\"," +
                                          "\"x\",\"F\",\"f\",\"D\" or \"d\".");

            Enum e = value as Enum;
            if (e != null)
                value = e.GetValue();

            char formatChar = format[0];
            string retVal;
            if ((formatChar == 'G' || formatChar == 'g'))
            {
                EnumInfo info = enumType.EnumInfo;
                if (info == null)
                    throw new ArgumentException("enumInfo is null");
                if (!info.flags)
                {
                    retVal = GetName(enumType, value);
                    if (retVal == null)
                        retVal = value.ToString();

                    return retVal;
                }

                formatChar = 'f';
            }

            if ((formatChar == 'f' || formatChar == 'F'))
                return FormatFlags(enumType, value);

            switch (formatChar)
            {
                case 'X':
                    retVal = FormatHex(enumType, value, true);
                    break;
                case 'x':
                    retVal = FormatHex(enumType, value, false);
                    break;
                case 'D':
                case 'd':
                    if (underlyingType == typeof(ulong))
                    {
                        ulong ulongValue = Convert.ToUInt64(value);
                        retVal = ulongValue.ToString();
                    }
                    else
                    {
                        long longValue = Convert.ToInt64(value);
                        retVal = longValue.ToString();
                    }
                    break;
                default:
                    throw new FormatException("Format String can be only \"G\",\"g\",\"X\"," +
                                              "\"x\",\"F\",\"f\",\"D\" or \"d\".");
            }
            return retVal;
        }
        #endregion
    }
}
