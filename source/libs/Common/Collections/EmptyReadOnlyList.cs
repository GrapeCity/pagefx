using System;
using System.Collections;
using System.Collections.Generic;

namespace DataDynamics.PageFX.Common.Collections
{
	public static class EmptyReadOnlyList
	{
		public static IReadOnlyList<T> Create<T>()
		{
			return new EmptyImpl<T>();
		}

		private sealed class EmptyImpl<T> : IReadOnlyList<T>
		{
			public int Count
			{
				get { return 0; }
			}

			public T this[int index]
			{
				get { throw new ArgumentOutOfRangeException("index"); }
			}

			public IEnumerator<T> GetEnumerator()
			{
				yield break;
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}
		}
	}
}