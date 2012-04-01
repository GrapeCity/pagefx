using System.Collections.Generic;
using System.Xml;

namespace DataDynamics.Tools
{
    internal class Utils
    {
        public static string FixTypeName(string name)
        {
            return name.Replace(':', '.');
        }

        public static string GetDesc(XmlElement e)
        {
            var x = e["shortDescription"];
            if (x != null)
                return x.InnerText;

            x = e["description"];
            if (x != null)
                return x.InnerText;
            return "";
        }

        public static IEnumerable<XmlElement> GetElems(XmlNode parent, bool recurse)
        {
            foreach (XmlNode childNode in parent.ChildNodes)
            {
                var e = childNode as XmlElement;
                if (e != null)
                {
                    yield return e;
                    if (recurse)
                    {
                        foreach (var element in GetElems(e, true))
                            yield return element;
                    }
                }
            }
        }

        public static void WriteSummary(XmlWriter writer, string tag, string summary)
        {
            if (string.IsNullOrEmpty(summary)) return;

            if (!string.IsNullOrEmpty(tag))
                writer.WriteStartElement(tag);

            var lines = summary.ReadLines(false, true);
            if (lines != null && lines.Length > 0)
            {
                var s = string.Join(" ", lines);
                writer.WriteRaw(s);
            }

            //foreach (var line in lines)
            //    writer.WriteRaw(line);

            if (!string.IsNullOrEmpty(tag))
                writer.WriteEndElement();
        }

        public static string ConvertType(string type)
        {
            switch (type)
            {
                case "*":
                case "Object":
                    return "System.Object";

                case "Class":
                case "ArgumentError":
                case "Array":
                case "Date":
                case "DefinitionError":
                case "Error":
                case "EvalError":
                case "Function":
                case "Math":
                case "Namespace":
                case "QName":
                case "RangeError":
                case "ReferenceError":
                case "RegExp":
                case "SecurityError":
                case "String":
                case "SyntaxError":
                case "TypeError":
                case "UninitializedError":
                case "URIError":
                case "VerifyError":
                case "XML":
                case "XMLList":
                    return "Avm." + type;

                case "Boolean":
                    return "System.Boolean";
                case "int":
                    return "System.Int32";
                case "uint":
                    return "System.UInt32";
                case "Number":
                    return "System.Double";

                case "restParam":
                    return "System.Object";
            }
            return type;
        }
    }
}