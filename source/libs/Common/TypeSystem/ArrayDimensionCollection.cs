using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataDynamics.PageFX.Common.Collections;
using DataDynamics.PageFX.Common.Extensions;

namespace DataDynamics.PageFX.Common.TypeSystem
{
	public sealed class ArrayDimensionCollection : IReadOnlyList<ArrayDimension>, IFormattable
	{
		private readonly IList<ArrayDimension> _list;

		public static readonly ArrayDimensionCollection Single = new ArrayDimensionCollection(null);

		public ArrayDimensionCollection(IEnumerable<ArrayDimension> dimensions)
		{
			_list = (dimensions ?? Enumerable.Empty<ArrayDimension>()).ToList().AsReadOnly();
		}

		public int Count
		{
			get { return _list.Count; }
		}

		public ArrayDimension this[int index]
		{
			get { return _list[index]; }
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public IEnumerator<ArrayDimension> GetEnumerator()
		{
			return _list.GetEnumerator();
		}

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
			if (ReferenceEquals(obj, null)) return true;
			if (ReferenceEquals(obj, this)) return true;
			var c = obj as ArrayDimensionCollection;
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

		private static bool AllDefaults(IReadOnlyList<ArrayDimension> dims)
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

		public static string Format(ArrayDimensionCollection dims, bool sig)
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