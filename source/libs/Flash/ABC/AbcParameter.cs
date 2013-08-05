using System.Text;
using System.Xml;

namespace DataDynamics.PageFX.FlashLand.Abc
{
    /// <summary>
    /// Represents method paramater.
    /// </summary>
    public sealed class AbcParameter : ISupportXmlDump
    {
	    public AbcParameter()
        {
        }

        public AbcParameter(AbcMultiname type)
        {
            Type = type;
        }

        public AbcParameter(AbcMultiname type, AbcConst<string> name)
        {
            Type = type;
            Name = name;
        }

	    public AbcConst<string> Name { get; set; }

	    public bool HasName
        {
            get
            {
                //if (_name == null) return false;
                //return !string.IsNullOrEmpty(_name.Value);
                return Name != null;
            }
        }

	    public AbcMultiname Type { get; set; }

	    public object Value { get; set; }

	    public bool IsOptional { get; set; }

	    public void DumpXml(XmlWriter writer)
        {
            writer.WriteStartElement("param");
            if (Name != null)
                writer.WriteAttributeString("name", Name.Value);

            if (Type != null)
            {
                writer.WriteAttributeString("type", Type.ToString());
            }

            if (IsOptional)
            {
                writer.WriteAttributeString("value", Value != null ? Value.ToString() : "null");
            }

            if (Type != null)
            {
                Type.DumpXml(writer, "type");
            }

            writer.WriteEndElement();
        }

	    public override string ToString()
        {
            return ToString("s");
        }

        public string ToString(string typeFormat)
        {
            var s = new StringBuilder();
            if (Name != null && !string.IsNullOrEmpty(Name.Value))
            {
                s.Append(Name.Value);
                s.Append(":");
            }
            s.Append(Type != null ? Type.ToString(typeFormat) : "*");
            if (IsOptional)
            {
                s.Append(" = ");
                s.Append(Value != null ? Value.ToString() : "null");
            }
            return s.ToString();
        }
    }
}