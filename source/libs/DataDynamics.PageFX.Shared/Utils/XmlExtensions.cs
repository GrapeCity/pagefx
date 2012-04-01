using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace DataDynamics
{
    public static class XmlExtensions
    {
    	public static XmlWriterSettings DefaultIndentedSettings
        {
            get
            {
                return new XmlWriterSettings
                {
                    Indent = true,
                    IndentChars = "  ",
                    Encoding = Encoding.UTF8
                };
            }
        }

        public static bool IsInvalidChar(char c)
        {
            //TODO:
            return c == 0x1B || c == 0x0C;
        }

        public static string EntifyString(string s)
        {
            if (string.IsNullOrEmpty(s)) return s;
            var sb = new StringBuilder();
            foreach (var c in s)
            {
                if (c == '<') sb.Append("&lt;");
                else if (c == '>') sb.Append("&gt;");
                else if (c == '&') sb.Append("&amp;");
                else if (c == '\'') sb.Append("&apos;");
                else if (c == '\"') sb.Append("&quot;");
                else if (IsInvalidChar(c))
                {
                    sb.AppendFormat("&#x{0:X};", (int)c);
                }
                else
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        public static string UnentifyString(string s)
        {
            if (string.IsNullOrEmpty(s)) return s;
            var sb = new StringBuilder();
            int n = s.Length;
            for (int i = 0; i < n; ++i)
            {
                char c = s[i];
                if (c == '&')
                {
                    string ent = "";
                    for (++i; i < n; ++i)
                    {
                        if (s[i] == ';')
                            break;
                        ent += s[i];
                    }
                    if (ent == "apos") sb.Append('\'');
                    else if (ent == "quot") sb.Append('\"');
                    else if (ent == "lt") sb.Append('<');
                    else if (ent == "gt") sb.Append('>');
                    else if (ent == "amp") sb.Append('&');
                    else if (ent.StartsWith("#x"))
                    {
                        c = (char)int.Parse(ent.Substring(2), NumberStyles.HexNumber, CultureInfo.InvariantCulture);
                        sb.Append(c);
                    }
                    else
                    {
                        //can not resolve entity ref
                        sb.Append("&" + ent + ";");
                    }
                }
                else
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        private static void WriteBeginTag(this TextWriter writer, string tag, params string[] attrs)
        {
            writer.Write("<{0}", tag);
            if (attrs != null)
            {
                int n = attrs.Length;
                for (int i = 0; i + 1 < n; i += 2)
                {
                    writer.Write(" {0}=\"{1}\"", attrs[i], EntifyString(attrs[i + 1]));
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

            var lines = Str.GetLines(text, false, true);
            if (lines == null) return;
            int n = lines.Length;
            if (n == 0) return;

            for (int i = 0; i < n; ++i)
                lines[i] = EntifyString(lines[i]);

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

        private static IEnumerable<XmlNode> GetChildNodes(this XmlNode parent)
        {
        	return parent.ChildNodes.Cast<XmlNode>();
        }

    	public static IEnumerable<XmlNode> GetDescendantNodes(this XmlNode parent)
    	{
    		return Algorithms.IterateTreeTopDown(parent, GetChildNodes).Where(node => node != parent);
    	}

    	public static IEnumerable<XmlElement> GetDescendantElements(this XmlNode parent)
    	{
    		return parent.GetDescendantNodes().OfType<XmlElement>();
    	}

    	public static void ProcessNodes(this XmlNode parent, Action<XmlNode> action)
        {
            foreach (var node in parent.GetDescendantNodes())
                action(node);
        }

        public static void ProcessElements(this XmlNode parent, Action<XmlElement> action)
        {
            foreach (var e in parent.GetDescendantElements())
                action(e);
        }
    }
}