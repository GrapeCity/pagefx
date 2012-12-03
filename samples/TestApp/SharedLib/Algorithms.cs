using System;
using System.Collections.Generic;

namespace SharedLib
{
    public static class Algorithms
    {
        public static int IndexOf<T>(IEnumerable<T> set, Predicate<T> p)
        {
            if (set != null)
            {
                int i = 0;
                foreach (var item in set)
                {
                    if (p(item))
                        return i;
                    ++i;
                }
            }
            return -1;
        }

        public static bool Contains<T>(IEnumerable<T> set, Predicate<T> p)
        {
            return IndexOf(set, p) >= 0;
        }

        public static T Find<T>(IEnumerable<T> set, Predicate<T> p)
        {
            if (set != null)
            {
                foreach (var item in set)
                {
                    if (p(item))
                        return item;
                }
            }
            return default(T);
        }
    }
}
