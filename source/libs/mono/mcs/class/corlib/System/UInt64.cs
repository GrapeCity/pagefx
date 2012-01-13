using System.Globalization;

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
#if IMPL64
        public static readonly UInt64 MinValue = new UInt64(0u);
        public static readonly UInt64 MaxValue = new UInt64(~0u, ~0u);
#else
        public const ulong MinValue = 0UL;
        public const ulong MaxValue = 18446744073709551615UL;
#endif

        internal const double DoubleK = 4294967296.0;
        #endregion

        #region Fields
        public uint m_hi;
        public uint m_lo;
        #endregion

        public UInt64 m_value
        {
            get { return new UInt64(m_hi, m_lo); }
        }

        #region Constructors
        public UInt64(uint hi, uint lo)
        {
            m_hi = hi;
            m_lo = lo;
        }

        public UInt64(uint value)
        {
            m_hi = 0;
            m_lo = value;
        }

        public UInt64(Int64 value)
        {
            m_hi = (uint)value.m_hi;
            m_lo = value.m_lo;
        }
        #endregion

        #region Arithmetic
        internal static UInt64 AddOvf(UInt64 x, UInt64 y)
        {
            uint h1 = x.m_hi;
            uint l1 = x.m_lo;
            uint h2 = y.m_hi;
            uint l2 = y.m_lo;
            l1 += l2;
            if (l1 < l2)
                throw new OverflowException();
            h1 += h2;
            return new UInt64(h1, l1);
        }

        internal static UInt64 SubOvf(UInt64 x, UInt64 y)
        {
            uint h1 = x.m_hi;
            uint l1 = x.m_lo;
            uint h2 = y.m_hi;
            uint l2 = y.m_lo;
            l1 -= l1;
            if (l1 > ~l2)
                throw new OverflowException();
            h1 -= h2;
            return new UInt64(h1, l1);
        }

        internal static UInt64 MulOvf(UInt64 x, UInt64 y)
        {
            //TODO: Optimze
            UInt64 z = x * y;
            if (y != 0L && z / y != x)
                throw new OverflowException();
            return z;   
        }

        #region Add
        public static UInt64 operator +(UInt64 x, UInt64 y)
        {
            uint y_lo = y.m_lo;
            uint l = x.m_lo + y_lo;
            uint h = x.m_hi + y.m_hi;
            if (l < y_lo)
                h += 1;
            return new UInt64(h, l);
        }

        public static UInt64 operator +(UInt64 x, uint y)
        {
            return x + new UInt64(y);
        }

        public static UInt64 operator +(UInt64 x, int y)
        {
            return x + new UInt64(y);
        }

        public static UInt64 operator +(UInt64 x, byte y)
        {
            return x + new UInt64(y);
        }

        public static UInt64 operator +(UInt64 x, sbyte y)
        {
            return x + new UInt64(y);
        }

        public static UInt64 operator +(UInt64 x, ushort y)
        {
            return x + new UInt64(y);
        }

        public static UInt64 operator +(UInt64 x, short y)
        {
            return x + new UInt64(y);
        }
        #endregion

        #region Sub
        public static UInt64 Sub(UInt64 x, UInt64 y)
        {
            uint h = x.m_hi - y.m_hi;
            uint l = x.m_lo;
            if (l < y.m_lo)
                h -= 1;
            l -= y.m_lo;
            return new UInt64(h, l);
        }

        public static UInt64 operator -(UInt64 x, UInt64 y)
        {
            return Sub(x, y);
        }

        public static UInt64 operator -(UInt64 x, uint y)
        {
            uint l = x.m_lo;
            uint h = x.m_hi;
            if (l < y)
                h -= 1;
            l -= y;
            return new UInt64(h, l);
        }

        public static UInt64 operator -(UInt64 x, int y)
        {
            return x - (uint)y;
        }

        public static UInt64 operator -(UInt64 x, sbyte y)
        {
            return x - (uint)y;
        }

        public static UInt64 operator -(UInt64 x, byte y)
        {
            return x - (uint)y;
        }

        public static UInt64 operator -(UInt64 x, short y)
        {
            return x - (uint)y;
        }

        public static UInt64 operator -(UInt64 x, ushort y)
        {
            return x - (uint)y;
        }
        #endregion

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
            return Multiply(x.m_lo, x.m_hi, y.m_lo, y.m_hi);
        }

        public static UInt64 operator *(UInt64 x, Int64 y)
        {
            return x * new UInt64(y);
        }

        public static UInt64 operator *(UInt64 d1, uint d2)
        {
            if (d2 <= ushort.MaxValue)
            {
                uint c0, c1;
                Mult64by16to64(d1.m_lo, d1.m_hi, (ushort)d2, out c0, out c1);
                return new UInt64(c1, c0);
            }
            else
            {
                return Multiply(d1.m_lo, d1.m_hi, d2, 0);
            }
        }

        public static UInt64 operator *(UInt64 x, int y)
        {
            return x * (uint)y;
        }

        public static UInt64 operator *(UInt64 x, sbyte y)
        {
            return x * new UInt64(y);
        }

        public static UInt64 operator *(UInt64 x, byte y)
        {
            return x * new UInt64(y);
        }

        public static UInt64 operator *(UInt64 x, short y)
        {
            return x * new UInt64(y);
        }

        public static UInt64 operator *(UInt64 x, ushort y)
        {
            return x * new UInt64(y);
        }
        #endregion

        #region Divide
        //http://www.bearcave.com/software/divide.htm
        private const uint MSB = 0x80000000;

        public static UInt64 Divide(UInt64 x, UInt64 y, out UInt64 rem)
        {
            if (y == x)
            {
                rem = new UInt64(0u);
                return new UInt64(1u);
            }

            if (y > x)
            {
                rem = new UInt64(x.m_hi, x.m_lo);
                return new UInt64(0u);
            }

            rem = new UInt64(0u);
            UInt64 q = new UInt64(0u);

            UInt64 dividend = new UInt64(x.m_hi, x.m_lo);
            UInt64 divisor = new UInt64(y.m_hi, y.m_lo);

            // here: dividend > divisor
            int nBits = 64;
            UInt64 d = new UInt64(0u);
            while (Compare(rem, divisor) < 0)
            {
                rem.ShiftLeft();
                if ((dividend.m_hi & MSB) != 0)
                    rem.OrOne();
                d = dividend;
                dividend.ShiftLeft();
                nBits--;
            }

            // undo the last loop iteration
            dividend = d;
            rem.ShiftRight();
            nBits++;

            while (nBits-- > 0)
            {
                rem.ShiftLeft();
                if ((dividend.m_hi & MSB) != 0)
                    rem.OrOne();

                UInt64 t = Sub(rem, divisor);
                dividend.ShiftLeft();
                q.ShiftLeft();
                if ((t.m_hi & MSB) == 0)
                {
                    q.OrOne();
                    rem = t;
                }
            }

            return q;
        }

        public static UInt64 Divide(UInt64 dividend, UInt64 divisor)
        {
            UInt64 r;
            return Divide(dividend, divisor, out r);
        }

        public static UInt64 operator /(UInt64 x, UInt64 y)
        {
            if (y == MinValue)
                throw new DivideByZeroException();
            if (y == x)
                return new UInt64(1);
            if (y > x)
                return MinValue;
            UInt64 r;
            return Divide(x, y, out r);
        }

        public static UInt64 operator /(UInt64 x, uint y)
        {
            return x / new UInt64(y);
        }

        public static UInt64 operator /(UInt64 x, int y)
        {
            return x / (uint)y;
        }

        public static UInt64 operator /(UInt64 x, sbyte y)
        {
            return x / (uint)y;
        }

        public static UInt64 operator /(UInt64 x, byte y)
        {
            return x / (uint)y;
        }

        public static UInt64 operator /(UInt64 x, short y)
        {
            return x / (uint)y;
        }

        public static UInt64 operator /(UInt64 x, ushort y)
        {
            return x / (uint)y;
        }
        #endregion

        #region Modulus
        public static UInt64 operator %(UInt64 x, UInt64 y)
        {
            UInt64 r;
            Divide(x, y, out r);
            return r;
        }

        public static UInt64 operator %(UInt64 x, uint y)
        {
            return x % new UInt64(y);
        }

        public static UInt64 operator %(UInt64 x, int y)
        {
            return x % (uint)y;
        }

        public static UInt64 operator %(UInt64 x, sbyte y)
        {
            return x % (uint)y;
        }

        public static UInt64 operator %(UInt64 x, byte y)
        {
            return x % (uint)y;
        }

        public static UInt64 operator %(UInt64 x, short y)
        {
            return x % (uint)y;
        }

        public static UInt64 operator %(UInt64 x, ushort y)
        {
            return x % (uint)y;
        }
        #endregion

        public static Int64 operator -(UInt64 x)
        {
            Int64 y = new Int64(x);
            y.Negate();
            return y;
        }
        #endregion

        #region Bitwise Operations
        public void ShiftLeft()
        {
            uint h = m_hi;
            uint l = m_lo;
            m_hi = (h << 1) | (l >> 31);
            m_lo = l << 1;
        }

        public void ShiftRight()
        {
            uint h = m_hi;
            uint l = m_lo;
            m_lo = (l >> 1) | (h << 31);
            m_hi = h >> 1;
        }

        //left shift:
        //y1 = (x1 << n) | x0 >> (32 - n) | x0 << (n - 32)
        //y0 = x0 << n
        public static UInt64 operator <<(UInt64 x, int n)
        {
            //Note: it seems that CLR truncates shift
            /* check bad cases: ((n > 63) || (n < 0)) */
            //if ((n & 63) != n) return Zero;
            n &= 63;
            uint l = x.m_lo;
            uint h = x.m_hi;
            if (n != 0)
            {
                if (n > 31)
                {
                    h = l << (n - 32);
                    l = 0;
                }
                else
                {
                    h = (h << n) | (l >> (32 - n));
                    l = l << n;
                }
            }
            return new UInt64(h, l);
        }

        public static UInt64 operator >>(UInt64 x, int n)
        {
            //Note: it seems that CLR truncates shift
            /* check bad cases: ((n > 63) || (n < 0)) */
            //if ((n & 63) != n) return Zero;
            //if ((n & 63) != n) return x;
            n &= 63;
            uint l = x.m_lo;
            uint h = x.m_hi;
            if (n != 0)
            {
                if (n > 31)
                {
                    l = h >> (n - 32);
                    h = 0;
                }
                else
                {
                    l = (l >> n) | (h << (32 - n));
                    h = h >> n;
                }
            }
            return new UInt64(h, l);
        }

        #region And
        public static UInt64 operator &(UInt64 x, UInt64 y)
        {
            return new UInt64(x.m_hi & y.m_hi, x.m_lo & y.m_lo);
        }

        public static UInt64 operator &(UInt64 x, Int64 y)
        {
            return new UInt64(x.m_hi & (uint)y.m_hi, x.m_lo & y.m_lo);
        }

        public static UInt64 operator &(UInt64 x, uint y)
        {
            return new UInt64(x.m_hi, x.m_lo & y);
        }

        public static UInt64 operator &(UInt64 x, int y)
        {
            return new UInt64(x.m_hi, x.m_lo & (uint)y);
        }

        public static UInt64 operator &(UInt64 x, sbyte y)
        {
            return x & (uint)y;
        }

        public static UInt64 operator &(UInt64 x, byte y)
        {
            return x & (uint)y;
        }

        public static UInt64 operator &(UInt64 x, short y)
        {
            return x & (uint)y;
        }

        public static UInt64 operator &(UInt64 x, ushort y)
        {
            return x & (uint)y;
        }
        #endregion

        #region Or
        public void OrOne()
        {
            m_lo = m_lo | 1u;
        }

        public static UInt64 operator |(UInt64 x, UInt64 y)
        {
            return new UInt64(x.m_hi | y.m_hi, x.m_lo | y.m_lo);
        }

        public static UInt64 operator |(UInt64 x, Int64 y)
        {
            return new UInt64(x.m_hi | (uint)y.m_hi, x.m_lo | y.m_lo);
        }

        public static UInt64 operator |(UInt64 x, uint y)
        {
            return new UInt64(x.m_hi, x.m_lo | y);
        }

        public static UInt64 operator |(UInt64 x, int y)
        {
            return new UInt64(x.m_hi, x.m_lo | (uint)y);
        }

        public static UInt64 operator |(UInt64 x, sbyte y)
        {
            return new UInt64(x.m_hi, x.m_lo | (uint)y);
        }

        public static UInt64 operator |(UInt64 x, byte y)
        {
            return new UInt64(x.m_hi, x.m_lo | (uint)y);
        }

        public static UInt64 operator |(UInt64 x, short y)
        {
            return new UInt64(x.m_hi, x.m_lo | (uint)y);
        }

        public static UInt64 operator |(UInt64 x, ushort y)
        {
            return new UInt64(x.m_hi, x.m_lo | (uint)y);
        }
        #endregion

        #region Xor
        public static UInt64 operator ^(UInt64 x, UInt64 y)
        {
            return new UInt64(x.m_hi ^ y.m_hi, x.m_lo ^ y.m_lo);
        }

        public static UInt64 operator ^(UInt64 x, Int64 y)
        {
            return new UInt64(x.m_hi ^ (uint)y.m_hi, x.m_lo ^ y.m_lo);
        }

        public static UInt64 operator ^(UInt64 x, uint y)
        {
            return new UInt64(x.m_hi, x.m_lo ^ y);
        }

        public static UInt64 operator ^(UInt64 x, int y)
        {
            return new UInt64(x.m_hi, x.m_lo ^ (uint)y);
        }

        public static UInt64 operator ^(UInt64 x, sbyte y)
        {
            return x ^ (uint)y;
        }

        public static UInt64 operator ^(UInt64 x, byte y)
        {
            return x ^ (uint)y;
        }

        public static UInt64 operator ^(UInt64 x, short y)
        {
            return x ^ (uint)y;
        }

        public static UInt64 operator ^(UInt64 x, ushort y)
        {
            return x ^ (uint)y;
        }
        #endregion

        public static UInt64 operator ~(UInt64 x)
        {
            return new UInt64(~x.m_hi, ~x.m_lo);
        }
        #endregion

        #region Comparison Operators
        #region Compare
        public static int Compare(UInt64 x, UInt64 y)
        {
            uint a = x.m_hi;
            uint b = y.m_hi;
            if (a < b) return -1;
            if (a > b) return 1;
            a = x.m_lo;
            b = y.m_lo;
            if (a < b) return -1;
            if (a > b) return 1;
            return 0;
        }

        public static int Compare(UInt64 x, Int64 y)
        {
            if (y.m_hi < 0) return 1;
            uint a = x.m_hi;
            uint b = (uint)y.m_hi;
            if (a < b) return -1;
            if (a > b) return 1;
            a = x.m_lo;
            b = y.m_lo;
            if (a < b) return -1;
            if (a > b) return 1;
            return 0;
        }

        public static int Compare(UInt64 x, uint y)
        {
            if (x.m_hi != 0) return 1;
            uint x_lo = x.m_lo;
            if (x_lo < y) return -1;
            if (x_lo > y) return 1;
            return 0;
        }

        public static int Compare(UInt64 x, int y)
        {
            if (y < 0 || x.m_hi != 0) return 1;
            uint x_lo = x.m_lo;
            if (x_lo < y) return -1;
            if (x_lo > y) return 1;
            return 0;
        }
        #endregion

        #region UInt64
        public static bool operator ==(UInt64 x, UInt64 y)
        {
            return x.m_hi == y.m_hi && x.m_lo == y.m_lo;
        }

        public static bool operator !=(UInt64 x, UInt64 y)
        {
            return x.m_hi != y.m_hi || x.m_lo != y.m_lo;
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
        #endregion

        #region UInt32
        public static bool operator ==(UInt64 x, uint y)
        {
            return Compare(x, y) == 0;
        }

        public static bool operator !=(UInt64 x, uint y)
        {
            return Compare(x, y) != 0;
        }

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
        public static bool operator ==(UInt64 x, int y)
        {
            return Compare(x, y) == 0;
        }

        public static bool operator !=(UInt64 x, int y)
        {
            return Compare(x, y) != 0;
        }

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
        public static bool operator ==(UInt64 x, ushort y)
        {
            return Compare(x, (uint)y) == 0;
        }

        public static bool operator !=(UInt64 x, ushort y)
        {
            return Compare(x, (uint)y) != 0;
        }

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
        public static bool operator ==(UInt64 x, short y)
        {
            return Compare(x, y) == 0;
        }

        public static bool operator !=(UInt64 x, short y)
        {
            return Compare(x, y) != 0;
        }

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

        #region Int64
        public static bool operator ==(UInt64 x, Int64 y)
        {
            return Compare(x, y) == 0;
        }

        public static bool operator !=(UInt64 x, Int64 y)
        {
            return Compare(x, y) != 0;
        }

        public static bool operator <(UInt64 x, Int64 y)
        {
            return Compare(x, y) < 0;
        }

        public static bool operator <=(UInt64 x, Int64 y)
        {
            return Compare(x, y) <= 0;
        }

        public static bool operator >(UInt64 x, Int64 y)
        {
            return Compare(x, y) > 0;
        }

        public static bool operator >=(UInt64 x, Int64 y)
        {
            return Compare(x, y) >= 0;
        }
        #endregion
        #endregion

        #region Cast Operators
        //TODO: Find and Write table with explicit/implicit conversions and correct using of implicit or explicit keywords.
        //TODO: Conversions with Decimal

        #region FromUInt64
        public static bool operator true(UInt64 v)
        {
            return v.m_hi != 0 || v.m_lo != 0;
        }

        public static bool operator false(UInt64 v)
        {
            return v.m_hi == 0 && v.m_lo == 0;
        }

        public static explicit operator bool(UInt64 v)
        {
            return v.m_hi != 0 || v.m_lo != 0;
        }

        public static explicit operator int(UInt64 v)
        {
            return (int)v.m_lo;
        }

        public static explicit operator uint(UInt64 v)
        {
            return v.m_lo;
        }

        public static implicit operator double(UInt64 v)
        {
            return DoubleK * v.m_hi + v.m_lo;
        }

        public static explicit operator float(UInt64 v)
        {
            double res = DoubleK * v.m_hi + v.m_lo;
            return (float)res;
        }

        public static explicit operator sbyte(UInt64 v)
        {
            return (sbyte)v.m_lo;
        }

        public static explicit operator byte(UInt64 v)
        {
            return (byte)v.m_lo;
        }

        public static explicit operator short(UInt64 v)
        {
            return (short)v.m_lo;
        }

        public static explicit operator ushort(UInt64 v)
        {
            return (ushort)v.m_lo;
        }

        public static explicit operator char(UInt64 v)
        {
            return (char)v.m_lo;
        }

        //public static explicit operator Int64(UInt64 v)
        //{
        //    return new Int64(v);
        //}

        //public static explicit operator decimal(UInt64 v)
        //{
        //    return new Decimal(this);
        //}
        #endregion

        #region ToUInt64
        public static explicit operator UInt64(bool v)
        {
            return new UInt64(v ? 1u : 0u);
        }

        public static explicit operator UInt64(char v)
        {
            return new UInt64(v);
        }

        public static implicit operator UInt64(byte v)
        {
            return new UInt64(v);
        }

        public static implicit operator UInt64(sbyte v)
        {
            return new UInt64((uint)v);
        }

        public static implicit operator UInt64(ushort v)
        {
            return new UInt64(v);
        }

        public static implicit operator UInt64(short v)
        {
            return new UInt64((uint)v);
        }

        public static implicit operator UInt64(uint v)
        {
            return new UInt64(v);
        }

        public static implicit operator UInt64(int v)
        {
            return new UInt64((uint)v);
        }

        public static explicit operator UInt64(double v)
        {
            uint hi = (uint)(v / DoubleK);
            uint lo = (uint)(v - DoubleK * hi);
            return new UInt64(hi, lo);
        }

        public static explicit operator UInt64(float v)
        {
            uint hi = (uint)(v / DoubleK);
            uint lo = (uint)(v - DoubleK * hi);
            return new UInt64(hi, lo);
        }

        //public static explicit operator UInt64(Int64 v)
        //{
        //    return new UInt64(v);
        //}

        //public static explicit operator UInt64(decimal v)
        //{
        //    return Decimal.u64(v);
        //}
        #endregion
        #endregion

        #region Object Override Members
        public override bool Equals(object obj)
        {
            if (obj is UInt64)
            {
                UInt64 v = (UInt64)obj;
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

            if (!(obj is UInt64))
                throw new ArgumentException("Value is not a System.UInt64", "obj");

            return CompareTo((UInt64)obj);
        }
        #endregion

        #region IComparable<UInt64> Members
        public int CompareTo(UInt64 other)
        {
            return Compare(this, other);
        }
        #endregion

        #region IEquatable<UInt64> Members
        public bool Equals(UInt64 other)
        {
            return other.m_hi == m_hi && other.m_lo == m_lo;
        }
        #endregion

        #region IConvertible Members
        public TypeCode GetTypeCode()
        {
            return TypeCode.UInt64;
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
            return Convert.ToDecimal((ulong)this);
#else
            return new Decimal(this);
#endif
        }

        double IConvertible.ToDouble(IFormatProvider provider)
        {
            return DoubleK * m_hi + m_lo;
        }

        short IConvertible.ToInt16(IFormatProvider provider)
        {
            if (m_hi != 0 || m_lo > Int16.MaxValue)
                throw new OverflowException("Value is greater than Int16.MaxValue");
            return (short)m_lo;
        }

        int IConvertible.ToInt32(IFormatProvider provider)
        {
            if (m_hi != 0 || m_lo > Int32.MaxValue)
                throw new OverflowException("Value is greater than Int32.MaxValue");
            return (int)m_lo;
        }

        long IConvertible.ToInt64(IFormatProvider provider)
        {
            //TODO:
            if (m_hi >= 0x80000000)
                throw new OverflowException("Value is greater than Int64.MaxValue");
            return (long)this;
            //return Convert.ToInt64(m_value);
        }

        sbyte IConvertible.ToSByte(IFormatProvider provider)
        {
            if (m_hi != 0 || m_lo > SByte.MaxValue)
                throw new OverflowException("Value is greater than SByte.MaxValue");
            return Convert.ToSByte(m_lo);
        }

        float IConvertible.ToSingle(IFormatProvider provider)
        {
            double d = DoubleK * m_hi + m_lo;
            float s = (float)d;
            return s;
            //return Convert.ToSingle(m_value);
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
            if (m_hi != 0 || m_lo > (ulong)UInt16.MaxValue)
                throw new OverflowException("Value is greater than UInt16.MaxValue");
            return (ushort)m_lo;
        }

        uint IConvertible.ToUInt32(IFormatProvider provider)
        {
            if (m_hi != 0)
                throw new OverflowException("Value is greater than UInt32.MaxValue");
            return m_lo;
        }

        ulong IConvertible.ToUInt64(IFormatProvider provider)
        {
#if IMPL64
            return (ulong)this;
#else
            return this;
#endif
        }
        #endregion

        #region Parsing
        [CLSCompliant(false)]
        public static UInt64 Parse(string s)
        {
            return Parse(s, NumberStyles.Integer, null);
        }

        [CLSCompliant(false)]
        public static UInt64 Parse(string s, IFormatProvider provider)
        {
            return Parse(s, NumberStyles.Integer, provider);
        }

        [CLSCompliant(false)]
        public static UInt64 Parse(string s, NumberStyles style)
        {
            return Parse(s, style, null);
        }

        internal static readonly UInt64 f0 = new UInt64(0xf0000000u, 0u);

        private class Num : IntParser.INumber
        {
            public UInt64 number;
            private UInt64 old;

            #region INumber Members
            public bool IsHexOverflow()
            {
                return (number & f0) != 0;
            }

            public void AddHexDigit(uint d)
            {
                number = number * 16u + d;
            }

            public bool IsOverflow()
            {
                return number < old;
            }

            public void AddDigit(int d)
            {
                old = number;
                number = number * 10u + (UInt64)d;
            }

            public bool CatchOverflowException
            {
                get { return false; }
            }

            public void BeforeLoop(IntParser parser)
            {
                number = new UInt64(0u);
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

        internal static bool Parse(string s, NumberStyles style, IFormatProvider provider, bool tryParse, out UInt64 result, out Exception exc)
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
        public static UInt64 Parse(string s, NumberStyles style, IFormatProvider fp)
        {
            Exception exc;
            UInt64 res;

            if (!Parse(s, style, fp, false, out res, out exc))
                throw exc;

            return res;
        }

        [CLSCompliant(false)]
        public static bool TryParse(string s, out UInt64 result)
        {
            Exception exc;
            if (!Parse(s, NumberStyles.Integer, null, true, out result, out exc))
            {
                result = 0;
                return false;
            }
            return true;
        }

        [CLSCompliant(false)]
        public static bool TryParse(string s, NumberStyles style, IFormatProvider provider, out UInt64 result)
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
            return ((ulong)this).ToString();
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

        public string ToString(string format, IFormatProvider formatProvider)
        {
#if IMPL64
            return ((ulong)this).ToString(format, formatProvider);
#else
            NumberFormatInfo nfi = NumberFormatInfo.GetInstance(formatProvider);
            return NumberFormatter.NumberToString(format, m_value, nfi);
            //return NumberFormatter.NumberToString(m_value, nfi);
#endif
        }

        public string ToString(IFormatProvider provider)
        {
#if IMPL64
            return ((ulong)this).ToString(provider);
#else
#if OLDNF
            return NumberFormatter.FormatGeneral(new NumberFormatter.NumberStore(this), provider);
#else
            return NumberFormatter.NumberToString(m_value, provider);
#endif
#endif
        }
        #endregion

        #region Conditional Members
#if IMPL64
        public UInt64(ulong value)
        {
            m_hi = (uint)(value >> 32);
            m_lo = (uint)value;
        }

        public static explicit operator ulong(UInt64 x)
        {
            return x.m_hi << 32 | x.m_lo;
        }

        public static explicit operator long(UInt64 x)
        {
            return x.m_hi << 32 | x.m_lo;
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

        public UInt64 Clone()
        {
            return new UInt64(m_hi, m_lo);
        }
    }
}