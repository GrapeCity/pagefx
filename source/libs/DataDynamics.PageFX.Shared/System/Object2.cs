using System.Collections;
using System.Collections.Generic;

namespace System
{
    public static class Object2
    {
        public static bool IsEmpty(IEnumerable x)
        {
            if (x == null) return true;
            var e = x.GetEnumerator();
            return !e.MoveNext();
        }

        public static bool Equals<T>(IList<T> x, IList<T> y)
        {
            if (x == null) return IsEmpty(y);
            if (y == null) return IsEmpty(x);
            int n = x.Count;
            if (y.Count != n) return false;
            for (int i = 0; i < n; ++i)
            {
                if (!Equals(x[i], y[i]))
                    return false;
            }
            return true;
        }

        public static bool Equals(IEnumerable x, IEnumerable y)
        {
            if (x == null) 
                return IsEmpty(y);
            if (y == null) 
                return IsEmpty(x);
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

        public static bool EqualsPairwise(params object[] args)
        {
            int n = args.Length;
            for (int i = 0; i < n; i += 2)
            {
                if (!Equals(args[i], args[i + 1]))
                    return false;
            }
            return true;
        }

        public static int GetHashCode(params object[] args)
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

        public static int GetHashCode(IEnumerable args)
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