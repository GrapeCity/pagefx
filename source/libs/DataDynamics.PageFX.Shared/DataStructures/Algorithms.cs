using System;
using System.Collections;
using System.Collections.Generic;
using DataDynamics.Collections;

namespace DataDynamics
{
    public delegate int ComparativePredicate<T>(T obj);

    public static class Algorithms
    {
        public static void ForEach<T>(IEnumerable<T> set, Action<T> action)
        {
            foreach (var item in set)
                action(item);
        }

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

        public static bool IsEmpty<T>(IEnumerable<T> set)
        {
            return !set.GetEnumerator().MoveNext();
        }

        public static T First<T>(IEnumerable<T> c)
        {
            var e = c.GetEnumerator();
            if (e.MoveNext())
                return e.Current;
            return default(T);
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

        #region CopyTo
        public static void CopyTo<T>(IEnumerable<T> collection, T[] array, int index)
        {
            //TODO: check params
            foreach (var value in collection)
            {
                array[index++] = value;
            }
        }

        public static void CopyTo<T>(IEnumerable collection, T[] array, int index)
        {
            //TODO: check params
            foreach (T value in collection)
            {
                array[index++] = value;
            }
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

        public static T[] ToArray<T>(IList<T> list)
        {
            int n = list.Count;
            var result = new T[n];
            list.CopyTo(result, 0);
            return result;
        }

        #region IndexOf, IndexOfAll
        /// <summary>
        /// Finds index of item in given set using specified predicate to find item.
        /// </summary>
        /// <typeparam name="T">type of item in set</typeparam>
        /// <param name="set">set in which you want to find item</param>
        /// <param name="p">predicate to find item</param>
        /// <returns></returns>
        public static int IndexOf<T>(IEnumerable<T> set, Predicate<T> p)
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

        public static int IndexOf(IEnumerable<string> set, string s, bool ignoreCase)
        {
            return IndexOf(set, i => string.Compare(i, s, ignoreCase) == 0);
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

        public static int[] IndexOfAll<T>(IEnumerable set, Predicate<T> p)
        {
            int i = 0;
            var list = new List<int>();
            foreach (T item in set)
            {
                if (p(item))
                    list.Add(i);
                ++i;
            }
            return list.ToArray();
        }

        public static int[] IndexOfAll<T>(IEnumerable<T> set, Predicate<T> p)
        {
            int i = 0;
            var list = new List<int>();
            foreach (var item in set)
            {
                if (p(item))
                    list.Add(i);
                ++i;
            }
            return list.ToArray();
        }

        public static int[] IndexOfAll<T>(IEnumerable<T> set, T item)
        {
            return IndexOfAll(set, i => Equals(i, item));
        }
        #endregion

        #region Contains
        public static bool Contains<T>(IEnumerable<T> set, Predicate<T> p)
        {
            return IndexOf(set, p) >= 0;
        }

        public static bool Contains(IEnumerable<string> set, string s, bool ignoreCase)
        {
            return IndexOf(set, s, ignoreCase) >= 0;
        }

        public static bool ContainsIgnoreCase(IEnumerable<string> set, string s)
        {
            return Contains(set, s, true);
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

        #region Find, FindAll
        public static T Find<T>(IEnumerable<T> set, Predicate<T> p)
        {
            foreach (var item in set)
            {
                if (p(item)) 
                    return item;
            }
            return default(T);
        }

        public static T Find<T>(IEnumerable<T> set, T item)
        {
            return Find(set, i => Equals(i, item));
        }

        public static T Find<T>(IEnumerable set, Predicate<T> p)
        {
            foreach (T item in set)
            {
                if (p(item))
                    return item;
            }
            return default(T);
        }

        public static List<T> FindAll<T>(IEnumerable<T> set, Predicate<T> p)
        {
            var list = new List<T>();
            foreach (var item in set)
            {
                if (p(item))
                    list.Add(item);
            }
            return list;
        }

        public static List<T> FindAll<T>(IEnumerable set, Predicate<T> p)
        {
            var list = new List<T>();
            foreach (T item in set)
            {
                if (p(item))
                    list.Add(item);
            }
            return list;
        }

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
        #endregion

        #region RemoveAll
        public static void RemoveAll<T>(IList<T> list, Predicate<T> p)
        {
            for (int i = 0; i < list.Count; ++i)
            {
                if (p(list[i]))
                {
                    list.RemoveAt(i);
                    --i;
                }
            }
        }

        public static void RemoveAll<T>(IList list, Predicate<T> p)
        {
            for (int i = 0; i < list.Count; ++i)
            {
                if (p((T)list[i]))
                {
                    list.RemoveAt(i);
                    --i;
                }
            }
        }
        #endregion

        /// <summary>
        /// Returns true if one of elemet in given set met given predicate.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="set"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public static bool TrueAny<T>(IEnumerable<T> set, Predicate<T> p)
        {
            foreach (T item in set)
            {
                if (p(item))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Returns true if all elements in given set met given predicate.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="set"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public static bool TrueAll<T>(IEnumerable<T> set, Predicate<T> p)
        {
            foreach (T item in set)
            {
                if (!p(item))
                    return false;
            }
            return true;
        }

        public static bool TrueAny<T>(IEnumerable<Predicate<T>> predicates, T item)
        {
            foreach (var p in predicates)
            {
                if (p(item))
                    return true;
            }
            return false;
        }

        public static bool TrueAll<T>(IEnumerable<Predicate<T>> predicates, T item)
        {
            foreach (var p in predicates)
            {
                if (!p(item))
                    return false;
            }
            return true;
        }

        public static IEnumerable<B> Convert<T, B>(IEnumerable<T> original) where T : B
        {
            return new BaseTypeEnumerable<T, B>(original);
        }

        public static IEnumerator<B> Convert<T, B>(IEnumerator<T> original) where T : B
        {
            return new BaseTypeEnumerator<T, B>(original);
        }

        public static IEnumerable<T> Merge<T>(params IEnumerable<T>[] args)
        {
            return new MergedEnumerator<T>(args);
        }

        public static IEnumerable<T> Filter<T>(IEnumerable<T> set, Predicate<T> p)
        {
            foreach (var item in set)
            {
                if (p(item))
                    yield return item;
            }
        }

        public static T Aggregate<T>(IEnumerable<T> set, Func<T, T, T> f)
        {
            T result = default(T);
            bool first = true;
            var e = set.GetEnumerator();
            while (e.MoveNext())
            {
                if (first)
                {
                    result = e.Current;
                    first = false;
                }
                else
                {
                    result = f(result, e.Current);
                }
            }
            return result;
        }

        public static bool IsUnique<T>(IEnumerable<T> set, Func<bool, T, T> eq)
        {
            int i = 0;
            foreach (var x in set)
            {
                int j = 0;
                foreach (var y in set)
                {
                    if (j == i) continue;
                    if (eq(x, y))
                        return false;
                    ++j;
                }
                ++i;
            }
            return true;
        }
        public static List<TOutput> Convert<TInput, TOutput>(IEnumerable<TInput> set, Converter<TInput, TOutput> converter)
        {
            var list = new List<TOutput>();
            foreach (var item in set)
                list.Add(converter(item));
            return list;
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