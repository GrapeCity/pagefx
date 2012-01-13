using System;
using System.Collections;
using System.Collections.Generic;

namespace DataDynamics
{
    public static class Logic
    {
        public static bool Implies(bool a, bool b)
        {
            return a ? b : true;
        }

        public static bool Bijection(bool a, bool b)
        {
            return Implies(a, b) ? Implies(b, a) : false;
        }

        private static readonly object Color = new object();

        public static List<T> Union<T>(IEnumerable<T> a, IEnumerable<T> b)
        {
            var list = new List<T>();
            var inList = new Hashtable();
            foreach (var item in a)
            {
                list.Add(item);
                inList[item] = Color;
            }
            foreach (var item in b)
            {
                if (inList[item] == null)
                {
                    list.Add(item);
                }
            }
            return list;
        }

        public static List<T> Cross<T>(IEnumerable<T> a, IEnumerable<T> b)
        {
            var h = new Hashtable();
            foreach (var item in a)
            {
                h[item] = Color;
            }
            var list = new List<T>();
            foreach (var item in b)
            {   
                if (h[item] != null)
                {
                    list.Add(item);
                }
            }
            return list;
        }

        public static IEnumerable<T> Filter<T>(IEnumerable<T> list, Predicate<T> filter)
        {
            foreach (var item in list)
            {
                if (filter(item))
                    yield return item;
            }
        }

        public static bool ForAll<T>(IEnumerable<T> list, Predicate<T> p)
        {
            foreach (var item in list)
            {
                if (!p(item))
                    return false;
            }
            return true;
        }

        public static bool ForAny<T>(IEnumerable<T> list, Predicate<T> p)
        {
            foreach (var item in list)
            {
                if (p(item))
                    return true;
            }
            return false;
        }

        public static bool Exists<T>(IEnumerable<T> list, Predicate<T> p)
        {
            return ForAny(list, p);
        }

        public static int CountOf<T>(IEnumerable<T> list, Predicate<T> p)
        {
            int n = 0;
            foreach (var item in list)
            {
                if (p(item))
                    ++n;
            }
            return n;
        }

        public static bool ExistsOnlyOne<T>(IEnumerable<T> list, Predicate<T> p)
        {
            int n = 0;
            foreach (var item in list)
            {
                if (p(item))
                {
                    if (n == 1) 
                        return false;
                    ++n;
                }
            }
            return true;
        }
    }
}