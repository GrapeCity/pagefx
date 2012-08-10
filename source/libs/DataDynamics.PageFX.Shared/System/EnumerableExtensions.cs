using System.Collections;
using System.Collections.Generic;

namespace System
{
    public static class EnumerableExtensions
    {
		public static IEnumerable<T> AsContinuous<T>(this IList<T> list)
		{
			if (list == null) throw new ArgumentNullException("list");

			return new ContinuousList<T>(list);
		}

		public static IEnumerable<T> AsContinuous<T>(this IReadOnlyList<T> list)
		{
			if (list == null) throw new ArgumentNullException("list");

			return new ContinuousReadOnlyList<T>(list);
		}

	    private sealed class ContinuousList<T> : IEnumerable<T>
	    {
		    private readonly IList<T> _list;

		    public ContinuousList(IList<T> list)
		    {
			    _list = list;
		    }

		    public IEnumerator<T> GetEnumerator()
		    {
			    return new Enumerator(_list);
		    }

		    private struct Enumerator : IEnumerator<T>
		    {
			    private IList<T> _list;
			    private int _index;

			    public Enumerator(IList<T> list)
			    {
				    _index = -1;
				    _list = list;
			    }

			    public void Dispose()
			    {
				    _list = null;
			    }

			    public bool MoveNext()
			    {
				    ++_index;
					return _index < _list.Count;
			    }

			    public void Reset()
			    {
				    _index = -1;
			    }

			    public T Current
			    {
					get { return _list[_index]; }
			    }

			    object IEnumerator.Current
			    {
				    get { return Current; }
			    }
		    }

		    IEnumerator IEnumerable.GetEnumerator()
		    {
			    return GetEnumerator();
		    }
	    }

		private sealed class ContinuousReadOnlyList<T> : IEnumerable<T>
		{
			private readonly IReadOnlyList<T> _list;

			public ContinuousReadOnlyList(IReadOnlyList<T> list)
			{
				_list = list;
			}

			public IEnumerator<T> GetEnumerator()
			{
				return new Enumerator(_list);
			}

			private struct Enumerator : IEnumerator<T>
			{
				private IReadOnlyList<T> _list;
				private int _index;

				public Enumerator(IReadOnlyList<T> list)
				{
					_index = -1;
					_list = list;
				}

				public void Dispose()
				{
					_list = null;
				}

				public bool MoveNext()
				{
					++_index;
					return _index < _list.Count;
				}

				public void Reset()
				{
					_index = -1;
				}

				public T Current
				{
					get { return _list[_index]; }
				}

				object IEnumerator.Current
				{
					get { return Current; }
				}
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}
		}

	    public static bool IsEmpty(this IEnumerable x)
        {
            if (x == null) return true;
            var e = x.GetEnumerator();
            return !e.MoveNext();
        }

        public static bool EqualsTo(this IEnumerable x, IEnumerable y)
        {
            if (x == null) 
                return y.IsEmpty();
            if (y == null) 
                return x.IsEmpty();

        	var list1 = x as IList;
        	var list2 = y as IList;
			if (list1 != null && list2 != null)
			{
				int n = list1.Count;
				if (list2.Count != n) return false;
				for (int i = 0; i < n; ++i)
				{
					if (!Equals(list1[i], list2[i]))
						return false;
				}
				return true;
			}

        	var ex = x.GetEnumerator();
            var ey = y.GetEnumerator();
            while(true)
            {
                if (ex.MoveNext())
                {
                    if (!ey.MoveNext())
                        return false;
                    if (!Equals(ex.Current, ey.Current))
                        return false;
                }
                else
                {
                    return !ey.MoveNext();
                }
            }
        }

        public static int EvalHashCode(this IEnumerable args)
        {
            int n = 0;
            int h = 0;
            foreach (var obj in args)
            {
                if (obj != null)
                    h ^= obj.GetHashCode();
                ++n;
            }
            return h ^ n;
        }
    }
}