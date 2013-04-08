using System;

namespace DataDynamics.PageFX.Common.TypeSystem
{
	public struct ArrayDimension : IFormattable
	{
		public readonly int LowerBound;
		public readonly int UpperBound;

		public ArrayDimension(int lower, int upper)
		{
			LowerBound = lower;
			UpperBound = upper;
		}

		public override bool Equals(object obj)
		{
			return obj is ArrayDimension && Equals((ArrayDimension)obj);
		}

		public bool Equals(ArrayDimension other)
		{
			return other.LowerBound == LowerBound && other.UpperBound == UpperBound;
		}

		public override int GetHashCode()
		{
			return LowerBound ^ UpperBound;
		}

		public override string ToString()
		{
			return ToString(false);
		}

		public string ToString(string format, IFormatProvider formatProvider)
		{
			return ToString(false);
		}

		public string ToString(bool sig)
		{
			return Format(LowerBound, UpperBound, sig);
		}

		public static string Format(int l, int u, bool sig)
		{
			if (l < 0)
			{
				if (u < 0) return "";
				return u.ToString();
			}
			if (u < 0)
			{
				if (l == 0) return "";
				if (sig) return l + "___";
				return l + "...";
			}
			if (sig) return l + "___" + u;
			return l + "..." + u;
		}
	}
}