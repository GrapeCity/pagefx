using System;
using System.Collections;
using System.Collections.Generic;
using DataDynamics.Collections;

namespace DataDynamics
{
    public delegate int ComparativePredicate<T>(T obj);

    public static class Algorithms
    {
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

        public static T[] ToArray<T>(this IList<T> list)
        {
            int n = list.Count;
            var result = new T[n];
            list.CopyTo(result, 0);
            return result;
        }

        #region IndexOf

        /// <summary>
        /// Finds index of item in given set using specified predicate to find item.
        /// </summary>
        /// <typeparam name="T">type of item in set</typeparam>
        /// <param name="set">set in which you want to find item</param>
        /// <param name="p">predicate to find item</param>
        /// <returns></returns>
        public static int IndexOf<T>(this IEnumerable<T> set, Predicate<T> p)
        {
            if (set == null) return -1;
            int i = 0;
            foreach (var item in set)
            {
                if (p(item))
                    return i;
                ++i;
            }
            return -1;
        }

    	public static int IndexOf<T>(IEnumerable set, Predicate<T> p)
        {
            if (set == null) return -1;
            int i = 0;
            foreach (T item in set)
            {
                if (p(item))
                    return i;
                ++i;
            }
            return -1;
        }

        public static int IndexOf<T>(IEnumerable<T> set, T item)
        {
            return IndexOf(set, i => Equals(i, item));
        }

    	#endregion

        #region Contains
        public static bool Contains<T>(IEnumerable<T> set, Predicate<T> p)
        {
            return IndexOf(set, p) >= 0;
        }

    	public static bool Contains<T>(IEnumerable set, Predicate<T> p)
        {
            return IndexOf(set, p) >= 0;
        }

        public static bool Contains<T>(IEnumerable<T> set, T item)
        {
            return IndexOf(set, item) >= 0;
        }
        #endregion

    	public static void Split<T>(IEnumerable set, out List<T> tlist, out List<T> flist, Predicate<T> p)
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

    	public static IEnumerable<B> Convert<T, B>(IEnumerable<T> original) where T : B
        {
            return new BaseTypeEnumerable<T, B>(original);
        }

        public static IEnumerator<B> Convert<T, B>(IEnumerator<T> original) where T : B
        {
            return new BaseTypeEnumerator<T, B>(original);
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