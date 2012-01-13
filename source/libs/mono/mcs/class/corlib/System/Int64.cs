using System.Globalization;

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
#if IMPL64
        public static readonly Int64 MinValue = new Int64(unchecked((int)0x80000000), 0);
        public static readonly Int64 MaxValue = new Int64(0x7fffffff, ~0u);
#else
        public const long MinValue = -9223372036854775808L;
        public const long MaxValue = 9223372036854775807L;
#endif

        internal static readonly Int64 Zero = new Int64(0, 0);
        internal static readonly Int64 NegativeOne = new Int64(~0, ~0u);
        #endregion

        #region Fields
        public int m_hi;
        public uint m_lo;
        #endregion

        public Int64 m_value
        {
            get { return new Int64(m_hi, m_lo); }
        }

        #region Constructors
        public Int64(int hi, uint lo)
        {
            m_hi = hi;
            m_lo = lo;
        }

        public Int64(uint hi, uint lo)
        {
            m_hi = (int)hi;
            m_lo = lo;
        }

        public Int64(uint value)
        {
            m_lo = value;
            m_hi = 0;
        }

        public Int64(int value)
        {
            m_lo = (uint)value;
            m_hi = value < 0 ? -1 : 0;
        }

        public Int64(UInt64 x)
        {
            m_hi = (int)x.m_hi;
            m_lo = x.m_lo;
        }
        #endregion

        #region Arithmetic
        private const uint SIGN_MASK = 0x80000000;

        internal static Int64 AddOvf(Int64 x, Int64 y)
        {
            uint h1 = (uint)x.m_hi;
            uint l1 = x.m_lo;
            uint h2 = (uint)y.m_hi;
            uint l2 = y.m_lo;

            uint s1 = h1 & SIGN_MASK;
            uint s2 = h2 & SIGN_MASK;

            l1 = l1 + l2;
            if (l1 < l2) h1++; //carry!

            h1 += h2;

            if (s1 == s2 && (h1 & SIGN_MASK) != s1)
                throw new OverflowException();

            return new Int64(h1, l1);
        }

        internal static Int64 SubOvf(Int64 x, Int64 y)
        {
            int h1 = x.m_hi;
            int h2 = y.m_hi;
            uint l2 = y.m_lo;
            uint l1 = x.m_lo;

            uint s1 = (uint)h1 & SIGN_MASK;
            uint s2 = (uint)h2 & SIGN_MASK;

            h1 -= h2;
            if (l1 < l2) //borrow?
                h1 -= 1;
            l1 -= l2;

            if (s1 != s2 && ((uint)h1 & SIGN_MASK) != s1)
                throw new OverflowException();

            return new Int64(h1, l1);
        }

        internal static Int64 MulOvf(Int64 x, Int64 y)
        {
            //TODO: Optimize
            if (y < Zero && x == MinValue)
                throw new OverflowException();

            Int64 z = x * y;
            if (y != Zero && z / y != x)
                throw new OverflowException();

            return z;
        }

        #region Add
        public static Int64 operator +(Int64 x, Int64 y)
        {
            int h = x.m_hi;
            uint l2 = y.m_lo;
            uint l = x.m_lo + l2;
            if (l < l2) h++; //carry!
            h += y.m_hi;
            return new Int64(h, l);
        }

        public static Int64 operator +(Int64 x, UInt64 y)
        {
            return x + new Int64(y);
        }

        public static Int64 operator +(Int64 x, int y)
        {
            return x + new Int64(y);
        }

        public static Int64 operator +(Int64 x, uint y)
        {
            return x + new Int64(y);
        }

        public static Int64 operator +(Int64 x, sbyte y)
        {
            return x + new Int64(y);
        }

        public static Int64 operator +(Int64 x, byte y)
        {
            return x + new Int64(y);
        }

        public static Int64 operator +(Int64 x, short y)
        {
            return x + new Int64(y);
        }

        public static Int64 operator +(Int64 x, ushort y)
        {
            return x + new Int64(y);
        }

        public static Int64 operator +(int x, Int64 y)
        {
            return new Int64(x) + y;
        }

        public static Int64 operator +(uint x, Int64 y)
        {
            return new Int64(x) + y;
        }

        public static Int64 operator +(sbyte x, Int64 y)
        {
            return new Int64(x) + y;
        }

        public static Int64 operator +(byte x, Int64 y)
        {
            return new Int64(x) + y;
        }

        public static Int64 operator +(short x, Int64 y)
        {
            return new Int64(x) + y;
        }

        public static Int64 operator +(ushort x, Int64 y)
        {
            return new Int64(x) + y;
        }
        #endregion

        #region Sub
        public static Int64 operator -(Int64 x, Int64 y)
        {
            uint l2 = y.m_lo;
            uint l = x.m_lo;
            int h = x.m_hi - y.m_hi;
            if (l < l2) //borrow?
                h -= 1;
            l -= l2;
            return new Int64(h, l);
        }

        public static Int64 operator -(Int64 x, UInt64 y)
        {
            return x - new Int64(y);
        }

        public static Int64 operator -(Int64 x, int y)
        {
            return x - new Int64(y);
        }

        public static Int64 operator -(Int64 x, uint y)
        {
            return x - new Int64(y);
        }

        public static Int64 operator -(Int64 x, sbyte y)
        {
            return x - new Int64(y);
        }

        public static Int64 operator -(Int64 x, byte y)
        {
            return x - new Int64(y);
        }

        public static Int64 operator -(Int64 x, short y)
        {
            return x - new Int64(y);
        }

        public static Int64 operator -(Int64 x, ushort y)
        {
            return x - new Int64(y);
        }

        public static Int64 operator -(int x, Int64 y)
        {
            return new Int64(x) - y;
        }

        public static Int64 operator -(uint x, Int64 y)
        {
            return new Int64(x) - y;
        }

        public static Int64 operator -(sbyte x, Int64 y)
        {
            return new Int64(x) - y;
        }

        public static Int64 operator -(byte x, Int64 y)
        {
            return new Int64(x) - y;
        }

        public static Int64 operator -(short x, Int64 y)
        {
            return new Int64(x) - y;
        }

        public static Int64 operator -(ushort x, Int64 y)
        {
            return new Int64(x) - y;
        }
        #endregion

        public void Negate()
        {
            uint l = (~m_lo) + 1;
            int h = ~m_hi;
            if (l == 0)
                ++h;
            m_lo = l;
            m_hi = h;
        }

        public static Int64 operator -(Int64 x)
        {
            uint l = (~x.m_lo) + 1;
            int h = ~x.m_hi;
            if (l == 0)
                ++h;
            return new Int64(h, l);
        }

        #region Multiply
        public static Int64 operator *(Int64 x, Int64 y)
        {
            bool sign1 = x.m_hi < 0;
            bool sign2 = y.m_hi < 0;

            if (sign1) x = -x;
            if (sign2) y = -y;

            UInt64 u = UInt64.Multiply(x.m_lo, (uint)x.m_hi, y.m_lo, (uint)y.m_hi);
            Int64 result = new Int64(u);
            if (sign1 != sign2)
                return -result;
            return result;
        }

        public static Int64 operator *(Int64 x, UInt64 y)
        {
            return x * new Int64(y);
        }

        public static Int64 operator *(Int64 x, int y)
        {
            //UInt32 hi = (y < 0) ? UInt32.MaxValue : 0;
            //UInt64 res = UInt64.Multiply(x.m_lo, (uint)x.m_hi, (UInt32)y, hi);
            //return new Int64(res);
            return x * new Int64(y);
        }

        public static Int64 operator *(Int64 x, uint y)
        {
            return x * new Int64(y);
        }

        public static Int64 operator *(Int64 x, sbyte y)
        {
            return x * new Int64(y);
        }

        public static Int64 operator *(Int64 x, byte y)
        {
            return x * new Int64(y);
        }

        public static Int64 operator *(Int64 x, short y)
        {
            return x * new Int64(y);
        }

        public static Int64 operator *(Int64 x, ushort y)
        {
            return x * new Int64(y);
        }

        public static Int64 operator *(int x, Int64 y)
        {
            return new Int64(x) * y;
        }

        public static Int64 operator *(uint x, Int64 y)
        {
            return new Int64(x) * y;
        }

        public static Int64 operator *(sbyte x, Int64 y)
        {
            return new Int64(x) * y;
        }

        public static Int64 operator *(byte x, Int64 y)
        {
            return new Int64(x) * y;
        }

        public static Int64 operator *(short x, Int64 y)
        {
            return new Int64(x) * y;
        }

        public static Int64 operator *(ushort x, Int64 y)
        {
            return new Int64(x) * y;
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
            UInt64 r;

            dend = Abs(dividend);
            dor = Abs(divisor);
            UInt64 q = UInt64.Divide(dend, dor, out r);

            /* the sign of the remainder is the same as the sign of the dividend
               and the quotient is negated if the signs of the operands are
               opposite */

            Int64 quotient = new Int64(q);
            remainder = new Int64(r);
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

        public static Int64 operator /(Int64 x, UInt64 y)
        {
            return x / new Int64(y);
        }

        public static Int64 operator /(Int64 x, int y)
        {
            return x / new Int64(y);
        }

        public static Int64 operator /(Int64 x, uint y)
        {
            return x / new Int64(y);
        }

        public static Int64 operator /(Int64 x, sbyte y)
        {
            return x / new Int64(y);
        }

        public static Int64 operator /(Int64 x, byte y)
        {
            return x / new Int64(y);
        }

        public static Int64 operator /(Int64 x, short y)
        {
            return x / new Int64(y);
        }

        public static Int64 operator /(Int64 x, ushort y)
        {
            return x / new Int64(y);
        }

        public static Int64 operator /(int x, Int64 y)
        {
            return new Int64(x) / y;
        }

        public static Int64 operator /(uint x, Int64 y)
        {
            return new Int64(x) / y;
        }

        public static Int64 operator /(sbyte x, Int64 y)
        {
            return new Int64(x) / y;
        }

        public static Int64 operator /(byte x, Int64 y)
        {
            return new Int64(x) / y;
        }

        public static Int64 operator /(short x, Int64 y)
        {
            return new Int64(x) / y;
        }

        public static Int64 operator /(ushort x, Int64 y)
        {
            return new Int64(x) / y;
        }
        #endregion

        #region Modulus
        public static Int64 operator %(Int64 x, Int64 y)
        {
            Int64 r;
            Divide(x, y, out r);
            return r;
        }

        public static Int64 operator %(Int64 x, UInt64 y)
        {
            return x % new Int64(y);
        }

        public static Int64 operator %(Int64 x, int y)
        {
            return x % new Int64(y);
        }

        public static Int64 operator %(Int64 x, uint y)
        {
            return x % new Int64(y);
        }

        public static Int64 operator %(Int64 x, sbyte y)
        {
            return x % new Int64(y);
        }

        public static Int64 operator %(Int64 x, byte y)
        {
            return x % new Int64(y);
        }

        public static Int64 operator %(Int64 x, short y)
        {
            return x % new Int64(y);
        }

        public static Int64 operator %(Int64 x, ushort y)
        {
            return x % new Int64(y);
        }

        public static Int64 operator %(int x, Int64 y)
        {
            return new Int64(x) % y;
        }

        public static Int64 operator %(uint x, Int64 y)
        {
            return new Int64(x) % y;
        }

        public static Int64 operator %(sbyte x, Int64 y)
        {
            return new Int64(x) % y;
        }

        public static Int64 operator %(byte x, Int64 y)
        {
            return new Int64(x) % y;
        }

        public static Int64 operator %(short x, Int64 y)
        {
            return new Int64(x) % y;
        }

        public static Int64 operator %(ushort x, Int64 y)
        {
            return new Int64(x) % y;
        }
        #endregion
        #endregion

        #region Bitwise Operations
        #region Shifts
        public static Int64 operator >>(Int64 x, int n)
        {
            //Note: it seems that CLR truncates shift
            /* check bad cases: ((n > 63) || (n < 0)) */
            //if ((n & 63) != n)
            //    return (x.hi >= 0) ? Zero : NegativeOne;
            n &= 63;
            int h = x.m_hi;
            uint l = x.m_lo;
            if (n != 0)
            {
                if (n > 31)
                {
                    l = (uint)(h >> (n - 32));
                    h = h >> 31;
                }
                else
                {
                    l = (l >> n) | ((uint)h << (32 - n));
                    h = h >> n;
                }
            }
            return new Int64(h, l);
        }

        public static Int64 operator <<(Int64 x, int n)
        {
            //Note: it seems that CLR truncates shift
            /* check bad cases: ((n > 63) || (n < 0)) */
            //if ((n & 63) != n) return Zero;
            n &= 63;
            int h = x.m_hi;
            uint l = x.m_lo;
            if (n != 0)
            {
                if (n > 31)
                {
                    h = (int)(l << (n - 32));
                    l = 0;
                }
                else
                {
                    h = (int)(((uint)h << n) | (l >> (32 - n)));
                    l = l << n;
                }
            }
            return new Int64(h, l);
        }
        #endregion

        #region And
        public static Int64 operator &(Int64 x, Int64 y)
        {
            return new Int64(x.m_hi & y.m_hi, x.m_lo & y.m_lo);
        }

        public static Int64 operator &(Int64 x, int y)
        {
            return x & new Int64(y);
        }

        public static Int64 operator &(Int64 x, uint y)
        {
            return x & new Int64(y);
        }

        public static Int64 operator &(Int64 x, sbyte y)
        {
            return x & new Int64(y);
        }

        public static Int64 operator &(Int64 x, byte y)
        {
            return x & new Int64(y);
        }

        public static Int64 operator &(Int64 x, short y)
        {
            return x & new Int64(y);
        }

        public static Int64 operator &(Int64 x, ushort y)
        {
            return x & new Int64(y);
        }

        public static Int64 operator &(int x, Int64 y)
        {
            return new Int64(x) & y;
        }

        public static Int64 operator &(uint x, Int64 y)
        {
            return new Int64(x) & y;
        }

        public static Int64 operator &(sbyte x, Int64 y)
        {
            return new Int64(x) & y;
        }

        public static Int64 operator &(byte x, Int64 y)
        {
            return new Int64(x) & y;
        }

        public static Int64 operator &(short x, Int64 y)
        {
            return new Int64(x) & y;
        }

        public static Int64 operator &(ushort x, Int64 y)
        {
            return new Int64(x) & y;
        }
        #endregion

        #region Or
        public static Int64 operator |(Int64 x, Int64 y)
        {
            return new Int64(x.m_hi | y.m_hi, x.m_lo | y.m_lo);
        }

        public static Int64 operator |(Int64 x, int y)
        {
            return x | new Int64(y);
        }

        public static Int64 operator |(Int64 x, uint y)
        {
            return x | new Int64(y);
        }

        public static Int64 operator |(Int64 x, sbyte y)
        {
            return x | new Int64(y);
        }

        public static Int64 operator |(Int64 x, byte y)
        {
            return x | new Int64(y);
        }

        public static Int64 operator |(Int64 x, short y)
        {
            return x | new Int64(y);
        }

        public static Int64 operator |(Int64 x, ushort y)
        {
            return x | new Int64(y);
        }

        public static Int64 operator |(int x, Int64 y)
        {
            return new Int64(x) | y;
        }

        public static Int64 operator |(uint x, Int64 y)
        {
            return new Int64(x) | y;
        }

        public static Int64 operator |(sbyte x, Int64 y)
        {
            return new Int64(x) | y;
        }

        public static Int64 operator |(byte x, Int64 y)
        {
            return new Int64(x) | y;
        }

        public static Int64 operator |(short x, Int64 y)
        {
            return new Int64(x) | y;
        }

        public static Int64 operator |(ushort x, Int64 y)
        {
            return new Int64(x) | y;
        }
        #endregion

        #region Xor
        public static Int64 operator ^(Int64 x, Int64 y)
        {
            return new Int64(x.m_hi ^ y.m_hi, x.m_lo ^ y.m_lo);
        }

        public static Int64 operator ^(Int64 x, int y)
        {
            return x ^ new Int64(y);
        }

        public static Int64 operator ^(Int64 x, uint y)
        {
            return x ^ new Int64(y);
        }

        public static Int64 operator ^(Int64 x, sbyte y)
        {
            return x ^ new Int64(y);
        }

        public static Int64 operator ^(Int64 x, byte y)
        {
            return x ^ new Int64(y);
        }

        public static Int64 operator ^(Int64 x, short y)
        {
            return x ^ new Int64(y);
        }

        public static Int64 operator ^(Int64 x, ushort y)
        {
            return x ^ new Int64(y);
        }

        public static Int64 operator ^(int x, Int64 y)
        {
            return new Int64(x) ^ y;
        }

        public static Int64 operator ^(uint x, Int64 y)
        {
            return new Int64(x) ^ y;
        }

        public static Int64 operator ^(sbyte x, Int64 y)
        {
            return new Int64(x) ^ y;
        }

        public static Int64 operator ^(byte x, Int64 y)
        {
            return new Int64(x) ^ y;
        }

        public static Int64 operator ^(short x, Int64 y)
        {
            return new Int64(x) ^ y;
        }

        public static Int64 operator ^(ushort x, Int64 y)
        {
            return new Int64(x) ^ y;
        }
        #endregion

        public static Int64 operator ~(Int64 x)
        {
            return new Int64(~x.m_hi, ~x.m_lo);
        }
        #endregion

        #region Comparison Operators
        #region Compare
        public static int Compare(Int64 x, Int64 y)
        {
            if (x.m_hi < y.m_hi) return -1;
            if (x.m_hi > y.m_hi) return 1;
            if (x.m_lo < y.m_lo) return -1;
            if (x.m_lo > y.m_lo) return 1;
            return 0;
        }

        public static int Compare(Int64 x, UInt64 y)
        {
            if (x.m_hi < 0) return -1;
            uint xh = (uint)x.m_hi;
            if (xh < y.m_hi) return -1;
            if (xh > y.m_hi) return 1;
            if (x.m_lo < y.m_lo) return -1;
            if (x.m_lo > y.m_lo) return 1;
            return 0;
        }

        public static int Compare(Int64 x, int y)
        {
            return Compare(x, new Int64(y));
        }

        public static int Compare(Int64 x, uint y)
        {
            int x_hi = x.m_hi;
            if (x_hi != 0) return x_hi > 0 ? 1 : -1;
            uint x_lo = x.m_lo;
            if (x_lo < y) return -1;
            if (x_lo > y) return 1;
            return 0;
        }
        #endregion

        #region Int64
        public static bool operator ==(Int64 x, Int64 y)
        {
            return x.m_hi == y.m_hi && x.m_lo == y.m_lo;
        }

        public static bool operator !=(Int64 x, Int64 y)
        {
            return x.m_hi != y.m_hi || x.m_lo != y.m_lo;
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
        #endregion

        #region Int32
        public static bool operator ==(Int64 x, int y)
        {
            return Compare(x, y) == 0;
        }

        public static bool operator !=(Int64 x, int y)
        {
            return Compare(x, y) != 0;
        }

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
        public static bool operator ==(Int64 x, uint y)
        {
            return Compare(x, y) == 0;
        }

        public static bool operator !=(Int64 x, uint y)
        {
            return Compare(x, y) != 0;
        }

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
        public static bool operator ==(Int64 x, short y)
        {
            return Compare(x, y) == 0;
        }

        public static bool operator !=(Int64 x, short y)
        {
            return Compare(x, y) != 0;
        }

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
        public static bool operator ==(Int64 x, ushort y)
        {
            return Compare(x, (uint)y) == 0;
        }

        public static bool operator !=(Int64 x, ushort y)
        {
            return Compare(x, (uint)y) != 0;
        }

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
        public static bool operator ==(Int64 x, sbyte y)
        {
            return Compare(x, y) == 0;
        }

        public static bool operator !=(Int64 x, sbyte y)
        {
            return Compare(x, y) != 0;
        }

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
        public static bool operator ==(Int64 x, byte y)
        {
            return Compare(x, (uint)y) == 0;
        }

        public static bool operator !=(Int64 x, byte y)
        {
            return Compare(x, (uint)y) != 0;
        }

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

        #region UInt64
        public static bool operator ==(Int64 x, UInt64 y)
        {
            return Compare(x, y) == 0;
        }

        public static bool operator !=(Int64 x, UInt64 y)
        {
            return Compare(x, y) != 0;
        }

        public static bool operator <(Int64 x, UInt64 y)
        {
            return Compare(x, y) < 0;
        }

        public static bool operator <=(Int64 x, UInt64 y)
        {
            return Compare(x, y) <= 0;
        }

        public static bool operator >(Int64 x, UInt64 y)
        {
            return Compare(x, y) > 0;
        }

        public static bool operator >=(Int64 x, UInt64 y)
        {
            return Compare(x, y) >= 0;
        }
        #endregion
        #endregion

        #region Cast Operators
        //TODO: Find and Write table with explicit/implicit conversions and correct using of implicit or explicit keywords.

        #region FromInt64
        public static bool operator true(Int64 v)
        {
            return v.m_hi != 0 || v.m_lo != 0;
        }

        public static bool operator false(Int64 v)
        {
            return v.m_hi == 0 && v.m_lo == 0;
        }

        public static explicit operator bool(Int64 v)
        {
            return v.m_hi != 0 || v.m_lo != 0;
        }

        public static explicit operator int(Int64 v)
        {
            return (int)v.m_lo;
        }

        public static explicit operator uint(Int64 v)
        {
            return v.m_lo;
        }

        public static explicit operator sbyte(Int64 v)
        {
            return (sbyte)v.m_lo;
        }

        public static explicit operator byte(Int64 v)
        {
            return (byte)v.m_lo;
        }

        public static explicit operator short(Int64 v)
        {
            return (short)v.m_lo;
        }

        public static explicit operator ushort(Int64 v)
        {
            return (ushort)v.m_lo;
        }

        public static explicit operator char(Int64 v)
        {
            return (char)v.m_lo;
        }

        public static explicit operator double(Int64 v)
        {
            return UInt64.DoubleK * v.m_hi + v.m_lo;
        }

        public static explicit operator float(Int64 v)
        {
            double d = UInt64.DoubleK * v.m_hi + v.m_lo;
            return (float)d;
        }

        //public static explicit operator decimal(Int64 v)
        //{
        //    return new Decimal(v);
        //}

        public static explicit operator UInt64(Int64 v)
        {
            return new UInt64(v);
        }
        #endregion

        #region ToInt64
        public static explicit operator Int64(char v)
        {
            return new Int64((uint)v);
        }

        public static explicit operator Int64(bool v)
        {
            return new Int64(v ? 1u : 0u);
        }

        public static implicit operator Int64(int v)
        {
            return new Int64(v);
        }

        public static implicit operator Int64(uint v)
        {
            return new Int64(v);
        }

        public static implicit operator Int64(sbyte v)
        {
            return new Int64(v);
        }

        public static implicit operator Int64(byte v)
        {
            return new Int64((uint)v);
        }

        public static implicit operator Int64(short v)
        {
            return new Int64(v);
        }

        public static implicit operator Int64(ushort v)
        {
            return new Int64((uint)v);
        }

        public static explicit operator Int64(UInt64 v)
        {
            return new Int64(v);
        }

        public static explicit operator Int64(double v)
        {
            Int64 res;
            UInt32 hi, lo;
            if (v < 0)
            {
                v = -v;
                hi = (UInt32)(v / UInt64.DoubleK);
                lo = (UInt32)(v - UInt64.DoubleK * hi);
                res = new Int64(hi, lo);
                return -res;
            }
            else
            {
                hi = (UInt32)(v / UInt64.DoubleK);
                lo = (UInt32)(v - UInt64.DoubleK * hi);
                res = new Int64(hi, lo);
            }
            return res;
        }

        public static explicit operator Int64(float v)
        {
            Int64 res;
            UInt32 hi, lo;
            if (v < 0)
            {
                v = -v;
                hi = (UInt32)(v / UInt64.DoubleK);
                lo = (UInt32)(v - UInt64.DoubleK * hi);
                res = new Int64(hi, lo);
                return -res;
            }
            else
            {
                hi = (UInt32)(v / UInt64.DoubleK);
                lo = (UInt32)(v - UInt64.DoubleK * hi);
                res = new Int64(hi, lo);
            }
            return res;
        }

        //public static explicit operator Int64(decimal v)
        //{
        //    return Decimal.s64(v);
        //}
        #endregion
        #endregion

        #region Object Override Members
        public override bool Equals(object obj)
        {
            if (obj is Int64)
            {
                Int64 v = (Int64)obj;
                return v.m_hi == m_hi && v.m_lo == m_lo;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return (int)(m_hi + m_lo);
        }
        #endregion

        #region IComparable Members
        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;

            if (!(obj is Int64))
                throw new ArgumentException("Value is not a System.Int64", "obj");

            return CompareTo((Int64)obj);
        }

        public int CompareTo(Int64 other)
        {
            return Compare(this, other);
        }

        public bool Equals(Int64 other)
        {
            return other.m_hi == m_hi && other.m_lo == m_lo;
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
            return m_hi != 0 || m_lo != 0;
        }

        byte IConvertible.ToByte(IFormatProvider provider)
        {
            if (m_hi != 0 || m_lo > Byte.MaxValue)
                throw new OverflowException("Value is greater than Byte.MaxValue");
            return (byte)m_lo;
        }

        char IConvertible.ToChar(IFormatProvider provider)
        {
            if (m_hi != 0 || m_lo > Char.MaxValue)
                throw new OverflowException("Value is greater than Char.MaxValue");
            return (char)m_lo;
        }

        DateTime IConvertible.ToDateTime(IFormatProvider provider)
        {
            return Convert.ToDateTime(m_lo);
        }

        decimal IConvertible.ToDecimal(IFormatProvider provider)
        {
#if IMPL64
            return Convert.ToDecimal((long)this);
#else
            return new Decimal(this);
#endif
        }

        double IConvertible.ToDouble(IFormatProvider provider)
        {
            return UInt64.DoubleK * m_hi + m_lo;
        }

        short IConvertible.ToInt16(IFormatProvider provider)
        {
            uint h = (uint)m_hi;
            if (h != 0 || m_lo >= 0x8000)
                if (h != 0xFFFFFFFF && m_lo != 0xFFFF8000)
                    throw new OverflowException("value was either too large or too small for an Int16.");
            return (short)m_lo;
        }

        int IConvertible.ToInt32(IFormatProvider provider)
        {
            uint h = (uint)m_hi;
            if (h != 0 || m_lo == 0x80000000)
                if (h != 0xFFFFFFFF && h != 0x8000000)
                    throw new OverflowException("value was either too large or too small for an Int32.");
            return (int)m_lo;
        }

        long IConvertible.ToInt64(IFormatProvider provider)
        {
#if IMPL64
            return (long)this;
#else
            return this;
#endif
        }

        sbyte IConvertible.ToSByte(IFormatProvider provider)
        {
            if (m_hi != 0 || m_lo >= 0x80)
                if (m_hi != 0xFFFFFFFF && m_hi != 0xFFFFFF80)
                    throw new OverflowException("value was either too large or too small for a signed byte.");
            return Convert.ToSByte(m_lo);
        }

        float IConvertible.ToSingle(IFormatProvider provider)
        {
            double d = UInt64.DoubleK * m_hi + m_lo;
            return (float)d;
        }

        object IConvertible.ToType(Type conversionType, IFormatProvider provider)
        {
#if IMPL64
            return null;
#else
            return Convert.ToType(this, conversionType, provider);
#endif
        }

        ushort IConvertible.ToUInt16(IFormatProvider provider)
        {
            if (m_hi != 0 || m_lo > 0xFFFF)
                throw new OverflowException("value was either too large or too small for a UInt16.");
            return (ushort)m_lo;
        }

        uint IConvertible.ToUInt32(IFormatProvider provider)
        {
            if (m_hi != 0)
                throw new OverflowException("value was either too large or too small for a UInt32.");
            return m_lo;
        }

        ulong IConvertible.ToUInt64(IFormatProvider provider)
        {
            return Convert.ToUInt64(this);
        }
        #endregion

        #region Parsing
        internal static bool Parse(string s, bool tryParse, out Int64 result, out Exception exc)
        {
            Int64 val = 0;
            int len;
            int i;
            int sign = 1;
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

                if (c >= '0' && c <= '9')
                {
                    if (tryParse)
                    {
                        val = val * 10 + (c - '0') * sign;

                        if (sign == 1)
                        {
                            if (val < 0)
                                return false;
                        }
                        else if (val > 0)
                            return false;
                    }
                    else
                        val = checked(val * 10 + (c - '0') * sign);
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

        public static Int64 Parse(string s, IFormatProvider fp)
        {
            return Parse(s, NumberStyles.Integer, fp);
        }

        public static Int64 Parse(string s, NumberStyles style)
        {
            return Parse(s, style, null);
        }

        private class Num : IntParser.INumber
        {
            public UInt64 hex;
            public Int64 number;
            private int sign;

            #region INumber Members
            public bool IsHexOverflow()
            {
                return (hex & UInt64.f0) != 0;
            }

            public void AddHexDigit(uint d)
            {
                hex = hex * 16u + (UInt64)d;
            }

            public bool IsOverflow()
            {
                return false;
            }

            public void AddDigit(int d)
            {
                Int64 digit = sign * d;
                number = checked(number * 10 + digit);
            }

            public bool CatchOverflowException
            {
                get { return true; }
            }

            public void BeforeLoop(IntParser parser)
            {
                number = new Int64(0);
                hex = new UInt64(0u);
                sign = parser.negative ? -1 : 1;
            }

            public void AfterLoop(IntParser parser)
            {
                if (parser.AllowHexSpecifier)
                    number = (Int64)hex;
            }

            public bool CheckNegative()
            {
                return true;
            }
            #endregion
        }

        internal static bool Parse(string s, NumberStyles style, IFormatProvider fp, bool tryParse, out Int64 result, out Exception exc)
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

        public static Int64 Parse(string s)
        {
            Exception exc;
            Int64 res;

            if (!Parse(s, false, out res, out exc))
                throw exc;

            return res;
        }

        public static Int64 Parse(string s, NumberStyles style, IFormatProvider fp)
        {
            Exception exc;
            Int64 res;

            if (!Parse(s, style, fp, false, out res, out exc))
                throw exc;

            return res;
        }

        public static bool TryParse(string s, out Int64 result)
        {
            Exception exc;
            if (!Parse(s, true, out result, out exc))
            {
                result = 0;
                return false;
            }
            return true;
        }

        public static bool TryParse(string s, NumberStyles style, IFormatProvider provider, out Int64 result)
        {
            Exception exc;
            if (!Parse(s, style, provider, true, out result, out exc))
            {
                result = 0;
                return false;
            }
            return true;
        }
        #endregion

        #region ToString
        public override string ToString()
        {
#if IMPL64
            return ((long)this).ToString();
#else
#if OLDNF
            return NumberFormatter.FormatGeneral(new NumberFormatter.NumberStore(this));
#else
            return NumberFormatter.NumberToString(m_value, null);
#endif
#endif
        }

        public string ToString(string format)
        {
            return ToString(format, null);
        }

        public string ToString(IFormatProvider fp)
        {
#if IMPL64
            return ((long)this).ToString(fp);
#else
#if OLDNF
            return NumberFormatter.FormatGeneral(new NumberFormatter.NumberStore(this), fp);
#else
            return NumberFormatter.NumberToString(m_value, fp);
#endif
#endif
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
#if IMPL64
            return ((long)this).ToString(format, formatProvider);
#else
            NumberFormatInfo nfi = NumberFormatInfo.GetInstance(formatProvider);
            return NumberFormatter.NumberToString(format, this, nfi);
#endif
        }
        #endregion

        #region Conditional Members
#if IMPL64
        public Int64(long value)
        {
            ulong v = (ulong)value;
            m_hi = (int)(v >> 32);
            m_lo = (uint)v;
        }

        public Int64(ulong value)
        {
            m_hi = (int)(value >> 32);
            m_lo = (uint)value;
        }

        public static explicit operator ulong(Int64 x)
        {
            return ((ulong)x.m_hi << 32) | x.m_lo;
        }

        public static explicit operator long(Int64 x)
        {
            return (long)(((ulong)x.m_hi << 32) | x.m_lo);
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

        public Int64 Clone()
        {
            return new Int64(m_hi, m_lo);
        }

#if !IMPL64
        // For speed we do some Int64 math with doubles. But we must be
        // sure that we don't loose precision in the conversion. This tests
        // whether an Int64 can fit into a double by making sure the number
        // of bits of precision is 53 or less.
        internal static bool FitIntoDouble(Int64 testVal)
        {
            Int64 n = testVal;
            //const long MAXDOUBLEINT = (1L << 53) + 1;
            const long MAXDOUBLEINT = 9007199254740993L;
            if (n == 0) return true;
            if (n < 0) n = -n;

            if (n < MAXDOUBLEINT) return true;	// Quick check before any math

            while ((n & 1) == 0)	// Get rid of zeros on the right
                n >>= 1;

            if (n < MAXDOUBLEINT) return true;
            return false;
        }
#endif
    }
}