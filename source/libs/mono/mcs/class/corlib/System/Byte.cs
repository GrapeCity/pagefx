//CHANGED

//
// System.Byte.cs
//
// Author:
//   Miguel de Icaza (miguel@ximian.com)
//
// (C) Ximian, Inc.  http://www.ximian.com
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

using System.Globalization;

namespace System
{
    [Serializable]
    public struct Byte : IFormattable, IConvertible, IComparable
#if NET_2_0
		, IComparable<Byte>, IEquatable <Byte>
#endif
    {
        public const byte MinValue = 0;
        public const byte MaxValue = 255;

        internal byte m_value;

        #region CompareTo, Equals, GetHashCode
        public int CompareTo(object value)
        {
            if (value == null)
                return 1;

            if (!(value is Byte))
                throw new ArgumentException(Locale.GetText("Value is not a System.Byte."));

            byte xv = (byte)value;

            if (m_value == xv)
                return 0;
            if (m_value > xv)
                return 1;
            return -1;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Byte))
                return false;

            return ((byte)obj) == m_value;
        }

        public override int GetHashCode()
        {
            return m_value;
        }

        //#if NET_2_0
        public int CompareTo(byte value)
        {
            if (m_value == value)
                return 0;
            if (m_value > value)
                return 1;
            return -1;
        }

        public bool Equals(byte value)
        {
            return value == m_value;
        }
        //#endif
        #endregion

        #region Parse
        public static byte Parse(string s, IFormatProvider provider)
        {
            return Parse(s, NumberStyles.Integer, provider);
        }

        public static byte Parse(string s, NumberStyles style)
        {
            return Parse(s, style, null);
        }

        public static byte Parse(string s, NumberStyles style, IFormatProvider provider)
        {
            uint tmpResult = UInt32.Parse(s, style, provider);
            if (tmpResult > Byte.MaxValue)
                throw new OverflowException(Locale.GetText("Value too large."));

            return (byte)tmpResult;
        }

        public static byte Parse(string s)
        {
            return Parse(s, NumberStyles.Integer, null);
        }

#if NET_2_0
		public static bool TryParse (string s, out byte result) 
		{
			return TryParse (s, NumberStyles.Integer, null, out result);
		}

		public static bool TryParse (string s, NumberStyles style, IFormatProvider provider, out byte result) 
		{
			uint tmpResult;
			result = 0;
			
			if (!UInt32.TryParse (s, style, provider, out tmpResult))
				return false;
				
			if (tmpResult > Byte.MaxValue)
				return false;
				
			result = (byte)tmpResult;
			return true;
		}
#endif
        #endregion

        public override string ToString()
        {
            return avm.ToString(m_value);
            //return NumberFormatter.FormatGeneral(new NumberFormatter.NumberStore(m_value));
        }

        public string ToString(string format)
        {
            return ToString(format, null);
        }

        public string ToString(IFormatProvider provider)
        {
#if OLDNF
            return NumberFormatter.FormatGeneral(new NumberFormatter.NumberStore(m_value), provider);
#else
            return NumberFormatter.NumberToString(m_value, provider);
#endif
        }

        public string ToString(string format, IFormatProvider provider)
        {
            NumberFormatInfo nfi = NumberFormatInfo.GetInstance(provider);
            return NumberFormatter.NumberToString(format, m_value, nfi);
        }

        #region IConvertible Methods
        public TypeCode GetTypeCode()
        {
            return TypeCode.Byte;
        }

        object IConvertible.ToType(Type conversionType, IFormatProvider provider)
        {
            return Convert.ToType(m_value, conversionType, provider);
        }

        bool IConvertible.ToBoolean(IFormatProvider provider)
        {
            return Convert.ToBoolean(m_value);
        }

        byte IConvertible.ToByte(IFormatProvider provider)
        {
            return m_value;
        }

        char IConvertible.ToChar(IFormatProvider provider)
        {
            return Convert.ToChar(m_value);
        }

        DateTime IConvertible.ToDateTime(IFormatProvider provider)
        {
            throw new InvalidCastException();
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

        /*
                string IConvertible.ToString (IFormatProvider provider)
                {
                    return ToString("G", provider);
                }
        */

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
        internal static byte AddOvf(byte x, byte y)
        {
            int z = x + y;
            if (z > byte.MaxValue || z < byte.MinValue)
                throw new OverflowException();
            return (byte)z;
        }

        internal static byte SubOvf(byte x, byte y)
        {
            int z = x - y;
            if (z > byte.MaxValue || z < byte.MinValue)
                throw new OverflowException();
            return (byte)z;
        }

        internal static byte MulOvf(byte x, byte y)
        {
            uint z = UInt32.MulOvf(x, y);
            if (z > byte.MaxValue || z < byte.MinValue)
                throw new OverflowException();
            return (byte)z;
        }
        #endregion
    }
}
