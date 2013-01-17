//CHANGED

//
// System.Math.cs
//
// Authors:
//   Bob Smith (bob@thestuff.net)
//   Dan Lewis (dihlewis@yahoo.co.uk)
//   Pedro Martínez Juliá (yoros@wanadoo.es)
//   Andreas Nahr (ClassDevelopment@A-SoftTech.com)
//
// (C) 2001 Bob Smith.  http://www.thestuff.net
// Copyright (C) 2003 Pedro Martínez Juliá <yoros@wanadoo.es>
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

using System.Runtime.CompilerServices;

namespace System
{
#if NET_2_0
	public static class Math
	{
#else
    public sealed class Math
    {
        private Math()
        {
        }
#endif

        public const double E = 2.7182818284590452354;
        public const double PI = 3.14159265358979323846;

        public static decimal Abs(decimal value)
        {
            return (value < 0) ? -value : value;
        }

        public static double Abs(double value)
        {
            return (value < 0) ? -value : value;
        }

        public static float Abs(float value)
        {
            return (value < 0) ? -value : value;
        }

        public static int Abs(int value)
        {
            if (value == Int32.MinValue)
                throw new OverflowException(Locale.GetText("Value is too small."));
            return (value < 0) ? -value : value;
        }

        public static long Abs(long value)
        {
            if (value == Int64.MinValue)
                throw new OverflowException(Locale.GetText("Value is too small."));
            return (value < 0) ? -value : value;
        }

        [CLSCompliant(false)]
        public static sbyte Abs(sbyte value)
        {
            if (value == SByte.MinValue)
                throw new OverflowException(Locale.GetText("Value is too small."));
            return (sbyte)((value < 0) ? -value : value);
        }

        public static short Abs(short value)
        {
            if (value == Int16.MinValue)
                throw new OverflowException(Locale.GetText("Value is too small."));
            return (short)((value < 0) ? -value : value);
        }

#if NET_2_0
		public static decimal Ceiling (decimal d)
		{
			decimal result = Floor(d);
			if (result != d) {
				result++;
			}
			return result;
		}
#endif

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern double Ceiling(double a); // Avm.Math.ceil(a);
        
        // The following methods are defined in ECMA specs but they are
        // not implemented in MS.NET. However, they are in MS.NET 1.1

#if (!NET_1_0)
        internal static long BigMul(int a, int b)
        {
            return ((long)a * (long)b);
        }

        internal static int DivRem(int a, int b, out int result)
        {
            result = (a % b);
            return (int)(a / b);
        }

        internal static long DivRem(long a, long b, out long result)
        {
            result = (a % b);
            return (long)(a / b);
        }
#endif

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern double Floor(double value); // Avm.Math.floor(value)
        
        public static double IEEERemainder(double x, double y)
        {
            double r;
            if (y == 0)
                return Double.NaN;
            r = x - (y * Math.Round(x / y));
            if (r != 0)
                return r;
            return (x > 0) ? 0 : (BitConverter.InternalInt64BitsToDouble(Int64.MinValue));
        }

        public static double Log(double a, double newBase)
        {
            double result = Log(a) / Log(newBase);
            return (result == -0) ? 0 : result;
        }

        public static byte Max(byte val1, byte val2)
        {
            return (val1 > val2) ? val1 : val2;
        }

        public static decimal Max(decimal val1, decimal val2)
        {
            return (val1 > val2) ? val1 : val2;
        }

        public static double Max(double val1, double val2)
        {
            if (Double.IsNaN(val1) || Double.IsNaN(val2))
            {
                return Double.NaN;
            }
            return (val1 > val2) ? val1 : val2;
        }

        public static float Max(float val1, float val2)
        {
            if (Single.IsNaN(val1) || Single.IsNaN(val2))
            {
                return Single.NaN;
            }
            return (val1 > val2) ? val1 : val2;
        }

        public static int Max(int val1, int val2)
        {
            return (val1 > val2) ? val1 : val2;
        }

        public static long Max(long val1, long val2)
        {
            return (val1 > val2) ? val1 : val2;
        }

        [CLSCompliant(false)]
        public static sbyte Max(sbyte val1, sbyte val2)
        {
            return (val1 > val2) ? val1 : val2;
        }

        public static short Max(short val1, short val2)
        {
            return (val1 > val2) ? val1 : val2;
        }

        [CLSCompliant(false)]
        public static uint Max(uint val1, uint val2)
        {
            return (val1 > val2) ? val1 : val2;
        }

        [CLSCompliant(false)]
        public static ulong Max(ulong val1, ulong val2)
        {
            return (val1 > val2) ? val1 : val2;
        }

        [CLSCompliant(false)]
        public static ushort Max(ushort val1, ushort val2)
        {
            return (val1 > val2) ? val1 : val2;
        }

        public static byte Min(byte val1, byte val2)
        {
            return (val1 < val2) ? val1 : val2;
        }

        public static decimal Min(decimal val1, decimal val2)
        {
            return (val1 < val2) ? val1 : val2;
        }

        public static double Min(double val1, double val2)
        {
            if (Double.IsNaN(val1) || Double.IsNaN(val2))
            {
                return Double.NaN;
            }
            return (val1 < val2) ? val1 : val2;
        }

        public static float Min(float val1, float val2)
        {
            if (Single.IsNaN(val1) || Single.IsNaN(val2))
            {
                return Single.NaN;
            }
            return (val1 < val2) ? val1 : val2;
        }

        public static int Min(int val1, int val2)
        {
            return (val1 < val2) ? val1 : val2;
        }

        public static long Min(long val1, long val2)
        {
            return (val1 < val2) ? val1 : val2;
        }

        [CLSCompliant(false)]
        public static sbyte Min(sbyte val1, sbyte val2)
        {
            return (val1 < val2) ? val1 : val2;
        }

        public static short Min(short val1, short val2)
        {
            return (val1 < val2) ? val1 : val2;
        }

        [CLSCompliant(false)]
        public static uint Min(uint val1, uint val2)
        {
            return (val1 < val2) ? val1 : val2;
        }

        [CLSCompliant(false)]
        public static ulong Min(ulong val1, ulong val2)
        {
            return (val1 < val2) ? val1 : val2;
        }

        [CLSCompliant(false)]
        public static ushort Min(ushort val1, ushort val2)
        {
            return (val1 < val2) ? val1 : val2;
        }

        public static decimal Round(decimal d)
        {
            // Just call Decimal.Round(d, 0); when it rounds well.
            decimal int_part = Decimal.Floor(d);
            decimal dec_part = d - int_part;
            if (((dec_part == 0.5M) &&
                ((2.0M * ((int_part / 2.0M) -
                Decimal.Floor(int_part / 2.0M))) != 0.0M)) ||
                (dec_part > 0.5M))
            {
                int_part++;
            }
            return int_part;
        }

        public static decimal Round(decimal d, int decimals)
        {
            return Decimal.Round(d, decimals);
        }

#if NET_2_0
		[MonoTODO ("Not implemented")]
		public static decimal Round (decimal d, MidpointRounding mode)
		{
			if ((mode != MidpointRounding.ToEven) && (mode != MidpointRounding.AwayFromZero))
				throw new ArgumentException ("The value '" + mode + "' is not valid for this usage of the type MidpointRounding.", "mode");

			if (mode == MidpointRounding.ToEven)
				return Round (d);
			throw new NotImplementedException ();
		}

		[MonoTODO ("Not implemented")]
		public static decimal Round (decimal d, int decimals, MidpointRounding mode)
		{
			if ((mode != MidpointRounding.ToEven) && (mode != MidpointRounding.AwayFromZero))
				throw new ArgumentException ("The value '" + mode + "' is not valid for this usage of the type MidpointRounding.", "mode");

			if (mode == MidpointRounding.ToEven)
				return Round (d, decimals);
			throw new NotImplementedException ();
		}
#endif

        public static double Round(double value)
        {
            //return Avm.Math.round(value);

            double int_part, dec_part;
            int_part = Floor(value);
            dec_part = value - int_part;
            if (dec_part < 0.5)
                return int_part;
            if (dec_part > 0.5)
                return int_part + 1;

            // Round towards even number
            int digit = (int)(int_part % 10);
            if ((digit & 1) == 1)	// Odd
                return int_part + 1;
            return int_part;
        }

        public static double Round(double value, int digits)
        {
            if (digits < 0 || digits > 15)
                throw new ArgumentOutOfRangeException(Locale.GetText("Value is too small or too big."));

            return Round2(value, digits);
        }

        private const double doubleRoundLimit = 1E+16;

        private static double Round2(double value, int digits)
        {
            if (digits == 0)
                return Round(value);

            if (Abs(value) < doubleRoundLimit)
            {
                double num = Pow(10, digits);
                value *= num;
                value = Round(value);
                value /= num;
            }
            return value;
        }


#if NET_2_0
		public static double Round (double value, MidpointRounding mode)
		{
			if ((mode != MidpointRounding.ToEven) && (mode != MidpointRounding.AwayFromZero))
				throw new ArgumentException ("The value '" + mode + "' is not valid for this usage of the type MidpointRounding.", "mode");

			if (mode == MidpointRounding.ToEven)
				return Round (value);
			if (value > 0)
				return Floor (value + 0.5);
			else
				return Ceiling (value - 0.5);
		}

		[MonoTODO ("Not implemented")]
		public static double Round (double value, int digits, MidpointRounding mode)
		{
			if ((mode != MidpointRounding.ToEven) && (mode != MidpointRounding.AwayFromZero))
				throw new ArgumentException ("The value '" + mode + "' is not valid for this usage of the type MidpointRounding.", "mode");

			if (mode == MidpointRounding.ToEven)
				return Round (value, digits);
			throw new NotImplementedException ();
		}

		public static double Truncate (double d)
		{
			if (d > 0D)
				return Floor (d);
			else if (d < 0D)
				return Ceiling (d);
			else
				return d;
		}

		public static decimal Truncate (decimal d)
		{
			return Decimal.Truncate (d);
		}

		public static decimal Floor (Decimal d)
		{
			return Decimal.Floor (d);
		}
#endif

        public static int Sign(decimal value)
        {
            if (value > 0) return 1;
            return (value == 0) ? 0 : -1;
        }

        public static int Sign(double value)
        {
            if (Double.IsNaN(value))
                throw new ArithmeticException("NAN");
            if (value > 0) return 1;
            return (value == 0) ? 0 : -1;
        }

        public static int Sign(float value)
        {
            if (Single.IsNaN(value))
                throw new ArithmeticException("NAN");
            if (value > 0) return 1;
            return (value == 0) ? 0 : -1;
        }

        public static int Sign(int value)
        {
            if (value > 0) return 1;
            return (value == 0) ? 0 : -1;
        }

        public static int Sign(long value)
        {
            if (value > 0) return 1;
            return (value == 0) ? 0 : -1;
        }

        [CLSCompliant(false)]
        public static int Sign(sbyte value)
        {
            if (value > 0) return 1;
            return (value == 0) ? 0 : -1;
        }

        public static int Sign(short value)
        {
            if (value > 0) return 1;
            return (value == 0) ? 0 : -1;
        }

        // internal calls
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern double Sin(double x); // Avm.Math.sin(x)

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern double Cos(double x); // Avm.Math.cos(x)

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern double Tan(double x); // Avm.Math.tan(x)
        
        public static double Sinh(double x)
        {
            return (Pow(E, x) - Pow(E, -x)) / 2;
        }

        public static double Cosh(double x)
        {
            return (Pow(E, x) + Pow(E, -x)) / 2;
        }

        public static double Tanh(double x)
        {
            if (Double.IsPositiveInfinity(x))
                return 1.0;
            if (Double.IsNegativeInfinity(x))
                return -1.0;
            double a = Pow(E, x);
            double b = Pow(E, -x);
            return (a - b) / (a + b);
        }

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern double Acos(double x); // Avm.Math.acos(x)

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern double Asin(double x); // Avm.Math.asin(x)

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern double Atan(double x); // Avm.Math.atan(x)

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern double Atan2(double y, double x); // Avm.Math.atan2(y, x)

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern double Exp(double x); // Avm.Math.exp(x)

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern double Log(double x); // Avm.Math.log(x)
        
        public static double Log10(double x)
        {
            return Log(x, 10.0);
        }

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern double Pow(double x, double y);
//        {
//            if (Double.IsNaN(x)) return x;
//            return Avm.Math.pow(x, y);
//        }

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern double Sqrt(double x); // Avm.Math.sqrt(x)

		// Avm.Math.random()
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern double random();
    }
}
