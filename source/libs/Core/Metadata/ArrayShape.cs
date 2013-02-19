using System.Text;
using DataDynamics.PageFX.Common.TypeSystem;

namespace DataDynamics.PageFX.Core.Metadata
{
	internal sealed class ArrayShape
	{
		public int Rank;
		public int[] Sizes;
		public int[] LoBounds;

		public static ArrayShape Single
		{
			get
			{
				return _single ?? (_single = new ArrayShape
					{
						LoBounds = new int[0],
						Sizes = new int[0]
					});
			}
		}
		private static ArrayShape _single;

		public int GetLowBound(int index)
		{
			if (LoBounds != null && index >= 0 && index < LoBounds.Length)
				return LoBounds[index];
			return -1;
		}

		public int GetUpperBound(int index)
		{
			if (LoBounds != null && index >= 0 && index < LoBounds.Length
			    && Sizes != null && index < Sizes.Length)
				return LoBounds[index] + Sizes[index] - 1;
			return -1;
		}

		public override string ToString()
		{
			var s = new StringBuilder();
			s.Append("[");
			for (int i = 0; i < Rank; i++)
			{
				if (i > 0) s.Append(", ");
				int l = GetLowBound(i);
				int u = GetUpperBound(i);
				s.Append(ArrayDimension.Format(l, u, false));
			}
			s.Append("]");
			return s.ToString();
		}

		public ArrayDimensionCollection ToDimension()
		{
			int n = Rank;
			if (n <= 0) return ArrayDimensionCollection.Single;

			var dim = new ArrayDimensionCollection();
			for (int i = 0; i < n; ++i)
			{
				dim.Add(new ArrayDimension(GetLowBound(i), GetUpperBound(i)));
			}

			return dim;
		}
	}
}