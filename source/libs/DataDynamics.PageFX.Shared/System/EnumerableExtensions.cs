using System.Collections;

namespace System
{
    public static class EnumerableExtensions
    {
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