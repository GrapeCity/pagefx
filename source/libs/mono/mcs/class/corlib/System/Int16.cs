//CHANGED

//
// System.Int16.cs
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
    public struct Int16 : IFormattable, IConvertible, IComparable
#if NET_2_0
		, IComparable<Int16>, IEquatable <Int16>
#endif
    {

        public const short MaxValue = 32767;
        public const short MinValue = -32768;

        internal short m_value;

        #region CompareTo, Equals, GetHashCode
        public int CompareTo(object v)
        {
            if (v == null)
                return 1;

            if (!(v is Int16))
                throw new ArgumentException(Locale.GetText("Value is not a System.Int16"));

            short xv = (short)v;
            if (m_value == xv)
                return 0;
            if (m_value > xv)
                return 1;
            return -1;
        }

        public override bool Equals(object o)
        {
            if (!(o is Int16))
                return false;

            return ((short)o) == m_value;
        }

        public override int GetHashCode()
        {
            return m_value;
        }

        //#if NET_2_0
        public int CompareTo(short value)
        {
            if (m_value == value)
                return 0;
            if (m_value > value)
                return 1;
            return -1;
        }

        public bool Equals(short value)
        {
            return value == m_value;
        }
        //#endif
        #endregion

        #region Parse
        public static short Parse(string s, IFormatProvider fp)
        {
            return Parse(s, NumberStyles.Integer, fp);
        }

        public static short Parse(string s, NumberStyles style)
        {
            return Parse(s, style, null);
        }

        public static short Parse(string s, NumberStyles style, IFormatProvider fp)
        {
            int tmpResult = Int32.Parse(s, style, fp);
            if (tmpResult > MaxValue || tmpResult < MinValue)
                throw new OverflowException("Value too large or too small.");

            return (short)tmpResult;
        }

        public static short Parse(string s)
        {
            int tmpResult = Int32.Parse(s);
            if (tmpResult > MaxValue || tmpResult < MinValue)
                throw new OverflowException("Value too large or too small.");

            return (short)tmpResult;
        }

#if NET_2_0
#if NOT_PFX		
        public static bool TryParse (string s, out short result) 
		{
			Exception exc;
			if (!Parse (s, true, out result, out exc)) {
				result = 0;
				return false;
			}

			return true;
		}
#endif

		public static bool TryParse (string s, NumberStyles style, IFormatProvider provider, out short result) 
		{
			int tmpResult;
			result = 0;
				
			if (!Int32.TryParse (s, style, provider, out tmpResult))
				return false;
			
			if (tmpResult > Int16.MaxValue || tmpResult < Int16.MinValue)
				return false;
				
			result = (short)tmpResult;
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
            return TypeCode.Int16;
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
            return m_value;
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
            return Convert.ToUInt32(m_value);
        }

        ulong IConvertible.ToUInt64(IFormatProvider provider)
        {
            return Convert.ToUInt64(m_value);
        }
        #endregion

        internal static short AddOvf(short x, short y)
        {
            int z = x + y;
            if (z > short.MaxValue || z < short.MinValue)
                throw new OverflowException();
            return (short)z;
        }

        internal static short SubOvf(short x, short y)
        {
            int z = x - y;
            if (z > short.MaxValue || z < short.MinValue)
                throw new OverflowException();
            return (short)z;
        }

        internal static short MulOvf(short x, short y)
        {
            int z = Int32.MulOvf(x, y);
            if (z > short.MaxValue || z < short.MinValue)
                throw new OverflowException();
            return (short)z;
        }
    }
}
