using System.Linq;

namespace System.Collections.Generic
{
    public interface IReadOnlyList<T> : IEnumerable<T>
    {
        int Count { get; }

        T this[int index] { get; }
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

        public static IReadOnlyList<T> AsReadOnlyList<T>(this T[] array)
        {
            return new ArrayAdapter<T>(array);
        }

        public static IReadOnlyList<T> AsReadOnlyList<T>(this IList<T> list)
        {
            return new ListAdapter<T>(list);
        }

		public static IReadOnlyList<T> AsReadOnlyList<T>(this IEnumerable<T> seq)
		{
			return new ListAdapter<T>(seq.ToList());
		}
    }
}