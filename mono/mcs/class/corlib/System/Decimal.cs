//
// System.Decimal.cs
//
// Represents a floating-point decimal data type with up to 29 
// significant digits, suitable for financial and commercial calculations.
//
// Author:
//   Martin Weindel (martin.weindel@t-online.de)
//
// (C) 2001 Martin Weindel
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
#if NODDCORLIB
using DDCorLibTest;
#endif
using System;
using System.Globalization;
using System.Text;
using System.Runtime.CompilerServices;

#if MSTEST
using System.Runtime.InteropServices;
#endif

#if NODDCORLIB
using DDDecimal = DDCorLibTest.Decimal;
namespace DDCorLibTest 
#else
using DDDecimal = System.Decimal;
namespace System
#endif
{
    /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;T:System.Decimal&quot;]/*"/>
    public struct Decimal : IFormattable, IConvertible, IComparable
#if NET_2_0
	, IComparable<Decimal>, IEquatable<Decimal>
#endif
    {
        #region Constants
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;F:System.Decimal.MinValue&quot;]/*"/>
        public const decimal MinValue = -79228162514264337593543950335m;
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;F:System.Decimal.MaxValue&quot;]/*"/>
        public const decimal MaxValue = 79228162514264337593543950335m;

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;F:System.Decimal.MinusOne&quot;]/*"/>
        public const decimal MinusOne = -1;
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;F:System.Decimal.One&quot;]/*"/>
        public const decimal One = 1;
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;F:System.Decimal.Zero&quot;]/*"/>
        public const decimal Zero = 0;
        #endregion

        private static readonly Decimal MaxValueDiv10 = MaxValue / 10;


        // maximal decimal value as double
        // private static readonly double dDecMaxValue = 7.922816251426433759354395033e28;
        // epsilon decimal value as double
        // private static readonly double dDecEpsilon = 0.5e-28;  // == 0.5 * 1 / 10^28

        // some constants
        private const int DECIMAL_DIVIDE_BY_ZERO = 5;
        private const uint MAX_SCALE = 28;
        private const int iMAX_SCALE = 28;
        private const uint SIGN_FLAG = 0x80000000;
        private const uint SCALE_MASK = 0x00FF0000;
        private const int SCALE_SHIFT = 16;
        private const uint RESERVED_SS32_BITS = 0x7F00FFFF;

        // internal representation of decimal
        internal uint lo32;
        internal uint mid32;
        internal uint hi32;
        internal uint ss32;

        #region ctors
#if NODDCORLIB
		public Decimal(System.Decimal d) {
			int[] bits = System.Decimal.GetBits(d);
			unchecked {
				lo32 = (uint) bits[0];
				mid32 = (uint) bits[1];
				hi32 = (uint) bits[2];
				ss32 = (uint) bits[3];
			}
		}
		public static implicit operator Decimal (System.Decimal d) {
			return new Decimal(d);
		}
		public static implicit operator System.Decimal (Decimal d) {
			int[] bits = Decimal.GetBits(d);
			return new System.Decimal(bits);
		}
#endif
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Decimal.#ctor(System.Int32,System.Int32,System.Int32,System.Boolean,System.Byte)&quot;]/*"/>
        public Decimal(int lo, int mid, int hi, bool isNegative, byte scale)
        {
            //Console.WriteLine("ctor Decimal lo={0} mid={1} hi={2} scale={3}",lo,mid,hi,scale);
            unchecked
            {
                lo32 = (uint)lo;
                mid32 = (uint)mid;
                hi32 = (uint)hi;

                if (scale > MAX_SCALE)
                {
                    throw new ArgumentOutOfRangeException(Locale.GetText("scale must be between 0 and 28"));
                }

                ss32 = scale;
                ss32 <<= SCALE_SHIFT;
                if (isNegative) ss32 |= SIGN_FLAG;
            }
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Decimal.#ctor(System.Int32)&quot;]/*"/>
        public Decimal(int val)
        {
            unchecked
            {
                hi32 = mid32 = 0;
                if (val < 0)
                {
                    ss32 = SIGN_FLAG;
                    lo32 = ((uint)~val) + 1;
                }
                else
                {
                    ss32 = 0;
                    lo32 = (uint)val;
                }
            }
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Decimal.#ctor(System.UInt32)&quot;]/*"/>
        [CLSCompliant(false)]
        public Decimal(uint val)
        {
            lo32 = val;
            ss32 = hi32 = mid32 = 0;
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Decimal.#ctor(System.Int64)&quot;]/*"/>
        public Decimal(long v)
        //    : this((int)v.m_lo, v.m_hi, 0, v.m_hi < 0, 0)
        {
            unchecked
            {
                hi32 = 0;
                if (v < 0)
                {
                    ss32 = SIGN_FLAG;
                    ulong u = ((ulong)~v) + 1;
                    lo32 = (uint)u;
                    mid32 = (uint)(u >> 32);
                }
                else
                {
                    ss32 = 0;
                    ulong u = (ulong)v;
                    lo32 = (uint)u;
                    mid32 = (uint)(u >> 32);
                }
            }
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Decimal.#ctor(System.UInt64)&quot;]/*"/>
        [CLSCompliant(false)]
        public Decimal(ulong v)
        {
            ss32 = hi32 = 0;
            lo32 = v.m_lo;
            mid32 = v.m_hi;
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Decimal.#ctor(System.Single)&quot;]/*"/>
        public Decimal(float val)
        {
            if (val > (float)Decimal.MaxValue || val < (float)Decimal.MinValue)
                throw new OverflowException(Locale.GetText(
                    "Value is greater than Decimal.MaxValue or less than Decimal.MinValue"));

            if (Single.IsNaN(val))
                throw new OverflowException(Locale.GetText(
                    "Value is not a number"));

            // we must respect the precision (double2decimal doesn't)
            Decimal d = Decimal.Parse(val.ToString(CultureInfo.InvariantCulture),
                    NumberStyles.Float, CultureInfo.InvariantCulture);
            ss32 = d.ss32;
            hi32 = d.hi32;
            lo32 = d.lo32;
            mid32 = d.mid32;
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Decimal.#ctor(System.Double)&quot;]/*"/>
        public Decimal(double val)
        {
            //            Console.WriteLine("Converting double to decimal");

            if (val > (double)Decimal.MaxValue || val < (double)Decimal.MinValue)
            {
                throw new OverflowException(Locale.GetText(
                    "Value is greater than Decimal.MaxValue or less than Decimal.MinValue"));
            }
            if (Double.IsNaN(val))
                throw new OverflowException(Locale.GetText(
                    "Value is not a number"));

            // we must respect the precision (double2decimal doesn't)
            Decimal d = Decimal.Parse(val.ToString(CultureInfo.InvariantCulture),
                    NumberStyles.Float, CultureInfo.InvariantCulture);
            ss32 = d.ss32;
            hi32 = d.hi32;
            lo32 = d.lo32;
            mid32 = d.mid32;
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Decimal.#ctor&quot;]/*"/>
        public Decimal(int[] bits)
        {
            if (bits == null)
            {
                throw new ArgumentNullException(Locale.GetText("Bits is a null reference"));
            }

            if (bits.GetLength(0) != 4)
            {
                throw new ArgumentException(Locale.GetText("bits does not contain four values"));
            }

            unchecked
            {
                lo32 = (uint)bits[0];
                mid32 = (uint)bits[1];
                hi32 = (uint)bits[2];
                ss32 = (uint)bits[3];
                byte scale = (byte)(ss32 >> SCALE_SHIFT);
                if (scale > MAX_SCALE || (ss32 & RESERVED_SS32_BITS) != 0)
                {
                    throw new ArgumentException(Locale.GetText("Invalid bits[3]"));
                }
            }
        }
        #endregion

        public Decimal Clone()
        {
            Decimal d = new Decimal();
            d.lo32 = lo32;
            d.mid32 = mid32;
            d.hi32 = hi32;
            d.ss32 = ss32;
            return d;
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Decimal.FromOACurrency(System.Int64)&quot;]/*"/>
        public static Decimal FromOACurrency(long cy)
        {
            return (Decimal)cy / (Decimal)10000;
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Decimal.GetBits(System.Decimal)&quot;]/*"/>
        public static int[] GetBits(Decimal d)
        {
            unchecked
            {
                return new int[] { (int)d.lo32, (int)d.mid32, (int)d.hi32, 
                                     (int)d.ss32 };
            }
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Decimal.Negate(System.Decimal)&quot;]/*"/>
        public static Decimal Negate(Decimal d)
        {
            d.ss32 ^= SIGN_FLAG;
            return d;
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Decimal.Add(System.Decimal,System.Decimal)&quot;]/*"/>
        public static Decimal Add(Decimal d1, Decimal d2)
        {
            if (DecimalEx.decimalAdd(ref d1, ref d2) == 0)
                return d1;
            else
                throw new OverflowException(Locale.GetText("Overflow on adding decimal number"));
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Decimal.Subtract(System.Decimal,System.Decimal)&quot;]/*"/>
        public static Decimal Subtract(Decimal d1, Decimal d2)
        {
            d2.ss32 ^= SIGN_FLAG;
            int result = DecimalEx.decimalAdd(ref d1, ref d2);
            if (result == 0)
                return d1;
            else
                throw new OverflowException(Locale.GetText("Overflow on subtracting decimal numbers (" + result + ")"));
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Decimal.GetHashCode&quot;]/*"/>
        public override int GetHashCode()
        {
            return (int)(ss32 ^ hi32 ^ lo32 ^ mid32);
        }

        #region Operators
        public static bool operator true(Decimal v)
        {
            return v.lo32 != 0 || v.mid32 != 0 || v.hi32 != 0 || v.ss32 != 0;
        }

        public static bool operator false(Decimal v)
        {
            return v.lo32 == 0 && v.mid32 == 0 && v.hi32 == 0 && v.ss32 == 0;
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Decimal.op_Addition(System.Decimal,System.Decimal)&quot;]/*"/>
        public static Decimal operator +(Decimal d1, Decimal d2)
        {
            return Add(d1, d2);
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Decimal.op_Decrement(System.Decimal)&quot;]/*"/>
        public static Decimal operator --(Decimal d)
        {
            return Add(d, MinusOne);
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Decimal.op_Increment(System.Decimal)&quot;]/*"/>
        public static Decimal operator ++(Decimal d)
        {
            return Add(d, One);
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Decimal.op_Subtraction(System.Decimal,System.Decimal)&quot;]/*"/>
        public static Decimal operator -(Decimal d1, Decimal d2)
        {
            return Subtract(d1, d2);
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Decimal.op_Subtraction(System.Decimal)&quot;]/*"/>
        public static Decimal operator -(Decimal d)
        {
            return Negate(d);
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Decimal.op_Addition(System.Decimal)&quot;]/*"/>
        public static Decimal operator +(Decimal d)
        {
            return d;
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Decimal.op_Multiply(System.Decimal,System.Decimal)&quot;]/*"/>
        public static Decimal operator *(Decimal d1, Decimal d2)
        {
            return Multiply(d1, d2);
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Decimal.op_Division(System.Decimal,System.Decimal)&quot;]/*"/>
        public static Decimal operator /(Decimal d1, Decimal d2)
        {
            return Divide(d1, d2);
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Decimal.op_Modulus(System.Decimal,System.Decimal)&quot;]/*"/>
        public static Decimal operator %(Decimal d1, Decimal d2)
        {
            return Remainder(d1, d2);
        }
        #endregion

        #region FromDecimal
        internal static ulong u64(Decimal value)
        {
            ulong result;

            decimalFloorAndTrunc(ref value, 0);
            if (decimal2UInt64(ref value, out result) != 0)
            {
                throw new System.OverflowException();
            }
            return result;
        }

        internal static long s64(Decimal value)
        {
            long result;

            decimalFloorAndTrunc(ref value, 0);
            if (decimal2Int64(ref value, out result) != 0)
            {
                throw new System.OverflowException();
            }
            return result;
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Decimal.op_Explicit(System.Decimal)~System.Byte&quot;]/*"/>
        public static explicit operator byte(Decimal val)
        {
            ulong result = u64(val);
            return checked((byte)result);
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Decimal.op_Explicit(System.Decimal)~System.SByte&quot;]/*"/>
        [CLSCompliant(false)]
        public static explicit operator sbyte(Decimal val)
        {
            long result = s64(val);
            return checked((sbyte)result);
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Decimal.op_Explicit(System.Decimal)~System.Char&quot;]/*"/>
        public static explicit operator char(Decimal val)
        {
            ulong result = u64(val);
            return checked((char)result);
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Decimal.op_Explicit(System.Decimal)~System.Int16&quot;]/*"/>
        public static explicit operator short(Decimal val)
        {
            long result = s64(val);
            return checked((short)result);
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Decimal.op_Explicit(System.Decimal)~System.UInt16&quot;]/*"/>
        [CLSCompliant(false)]
        public static explicit operator ushort(Decimal val)
        {
            ulong result = u64(val);
            return checked((ushort)result);
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Decimal.op_Explicit(System.Decimal)~System.Int32&quot;]/*"/>
        public static explicit operator int(Decimal val)
        {
            long result = s64(val);
            return checked((int)result);
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Decimal.op_Explicit(System.Decimal)~System.UInt32&quot;]/*"/>
        [CLSCompliant(false)]
        public static explicit operator uint(Decimal val)
        {
            ulong result = u64(val);
            return checked((uint)result);
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Decimal.op_Explicit(System.Decimal)~System.Int64&quot;]/*"/>
        public static explicit operator long(Decimal val)
        {
            return s64(val);
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Decimal.op_Explicit(System.Decimal)~System.UInt64&quot;]/*"/>
        [CLSCompliant(false)]
        public static explicit operator ulong(Decimal val)
        {
            return u64(val);
        }
        #endregion

        #region ToDecimal
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Decimal.op_Implicit(System.Byte)~System.Decimal&quot;]/*"/>
        public static implicit operator Decimal(byte val)
        {
            return new Decimal(val);
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Decimal.op_Implicit(System.SByte)~System.Decimal&quot;]/*"/>
        [CLSCompliant(false)]
        public static implicit operator Decimal(sbyte val)
        {
            return new Decimal(val);
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Decimal.op_Implicit(System.Int16)~System.Decimal&quot;]/*"/>
        public static implicit operator Decimal(short val)
        {
            return new Decimal(val);
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Decimal.op_Implicit(System.UInt16)~System.Decimal&quot;]/*"/>
        [CLSCompliant(false)]
        public static implicit operator Decimal(ushort val)
        {
            return new Decimal(val);
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Decimal.op_Implicit(System.Char)~System.Decimal&quot;]/*"/>
        public static implicit operator Decimal(char val)
        {
            return new Decimal(val);
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Decimal.op_Implicit(System.Int32)~System.Decimal&quot;]/*"/>
        public static implicit operator Decimal(int val)
        {
            return new Decimal(val);
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Decimal.op_Implicit(System.UInt32)~System.Decimal&quot;]/*"/>
        [CLSCompliant(false)]
        public static implicit operator Decimal(uint val)
        {
            return new Decimal(val);
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Decimal.op_Implicit(System.Int64)~System.Decimal&quot;]/*"/>
        public static implicit operator Decimal(long val)
        {
            return new Decimal(val);
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Decimal.op_Implicit(System.UInt64)~System.Decimal&quot;]/*"/>
        [CLSCompliant(false)]
        public static implicit operator Decimal(ulong val)
        {
            return new Decimal(val);
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Decimal.op_Explicit(System.Single)~System.Decimal&quot;]/*"/>
        public static explicit operator Decimal(float val)
        {
            return new Decimal(val);
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Decimal.op_Explicit(System.Double)~System.Decimal&quot;]/*"/>
        public static explicit operator Decimal(double val)
        {
            return new Decimal(val);
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Decimal.op_Explicit(System.Decimal)~System.Single&quot;]/*"/>
        public static explicit operator float(Decimal val)
        {
            return (float)(double)val;
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Decimal.op_Explicit(System.Decimal)~System.Double&quot;]/*"/>
        public static explicit operator double(Decimal val)
        {
            return decimal2double(ref val);
        }
        #endregion

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Decimal.op_Inequality(System.Decimal,System.Decimal)&quot;]/*"/>
        public static bool operator !=(Decimal d1, Decimal d2)
        {
            return !Equals(d1, d2);
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Decimal.op_Equality(System.Decimal,System.Decimal)&quot;]/*"/>
        public static bool operator ==(Decimal d1, Decimal d2)
        {
            return Equals(d1, d2);
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Decimal.op_GreaterThan(System.Decimal,System.Decimal)&quot;]/*"/>
        public static bool operator >(Decimal d1, Decimal d2)
        {
            return decimalCompare(ref d1, ref d2) > 0;
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Decimal.op_GreaterThanOrEqual(System.Decimal,System.Decimal)&quot;]/*"/>
        public static bool operator >=(Decimal d1, Decimal d2)
        {
            return decimalCompare(ref d1, ref d2) >= 0;
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Decimal.op_LessThan(System.Decimal,System.Decimal)&quot;]/*"/>
        public static bool operator <(Decimal d1, Decimal d2)
        {
            return decimalCompare(ref d1, ref d2) < 0;
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Decimal.op_LessThanOrEqual(System.Decimal,System.Decimal)&quot;]/*"/>
        public static bool operator <=(Decimal d1, Decimal d2)
        {
            return decimalCompare(ref d1, ref d2) <= 0;
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Decimal.Equals(System.Decimal,System.Decimal)&quot;]/*"/>
        public static bool Equals(Decimal d1, Decimal d2)
        {
            return decimalCompare(ref d1, ref d2) == 0;
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Decimal.Equals(System.Object)&quot;]/*"/>
        public override bool Equals(object o)
        {
            if (!(o is Decimal))
                return false;

            return Equals((Decimal)o, this);
        }

        // avoid unmanaged call
        private bool IsZero()
        {
            return ((hi32 == 0) && (lo32 == 0) && (mid32 == 0));
        }

        // avoid unmanaged call
        private bool IsNegative()
        {
            return ((ss32 & 0x80000000) == 0x80000000);
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Decimal.Floor(System.Decimal)&quot;]/*"/>
        public static Decimal Floor(Decimal d)
        {
            decimalFloorAndTrunc(ref d, 1);
            return d;
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Decimal.Truncate(System.Decimal)&quot;]/*"/>
        public static Decimal Truncate(Decimal d)
        {
            decimalFloorAndTrunc(ref d, 0);
            return d;
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Decimal.Round(System.Decimal,System.Int32)&quot;]/*"/>
        public static Decimal Round(Decimal d, int decimals)
        {
            //Console.WriteLine("round1 d={0} decimals={1}",d.ToString(),decimals);
            if (decimals < 0 || decimals > 28)
            {
                throw new ArgumentOutOfRangeException("decimals", "[0,28]");
            }
            //Console.WriteLine("round2");

            bool negative = d.IsNegative();
            //Console.WriteLine("round3");
            if (negative)
                d.ss32 ^= SIGN_FLAG;
            //Console.WriteLine("round4");

            // Moved from Math.cs because it's easier to fix the "sign"
            // issue here :( as the logic is OK only for positive numbers
            //Console.WriteLine("round5");
            Decimal int_part = Decimal.Floor(d);
            if (decimals == 0)
                return int_part;

            double doubleP = Math.Pow(10, decimals);
            //Console.WriteLine("doubleP={0}", doubleP);
            Decimal p = (Decimal)Math.Pow(10, decimals);
            //Console.WriteLine("decimalP={0} {1}:{2}:{3}:{4}", p,p.lo32,p.mid32,p.hi32,p.ss32);
            //Console.WriteLine("round6 int_part={0} p={1}",int_part.ToString(),p);
            Decimal dec_part = d - int_part;
            //Console.WriteLine("round7 dec_part="+dec_part.ToString());
            Decimal factor = 10000000000000000000000000000M;
            //Console.WriteLine("round8 p={0} factor={1}",p,factor);
            dec_part *= factor; //10000000000000000000000000000M;
            //Console.WriteLine("round9");
            dec_part = Decimal.Floor(dec_part);
            //Console.WriteLine("round10 p={0} factor={1}",p,factor);
            dec_part /= (factor /*( 10000000000000000000000000000M */ / p);
            //Console.WriteLine("round11");
            dec_part = Math.Round(dec_part);
            //Console.WriteLine("round12");
            dec_part /= p;
            //Console.WriteLine("round13");
            Decimal result = int_part + dec_part;
            //Console.WriteLine("round14");

            // that fixes the precision/scale (which we must keep for output)
            // (moved and adapted from System.Data.SqlTypes.SqlMoney)
            long scaleDiff = decimals - ((result.ss32 & 0x7FFF0000) >> 16);
            //Console.WriteLine("round15");
            // integrify
            if (scaleDiff > 0)
            {
                // This loop should be optimised into a single multiply
                // but its complicated in that we can't overflow
                while (scaleDiff > 0)
                {
                    //Console.WriteLine("round16 {0}",scaleDiff);
                    if (result > MaxValueDiv10)
                        break;
                    result *= 10;

                    scaleDiff--;
                }
            }
            else if (scaleDiff < 0)
            {
                // Decimal division is very expensive, but we've optimised division
                // by a 16bit number, so let's exploit that.
                int[] powerTens = { 1, 10, 100, 1000, 10000 };
                while (scaleDiff < 0)
                {
                    //Console.WriteLine("round17 {0}",scaleDiff);
                    long n = scaleDiff <= -4 ? 4 : -scaleDiff;
                    result /= powerTens[n];
                    scaleDiff += n;

                    //result /= 10;
                    //scaleDiff++;
                }
            }
            //Console.WriteLine("round18 {0}",scaleDiff);
            result.ss32 = (uint)((decimals - scaleDiff) << SCALE_SHIFT);
            //Console.WriteLine("round19 {0}",scaleDiff);
            if (negative)
                result.ss32 ^= SIGN_FLAG;
            return result;
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Decimal.Multiply(System.Decimal,System.Decimal)&quot;]/*"/>
        public static Decimal Multiply(Decimal d1, Decimal d2)
        {
            if (d1.IsZero() || d2.IsZero())
                return Decimal.Zero;

            if (decimalMult(ref d1, ref d2) != 0)
                throw new OverflowException();
            return d1;
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Decimal.Divide(System.Decimal,System.Decimal)&quot;]/*"/>
        public static Decimal Divide(Decimal d1, Decimal d2)
        {
            //Console.WriteLine("Divide1");
            if (d2.IsZero())
                throw new DivideByZeroException();
            //Console.WriteLine("Divide2");
            if (d1.IsZero())
                return Decimal.Zero;
            //Console.WriteLine("Divide3");
            if (d1 == d2)
                return Decimal.One;
            //Console.WriteLine("Divide4");

            d1.ss32 ^= SIGN_FLAG;
            //Console.WriteLine("Divide5");

            if (d1 == d2)
                return Decimal.MinusOne;
            //Console.WriteLine("Divide6");
            d1.ss32 ^= SIGN_FLAG;
            //Console.WriteLine("Divide7");
            Decimal result;
            if (DecimalEx.decimalDiv(out result, ref d1, ref d2) != 0)
                throw new OverflowException();
            //Console.WriteLine("Divide8 complete");
            return result;
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Decimal.Remainder(System.Decimal,System.Decimal)&quot;]/*"/>
        public static Decimal Remainder(Decimal d1, Decimal d2)
        {
            if (d2.IsZero())
                throw new DivideByZeroException();
            if (d1.IsZero())
                return Decimal.Zero;

            bool negative = d1.IsNegative();
            if (negative)
                d1.ss32 ^= SIGN_FLAG;
            if (d2.IsNegative())
                d2.ss32 ^= SIGN_FLAG;

            Decimal result;
            if (d1 == d2)
            {
                return Decimal.Zero;
            }
            else if (d2 > d1)
            {
                result = d1;
            }
            else
            {
                if (decimalIntDiv(out result, ref d1, ref d2) != 0)
                    throw new OverflowException();
                result = d1 - result * d2;
            }

            if (negative)
                result.ss32 ^= SIGN_FLAG;
            return result;
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Decimal.Compare(System.Decimal,System.Decimal)&quot;]/*"/>
        public static int Compare(Decimal d1, Decimal d2)
        {
            return decimalCompare(ref d1, ref d2);
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Decimal.CompareTo(System.Object)&quot;]/*"/>
        public int CompareTo(object val)
        {
            if (val == null)
                return 1;

            if (!(val is Decimal))
                throw new ArgumentException(Locale.GetText("Value is not a System.Decimal"));

            Decimal d2 = (Decimal)val;
            return decimalCompare(ref this, ref d2);
        }

        public int CompareTo(Decimal value)
        {
            return decimalCompare(ref this, ref value);
        }

        public bool Equals(Decimal value)
        {
            return Equals(value, this);
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Decimal.Parse(System.String)&quot;]/*"/>
        public static Decimal Parse(string s)
        {
            return Parse(s, NumberStyles.Number, null);
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Decimal.Parse(System.String,System.Globalization.NumberStyles)&quot;]/*"/>
        public static Decimal Parse(string s, NumberStyles style)
        {
            return Parse(s, style, null);
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Decimal.Parse(System.String,System.IFormatProvider)&quot;]/*"/>
        public static Decimal Parse(string s, IFormatProvider provider)
        {
            return Parse(s, NumberStyles.Number, provider);
        }

        private static string stripStyles(string s, NumberStyles style, NumberFormatInfo nfi,
            out int decPos, out bool isNegative, out bool expFlag, out int exp)
        {
            string invalidChar = Locale.GetText("Invalid character at position ");
            string invalidExponent = Locale.GetText("Invalid exponent");
            isNegative = false;
            expFlag = false;
            exp = 0;
            decPos = -1;

            bool hasSign = false;
            bool hasOpeningParentheses = false;
            bool hasDecimalPoint = false;
            bool allowedLeadingWhiteSpace = ((style & NumberStyles.AllowLeadingWhite) != 0);
            bool allowedTrailingWhiteSpace = ((style & NumberStyles.AllowTrailingWhite) != 0);
            bool allowedLeadingSign = ((style & NumberStyles.AllowLeadingSign) != 0);
            bool allowedTrailingSign = ((style & NumberStyles.AllowTrailingSign) != 0);
            bool allowedParentheses = ((style & NumberStyles.AllowParentheses) != 0);
            bool allowedThousands = ((style & NumberStyles.AllowThousands) != 0);
            bool allowedDecimalPoint = ((style & NumberStyles.AllowDecimalPoint) != 0);
            bool allowedExponent = ((style & NumberStyles.AllowExponent) != 0);

            // get rid of currency symbol
            bool hasCurrency = false;
            if ((style & NumberStyles.AllowCurrencySymbol) != 0)
            {
                int index = s.IndexOf(nfi.CurrencySymbol);
                if (index >= 0)
                {
                    s = s.Remove(index, nfi.CurrencySymbol.Length);
                    hasCurrency = true;
                }
            }

            string decimalSep = (hasCurrency) ? nfi.CurrencyDecimalSeparator : nfi.NumberDecimalSeparator;
            string groupSep = (hasCurrency) ? nfi.CurrencyGroupSeparator : nfi.NumberGroupSeparator;

            int pos = 0;
            int len = s.Length;

            StringBuilder sb = new StringBuilder(len);

            // leading
            while (pos < len)
            {
                char ch = s[pos];
                if (Char.IsDigit(ch))
                {
                    break; // end of leading
                }
                else if (allowedLeadingWhiteSpace && Char.IsWhiteSpace(ch))
                {
                    pos++;
                }
                else if (allowedParentheses && ch == '(' && !hasSign && !hasOpeningParentheses)
                {
                    hasOpeningParentheses = true;
                    hasSign = true;
                    isNegative = true;
                    pos++;
                }
                else if (allowedLeadingSign && ch == nfi.NegativeSign[0] && !hasSign)
                {
                    int slen = nfi.NegativeSign.Length;
                    if (slen == 1 || s.IndexOf(nfi.NegativeSign, pos, slen) == pos)
                    {
                        hasSign = true;
                        isNegative = true;
                        pos += slen;
                    }
                }
                else if (allowedLeadingSign && ch == nfi.PositiveSign[0] && !hasSign)
                {
                    int slen = nfi.PositiveSign.Length;
                    if (slen == 1 || s.IndexOf(nfi.PositiveSign, pos, slen) == pos)
                    {
                        hasSign = true;
                        pos += slen;
                    }
                }
                else if (allowedDecimalPoint && ch == decimalSep[0])
                {
                    int slen = decimalSep.Length;
                    if (slen != 1 && s.IndexOf(decimalSep, pos, slen) != pos)
                    {
                        throw new FormatException(invalidChar + pos);
                    }
                    break;
                }
                else
                {
                    throw new FormatException(invalidChar + pos);
                }
            }

            if (pos == len)
                throw new FormatException(Locale.GetText("No digits found"));

            // digits 
            while (pos < len)
            {
                char ch = s[pos];
                if (Char.IsDigit(ch))
                {
                    sb.Append(ch);
                    pos++;
                }
                else if (allowedThousands && ch == groupSep[0])
                {
                    int slen = groupSep.Length;
                    if (slen != 1 && s.IndexOf(groupSep, pos, slen) != pos)
                    {
                        throw new FormatException(invalidChar + pos);
                    }
                    pos += slen;
                }
                else if (allowedDecimalPoint && ch == decimalSep[0] && !hasDecimalPoint)
                {
                    int slen = decimalSep.Length;
                    if (slen == 1 || s.IndexOf(decimalSep, pos, slen) == pos)
                    {
                        decPos = sb.Length;
                        hasDecimalPoint = true;
                        pos += slen;
                    }
                }
                else
                {
                    break;
                }
            }

            // exponent
            if (pos < len)
            {
                char ch = s[pos];
                if (allowedExponent && Char.ToUpper(ch) == 'E')
                {
                    // FIXUP KPV if (allowedExponent && Char.ToUpperInvariant (ch) == 'E')

                    expFlag = true;
                    pos++; if (pos >= len) throw new FormatException(invalidExponent);
                    ch = s[pos];
                    bool isNegativeExp = false;
                    if (ch == nfi.PositiveSign[0])
                    {
                        int slen = nfi.PositiveSign.Length;
                        if (slen == 1 || s.IndexOf(nfi.PositiveSign, pos, slen) == pos)
                        {
                            pos += slen; if (pos >= len) throw new FormatException(invalidExponent);
                        }
                    }
                    else if (ch == nfi.NegativeSign[0])
                    {
                        int slen = nfi.NegativeSign.Length;
                        if (slen == 1 || s.IndexOf(nfi.NegativeSign, pos, slen) == pos)
                        {
                            pos += slen; if (pos >= len) throw new FormatException(invalidExponent);
                            isNegativeExp = true;
                        }
                    }
                    ch = s[pos];
                    if (!Char.IsDigit(ch)) throw new FormatException(invalidExponent);
                    exp = ch - '0';
                    pos++;
                    while (pos < len && Char.IsDigit(s[pos]))
                    {
                        exp *= 10;
                        exp += s[pos] - '0';
                        pos++;
                    }
                    if (isNegativeExp) exp *= -1;
                }
            }

            // trailing
            while (pos < len)
            {
                char ch = s[pos];
                if (allowedTrailingWhiteSpace && Char.IsWhiteSpace(ch))
                {
                    pos++;
                }
                else if (allowedParentheses && ch == ')' && hasOpeningParentheses)
                {
                    hasOpeningParentheses = false;
                    pos++;
                }
                else if (allowedTrailingSign && ch == nfi.NegativeSign[0] && !hasSign)
                {
                    int slen = nfi.NegativeSign.Length;
                    if (slen == 1 || s.IndexOf(nfi.NegativeSign, pos, slen) == pos)
                    {
                        hasSign = true;
                        isNegative = true;
                        pos += slen;
                    }
                }
                else if (allowedTrailingSign && ch == nfi.PositiveSign[0] && !hasSign)
                {
                    int slen = nfi.PositiveSign.Length;
                    if (slen == 1 || s.IndexOf(nfi.PositiveSign, pos, slen) == pos)
                    {
                        hasSign = true;
                        pos += slen;
                    }
                }
                else
                {
                    throw new FormatException(invalidChar + pos);
                }
            }

            if (hasOpeningParentheses) throw new FormatException(
            Locale.GetText("Closing Parentheses not found"));

            if (!hasDecimalPoint) decPos = sb.Length;

            return sb.ToString();
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Decimal.Parse(System.String,System.Globalization.NumberStyles,System.IFormatProvider)&quot;]/*"/>
        public static Decimal Parse(string s, NumberStyles style, IFormatProvider provider)
        {
            //            Console.WriteLine("  Entered Decimal.Parse with s={0}", s);
            if (s == null)
                throw new ArgumentNullException("s");

            NumberFormatInfo nfi = NumberFormatInfo.GetInstance(provider);

            int iDecPos, exp;
            bool isNegative, expFlag;
            s = stripStyles(s, style, nfi, out iDecPos, out isNegative, out expFlag, out exp);

            if (iDecPos < 0)
                throw new Exception(Locale.GetText("Error in System.Decimal.Parse"));

            // first we remove leading 0
            int len = s.Length;
            int i = 0;
            while ((i < iDecPos) && (s[i] == '0'))
                i++;
            if ((i > 1) && (len > 1))
            {
                s = s.Substring(i, len - i);
                iDecPos -= i;
            }

            //            Console.WriteLine("  after removing leading 0's : {0}", s);

            // first 0. may not be here but is part of the maximum length
            int max = ((iDecPos == 0) ? 27 : 28);
            len = s.Length;
            if (len >= max + 1)
            {
                // number lower than MaxValue (base-less) can have better precision
                if (String.Compare(s, 0, "79228162514264337593543950335", 0, max + 1,
                    false, CultureInfo.InvariantCulture) <= 0)
                {
                    max++;
                }
            }

            // then we trunc the string
            if ((len > max) && (iDecPos < len))
            {
                int round = (s[max] - '0');
                s = s.Substring(0, max);

                bool addone = false;
                if (round > 5)
                {
                    addone = true;
                }
                else if (round == 5)
                {
                    if (isNegative)
                    {
                        addone = true;
                    }
                    else
                    {
                        // banker rounding applies :(
                        int previous = (s[max - 1] - '0');
                        addone = ((previous & 0x01) == 0x01);
                    }
                }
                if (addone)
                {
                    char[] array = s.ToCharArray();
                    int p = max - 1;
                    while (p >= 0)
                    {
                        int b = (array[p] - '0');
                        if (array[p] != '9')
                        {
                            array[p] = (char)(b + '1');
                            break;
                        }
                        else
                        {
                            array[p--] = '0';
                        }
                    }
                    if ((p == -1) && (array[0] == '0'))
                    {
                        iDecPos++;
                        s = "1".PadRight(iDecPos, '0');
                    }
                    else
                        s = new String(array);
                }
            }

            Decimal result;
            // always work in positive (rounding issues)
            if (DecimalEx.string2decimal(out result, s, (uint)iDecPos, 0) != 0)
                throw new OverflowException();

            //            Console.WriteLine("  after string2decimal result = {0},{1},{2},{3}", result.lo32, result.mid32, result.hi32, result.ss32);

            if (expFlag)
            {
                if (decimalSetExponent(ref result, exp) != 0)
                    throw new OverflowException();
            }

            if (isNegative)
                result.ss32 ^= SIGN_FLAG;

            //            Console.WriteLine("  Exit Decimal.Parse with result={0}", result);
            return result;
        }

        public TypeCode GetTypeCode()
        {
            return TypeCode.Decimal;
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Decimal.ToByte(System.Decimal)&quot;]/*"/>
        public static byte ToByte(Decimal value)
        {
            value = Decimal.Truncate(value);
            if (value > Byte.MaxValue || value < Byte.MinValue)
                throw new OverflowException(Locale.GetText(
                    "Value is greater than Byte.MaxValue or less than Byte.MinValue"));
            return (byte)value;
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Decimal.ToDouble(System.Decimal)&quot;]/*"/>
        public static double ToDouble(Decimal value)
        {
            return Convert.ToDouble(value);
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Decimal.ToInt16(System.Decimal)&quot;]/*"/>
        public static short ToInt16(Decimal value)
        {
            value = Decimal.Truncate(value);
            if (value > Int16.MaxValue || value < Int16.MinValue)
                throw new OverflowException(Locale.GetText(
                    "Value is greater than Int16.MaxValue or less than Int16.MinValue"));

            // return truncated value
            return (Int16)value;
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Decimal.ToInt32(System.Decimal)&quot;]/*"/>
        public static int ToInt32(Decimal value)
        {
            value = Decimal.Truncate(value);
            if (value > Int32.MaxValue || value < Int32.MinValue)
                throw new OverflowException(Locale.GetText(
                    "Value is greater than Int32.MaxValue or less than Int32.MinValue"));

            // return truncated value
            return (Int32)value;
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Decimal.ToInt64(System.Decimal)&quot;]/*"/>
        public static long ToInt64(Decimal value)
        {
            value = Decimal.Truncate(value);
            if (value > Int64.MaxValue || value < Int64.MinValue)
                throw new OverflowException(Locale.GetText(
                    "Value is greater than Int64.MaxValue or less than Int64.MinValue"));

            // return truncated value
            return s64(value);
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Decimal.ToOACurrency(System.Decimal)&quot;]/*"/>
        public static long ToOACurrency(Decimal value)
        {
            value = Decimal.Round(value * 10000, 0);
            return s64(value);
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Decimal.ToSByte(System.Decimal)&quot;]/*"/>
        [CLSCompliant(false)]
        public static sbyte ToSByte(Decimal value)
        {
            value = Decimal.Truncate(value);
            if (value > SByte.MaxValue || value < SByte.MinValue)
                throw new OverflowException(Locale.GetText(
                    "Value is greater than SByte.MaxValue or less than SByte.MinValue"));

            // return truncated value
            return (SByte)value;
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Decimal.ToSingle(System.Decimal)&quot;]/*"/>
        public static float ToSingle(Decimal value)
        {
            return Convert.ToSingle(value);
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Decimal.ToUInt16(System.Decimal)&quot;]/*"/>
        [CLSCompliant(false)]
        public static ushort ToUInt16(Decimal value)
        {
            value = Decimal.Truncate(value);
            if (value > UInt16.MaxValue || value < UInt16.MinValue)
                throw new OverflowException(Locale.GetText(
                    "Value is greater than UInt16.MaxValue or less than UInt16.MinValue"));

            // return truncated value
            return (UInt16)value;
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Decimal.ToUInt32(System.Decimal)&quot;]/*"/>
        [CLSCompliant(false)]
        public static uint ToUInt32(Decimal value)
        {
            value = Decimal.Truncate(value);
            if (value > UInt32.MaxValue || value < UInt32.MinValue)
                throw new OverflowException(Locale.GetText(
                    "Value is greater than UInt32.MaxValue or less than UInt32.MinValue"));

            // return truncated value
            return (UInt32)value;
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Decimal.ToUInt64(System.Decimal)&quot;]/*"/>
        [CLSCompliant(false)]
        public static ulong ToUInt64(Decimal value)
        {
            value = Decimal.Truncate(value);
            if (value > UInt64.MaxValue || value < UInt64.MinValue)
                throw new OverflowException(Locale.GetText(
                    "Value is greater than UInt64.MaxValue or less than UInt64.MinValue"));

            // return truncated value
            return u64(value);
        }

        object IConvertible.ToType(Type conversionType, IFormatProvider provider)
        {
            return new Object();
            // FIXUP return Convert.ToType (this, conversionType, provider);
        }

        bool IConvertible.ToBoolean(IFormatProvider provider)
        {
            return Convert.ToBoolean(this);
        }

        byte IConvertible.ToByte(IFormatProvider provider)
        {
            return Convert.ToByte(this);
        }

        char IConvertible.ToChar(IFormatProvider provider)
        {
            throw new InvalidCastException("316");
        }

        DateTime IConvertible.ToDateTime(IFormatProvider provider)
        {
            throw new InvalidCastException("317");
        }

        decimal IConvertible.ToDecimal(IFormatProvider provider)
        {
            return this;
        }

        double IConvertible.ToDouble(IFormatProvider provider)
        {
            return Convert.ToDouble(this);
        }

        short IConvertible.ToInt16(IFormatProvider provider)
        {
            return Convert.ToInt16(this);
        }

        int IConvertible.ToInt32(IFormatProvider provider)
        {
            return Convert.ToInt32(this);
        }

        long IConvertible.ToInt64(IFormatProvider provider)
        {
            return Convert.ToInt64(this);
        }

        sbyte IConvertible.ToSByte(IFormatProvider provider)
        {
            return Convert.ToSByte(this);
        }

        float IConvertible.ToSingle(IFormatProvider provider)
        {
            return Convert.ToSingle(this);
        }

        ushort IConvertible.ToUInt16(IFormatProvider provider)
        {
            return Convert.ToUInt16(this);
        }

        uint IConvertible.ToUInt32(IFormatProvider provider)
        {
            return Convert.ToUInt32(this);
        }

        ulong IConvertible.ToUInt64(IFormatProvider provider)
        {
            return Convert.ToUInt64(this);
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Decimal.ToString(System.String,System.IFormatProvider)&quot;]/*"/>
        public string ToString(string format, IFormatProvider provider)
        {
            //Console.WriteLine("Decimal::ToString");

            NumberFormatInfo nfi = NumberFormatInfo.GetInstance(provider);
            String result;

            // use "G" for null or empty string
            if ((format == null) || (format.Length == 0))
                format = "G";

            //Console.WriteLine("Decimalformatter called with {0}:{1}:{2}:{3}",this.ss32,this.hi32,this.mid32,this.lo32);
            result = DecimalFormatter.NumberToString(format, nfi, this);
            return result;
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Decimal.ToString&quot;]/*"/>
        public override string ToString()
        {
            return ToString("G", null);
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Decimal.ToString(System.String)&quot;]/*"/>
        public string ToString(string format)
        {
            return ToString(format, null);
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Decimal.ToString(System.IFormatProvider)&quot;]/*"/>
        public string ToString(IFormatProvider provider)
        {
            return ToString("G", provider);
        }

        internal int Scale
        {
            get
            {
                return ((int)(ss32 & SCALE_MASK)) >> SCALE_SHIFT;
            }
            set
            {
                ss32 &= (~SCALE_MASK);
                ss32 |= ((UInt32)value << SCALE_SHIFT);
            }
        }
        internal int Sign
        {
            get
            {
                return (ss32 & SIGN_FLAG) == 0 ? 0 : 1;
            }
        }
        //========================================================
        // Routines below were all originally written in C
        //

        private static int decimal2UInt64(ref DDDecimal val,
            out ulong result)
        {
            int n = DecimalEx.decimal2UInt64(ref val, out result);
            return n;
        }

        private static int decimal2Int64(ref DDDecimal val,
            out long result)
        {
            int n = DecimalEx.decimal2Int64(ref val, out result);
            return n;
        }

#if false // Never used? KPV
        private static int double2decimal(out Decimal erg, 
            double val, int digits)
		{
			throw new PageFX.NotImplementedException("Decimal.cs");
		}
#endif


        internal static int decimalSetExponent(ref Decimal val, int exp)
        {
            int n = DecimalEx.decimalSetExponent(ref val, exp);
            return n;
        }

        private static double decimal2double(ref Decimal val)
        {
            double d;
#if false
			d = DDCorLibTest.DecimalEx.decimal2double(ref val);
#else
            string s = val.ToString();
            d = Double.Parse(s);
#endif
            return d;
        }

        private static void decimalFloorAndTrunc(ref Decimal val, int floorFlag)
        {
            DecimalEx.decimalFloorAndTrunc(ref val, floorFlag);
        }

        //private static void decimalRound(ref Decimal val, int decimals)
        //{
        //    DecimalEx.decimalRound (ref val, decimals);
        //}

        private static int decimalMult(ref Decimal pd1, ref Decimal pd2)
        {
            int n = DecimalEx.decimalMult(ref pd1, pd2);
            return n;
        }

        /*private static int decimalDiv(out Decimal pc, ref Decimal pa, ref Decimal pb)
        {
            int n = DecimalEx.decimalDiv(out pc, ref pa, ref pb);
            return n;
        }*/

        private static int decimalIntDiv(out Decimal pc, ref Decimal pa, ref Decimal pb)
        {
            int n = DecimalEx.decimalIntDiv(out pc, ref pa, ref pb);
            return n;
        }

        private static int decimalCompare(ref Decimal d1, ref Decimal d2)
        {
            return DecimalEx.decimalCompare(ref d1, ref d2);
        }
    }
}
