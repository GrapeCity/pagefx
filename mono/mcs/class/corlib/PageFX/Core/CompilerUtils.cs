using System.Collections;
using Native;

namespace PageFX
{
    class CompilerUtils
    {
        static object SafeUnbox(object o)
        {
            return o != null ? o.GetType().Unbox(o) : o;
        }

        public static NativeArray ToArray(IEnumerable collection)
        {
			NativeArray arr = new NativeArray();
            var dic = collection as IDictionary;
            if (dic != null)
            {
                foreach (DictionaryEntry e in dic)
                {
                    object key = SafeUnbox(e.Key);
                    object val = SafeUnbox(e.Value);
                    arr.push(new DictionaryEntry(key, val));
                }
            }
            else
            {
                foreach (var o in collection)
                {
                    arr.push(SafeUnbox(o));
                }
            }
            return arr;
        }
    }
}