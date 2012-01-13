//CHANGED
//
// System.UInt32.cs
//
// Author:
//   Miguel de Icaza (miguel@ximian.com)
//
// (C) Ximian, Inc.  http://www.ximian.com
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
    [Serializable]
    [CLSCompliant(false)]
    public struct UInt32 : IFormattable, IConvertible, IComparable
#if NET_2_0
		, IComparable<UInt32>, IEquatable <UInt32>
#endif
    {
        public const uint MaxValue = 0xffffffff;
        public const uint MinValue = 0;

        internal uint m_value;

        #region CompareTo, Equals, GetHashCode
        public int CompareTo(object value)
        {
            if (value == null)
                return 1;

            if (!(value is UInt32))
                throw new ArgumentException(Locale.GetText("Value is not a System.UInt32."));

            uint v = (uint)value;
            if (m_value == v)
                return 0;

            if (m_value < v)
                return -1;

            return 1;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is UInt32))
                return false;

            return ((uint)obj) == m_value;
        }

        public override int GetHashCode()
        {
            return (int)m_value;
        }

        public int CompareTo(uint value)
        {
            if (m_value == value)
                return 0;
            if (m_value > value)
                return 1;
            return -1;
        }

        public bool Equals(uint value)
        {
            return value == m_value;
        }
        #endregion

        #region Parse
        internal static bool Parse(string s, bool tryParse, out uint result, out Exception exc)
        {
            uint val = 0;
            int len;
            int i;
            bool digits_seen = false;
            bool has_negative_sign = false;

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

            if (s[i] == '+')
                i++;
            else
                if (s[i] == '-')
                {
                    i++;
                    has_negative_sign = true;
                }

            for (; i < len; i++)
            {
                c = s[i];

                if (c >= '0' && c <= '9')
                {
                    uint d = (uint)(c - '0');

                    ulong v = ((ulong)val) * 10 + d;
                    if (v > MaxValue)
                    {
                        if (!tryParse)
                            exc = IntParser.GetOverflowException();
                        return false;
                    }
                    val = (uint)v;
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

            // -0 is legal but other negative values are not
            if (has_negative_sign && (val > 0))
            {
                if (!tryParse)
                    exc = new OverflowException(
                        Locale.GetText("Negative number"));
                return false;
            }

            result = val;
            return true;
        }

        private class Num : IntParser.INumber
        {
            private bool ovf;
            public uint number;

            #region INumber Members
            public bool IsHexOverflow()
            {
                return IntParser.IsHexOverflow(number);
            }

            public void AddHexDigit(uint d)
            {
                number = number * 16u + d;
            }

            public bool IsOverflow()
            {
                return ovf;
            }

            public void AddDigit(int d)
            {
                uint a = number * 10;
                uint b = (uint)d;
                number = a + b;
                ovf = number < a;
            }

            public bool CatchOverflowException
            {
                get { return false; }
            }

            public void BeforeLoop(IntParser parser)
            {
            }

            public void AfterLoop(IntParser parser)
            {
            }

            public bool CheckNegative()
            {
                // -0 is legal but other negative values are not
                if (number > 0)
                    return false;
                return true;
            }
            #endregion
        }

        internal static bool Parse(string s, NumberStyles style, IFormatProvider provider, bool tryParse, out uint result, out Exception exc)
        {
            result = 0;

            if (!IntParser.CheckInput(s, style, tryParse, out exc))
                return false;

            IntParser parser = new IntParser(s, style, provider, tryParse);
            Num n = new Num();
            if (!parser.Start(n))
            {
                exc = parser.exc;
                return false;
            }

            result = n.number;

            return true;
        }

        [CLSCompliant(false)]
        public static uint Parse(string s)
        {
            Exception exc;
            uint res;

            if (!Parse(s, false, out res, out exc))
                throw exc;

            return res;
        }

        [CLSCompliant(false)]
        public static uint Parse(string s, NumberStyles style, IFormatProvider fp)
        {
            Exception exc;
            uint res;

            if (!Parse(s, style, fp, false, out res, out exc))
                throw exc;

            return res;
        }

        [CLSCompliant(false)]
        public static uint Parse(string s, IFormatProvider provider)
        {
            return Parse(s, NumberStyles.Integer, provider);
        }

        [CLSCompliant(false)]
        public static uint Parse(string s, NumberStyles style)
        {
            return Parse(s, style, null);
        }

#if NET_2_0
		[CLSCompliant (false)]
		public static bool TryParse (string s, out uint result) 
		{
			Exception exc;
			if (!Parse (s, true, out result, out exc)) {
				result = 0;
				return false;
			}

			return true;
		}

		[CLSCompliant (false)]
		public static bool TryParse (string s, NumberStyles style, IFormatProvider provider, out uint result) 
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
            //return NumberFormatter.FormatGeneral(new NumberFormatter.NumberStore(m_value));
        }

        public string ToString(IFormatProvider fp)
        {
#if OLDNF
            return NumberFormatter.FormatGeneral(new NumberFormatter.NumberStore(m_value), fp);
#else
            return NumberFormatter.NumberToString(m_value, fp);
#endif
        }

        public string ToString(string format)
        {
            return ToString(format, null);
        }

        public string ToString(string format, IFormatProvider fp)
        {
            NumberFormatInfo nfi = NumberFormatInfo.GetInstance(fp);
            return NumberFormatter.NumberToString(format, m_value, nfi);
        }

        #region IConvertible Methods
        public TypeCode GetTypeCode()
        {
            return TypeCode.UInt32;
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
            return m_value;
        }

        ulong IConvertible.ToUInt64(IFormatProvider provider)
        {
            return Convert.ToUInt64(m_value);
        }
        #endregion

        #region Artihmetic with Overflow Checking
        internal static uint AddOvf(uint a, uint b)
        {
            uint c = a + b;
            if (c < a)
                throw new OverflowException();
            return c;
        }

        internal static int Nlz(uint x)
        {
            if (x == 0) return 32;
            int n = 0;
            if (x <= 0x0000FFFF) { n = n + 16; x = x << 16; }
            if (x <= 0x00FFFFFF) { n = n + 8; x = x << 8; }
            if (x <= 0x0FFFFFFF) { n = n + 4; x = x << 4; }
            if (x <= 0x3FFFFFFF) { n = n + 2; x = x << 2; }
            if (x <= 0x7FFFFFFF) { n = n + 1; }
            return n;
        }

        internal static uint MulOvf(uint x, uint y)
        {
            int m = Nlz(x);
            int n = Nlz(y);
            if (m + n <= 30)
                throw new OverflowException();
            uint t = x * (y >> 1);
            if ((int)t < 0)
                throw new OverflowException();
            uint z = t * 2;
            if ((y & 1) != 0)
            {
                z = z + x;
                if (z < x)
                    throw new OverflowException();
            }
            return z;
        }
        #endregion
    }
}
