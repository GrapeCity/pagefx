using System;
using System.Xml;

namespace DataDynamics.PageFX.CodeModel
{
    public class XmlElementNameAttribute : Attribute
    {
        public XmlElementNameAttribute(string name)
        {
            _name = name;
        }

        public string Name
        {
            get { return _name; }
        }
        private readonly string _name;
    }

    public interface IXmlSerializationFeedback
    {
        string XmlElementName { get; }

        void WriteProperties(XmlWriter writer);
    }

    internal class XmlSerializationService
    {
        public static void Serialize(XmlWriter writer, ICodeNode node)
        {
            var feedback = node as IXmlSerializationFeedback;
            string name = null;
            if (feedback != null)
                name = feedback.XmlElementName;

            if (string.IsNullOrEmpty(name))
            {
                var type = node.GetType();
                var attr = type.GetAttribute<XmlElementNameAttribute>(false);
                name = attr != null ? attr.Name : type.Name;
            }

            writer.WriteStartElement(name);

            if (feedback != null)
                feedback.WriteProperties(writer);

            var kids = node.ChildNodes;
            if (kids != null)
            {
                foreach (var kid in kids)
                {
                    if (kid != null)
                    {
                        Serialize(writer, kid);
                    }
                }
            }
            writer.WriteEndElement();
        }
    }
}