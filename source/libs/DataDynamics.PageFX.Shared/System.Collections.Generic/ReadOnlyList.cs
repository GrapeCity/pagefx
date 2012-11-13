using System.Linq;

namespace System.Collections.Generic
{
    public interface IReadOnlyList<T> : IEnumerable<T>
    {
        int Count { get; }

        T this[int index] { get; }
    }

	public class ListEx<T> : List<T>, IReadOnlyList<T>
	{
	}

    public static class ReadOnlyListExtensions
    {
        private class ArrayAdapter<T> : IReadOnlyList<T>
        {
            public ArrayAdapter(T[] array)
            {
                _array = array;
            }
            private readonly T[] _array;

            public int Count
            {
                get { return _array.Length; }
            }

            public T this[int index]
            {
                get { return _array[index]; }
            }

        	public IEnumerator<T> GetEnumerator()
            {
            	return ((IEnumerable<T>)_array).GetEnumerator();
            }

        	IEnumerator IEnumerable.GetEnumerator()
            {
                return _array.GetEnumerator();
            }
        }

        private class ListAdapter<T> : IReadOnlyList<T>
        {
            public ListAdapter(IList<T> list)
            {
                _list = list;
            }
            private readonly IList<T> _list;

        	public int Count
            {
                get { return _list.Count; }
            }

            public T this[int index]
            {
                get { return _list[index]; }
            }

        	public IEnumerator<T> GetEnumerator()
            {
                return _list.GetEnumerator();
            }

        	IEnumerator IEnumerable.GetEnumerator()
            {
                return _list.GetEnumerator();
            }
        }

		private sealed class SegmentImpl<T> : IReadOnlyList<T>
		{
			private readonly IReadOnlyList<T> _list;
			private readonly int _startIndex;
			private readonly int _count;

			public SegmentImpl(IReadOnlyList<T> list, int startIndex, int count)
			{
				_list = list;
				_startIndex = startIndex;
				_count = count;
			}

			public IEnumerator<T> GetEnumerator()
			{
				for (int i = 0; i < Count; i++)
				{
					yield return this[i];
				}
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}

			public int Count
			{
				get { return _count; }
			}

			public T this[int index]
			{
				get { return _list[_startIndex + index]; }
			}
		}

        public static IReadOnlyList<T> AsReadOnlyList<T>(this T[] array)
        {
        	if (array == null) throw new ArgumentNullException("array");

        	return new ArrayAdapter<T>(array);
        }

    	public static IReadOnlyList<T> AsReadOnlyList<T>(this IList<T> list)
    	{
    		if (list == null) throw new ArgumentNullException("list");

    		return new ListAdapter<T>(list);
    	}

    	public static IReadOnlyList<T> AsReadOnlyList<T>(this IEnumerable<T> seq)
		{
			return new ListAdapter<T>(seq.ToList());
		}

		public static IReadOnlyList<T> Segment<T>(this IReadOnlyList<T> list, int startIndex, int count)
		{
			if (list == null) throw new ArgumentNullException("list");

			return new SegmentImpl<T>(list, startIndex, count);
		}

		public static int BinarySearch<T>(this IReadOnlyList<T> list, int index, int length, Func<T, int> p)
		{
			int left = index;
			int right = (index + length) - 1;
			while (left <= right)
			{
				int mid = left + ((right - left) >> 1);
				int cr = p(list[mid]);
				if (cr == 0)
				{
					return mid;
				}
				if (cr < 0)
				{
					left = mid + 1;
				}
				else
				{
					right = mid - 1;
				}
			}
			return ~left;
		}

		public static int BinarySearch<T>(this T[] array, int index, int length, Func<T, int> p)
		{
			return array.AsReadOnlyList().BinarySearch(index, length, p);
		}

		public static int BinarySearch<T>(this IList<T> list, int index, int length, Func<T, int> p)
		{
			return list.AsReadOnlyList().BinarySearch(index, length, p);
		}

		public static T[] ToArray<T>(this IReadOnlyList<T> list)
		{
			if (list == null)
				throw new ArgumentNullException("list");

			var array = new T[list.Count];

			for (int i = 0; i < list.Count; i++)
			{
				array[i] = list[i];
			}

			return array;
		}
    }
}