#if IMPL64
using System;
namespace DataDynamics
#else
namespace System
#endif
{
    public struct Int64 : IComparable, IFormattable, IConvertible
#if NET_2_0
        , IComparable<Int64>, IEquatable<Int64>
#endif
    {
        #region Shared Fields
        public static readonly Int64 MinValue = new Int64(unchecked((int)0x80000000), 0);
        public static readonly Int64 MaxValue = new Int64(0x7fffffff, ~0u);

        public static readonly Int64 Zero = new Int64(0, 0);
        public static readonly Int64 NegativeOne = new Int64(~0, ~0u);
        #endregion

        #region Fields
        public int hi;
        public uint lo;
        #endregion

        #region Constructors
        public Int64(int hi, uint lo)
        {
            this.hi = hi;
            this.lo = lo;
        }

        public Int64(uint hi, uint lo)
        {
            this.hi = (int)hi;
            this.lo = lo;
        }

        public Int64(uint value)
        {
            lo = value;
            hi = 0;
        }

        public Int64(int value)
        {
            lo = (uint) value;
            hi = value < 0 ? -1 : 0;
        }

        public Int64(UInt64 x)
        {
            hi = (int)x.hi;
            lo = x.lo;
        }
        #endregion

        #region Arithmetic
        public static Int64 operator +(Int64 x, Int64 y)
        {
            x.lo += y.lo;
            x.hi += y.hi;
            if (x.lo < y.lo) //carry?
                x.hi += 1;
            return x;
        }

        public static Int64 operator +(Int64 x, int y)
        {
            return x + new Int64(y);
        }

        public static Int64 operator -(Int64 x, Int64 y)
        {
            x.hi -= y.hi;
            if (x.lo < y.lo) //borrow?
                x.hi -= 1;
            x.lo -= y.lo;
            return x;
        }

        internal void Negate()
        {
            lo = (~lo) + 1;
            hi = ~hi;
            if (lo == 0)
                ++hi;
        }

        public static Int64 operator -(Int64 x)
        {
            x.Negate();
            return x;
        }

        #region Multiply
        public static Int64 operator *(Int64 d1, Int64 d2)
        {
            bool sign1 = d1.hi < 0;
            bool sign2 = d2.hi < 0;

            if (sign1) d1.Negate();
            if (sign2) d2.Negate();

            UInt64 u = UInt64.Multiply(d1.lo, (uint)d1.hi, d2.lo, (uint)d2.hi);
            Int64 result = new Int64(u);
            if (sign1 != sign2)
                result.Negate();
            return result;
        }

        public static Int64 operator *(Int64 x, int y)
        {
            UInt32 hi = (y < 0) ? UInt32.MaxValue : 0;
            UInt64 res = UInt64.Multiply(x.lo, (uint)x.hi, (UInt32)y, hi);
            return new Int64(res);
        }
        #endregion

        #region Divide
        //http://www.bearcave.com/software/divide.htm

        public static UInt64 Abs(Int64 x)
        {
            return new UInt64(x < 0 ? -x : x);
        }

        public static Int64 Divide(Int64 dividend, Int64 divisor, out Int64 remainder)
        {
            UInt64 dend, dor;
            UInt64 q, r;

            dend = Abs(dividend);
            dor = Abs(divisor);
            q = UInt64.Divide(dend, dor, out r);

            /* the sign of the remainder is the same as the sign of the dividend
               and the quotient is negated if the signs of the operands are
               opposite */
            Int64 quotient = (Int64)q;
            remainder = (Int64)r;
            if (dividend < 0)
            {
                remainder = -remainder;
                if (divisor > 0)
                    quotient = -quotient;
            }
            else
            {
                /* positive dividend */
                if (divisor < 0)
                    quotient = -quotient;
            }
            return quotient;
        }

        public static Int64 operator /(Int64 x, Int64 y)
        {
            if (y == Zero)
                throw new DivideByZeroException();
            if (y == x)
                return new Int64(1);
            Int64 r;
            return Divide(x, y, out r);
        }
        
        public static Int64 operator %(Int64 x, Int64 y)
        {
            Int64 r;
            Divide(x, y, out r);
            return r;
        }
        #endregion
        #endregion

        #region Bitwise Operations
        public static Int64 operator >>(Int64 x, int n)
        {
            //Note: it seems that CLR truncates shift
            /* check bad cases: ((n > 63) || (n < 0)) */
            //if ((n & 63) != n)
            //    return (x.hi >= 0) ? Zero : NegativeOne;
            n &= 63;
            if (n > 31)
            {
                x.lo = (uint)(x.hi >> (n - 32));
                x.hi = x.hi >> 31;
            }
            else if (n != 0)
            {
                x.lo = (x.lo >> n) | ((uint)x.hi << (32 - n));
                x.hi = x.hi >> n;
            }
            return x;
        }

        public static Int64 operator <<(Int64 x, int n)
        {
            //Note: it seems that CLR truncates shift
            /* check bad cases: ((n > 63) || (n < 0)) */
            //if ((n & 63) != n) return Zero;
            n &= 63;
            if (n > 31)
            {
                x.hi = (int)(x.lo << (n - 32));
                x.lo = 0;
            }
            else if (n != 0)
            {
                x.hi = (int)(((uint)x.hi << n) | (x.lo >> (32 - n)));
                x.lo = x.lo << n;
            }
            return x;
        }

        public static Int64 operator &(Int64 x, Int64 y)
        {
            x.lo &= y.lo;
            x.hi &= y.hi;
            return x;
        }

        public static Int64 operator |(Int64 x, Int64 y)
        {
            x.lo |= y.lo;
            x.hi |= y.hi;
            return x;
        }

        public static Int64 operator ^(Int64 x, Int64 y)
        {
            x.lo ^= y.lo;
            x.hi ^= y.hi;
            return x;
        }

        public static Int64 operator ~(Int64 x)
        {
            x.lo = ~x.lo;
            x.hi = ~x.hi;
            return x;
        }
        #endregion

        #region Comparison Operators
        private static int Compare(Int64 x, Int64 y)
        {
            if (x.hi < y.hi) return -1;
            if (x.hi > y.hi) return 1;
            if (x.lo < y.lo) return -1;
            if (x.lo > y.lo) return 1;
            return 0;
        }

        private static int Compare(Int64 x, int y)
        {
            if (x.hi < 0) return -1;
            if (x.hi > 0) return 1;
            if (x.lo < y) return -1;
            if (x.lo > y) return 1;
            return 0;
        }

        private static int Compare(Int64 x, uint y)
        {
            if (x.hi < 0) return -1;
            if (x.hi > 0) return 1;
            if (x.lo < y) return -1;
            if (x.lo > y) return 1;
            return 0;
        }

        public static bool operator ==(Int64 x, Int64 y)
        {
            return x.hi == y.hi && x.lo == y.lo;
        }

        public static bool operator !=(Int64 x, Int64 y)
        {
            return x.hi != y.hi || x.lo != y.lo;
        }

        public static bool operator <(Int64 x, Int64 y)
        {
            return Compare(x, y) < 0;
        }

        public static bool operator <=(Int64 x, Int64 y)
        {
            return Compare(x, y) <= 0;
        }

        public static bool operator >(Int64 x, Int64 y)
        {
            return Compare(x, y) > 0;
        }

        public static bool operator >=(Int64 x, Int64 y)
        {
            return Compare(x, y) >= 0;
        }

        #region Int32
        public static bool operator <(Int64 x, int y)
        {
            return Compare(x, y) < 0;
        }

        public static bool operator <=(Int64 x, int y)
        {
            return Compare(x, y) <= 0;
        }

        public static bool operator >(Int64 x, int y)
        {
            return Compare(x, y) > 0;
        }

        public static bool operator >=(Int64 x, int y)
        {
            return Compare(x, y) >= 0;
        }
        #endregion

        #region UInt32
        public static bool operator <(Int64 x, uint y)
        {
            return Compare(x, y) < 0;
        }

        public static bool operator <=(Int64 x, uint y)
        {
            return Compare(x, y) <= 0;
        }

        public static bool operator >(Int64 x, uint y)
        {
            return Compare(x, y) > 0;
        }

        public static bool operator >=(Int64 x, uint y)
        {
            return Compare(x, y) >= 0;
        }
        #endregion

        #region Int16
        public static bool operator <(Int64 x, short y)
        {
            return Compare(x, y) < 0;
        }

        public static bool operator <=(Int64 x, short y)
        {
            return Compare(x, y) <= 0;
        }

        public static bool operator >(Int64 x, short y)
        {
            return Compare(x, y) > 0;
        }

        public static bool operator >=(Int64 x, short y)
        {
            return Compare(x, y) >= 0;
        }
        #endregion

        #region UInt16
        public static bool operator <(Int64 x, ushort y)
        {
            return Compare(x, (uint)y) < 0;
        }

        public static bool operator <=(Int64 x, ushort y)
        {
            return Compare(x, (uint)y) <= 0;
        }

        public static bool operator >(Int64 x, ushort y)
        {
            return Compare(x, (uint)y) > 0;
        }

        public static bool operator >=(Int64 x, ushort y)
        {
            return Compare(x, (uint)y) >= 0;
        }
        #endregion

        #region Int8
        public static bool operator <(Int64 x, sbyte y)
        {
            return Compare(x, y) < 0;
        }

        public static bool operator <=(Int64 x, sbyte y)
        {
            return Compare(x, y) <= 0;
        }

        public static bool operator >(Int64 x, sbyte y)
        {
            return Compare(x, y) > 0;
        }

        public static bool operator >=(Int64 x, sbyte y)
        {
            return Compare(x, y) >= 0;
        }
        #endregion

        #region UInt8
        public static bool operator <(Int64 x, byte y)
        {
            return Compare(x, (uint)y) < 0;
        }

        public static bool operator <=(Int64 x, byte y)
        {
            return Compare(x, (uint)y) <= 0;
        }

        public static bool operator >(Int64 x, byte y)
        {
            return Compare(x, (uint)y) > 0;
        }

        public static bool operator >=(Int64 x, byte y)
        {
            return Compare(x, (uint)y) >= 0;
        }
        #endregion
        #endregion

        #region Cast Operators
        //TODO: Find and Write table with explicit/implicit conversions and correct using of implicit or explicit keywords.
        //TODO: Conversions with Decimal

        #region FromInt64
        public static bool operator true(Int64 x)
        {
            return x.hi != 0 || x.lo != 0;
        }

        public static bool operator false(Int64 x)
        {
            return x.hi == 0 && x.lo == 0;
        }

        public static explicit operator bool(Int64 x)
        {
            return x.hi != 0 || x.lo != 0;
        }

        public static explicit operator int(Int64 x)
        {
            return (int)x.lo;
        }

        public static explicit operator uint(Int64 x)
        {
            return x.lo;
        }

        public static explicit operator sbyte(Int64 x)
        {
            return (sbyte)x.lo;
        }

        public static explicit operator byte(Int64 x)
        {
            return (byte)x.lo;
        }

        public static explicit operator short(Int64 x)
        {
            return (short)x.lo;
        }

        public static explicit operator ushort(Int64 x)
        {
            return (ushort)x.lo;
        }

        public static explicit operator char(Int64 x)
        {
            return (char)x.lo;
        }

        public static explicit operator double(Int64 x)
        {
            throw new NotImplementedException();
        }

        public static explicit operator float(Int64 x)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region ToInt64
        public static explicit operator Int64(char value)
        {
            return new Int64((uint)value);
        }

        public static explicit operator Int64(bool value)
        {
            return new Int64(value ? 1u : 0u);
        }

        public static implicit operator Int64(int value)
        {
            return new Int64(value);
        }

        public static implicit operator Int64(uint value)
        {
            return new Int64(value);
        }

        public static implicit operator Int64(sbyte value)
        {
            return new Int64(value);
        }

        public static implicit operator Int64(byte value)
        {
            return new Int64((uint)value);
        }

        public static implicit operator Int64(short value)
        {
            return new Int64(value);
        }

        public static implicit operator Int64(ushort value)
        {
            return new Int64((uint)value);
        }

        public static explicit operator Int64(UInt64 x)
        {
            return new Int64(x);
        }

        public static explicit operator Int64(double value)
        {
            Int64 res;
            UInt32 hi, lo;
            if (value < 0)
            {
                value = -value;
                hi = (UInt32)(value / UInt64.DoubleK);
                lo = (UInt32)(value - UInt64.DoubleK * hi);
                res = new Int64(hi, lo);
                res.Negate();
            }
            else
            {
                hi = (UInt32)(value / UInt64.DoubleK);
                lo = (UInt32)(value - UInt64.DoubleK * hi);
                res = new Int64(hi, lo);
            }
            return res;
        }

        public static explicit operator Int64(float value)
        {
            Int64 res;
            UInt32 hi, lo;
            if (value < 0)
            {
                value = -value;
                hi = (UInt32)(value / UInt64.DoubleK);
                lo = (UInt32)(value - UInt64.DoubleK * hi);
                res = new Int64(hi, lo);
                res.Negate();
            }
            else
            {
                hi = (UInt32)(value / UInt64.DoubleK);
                lo = (UInt32)(value - UInt64.DoubleK * hi);
                res = new Int64(hi, lo);
            }
            return res;
        }
        #endregion
        #endregion

        #region Object Override Members
        public override bool Equals(object obj)
        {
            if (obj is Int64)
            {
                Int64 v = (Int64)obj;
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

        #region IComparable Members
        public int CompareTo(object obj)
        {
            return CompareTo((Int64)obj);
        }
        #endregion

        #region IFormattable Members
        public string ToString(string format, IFormatProvider formatProvider)
        {
            //TODO:
            return ((long)this).ToString();
        }
        #endregion

        #region IConvertible Members
        //Note: Since we check range by self then we should avoid redundant using methods of Convert class.

        TypeCode IConvertible.GetTypeCode()
        {
            return TypeCode.Int64;
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
            throw new Exception("The method or operation is not implemented.");
        }

        double IConvertible.ToDouble(IFormatProvider provider)
        {
            return UInt64.DoubleK * hi + lo;
        }

        short IConvertible.ToInt16(IFormatProvider provider)
        {
            if (hi != 0 || lo >= 0x8000)
                if (hi != 0xFFFFFFFF && lo != 0xFFFF8000)
                    throw new OverflowException("value was either too large or too small for an Int16.");
            return (short)lo;
        }

        int IConvertible.ToInt32(IFormatProvider provider)
        {
            if (hi != 0 || lo == 0x80000000)
                if (hi != 0xFFFFFFFF && hi != 0x8000000)
                    throw new OverflowException("value was either too large or too small for an Int32.");
            return (int)lo;
        }

        long IConvertible.ToInt64(IFormatProvider provider)
        {
            //TODO:
            return (long)this;
        }

        sbyte IConvertible.ToSByte(IFormatProvider provider)
        {
            if (hi != 0 || lo >= 0x80)
                if (hi != 0xFFFFFFFF && hi != 0xFFFFFF80)
                    throw new OverflowException("value was either too large or too small for a signed byte.");
            return Convert.ToSByte(lo);
        }

        float IConvertible.ToSingle(IFormatProvider provider)
        {
            double d = UInt64.DoubleK * hi + lo;
            return (float)d;
        }

        public string ToString(IFormatProvider provider)
        {
            return ToString(null, provider);
        }

        object IConvertible.ToType(Type conversionType, IFormatProvider provider)
        {
            //TODO:
            return null;
        }

        ushort IConvertible.ToUInt16(IFormatProvider provider)
        {
            if (hi != 0 || lo > 0xFFFF)
                throw new OverflowException("value was either too large or too small for a UInt16.");
            return (ushort)lo;
        }

        uint IConvertible.ToUInt32(IFormatProvider provider)
        {
            if (hi != 0)
                throw new OverflowException("value was either too large or too small for a UInt32.");
            return lo;
        }

        ulong IConvertible.ToUInt64(IFormatProvider provider)
        {
            //TODO:
            return (ulong)this;
        }
        #endregion

#if NET_2_0
        #region IComparable<Int64> Members
        public int CompareTo(Int64 other)
        {
            return Compare(this, other);
        }
        #endregion

        #region IEquatable<Int64> Members
        public bool Equals(Int64 other)
        {
            return other.hi == hi && other.lo == lo;
        }
        #endregion
#endif

        #region Conditional Members
#if IMPL64
        public Int64(long value)
        {
            ulong v = (ulong)value;
            hi = (int)(v >> 32);
            lo = (uint)v;
        }

        public Int64(ulong value)
        {
            hi = (int)(value >> 32);
            lo = (uint)value;
        }

        public static explicit operator ulong(Int64 x)
        {
            return ((ulong)x.hi << 32) | x.lo;
        }

        public static explicit operator long(Int64 x)
        {
            return (long)(((ulong)x.hi << 32) | x.lo);
        }

        public static explicit operator Int64(ulong x)
        {
            return new Int64(x);
        }

        public static explicit operator Int64(long x)
        {
            return new Int64(x);
        }
#endif
        #endregion
    }
}