using System.Globalization;
using System.IO;
using System.Xml;

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

        public static int GetInt32(this XmlElement element, string attr, int defval)
        {
            string s = element.GetAttribute(attr);
            if (string.IsNullOrEmpty(s))
                return defval;
            int v;
            if (int.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, out v))
                return v;
            return defval;
        }

        public static double GetDouble(this XmlElement element, string attr, int defval)
        {
            string s = element.GetAttribute(attr);
            if (string.IsNullOrEmpty(s))
                return defval;
            return XmlConvert.ToDouble(s);
        }

        public static bool GetBool(this XmlElement element, string attr, bool defval)
        {
            string s = element.GetAttribute(attr);
            if (string.IsNullOrEmpty(s))
                return false;
            if (string.Compare(s, "true", true) == 0)
                return true;
            if (string.Compare(s, "false", true) == 0)
                return false;
            int v;
            if (int.TryParse(s, out v))
                return v != 0;
            return false;
        }
    }
}