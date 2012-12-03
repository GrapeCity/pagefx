//CHANGED

//
// System.SByte.cs
//
// Author:
// Miguel de Icaza (miguel@ximian.com)
//
// (C) Ximian, Inc. http://www.ximian.com
// Copyright (C) 2004 Novell (http://www.novell.com)
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

using System.Globalization;

namespace System
{
    [CLSCompliant(false)]
    [Serializable]
    public struct SByte : IFormattable, IConvertible, IComparable
#if NET_2_0
		, IComparable<SByte>, IEquatable <SByte>
#endif
    {
        public const sbyte MinValue = -128;
        public const sbyte MaxValue = 127;

        internal sbyte m_value;

        #region CompareTo, Equals, GetHashCode
        public int CompareTo(object v)
        {
            if (v == null)
                return 1;

            if (!(v is SByte))
                throw new ArgumentException(Locale.GetText("Value is not a System.SByte."));

            sbyte xv = (sbyte)v;
            if (m_value == xv)
                return 0;
            if (m_value > xv)
                return 1;
            return -1;
        }

        public override bool Equals(object o)
        {
            if (!(o is SByte))
                return false;

            return ((sbyte)o) == m_value;
        }

        public override int GetHashCode()
        {
            return m_value;
        }

        public int CompareTo(sbyte value)
        {
            if (m_value == value)
                return 0;
            if (m_value > value)
                return 1;
            return -1;
        }

        public bool Equals(sbyte value)
        {
            return value == m_value;
        }
        #endregion

        #region Parse
        internal static bool Parse(string s, bool tryParse, out sbyte result, out Exception exc)
        {
            int ival = 0;
            int len;
            int i;
            bool neg = false;
            bool digits_seen = false;

            result = 0;
            exc = null;

            if (s == null)
            {
                if (!tryParse)
                    exc = new ArgumentNullException("s");
                return false;
            }

            len = s.Length;

            char c;
            for (i = 0; i < len; i++)
            {
                c = s[i];
                if (!Char.IsWhiteSpace(c))
                    break;
            }

            if (i == len)
            {
                if (!tryParse)
                    exc = IntParser.GetFormatException();
                return false;
            }

            c = s[i];
            if (c == '+')
                i++;
            else if (c == '-')
            {
                neg = true;
                i++;
            }

            for (; i < len; i++)
            {
                c = s[i];

                if (c >= '0' && c <= '9')
                {
                    if (tryParse)
                    {
                        int intval = ival * 10 - (int)(c - '0');

                        if (intval < MinValue)
                            return false;
                        ival = (sbyte)intval;
                    }
                    else
                        ival = checked(ival * 10 - (int)(c - '0'));
                    digits_seen = true;
                }
                else
                {
                    if (Char.IsWhiteSpace(c))
                    {
                        for (i++; i < len; i++)
                        {
                            if (!Char.IsWhiteSpace(s[i]))
                            {
                                if (!tryParse)
                                    exc = IntParser.GetFormatException();
                                return false;
                            }
                        }
                        break;
                    }
                    else
                    {
                        if (!tryParse)
                            exc = IntParser.GetFormatException();
                        return false;
                    }
                }
            }
            if (!digits_seen)
            {
                if (!tryParse)
                    exc = IntParser.GetFormatException();
                return false;
            }

            ival = neg ? ival : -ival;
            if (ival < MinValue || ival > MaxValue)
            {
                if (!tryParse)
                    exc = new OverflowException();
                return false;
            }

            result = (sbyte)ival;
            return true;
        }

        [CLSCompliant(false)]
        public static sbyte Parse(string s, IFormatProvider provider)
        {
            return Parse(s, NumberStyles.Integer, provider);
        }

        [CLSCompliant(false)]
        public static sbyte Parse(string s, NumberStyles style)
        {
            return Parse(s, style, null);
        }

        [CLSCompliant(false)]
        public static sbyte Parse(string s, NumberStyles style, IFormatProvider provider)
        {
            int tmpResult = Int32.Parse(s, style, provider);
            if (tmpResult > SByte.MaxValue || tmpResult < SByte.MinValue)
                throw new OverflowException(Locale.GetText("Value too large or too small."));

            return (sbyte)tmpResult;
        }

        [CLSCompliant(false)]
        public static sbyte Parse(string s)
        {
            Exception exc;
            sbyte res;

            if (!Parse(s, false, out res, out exc))
                throw exc;

            return res;
        }

#if NET_2_0
		[CLSCompliant(false)]
		public static bool TryParse (string s, out sbyte result) 
		{
			Exception exc;
			if (!Parse (s, true, out result, out exc)) {
				result = 0;
				return false;
			}

			return true;
		}

		[CLSCompliant(false)]
		public static bool TryParse (string s, NumberStyles style, IFormatProvider provider, out sbyte result) 
		{
			int tmpResult;
			result = 0;

			if (!Int32.TryParse (s, style, provider, out tmpResult))
				return false;
			if (tmpResult > SByte.MaxValue || tmpResult < SByte.MinValue)
				return false;
				
			result = (sbyte)tmpResult;
			return true;
		}
#endif
        #endregion

        public override string ToString()
        {
            return avm.ToString(m_value);
            //return NumberFormatter.FormatGeneral(new NumberFormatter.NumberStore(m_value));
        }

        public string ToString(IFormatProvider provider)
        {
#if OLDNF
            return NumberFormatter.FormatGeneral(new NumberFormatter.NumberStore(m_value), provider);
#else
            return NumberFormatter.NumberToString(m_value, provider);
#endif
        }

        public string ToString(string format)
        {
            return ToString(format, null);
        }

        public string ToString(string format, IFormatProvider provider)
        {
            NumberFormatInfo nfi = NumberFormatInfo.GetInstance(provider);
            return NumberFormatter.NumberToString(format, m_value, nfi);
        }

        #region ICovnertible Methods
        public TypeCode GetTypeCode()
        {
            return TypeCode.SByte;
        }

        bool IConvertible.ToBoolean(IFormatProvider provider)
        {
            return Convert.ToBoolean(m_value);
        }

        byte IConvertible.ToByte(IFormatProvider provider)
        {
            return Convert.ToByte(m_value);
        }

        char IConvertible.ToChar(IFormatProvider provider)
        {
            return Convert.ToChar(m_value);
        }

        DateTime IConvertible.ToDateTime(IFormatProvider provider)
        {
            return Convert.ToDateTime(m_value);
        }

        decimal IConvertible.ToDecimal(IFormatProvider provider)
        {
            return Convert.ToDecimal(m_value);
        }

        double IConvertible.ToDouble(IFormatProvider provider)
        {
            return Convert.ToDouble(m_value);
        }

        short IConvertible.ToInt16(IFormatProvider provider)
        {
            return Convert.ToInt16(m_value);
        }

        int IConvertible.ToInt32(IFormatProvider provider)
        {
            return Convert.ToInt32(m_value);
        }

        long IConvertible.ToInt64(IFormatProvider provider)
        {
            return Convert.ToInt64(m_value);
        }

        sbyte IConvertible.ToSByte(IFormatProvider provider)
        {
            return m_value;
        }

        float IConvertible.ToSingle(IFormatProvider provider)
        {
            return Convert.ToSingle(m_value);
        }

        object IConvertible.ToType(Type conversionType, IFormatProvider provider)
        {
            return Convert.ToType(m_value, conversionType, provider);
        }

        ushort IConvertible.ToUInt16(IFormatProvider provider)
        {
            return Convert.ToUInt16(m_value);
        }

        uint IConvertible.ToUInt32(IFormatProvider provider)
        {
            return Convert.ToUInt32(m_value);
        }

        ulong IConvertible.ToUInt64(IFormatProvider provider)
        {
            return Convert.ToUInt64(m_value);
        }
        #endregion

        internal static sbyte AddOvf(sbyte x, sbyte y)
        {
            int z = x + y;
            if (z > sbyte.MaxValue || z < sbyte.MinValue)
                throw new OverflowException();
            return (sbyte)z;
        }

        internal static sbyte SubOvf(sbyte x, sbyte y)
        {
            int z = x - y;
            if (z > sbyte.MaxValue || z < sbyte.MinValue)
                throw new OverflowException();
            return (sbyte)z;
        }

        internal static sbyte MulOvf(sbyte x, sbyte y)
        {
            int z = Int32.MulOvf(x, y);
            if (z > sbyte.MaxValue || z < sbyte.MinValue)
                throw new OverflowException();
            return (sbyte)z;
        }
    }
}
