using System;
using System.Collections.Generic;
using System.Text;
using DataDynamics.PageFX.Common.Collections;
using DataDynamics.PageFX.Common.Extensions;

namespace DataDynamics.PageFX.Common.TypeSystem
{
	public sealed class ArrayDimensionCollection : List<IArrayDimension>, IArrayDimensionCollection
	{
		public static readonly ArrayDimensionCollection Single = new ArrayDimensionCollection();

		public string ToString(string format, IFormatProvider formatProvider)
		{
			var s = new StringBuilder();
			s.Append("[");
			int n = Count;
			for (int i = 0; i < n; ++i)
			{
				if (i > 0) s.Append(",");
				s.Append(this[i]);
			}
			s.Append("]");
			return s.ToString();
		}

		public override bool Equals(object obj)
		{
			if (obj == this) return false;
			var c = obj as IArrayDimensionCollection;
			if (c == null) return false;
			return this.EqualsTo(c);
		}

		public override int GetHashCode()
		{
			return this.EvalHashCode();
		}

		public override string ToString()
		{
			return ToString(false);
		}

		public string ToString(bool sig)
		{
			return Format(this, sig);
		}

		private static bool AllDefaults(IReadOnlyList<IArrayDimension> dims)
		{
			int n = dims.Count;
			for (int i = 0; i < n; ++i)
			{
				var d = dims[i];
				if (!(d.LowerBound <= 0 && d.UpperBound < 0))
					return false;
			}
			return true;
		}

		public static string Format(IArrayDimensionCollection dims, bool sig)
		{
			var sb = new StringBuilder();
			int n = dims.Count;
			if (sig)
			{
				if (n == 0) return "_array";
				sb.Append("_" + n + "D_array");
				if (!AllDefaults(dims))
				{
					for (int i = 0; i < n; ++i)
					{
						if (i > 0) sb.Append('_');
						var d = dims[i];
						sb.Append(ArrayDimension.Format(d.LowerBound, d.UpperBound, true));
					}
				}
			}
			else
			{
				sb.Append('[');
				for (int i = 0; i < n; ++i)
				{
					if (i > 0) sb.Append(',');
					var d = dims[i];
					sb.Append(ArrayDimension.Format(d.LowerBound, d.UpperBound, false));
				}
				sb.Append(']');
			}
			return sb.ToString();
		}
	}
}