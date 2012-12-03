using System;
using DataDynamics.PageFX.Common.Utilities;

namespace DataDynamics.PageFX.FLI
{
	internal static class NumericExtensions
	{
		public static float ToFixedSingle(this int value, int q)
		{
			return (float)value / (1 << q);
		}

		public static short FixedToInt16(this float v, int q)
		{
			return (short)(v * (1 << q));
		}

		public static int FixedToInt32(this float v, int q)
		{
			return (int)(v * (1 << q));
		}

		public static int FixedToInt32(this float value)
		{
			return (int)(value * 65536);
		}

		public static int GetMinBits(this byte value)
		{
			return ((uint)value).GetMinBits();
		}

		public static int GetMinBits(this ushort value)
		{
			return ((uint)value).GetMinBits();
		}

		public static int GetMinBits(this short value)
		{
			return ((uint)Math.Abs(value)).GetMinBits() + 1;
		}

		public static int GetMinBits(this uint value)
		{
			return 32 - Tricks.Nlz(value);
		}

		public static int GetMinBits(this int value)
		{
			uint v = (uint)value;
			if (value >= 0)
				return v.GetMinBits() + 1;
			v = ~v;
			int n = Tricks.Nlz(v);
			return 33 - n;
		}

		public static int GetMinBits(this int x1, int x2)
		{
			return Math.Max(x1.GetMinBits(), x2.GetMinBits());
		}

		public static int GetMinBits(this int x1, int x2, int x3, int x4)
		{
			return Math.Max(Math.Max(Math.Max(x1.GetMinBits(), x2.GetMinBits()), x3.GetMinBits()), x4.GetMinBits());
		}

		public static double ToDoublePrecisely(this float value)
		{
			// Standard explicit conversion of float to doube with "(double)" doesn't work.
			// Read http://www.yoda.arachsys.com/csharp/floatingpoint.html

			if (Single.IsNaN(value))
				return Double.NaN;
			if (Single.IsPositiveInfinity(value))
				return Double.PositiveInfinity;
			if (Single.IsNegativeInfinity(value))
				return Double.NegativeInfinity;

			// decimal max/min
			if (value > -79228162514264337593543950335.0f && value < 79228162514264337593543950335.0f)
				return Convert.ToDouble(Convert.ToDecimal(value));

			return value;
		}
	}
}
