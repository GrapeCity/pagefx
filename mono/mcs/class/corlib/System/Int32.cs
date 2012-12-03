//CHANGED
//
// System.Int32.cs
//
// Author:
//   Miguel de Icaza (miguel@ximian.com)
//
// (C) Ximian, Inc.  http://www.ximian.com
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

using System.Globalization;


namespace System
{
    [Serializable]
    public struct Int32 : IFormattable, IConvertible, IComparable
#if NET_2_0
		, IComparable<Int32>, IEquatable <Int32>
#endif
    {
        public const int MaxValue = 0x7fffffff;
        public const int MinValue = -2147483648;

        // This field is looked up by name in the runtime
        internal int m_value;

        #region CompareTo, Equals, GetHashCode
        public int CompareTo(object v)
        {
            if (v == null)
                return 1;

            if (!(v is Int32))
                throw new ArgumentException(Locale.GetText("Value is not a System.Int32"));

            int xv = (int)v;
            if (m_value == xv)
                return 0;
            if (m_value > xv)
                return 1;
            return -1;
        }

        public override bool Equals(object o)
        {
            if (o is Int32)
                return ((int)o) == m_value;
            return false;
        }

        public override int GetHashCode()
        {
            return m_value;
        }

        //#if NET_2_0
        public int CompareTo(int value)
        {
            if (m_value == value)
                return 0;
            if (m_value > value)
                return 1;
            else
                return -1;
        }

        public bool Equals(int value)
        {
            return value == m_value;
        }
        //#endif
        #endregion

        #region Parse
        internal static bool Parse(string s, bool tryParse, out int result, out Exception exc)
        {
            int val = 0;
            int len;
            int i, sign = 1;
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
                sign = -1;
                i++;
            }

            for (; i < len; i++)
            {
                c = s[i];

                if (c == '\0')
                {
                    i = len;
                    continue;
                }

                if (c >= '0' && c <= '9')
                {
                    byte d = (byte)(c - '0');

                    val = val * 10 + d * sign;
                    if (IntParser.IsOverflow(val, sign))
                    {
                        if (!tryParse)
                            exc = IntParser.GetOverflowException();
                        return false;
                    }

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

            result = val;

            return true;
        }

        public static int Parse(string s, IFormatProvider fp)
        {
            return Parse(s, NumberStyles.Integer, fp);
        }

        public static int Parse(string s, NumberStyles style)
        {
            return Parse(s, style, null);
        }

        private class Num : IntParser.INumber
        {
            public uint hex;
            public int number;
            public int sign;

            #region INumber Members
            public bool IsHexOverflow()
            {
                return IntParser.IsHexOverflow(hex);
            }

            public void AddHexDigit(uint d)
            {
                hex = hex * 16u + d;
            }

            public bool IsOverflow()
            {
                return IntParser.IsOverflow(number, sign);
            }

            public void AddDigit(int d)
            {
                number = (number * 10) + (sign * d);
            }

            public bool CatchOverflowException
            {
                get { return false; }
            }

            public void BeforeLoop(IntParser parser)
            {
                sign = parser.negative ? -1 : 1;
            }

            public void AfterLoop(IntParser parser)
            {
                if (parser.AllowHexSpecifier)
                {
                    number = (int)hex;
                }
            }

            public bool CheckNegative()
            {
                return true;
            }
            #endregion
        }

        internal static bool Parse(string s, NumberStyles style, IFormatProvider fp, bool tryParse, out int result, out Exception exc)
        {
            result = 0;

            if (!IntParser.CheckInput(s, style, tryParse, out exc))
                return false;

            IntParser parser = new IntParser(s, style, fp, tryParse);
            Num n = new Num();

            if (!parser.Start(n))
            {
                exc = parser.exc;
                return false;
            }

            result = n.number;

            return true;
        }

        public static int Parse(string s)
        {
            Exception exc;
            int res;

            if (!Parse(s, false, out res, out exc))
                throw exc;

            return res;
        }

        public static int Parse(string s, NumberStyles style, IFormatProvider fp)
        {
            Exception exc;
            int res;

            if (!Parse(s, style, fp, false, out res, out exc))
                throw exc;

            return res;
        }

#if NET_2_0
		public static bool TryParse (string s, out int result) 
		{
			Exception exc;
			
			if (!Parse (s, true, out result, out exc)) {
				result = 0;
				return false;
			}

			return true;
		}

		public static bool TryParse (string s, NumberStyles style, IFormatProvider provider, out int result) 
		{
			Exception exc;
			if (!Parse (s, style, provider, true, out result, out exc)) {
				result = 0;
				return false;
			}

			return true;
		}
#endif
        #endregion

        public override string ToString()
        {
            return avm.ToString(m_value);
            //return ToString(null, null);
        }

        public string ToString(IFormatProvider fp)
        {
            if (fp == null)
                return ToString();
            return ToString(null, fp);
        }

        public string ToString(string format)
        {
            if (string.IsNullOrEmpty(format))
                return ToString();
            return ToString(format, null);
        }

        public string ToString(string format, IFormatProvider fp)
        {
            if (format == null && fp == null)
                return ToString();
            NumberFormatInfo nfi = NumberFormatInfo.GetInstance(fp);
            return NumberFormatter.NumberToString(format, m_value, nfi);
        }

        #region IConvertible Methods
        public TypeCode GetTypeCode()
        {
            return TypeCode.Int32;
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
            return m_value;
        }

        long IConvertible.ToInt64(IFormatProvider provider)
        {
            return Convert.ToInt64(m_value);
        }

        sbyte IConvertible.ToSByte(IFormatProvider provider)
        {
            return Convert.ToSByte(m_value);
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

        #region Arithmetic with Overflow Checking
        private const uint SIGN = 0x80000000;

        internal static int AddOvf(int x, int y)
        {
            uint s1 = (uint)x & SIGN;
            uint s2 = (uint)y & SIGN;
            x = x + y;
            if (s1 == s2 && ((uint)x & SIGN) != s1)
                throw new OverflowException();
            return x;
        }

        internal static int SubOvf(int x, int y)
        {
            uint s1 = (uint)x & SIGN;
            uint s2 = (uint)y & SIGN;
            x = x - y;
            if (s1 != s2 && ((uint)x & SIGN) != s1)
                throw new OverflowException();
            return x;
        }

        internal static int MulOvf(int x, int y)
        {
            //TODO: Optimize code below
            if (y < 0 && x == int.MinValue)
                throw new OverflowException();

            int z = x * y;
            if (y != 0 && z / y != x)
                throw new OverflowException();

            return z;
        }
        #endregion
    }
}
