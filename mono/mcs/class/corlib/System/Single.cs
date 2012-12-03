//CHANGED
//
// System.Single.cs
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
#if NET_2_0
	[System.Runtime.InteropServices.ComVisible (true)]
#endif
    public struct Single : IComparable, IFormattable, IConvertible
#if NET_2_0
		, IComparable <float>, IEquatable <float>
#endif
    {
        public const float Epsilon = 1.4e-45f;
        public const float MaxValue = 3.40282346638528859e38f;
        public const float MinValue = -3.40282346638528859e38f;
        public const float NaN = 0.0f / 0.0f;
        public const float PositiveInfinity = 1.0f / 0.0f;
        public const float NegativeInfinity = -1.0f / 0.0f;

        internal float m_value;

        public int CompareTo(object v)
        {
            if (v == null)
                return 1;

            if (!(v is System.Single))
                throw new ArgumentException(Locale.GetText("Value is not a System.Single."));

            float fv = (float)v;

            if (IsPositiveInfinity(m_value) && IsPositiveInfinity(fv))
                return 0;

            if (IsNegativeInfinity(m_value) && IsNegativeInfinity(fv))
                return 0;

            if (IsNaN(fv))
                if (IsNaN(m_value))
                    return 0;
                else
                    return 1;

            if (IsNaN(m_value))
                if (IsNaN(fv))
                    return 0;
                else
                    return -1;

            if (this.m_value == fv)
                return 0;
            else if (this.m_value > fv)
                return 1;
            else
                return -1;
        }

        public override bool Equals(object o)
        {
            if (!(o is System.Single))
                return false;

            if (IsNaN((float)o))
            {
                return IsNaN(m_value);
            }

            return ((float)o) == m_value;
        }

#if NET_2_0
		public int CompareTo (float value)
		{
			if (IsPositiveInfinity (m_value) && IsPositiveInfinity (value))
				return 0;

			if (IsNegativeInfinity (m_value) && IsNegativeInfinity (value))
				return 0;

			if (IsNaN (value))
				if (IsNaN (m_value))
					return 0;
				else
					return 1;

			if (IsNaN (m_value))
				if (IsNaN (value))
					return 0;
				else
					return -1;

			if (this.m_value == value)
				return 0;
			else if (this.m_value > value)
				return 1;
			else
				return -1;
		}

		public bool Equals (float value)
		{
			if (IsNaN (value))
				return IsNaN (m_value);

			return value == m_value;
		}
#endif

        public override int GetHashCode()
        {
            //TODO:
            //float f = m_value;
            //return *((int*)&f);
            return (int)m_value;
        }

        public static bool IsInfinity(float f)
        {
            return (f == PositiveInfinity || f == NegativeInfinity);
        }

        public static bool IsNaN(float f)
        {
#pragma warning disable 1718
            return (f != f);
#pragma warning restore
        }

        public static bool IsNegativeInfinity(float f)
        {
            return (f < 0.0f && (f == NegativeInfinity || f == PositiveInfinity));
        }

        public static bool IsPositiveInfinity(float f)
        {
            return (f > 0.0f && (f == NegativeInfinity || f == PositiveInfinity));
        }

        public static float Parse(string s)
        {
            double parsed_value = Double.Parse(
                s, (NumberStyles.Float | NumberStyles.AllowThousands), null);
            if (parsed_value > (double)float.MaxValue)
                throw new OverflowException();

            return (float)parsed_value;
        }

        public static float Parse(string s, IFormatProvider provider)
        {
            double parsed_value = Double.Parse(
                s, (NumberStyles.Float | NumberStyles.AllowThousands), provider);
            if (parsed_value > (double)float.MaxValue)
                throw new OverflowException();

            return (float)parsed_value;
        }

        public static float Parse(string s, NumberStyles style)
        {
            double parsed_value = Double.Parse(s, style, null);
            if (parsed_value > (double)float.MaxValue)
                throw new OverflowException();

            return (float)parsed_value;
        }

        public static float Parse(string s, NumberStyles style, IFormatProvider provider)
        {
            double parsed_value = Double.Parse(s, style, provider);
            if (parsed_value > (double)float.MaxValue)
                throw new OverflowException();

            return (float)parsed_value;
        }
#if NET_2_0
		public static bool TryParse (string s, NumberStyles style, IFormatProvider provider, out float result)
		{
			double parsed_value;
			Exception exc;
			if (!Double.Parse (s, style, provider, true, out parsed_value, out exc)) {
				result = 0;
				return false;
			} else if (parsed_value > (double) float.MaxValue) {
				result = 0;
				return false;
			}
			result = (float) parsed_value;
			return true;
		}

		public static bool TryParse (string s, out float result)
		{
			return TryParse (s, NumberStyles.Any, null, out result);
		}
#endif
        public override string ToString()
        {
            return avm.ToString(m_value);
            //return ToString(null, null);
        }

        public string ToString(IFormatProvider provider)
        {
            return ToString(null, provider);
        }

        public string ToString(string format)
        {
            return ToString(format, null);
        }

        public string ToString(string format, IFormatProvider provider)
        {
            NumberFormatInfo nfi = NumberFormatInfo.GetInstance (provider);
            return NumberFormatter.NumberToString (format, m_value, nfi);
            //return new string(avm.ToString(m_value));
        }

        #region IConvertible Methods
        public TypeCode GetTypeCode()
        {
            return TypeCode.Single;
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
            return Convert.ToUInt32(m_value);
        }

        ulong IConvertible.ToUInt64(IFormatProvider provider)
        {
            return Convert.ToUInt64(m_value);
        }
        #endregion
    }
}
