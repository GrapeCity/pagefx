using System.Collections.Generic;

namespace System.IO
{
	public static class TextWriterExtensions
    {
		public static void WriteList<T>(this TextWriter writer, IEnumerable<T> list, Func<T,string> stringer, string prefix, string suffix, string separator)
        {
	        if (list == null) return;

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

		public static void WriteList<T>(this TextWriter writer, IEnumerable<T> list, string separator)
        {
            writer.WriteList(list, null, "", "", separator);
        }
    }
}