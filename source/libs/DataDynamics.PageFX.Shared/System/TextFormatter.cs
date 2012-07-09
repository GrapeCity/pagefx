using System.Collections.Generic;
using System.IO;

namespace System
{
	public static class TextFormatter
    {
        public static string ToString<T>(IEnumerable<T> list)
        {
            return ToString(list, "", "", ", ");
        }

        public static string ToString<T>(IEnumerable<T> list, string prefix, string suffix, string separator)
        {
            using (var writer = new StringWriter())
            {
                WriteList(writer, list, null, prefix, suffix, separator);
                return writer.ToString();
            }
        }

        public static void WriteList<T>(TextWriter writer, IEnumerable<T> list, Func<T,string> stringer, string prefix, string suffix, string separator)
        {
            if (list != null)
            {
                bool first = true;
                bool sep = false;
                foreach (var item in list)
                {
                    if (first)
                    {
                        writer.Write(prefix);
                        first = false;
                    }
                    if (sep) writer.Write(separator);
                	writer.Write(stringer != null ? stringer(item) : item.ToString());
                	sep = true;
                }
                if (!first) writer.Write(suffix);
            }
        }

        public static void WriteList<T>(TextWriter writer, IEnumerable<T> list, string prefix, string suffix, string separator)
        {
            WriteList(writer, list, null, prefix, suffix, separator);
        }

        public static void WriteList<T>(TextWriter writer, IEnumerable<T> list, string separator)
        {
            WriteList(writer, list, null, "", "", separator);
        }
    }
}