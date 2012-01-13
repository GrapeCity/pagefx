#if IMPL64
using System;
namespace DataDynamics
#else
namespace System
#endif
{
    public struct UInt64 : IComparable, IFormattable, IConvertible
#if NET_2_0
        , IComparable<UInt64>, IEquatable<UInt64>
#endif
    {
        #region Shared Fields
        public static readonly UInt64 Zero = new UInt64(0u, 0u);
        public static readonly UInt64 MaxValue = new UInt64(~0u, ~0u);

        internal const double DoubleK = 4294967296.0;
        #endregion

        #region Fields
        public uint hi;
        public uint lo;
        #endregion

        #region Constructors
        public UInt64(uint hi, uint lo)
        {
            this.hi = hi;
            this.lo = lo;
        }

        public UInt64(uint value)
        {
            hi = 0;
            lo = value;
        }

        public UInt64(Int64 value)
        {
            hi = (uint)value.hi;
            lo = value.lo;
        }
        #endregion

        #region Arithmetic
        public static UInt64 operator +(UInt64 x, UInt64 y)
        {
            x.lo += y.lo;
            x.hi += y.hi;
            if (x.lo < y.lo)
                x.hi += 1;
            return x;
        }

        public static UInt64 AddOvf(UInt64 x, UInt64 y)
        {
            x.lo += y.lo;
            if (x.lo < y.lo)
                throw new OverflowException();
            x.hi += y.hi;
            return x;
        }

        public static UInt64 operator -(UInt64 x, UInt64 y)
        {
            x.hi -= y.hi;
            if (x.lo < y.lo)
                x.hi -= 1;
            x.lo -= y.lo;
            return x;
        }

        public static UInt64 operator -(UInt64 x, uint y)
        {
            if (x.lo < y)
                x.hi -= 1;
            x.lo -= y;
            return x;
        }

        public static UInt64 SubOvf(UInt64 x, UInt64 y)
        {
            x.lo -= y.lo;
            if (x.lo > ~y.lo)
                throw new OverflowException();
            x.hi -= y.hi;
            return x;
        }

        #region Multiply
        internal static void Mult64by16to64(uint alo, uint ahi, ushort b, out uint clo, out uint chi)
        {
            uint val, mid, carry0, carry1;
            ushort a0, a1, a2, a3;
            ushort h0, h1, h2, h3, h4;
            a0 = (ushort)alo;
            a1 = (ushort)(alo >> 16);
            a2 = (ushort)ahi;
            a3 = (ushort)(ahi >> 16);

            val = ((uint)a0) * b;
            h0 = (ushort)val;

            val >>= 16;
            carry0 = 0;
            mid = ((uint)a1) * b;
            val += mid;
            if (val < mid) ++carry0;
            h1 = (ushort)val;

            val >>= 16;
            carry1 = 0;
            mid = ((uint)a2) * b;
            val += mid;
            if (val < mid) ++carry1;
            h2 = (ushort)val;

            val >>= 16;
            val += carry0;
            mid = ((uint)a3) * b;
            val += mid;
            h3 = (ushort)val;

            val >>= 16;
            val += carry1;
            h4 = (ushort)val;
            if (h4 != 0)
                throw new OverflowException();

            clo = ((uint)h1) << 16 | h0;
            chi = ((uint)h3) << 16 | h2;
        }

        internal static void Mult32by32to64(uint a, uint b, out uint clo, out uint chi)
        {
            Mult32by32to64((ushort)a, (ushort)(a >> 16), (ushort)b, (ushort)(b >> 16), out clo, out chi);
        }

        private static void Mult32by32to64(ushort alo, ushort ahi, ushort blo, ushort bhi, out uint clo, out uint chi)
        {
            uint a, b, c, d;
            ushort h0, h1, h2, h3, carry;

            a = ((uint)alo) * blo;
            h0 = (ushort)a;

            a >>= 16;
            carry = 0;
            b = ((uint)alo) * bhi;
            c = ((uint)ahi) * blo;
            a += b;
            if (a < b) ++carry;
            a += c;
            if (a < c) ++carry;
            h1 = (ushort)a;

            a >>= 16;
            d = ((uint)ahi) * bhi;
            a += d;
            h2 = (ushort)a;

            a >>= 16;
            a += carry;
            h3 = (ushort)a;

            clo = ((uint)h1) << 16 | h0;
            chi = ((uint)h3) << 16 | h2;
        }

        internal static UInt64 Multiply(uint alo, uint ahi, uint blo, uint bhi)
        {
            uint p1_lo, p1_hi, p2_lo, p2_hi, p3_lo, p3_hi;
            uint sum;

            Mult32by32to64(alo, blo, out p1_lo, out p1_hi);

            uint c0 = p1_lo;

            sum = p1_hi;
            Mult32by32to64(ahi, blo, out p2_lo, out p2_hi);
            Mult32by32to64(alo, bhi, out p3_lo, out p3_hi);
            sum += p2_lo;
            sum += p3_lo;
            uint c1 = sum;

            return new UInt64(c1, c0);
        }

        public static UInt64 operator *(UInt64 x, UInt64 y)
        {
            return Multiply(x.lo, x.hi, y.lo, y.hi);
        }

        public static UInt64 operator *(UInt64 d1, uint d2)
        {
            if (d2 <= ushort.MaxValue)
            {
                uint c0, c1;
                Mult64by16to64(d1.lo, d1.hi, (ushort)d2, out c0, out c1);
                return new UInt64(c1, c0);
            }
            else
            {
                return Multiply(d1.lo, d1.hi, d2, 0);
            }
        }
        #endregion

        #region Divide
        //http://www.bearcave.com/software/divide.htm
        private const uint MSB = 0x80000000;

        public static UInt64 Divide(UInt64 dividend, UInt64 divisor, out UInt64 remainder)
        {
            if (divisor == dividend)
            {
                remainder = Zero;
                return new UInt64(1u);
            }
            if (divisor > dividend)
            {
                remainder = dividend;
                return Zero;
            }

            remainder = Zero;
            UInt64 quotient = Zero;

            // here: dividend > divisor
            int nBits = 64;
            UInt64 d = Zero;
            while (remainder < divisor)
            {
                remainder <<= 1;
                if ((dividend.hi & MSB) != 0)
                {
                    remainder |= 1u;
                }
                d = dividend;
                dividend <<= 1;
                nBits--;
            }

            // undo the last loop iteration
            dividend = d;
            remainder >>= 1;
            nBits++;

            for (int i = 0; i < nBits; i++)
            {
                remainder <<= 1;
                if ((dividend.hi & MSB) != 0)
                {
                    remainder |= 1u;
                }

                UInt64 t = remainder - divisor;
                dividend <<= 1;
                quotient <<= 1;
                if ((t.hi & MSB) == 0)
                {
                    quotient |= 1;
                    remainder = t;
                }
            }
            return quotient;
        }

        public static UInt64 operator /(UInt64 x, UInt64 y)
        {
            if (y == Zero)
                throw new DivideByZeroException();
            if (y == x)
                return new UInt64(1);
            if (y > x)
                return Zero;
            UInt64 r;
            return Divide(x, y, out r);
        }

        public static UInt64 operator %(UInt64 x, UInt64 y)
        {
            UInt64 r;
            Divide(x, y, out r);
            return r;
        }
        #endregion

        #endregion

        #region Bitwise Operations
        public static UInt64 operator >>(UInt64 x, int n)
        {
            //Note: it seems that CLR truncates shift
            /* check bad cases: ((n > 63) || (n < 0)) */
            //if ((n & 63) != n) return Zero;
            //if ((n & 63) != n) return x;
            n &= 63;
            if (n > 31)
            {
                x.lo = x.hi >> (n - 32);
                x.hi = 0;
            }
            else if (n != 0)
            {
                x.lo = (x.lo >> n) | (x.hi << (32 - n));
                x.hi = x.hi >> n;
            }
            return x;
        }

        public static UInt64 operator <<(UInt64 x, int n)
        {
            //Note: it seems that CLR truncates shift
            /* check bad cases: ((n > 63) || (n < 0)) */
            //if ((n & 63) != n) return Zero;
            n &= 63;
            if (n > 31)
            {
                x.hi = x.lo << (n - 32);
                x.lo = 0;
            }
            else if (n != 0)
            {
                x.hi = (x.hi << n) | (x.lo >> (32 - n));
                x.lo = x.lo << n;
            }
            return x;
        }

        public static UInt64 operator &(UInt64 x, UInt64 y)
        {
            x.lo &= y.lo;
            x.hi &= y.hi;
            return x;
        }

        public static UInt64 operator &(UInt64 x, uint y)
        {
            x.lo &= y;
            return x;
        }

        public static UInt64 operator |(UInt64 x, UInt64 y)
        {
            x.lo |= y.lo;
            x.hi |= y.hi;
            return x;
        }

        public static UInt64 operator |(UInt64 x, uint y)
        {
            x.lo |= y;
            return x;
        }

        public static UInt64 operator ^(UInt64 x, UInt64 y)
        {
            x.lo ^= y.lo;
            x.hi ^= y.hi;
            return x;
        }

        public static UInt64 operator ^(UInt64 x, uint y)
        {
            x.lo ^= y;
            return x;
        }

        public static UInt64 operator ~(UInt64 x)
        {
            x.lo = ~x.lo;
            x.hi = ~x.hi;
            return x;
        }
        #endregion

        #region Comparison Operators
        private static int Compare(UInt64 x, UInt64 y)
        {
            if (x.hi < y.hi) return -1;
            if (x.hi > y.hi) return 1;
            if (x.lo < y.lo) return -1;
            if (x.lo > y.lo) return 1;
            return 0;
        }

        private static int Compare(UInt64 x, uint y)
        {
            if (x.hi != 0) return 1;
            if (x.lo < y) return -1;
            if (x.lo > y) return 1;
            return 0;
        }

        private static int Compare(UInt64 x, int y)
        {
            if (y < 0) return -1;
            if (x.hi != 0) return 1;
            if (x.lo < y) return -1;
            if (x.lo > y) return 1;
            return 0;
        }

        public static bool operator ==(UInt64 x, UInt64 y)
        {
            return x.hi == y.hi && x.lo == y.lo;
        }

        public static bool operator !=(UInt64 x, UInt64 y)
        {
            return x.hi != y.hi || x.lo != y.lo;
        }

        public static bool operator <(UInt64 x, UInt64 y)
        {
            return Compare(x, y) < 0;
        }

        public static bool operator <=(UInt64 x, UInt64 y)
        {
            return Compare(x, y) <= 0;
        }

        public static bool operator >(UInt64 x, UInt64 y)
        {
            return Compare(x, y) > 0;
        }

        public static bool operator >=(UInt64 x, UInt64 y)
        {
            return Compare(x, y) >= 0;
        }

        #region UInt32
        public static bool operator <(UInt64 x, uint y)
        {
            return Compare(x, y) < 0;
        }

        public static bool operator <=(UInt64 x, uint y)
        {
            return Compare(x, y) <= 0;
        }

        public static bool operator >(UInt64 x, uint y)
        {
            return Compare(x, y) > 0;
        }

        public static bool operator >=(UInt64 x, uint y)
        {
            return Compare(x, y) >= 0;
        }
        #endregion

        #region Int32
        public static bool operator <(UInt64 x, int y)
        {
            return Compare(x, y) < 0;
        }

        public static bool operator <=(UInt64 x, int y)
        {
            return Compare(x, y) <= 0;
        }

        public static bool operator >(UInt64 x, int y)
        {
            return Compare(x, y) > 0;
        }

        public static bool operator >=(UInt64 x, int y)
        {
            return Compare(x, y) >= 0;
        }
        #endregion

        #region UInt16
        public static bool operator <(UInt64 x, ushort y)
        {
            return Compare(x, (uint)y) < 0;
        }

        public static bool operator <=(UInt64 x, ushort y)
        {
            return Compare(x, (uint)y) <= 0;
        }

        public static bool operator >(UInt64 x, ushort y)
        {
            return Compare(x, (uint)y) > 0;
        }

        public static bool operator >=(UInt64 x, ushort y)
        {
            return Compare(x, (uint)y) >= 0;
        }
        #endregion

        #region Int16
        public static bool operator <(UInt64 x, short y)
        {
            return Compare(x, y) < 0;
        }

        public static bool operator <=(UInt64 x, short y)
        {
            return Compare(x, y) <= 0;
        }

        public static bool operator >(UInt64 x, short y)
        {
            return Compare(x, y) > 0;
        }

        public static bool operator >=(UInt64 x, short y)
        {
            return Compare(x, y) >= 0;
        }
        #endregion
        #endregion

        #region Cast Operators
        //TODO: Find and Write table with explicit/implicit conversions and correct using of implicit or explicit keywords.
        //TODO: Conversions with Decimal

        #region FromUInt64
        public static bool operator true(UInt64 value)
        {
            return value.hi != 0 || value.lo != 0;
        }

        public static bool operator false(UInt64 value)
        {
            return value.hi == 0 && value.lo == 0;
        }

        public static explicit operator bool(UInt64 value)
        {
            return value.hi != 0 || value.lo != 0;
        }

        public static explicit operator int(UInt64 value)
        {
            return (int)value.lo;
        }

        public static explicit operator uint(UInt64 value)
        {
            return value.lo;
        }

        public static implicit operator double(UInt64 value)
        {
            return DoubleK * value.hi + value.lo;
        }

        public static explicit operator float(UInt64 value)
        {
            double v = DoubleK * value.hi + value.lo;
            return (float)v;
        }

        public static explicit operator sbyte(UInt64 value)
        {
            return (sbyte)value.lo;
        }

        public static explicit operator byte(UInt64 value)
        {
            return (byte)value.lo;
        }

        public static explicit operator short(UInt64 value)
        {
            return (short)value.lo;
        }

        public static explicit operator ushort(UInt64 value)
        {
            return (ushort)value.lo;
        }

        public static explicit operator char(UInt64 value)
        {
            return (char)value.lo;
        }
        #endregion

        #region ToUInt64
        public static explicit operator UInt64(bool value)
        {
            return new UInt64(value ? 1u : 0u);
        }

        public static explicit operator UInt64(char value)
        {
            return new UInt64(value);
        }

        public static implicit operator UInt64(byte value)
        {
            return new UInt64(value);
        }

        public static implicit operator UInt64(sbyte value)
        {
            return new UInt64((uint)value);
        }

        public static implicit operator UInt64(ushort value)
        {
            return new UInt64(value);
        }

        public static implicit operator UInt64(short value)
        {
            return new UInt64((uint)value);
        }

        public static implicit operator UInt64(uint value)
        {
            return new UInt64(value);
        }

        public static implicit operator UInt64(int value)
        {
            return new UInt64((uint)value);
        }

        public static explicit operator UInt64(Int64 value)
        {
            return new UInt64(value);
        }

        public static explicit operator UInt64(double value)
        {
            uint hi = (uint)(value / DoubleK);
            uint lo = (uint)(value - DoubleK * hi);
            return new UInt64(hi, lo);
        }

        public static explicit operator UInt64(float value)
        {
            uint hi = (uint)(value / DoubleK);
            uint lo = (uint)(value - DoubleK * hi);
            return new UInt64(hi, lo);
        }
        #endregion
        #endregion

        #region Object Override Members
        public override bool Equals(object obj)
        {
            if (obj is UInt64)
            {
                UInt64 v = (UInt64)obj;
                return v.hi == hi && v.lo == lo;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return (int)(hi + lo);
        }

        public override string ToString()
        {
            return ToString(null, null);
        }
        #endregion

        #region IFormattable Members
        public string ToString(string format, IFormatProvider formatProvider)
        {
            //return hi.ToString(format, formatProvider) + lo.ToString(format, formatProvider);
            return ((ulong)this).ToString(format, formatProvider);
        }
        #endregion

        #region IComparable Members
        public int CompareTo(object obj)
        {
            return CompareTo((UInt64)obj);
        }
        #endregion

        #region IConvertible Members
        //Note: Since we check range by self then we should avoid redundant using methods of Convert class.

        public TypeCode GetTypeCode()
        {
            return TypeCode.UInt64;
        }

        bool IConvertible.ToBoolean(IFormatProvider provider)
        {
            return hi != 0 || lo != 0;
        }

        byte IConvertible.ToByte(IFormatProvider provider)
        {
            if (hi != 0 || lo > Byte.MaxValue)
                throw new OverflowException("Value is greater than Byte.MaxValue");
            return (byte)lo;
        }

        char IConvertible.ToChar(IFormatProvider provider)
        {
            if (hi != 0 || lo > Char.MaxValue)
                throw new OverflowException("Value is greater than Char.MaxValue");
            return (char)lo;
        }

        DateTime IConvertible.ToDateTime(IFormatProvider provider)
        {
            return Convert.ToDateTime(lo);
        }

        decimal IConvertible.ToDecimal(IFormatProvider provider)
        {
            //return new Decimal((int)m_lo, (int)m_hi, 0, false, 0);
            return Convert.ToDecimal((ulong)this);
        }

        double IConvertible.ToDouble(IFormatProvider provider)
        {
            return DoubleK * hi + lo;
        }

        short IConvertible.ToInt16(IFormatProvider provider)
        {
            if (hi != 0 || lo > Int16.MaxValue)
                throw new OverflowException("Value is greater than Int16.MaxValue");
            return (short)lo;
        }

        int IConvertible.ToInt32(IFormatProvider provider)
        {
            if (hi != 0 || lo > Int32.MaxValue)
                throw new OverflowException("Value is greater than Int32.MaxValue");
            return (int)lo;
        }

        long IConvertible.ToInt64(IFormatProvider provider)
        {
            //TODO:
            if (hi >= 0x80000000)
                throw new OverflowException("Value is greater than Int64.MaxValue");
            return (long)this;
            //return Convert.ToInt64(m_value);
        }

        sbyte IConvertible.ToSByte(IFormatProvider provider)
        {
            if (hi != 0 || lo > SByte.MaxValue)
                throw new OverflowException("Value is greater than SByte.MaxValue");
            return Convert.ToSByte(lo);
        }

        float IConvertible.ToSingle(IFormatProvider provider)
        {
            double d = DoubleK * hi + lo;
            float s = (float)d;
            return s;
            //return Convert.ToSingle(m_value);
        }

        object IConvertible.ToType(Type conversionType, IFormatProvider provider)
        {
            //TODO:
            return null;
            //return Convert.ToType(m_value, conversionType, provider);
        }

        ushort IConvertible.ToUInt16(IFormatProvider provider)
        {
            if (hi != 0 || lo > (ulong)UInt16.MaxValue)
                throw new OverflowException("Value is greater than UInt16.MaxValue");
            return (ushort)lo;
        }

        uint IConvertible.ToUInt32(IFormatProvider provider)
        {
            if (hi != 0)
                throw new OverflowException("Value is greater than UInt32.MaxValue");
            return lo;
        }

        ulong IConvertible.ToUInt64(IFormatProvider provider)
        {
            //TODO:
            return (ulong)this;
        }

        public string ToString(IFormatProvider provider)
        {
            //return NumberFormatter.FormatGeneral(new NumberFormatter.NumberStore(m_value), provider);
            return ToString(null, provider);
        }
        #endregion

#if NET_2_0
        #region IComparable<UInt64> Members
        public int CompareTo(UInt64 other)
        {
            return Compare(this, other);
        }
        #endregion

        #region IEquatable<UInt64> Members
        public bool Equals(UInt64 other)
        {
            return other.hi == hi && other.lo == lo;
        }
        #endregion
#endif

        #region Conditional Members
#if IMPL64
        public UInt64(ulong value)
        {
            hi = (uint)(value >> 32);
            lo = (uint)value;
        }

        public static explicit operator ulong(UInt64 x)
        {
            return x.hi << 32 | x.lo;
        }

        public static explicit operator long(UInt64 x)
        {
            return x.hi << 32 | x.lo;
        }

        public static explicit operator UInt64(ulong value)
        {
            return new UInt64(value);
        }

        public static explicit operator UInt64(long value)
        {
            return new UInt64((ulong)value);
        }
#endif
        #endregion
    }
}