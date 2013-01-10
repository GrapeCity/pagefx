using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.Common.Collections;

namespace DataDynamics.PageFX.Common.Extensions
{
    public static class EnumerableExtensions
	{
		#region AsContinuous

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

		#endregion

		public static IEnumerable<Pair<T>> Pairwise<T>(this IEnumerable<T> seq)
		{
			var p = new T[2];
			var i = 0;
			foreach (var item in seq)
			{
				p[i++] = item;
				if (i != 2) continue;

				yield return new Pair<T>(p[0], p[1]);

				i = 0;
				p = new T[2];
			}
		}

		public static IEnumerable<Pair<T>> Pairwise<T>(this IEnumerable<T> seq, Func<T,int> indexSelector)
		{
			var p = new T[2];
			int prev = -1;
			bool empty = true;
			foreach (var item in seq)
			{
				var i = indexSelector(item);
				if (i == 0 && prev == 0)
				{
					yield return new Pair<T>(p[0], p[1]);
					p = new T[2];
				}

				p[i] = item;
				empty = false;

				if (i == 1)
				{
					yield return new Pair<T>(p[0], p[1]);
					p = new T[2];
					empty = true;
				}

				prev = i;
			}

			if (!empty)
			{
				yield return new Pair<T>(p[0], p[1]);
			}
		}

		public static IEnumerable<T> Append<T>(this IEnumerable<T> seq, params T[] items)
		{
			return seq.Concat(items);
		}

		public static int IndexOf<T>(this IEnumerable<T> seq, Func<T,bool> predicate)
		{
			int i = 0;
			foreach (var item in seq)
			{
				if (predicate(item))
					return i;
				i++;
			}
			return -1;
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

			var c1 = x as ICollection;
			var c2 = y as ICollection;
			if (c1 != null && c2 != null && c1.Count != c2.Count)
				return false;

        	var it1 = x.GetEnumerator();
            var it2 = y.GetEnumerator();
            while (true)
            {
	            var f1 = it1.MoveNext();
	            var f2 = it2.MoveNext();
                if (f1 && f2)
                {
                    if (!Equals(it1.Current, it2.Current))
                        return false;
                }
                else
                {
                    return f1 == f2;
                }
            }
        }

		public static bool EqualsTo<T>(this IEnumerable<T> x, IEnumerable<T> y)
		{
			return EqualsTo(x, y, null);
		}

		public static bool EqualsTo<T>(this IEnumerable<T> x, IEnumerable<T> y, Func<T,T,bool> equals)
		{
			if (x == null)
				return y.IsEmpty();
			if (y == null)
				return x.IsEmpty();

			var c1 = x as ICollection;
			var c2 = y as ICollection;
			if (c1 != null && c2 != null && c1.Count != c2.Count)
				return false;

			var l1 = x as IReadOnlyList<T>;
			var l2 = y as IReadOnlyList<T>;
			if (l1 != null && l2 != null && l1.Count != l2.Count)
				return false;

			if (equals == null)
				equals = (a, b) => Equals(a, b);

			var it1 = x.GetEnumerator();
			var it2 = y.GetEnumerator();
			while (true)
			{
				var f1 = it1.MoveNext();
				var f2 = it2.MoveNext();
				if (f1 && f2)
				{
					if (!equals(it1.Current, it2.Current))
						return false;
				}
				else
				{
					return f1 == f2;
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