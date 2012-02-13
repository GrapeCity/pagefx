using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DataDynamics.Collections;

namespace DataDynamics
{
    public delegate int ComparativePredicate<T>(T obj);

    public static class Algorithms
    {
    	#region GetHashCode
        public static int GetHashCode(IEnumerable set)
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

        public static int GetHashCode(params object[] args)
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

        #region Equals
        public static bool Equals<T>(T[] x, T[] y)
        {
            if (x != null)
            {
                if (y == null) return false;
                int n = x.Length;
                if (n != y.Length) return false;
                for (int i = 0; i < n; ++i)
                    if (!Equals(x[i], y[i]))
                        return false;
                return true;
            }
            return y == null;
        }

        public static bool Equals<T>(List<T> x, List<T> y)
        {
            if (x != null)
            {
                if (y == null) return false;
                int n = x.Count;
                if (n != y.Count) return false;
                for (int i = 0; i < n; ++i)
                    if (!Equals(x[i], y[i]))
                        return false;
                return true;
            }
            return y == null;
        }
        #endregion

        public static bool IsEmpty<T>(this IEnumerable<T> set)
        {
			return !set.GetEnumerator().MoveNext();
        }

        #region Generic MaxMin
        public static T Max<T>(IEnumerable<T> set, IComparer<T> comparer)
        {
            if (comparer == null)
                comparer = Comparer<T>.Default;

            var result = default(T);
            bool first = true;
            foreach (var t in set)
            {
                if (first)
                {
                    result = t;
                    first = false;
                }
                else
                {
                    int n = comparer.Compare(t, result);
                    if (n > 0)
                        result = t;
                }
            }
            return result;
        }

        public static T Max<T>(IEnumerable<T> set, Comparison<T> comparison)
        {
            var result = default(T);
            bool first = true;
            foreach (var t in set)
            {
                if (first)
                {
                    result = t;
                    first = false;
                }
                else
                {
                    int n = comparison(t, result);
                    if (n > 0)
                        result = t;
                }
            }
            return result;
        }

        public static T Max<T>(IEnumerable<T> set)
        {
            return Max(set, Comparer<T>.Default);
        }

        public static T Min<T>(IEnumerable<T> set, IComparer<T> comparer)
        {
            if (comparer == null)
                comparer = Comparer<T>.Default;

            var result = default(T);
            bool first = true;
            foreach (var t in set)
            {
                if (first)
                {
                    result = t;
                    first = false;
                }
                else
                {
                    int n = comparer.Compare(t, result);
                    if (n < 0)
                        result = t;
                }
            }
            return result;
        }

        public static T Min<T>(IEnumerable<T> set, Comparison<T> comparison)
        {
            var result = default(T);
            bool first = true;
            foreach (var t in set)
            {
                if (first)
                {
                    result = t;
                    first = false;
                }
                else
                {
                    int n = comparison(t, result);
                    if (n < 0)
                        result = t;
                }
            }
            return result;
        }

        public static T Min<T>(IEnumerable<T> set)
        {
            return Min(set, Comparer<T>.Default);
        }

        /// <summary>
        /// This procedure computes minimum min and maximum max
        /// using only (3n/2) - 2 comparisons.
        /// ----------------------------------------------------
        /// ref: Aho A.V., Hopcroft J.E., Ullman J.D., 
        /// The design and analysis of computer algorithms, 
        /// Addison-Wesley, Reading, 1974.
        /// ----------------------------------------------------
        /// </summary>
        public static void MaxMin<T>(T[] list, IComparer<T> comparer, out T max, out T min)
        {
            int n = list.Length;
            int i1 = 0, i2 = 0, i, j = 0, k1, k2;
            T x, y;

            min = list[0];
            max = list[0];
            if ((n & 1) != 0) j = 1;
            for (i = j; i < n; i += 2)
            {
                k1 = i; k2 = i + 1;
                x = list[k1];
                y = list[k2];
                if (comparer.Compare(x, y) > 0)
                {
                    k1 = k2; k2 = i;
                    x = y; y = list[k2];
                }
                if (comparer.Compare(x, min) < 0)
                {
                    min = x;
                    i1 = k1;
                }
                if (comparer.Compare(y, max) > 0)
                {
                    max = y;
                    i2 = k2;
                }
            }
        }

        public static void MaxMin<T>(T[] list, Comparison<T> comparison, out T max, out T min)
        {
            MaxMin(list, new FunctorComparer<T>(comparison), out max, out min);
        }

        public static void MaxMin<T>(T[] list, out T max, out T min) where T : IComparable
        {
            MaxMin(list, Comparer<T>.Default, out max, out min);
        }

        /// <summary>
        /// This procedure computes minimum min and maximum max
        /// using only (3n/2) - 2 comparisons.
        /// ----------------------------------------------------
        /// ref: Aho A.V., Hopcroft J.E., Ullman J.D., 
        /// The design and analysis of computer algorithms, 
        /// Addison-Wesley, Reading, 1974.
        /// ----------------------------------------------------
        /// </summary>
        public static void MaxMin<T>(List<T> list, IComparer<T> comparer, out T max, out T min)
        {
            int n = list.Count;
            int i1 = 0, i2 = 0, i, j = 0, k1, k2;
            T x, y;

            min = list[0];
            max = list[0];
            if ((n & 1) != 0) j = 1;
            for (i = j; i < n; i += 2)
            {
                k1 = i; k2 = i + 1;
                x = list[k1];
                y = list[k2];
                if (comparer.Compare(x, y) > 0)
                {
                    k1 = k2; k2 = i;
                    x = y; y = list[k2];
                }
                if (comparer.Compare(x, min) < 0)
                {
                    min = x;
                    i1 = k1;
                }
                if (comparer.Compare(y, max) > 0)
                {
                    max = y;
                    i2 = k2;
                }
            }
        }

        public static void MaxMin<T>(List<T> list, Comparison<T> comparison, out T max, out T min)
        {
            MaxMin(list, new FunctorComparer<T>(comparison), out max, out min);
        }

        public static void MaxMin<T>(List<T> list, out T max, out T min) where T : IComparable
        {
            MaxMin(list, Comparer<T>.Default, out max, out min);
        }
        #endregion

        #region BinarySearch
        public static int BinarySearch<T>(ISimpleList<T> list, int index, int length, ComparativePredicate<T> p)
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
            return BinarySearch(SimpleList.FromArray(array), index, length, p);
        }

        public static int BinarySearch<T>(IList<T> list, int index, int length, ComparativePredicate<T> p)
        {
            return BinarySearch(SimpleList.FromList(list), index, length, p);
        }
        #endregion

    	public static void Split<T>(this IEnumerable set, out List<T> tlist, out List<T> flist, Predicate<T> p)
        {
            tlist = new List<T>();
            flist = new List<T>();
            foreach (T item in set)
            {
                if (p(item))
                    tlist.Add(item);
                else
                    flist.Add(item);
            }
        }

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
        private static readonly Random random = new Random();

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
                int i = random.Next(0, 1);
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
                    int k = random.Next(0, list.Count - 1);
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