using System.IO;

namespace DataDynamics
{
    public static class XmlExtensions
    {
    	private static void WriteBeginTag(this TextWriter writer, string tag, params string[] attrs)
        {
            writer.Write("<{0}", tag);
            if (attrs != null)
            {
                int n = attrs.Length;
                for (int i = 0; i + 1 < n; i += 2)
                {
                    writer.Write(" {0}=\"{1}\"", attrs[i], attrs[i + 1].ToXmlString());
                }
            }
            writer.Write(">");
        }

        private static void WriteEndTag(this TextWriter writer, string tag)
        {
            writer.Write("</{0}>", tag);
        }

        public static void WriteXmlComments(this TextWriter writer, string text, string tab, string tag, params string[] attrs)
        {
            if (string.IsNullOrEmpty(text)) return;

            var lines = text.ReadLines(false, true);
            if (lines == null) return;
            int n = lines.Length;
            if (n == 0) return;

            for (int i = 0; i < n; ++i)
                lines[i] = lines[i].ToXmlString();

            if (tab == null)
                tab = string.Empty;

            writer.Write("{0}/// ", tab);
            writer.WriteBeginTag(tag, attrs);
            if (n > 1)
            {
                writer.WriteLine();
                foreach (var line in lines)
                {
                    writer.WriteLine("{0}/// {1}", tab, line);
                }
                writer.Write("{0}/// ", tab);
                writer.WriteEndTag(tag);
                writer.WriteLine();
            }
            else
            {
                writer.Write(lines[0]);
                writer.WriteEndTag(tag);
                writer.WriteLine();
            }
        }
    }
}