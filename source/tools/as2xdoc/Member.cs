using System.Xml;

namespace DataDynamics.Tools
{
    class Member
    {
        public string Name;
        public string TypeName;
        public string Summary;
        public string Type;

        public virtual void Write(XmlWriter writer)
        {
            writer.WriteStartElement(XmlElementName);
            writer.WriteAttributeString("name", Name + NameSuffix);
            WriteAttrs(writer);

            if (WrapSummary)
            {
                Utils.WriteSummary(writer, "summary", Summary);
            }
            else
            {
                Utils.WriteSummary(writer, null, Summary);
            }

            WriteBody(writer);

            writer.WriteEndElement();
        }

        protected virtual string XmlElementName
        {
            get { return "member"; }
        }

        protected virtual string NameSuffix
        {
            get { return ""; }
        }

        protected virtual bool WrapSummary
        {
            get { return true; }
        }

        protected virtual void WriteAttrs(XmlWriter writer)
        {
        }

        protected virtual void WriteBody(XmlWriter writer)
        {
        }

        
    }
}