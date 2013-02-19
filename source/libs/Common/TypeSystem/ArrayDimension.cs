using System;

namespace DataDynamics.PageFX.Common.TypeSystem
{
	public sealed class ArrayDimension : IArrayDimension
	{
		public ArrayDimension(int lower, int upper)
		{
			LowerBound = lower;
			UpperBound = upper;
		}

		public int LowerBound { get; private set; }
		public int UpperBound { get; private set; }

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(obj, null)) return false;
			if (ReferenceEquals(obj, this)) return true;
			return obj is ArrayDimension && Equals((ArrayDimension)obj);
		}

		public bool Equals(ArrayDimension other)
		{
			if (ReferenceEquals(other, null)) return false;
			if (ReferenceEquals(other, this)) return true;
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