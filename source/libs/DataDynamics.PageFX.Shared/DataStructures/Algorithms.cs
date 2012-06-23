using System;
using System.Collections;
using System.Collections.Generic;
using DataDynamics.Collections;

namespace DataDynamics
{
    public delegate int ComparativePredicate<T>(T obj);

    public static class Algorithms
    {
    	#region GetHashCode
        public static int EvalHashCode(this IEnumerable set)
        {
            if (set == null) return 0;
            int h = 0;
            int n = 0;
            foreach (var o in set)
            {
                if (o != null)
                    h ^= o.GetHashCode();
                ++n;
            }
            return h ^ n;
        }

        public static int EvalHashCode(params object[] args)
        {
            if (args == null) return 0;
            int h = 0;
            int n = 0;
            foreach (var o in args)
            {
                if (o != null)
                    h ^= o.GetHashCode();
                ++n;
            }
            return h ^ n;
        }
        #endregion

        #region EqualsTo
        public static bool EqualsTo<T>(this T[] x, T[] y)
        {
        	if (x == null)
        	{
        		return y == null;
        	}
        	if (y == null)
        	{
        		return false;
        	}

        	int n = x.Length;
        	if (n != y.Length)
        	{
        		return false;
        	}

			for (int i = 0; i < n; ++i)
			{
				if (!Equals(x[i], y[i]))
				{
					return false;
				}
			}

        	return true;
        }

        public static bool EqualsTo<T>(this IList<T> x, IList<T> y)
        {
        	if (x == null)
        	{
        		return y == null;
        	}
        	if (y == null)
        	{
        		return false;
        	}

        	int n = x.Count;
        	if (n != y.Count)
        	{
        		return false;
        	}

			for (int i = 0; i < n; ++i)
			{
				if (!Equals(x[i], y[i]))
				{
					return false;
				}
			}

        	return true;
        }
        #endregion

    	#region BinarySearch
        public static int BinarySearch<T>(IReadOnlyList<T> list, int index, int length, ComparativePredicate<T> p)
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

        public static int BinarySearch<T>(T[] array, int index, int length, ComparativePredicate<T> p)
        {
            return BinarySearch(array.AsReadOnlyList(), index, length, p);
        }

        public static int BinarySearch<T>(IList<T> list, int index, int length, ComparativePredicate<T> p)
        {
            return BinarySearch(list.AsReadOnlyList(), index, length, p);
        }
        #endregion

    	#region Tree Iterators
        public static IEnumerable<T> IterateTreeTopDown<T>(IEnumerable<T> set, Converter<T, IEnumerable<T>> getKids)
        {
            foreach (var item in set)
            {
                yield return item;
                var kids = getKids(item);
                if (kids != null)
                {
                    foreach (var kid in IterateTreeTopDown(kids, getKids))
                        yield return kid;
                }
            }
        }

        public static IEnumerable<T> IterateTreeTopDown<T>(T root, Converter<T, IEnumerable<T>> getKids)
        {
            yield return root;
            var kids = getKids(root);
            if (kids != null)
            {
                foreach (var kid in IterateTreeTopDown(kids, getKids))
                    yield return kid;
            }
        }

        public static IEnumerable<T> IterateTreeBottomUp<T>(IEnumerable<T> set, Converter<T, IEnumerable<T>> getKids)
        {
            foreach (var item in set)
            {
                var kids = getKids(item);
                if (kids != null)
                {
                    foreach (var kid in IterateTreeBottomUp(kids, getKids))
                        yield return kid;
                }
                yield return item;
            }
        }

        public static IEnumerable<T> IterateTreeBottomUp<T>(T root, Converter<T, IEnumerable<T>> getKids)
        {
            var kids = getKids(root);
            if (kids != null)
            {
                foreach (var kid in IterateTreeBottomUp(kids, getKids))
                    yield return kid;
            }
            yield return root;
        }
        #endregion

        #region Shuffle
        private static readonly Random Random = new Random();

        public static int[] Shuffle(int n)
        {
            if (n < 0)
                throw new ArgumentOutOfRangeException("n", "n < 0");
            if (n == 0)
                return new int[0];
            if (n == 1)
                return new[] {0};
            if (n == 2)
            {
                int i = Random.Next(0, 1);
                return new[] {i, 1 - i};
            }

            var list = new List<int>(n);
            for (int i = 0; i < n; ++i)
                list.Add(i);

            var arr = new int[n];
            for (int i = 0; i < n; ++i)
            {
                if (list.Count > 1)
                {
                    int k = Random.Next(0, list.Count - 1);
                    arr[i] = list[k];
                    list.RemoveAt(k);
                }
                else
                {
                    arr[i] = list[0];
                }
            }
            return arr;
        }

        public static void Shuffle<T>(IList<T> list)
        {
            var copy = new List<T>(list);
            int n = list.Count;
            var order = Shuffle(n);
            for (int i = 0; i < n; ++i)
                list[i] = copy[order[i]];
        }
        #endregion
    }
}