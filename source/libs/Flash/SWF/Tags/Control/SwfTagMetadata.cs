using System.Xml;

namespace DataDynamics.PageFX.FlashLand.Swf.Tags.Control
{
    [SwfTag(SwfTagCode.Metadata)]
    public class SwfTagMetadata : SwfTag
    {
        public SwfTagMetadata()
        {    
        }

        public SwfTagMetadata(string xml)
        {
            XmlContent = xml;
        }

	    public string XmlContent { get; set; }

	    public XmlDocument XmlDocument { get; set; }

	    public override SwfTagCode TagCode
        {
            get { return SwfTagCode.Metadata; }
        }

        public override void ReadTagData(SwfReader reader)
        {
            XmlContent = reader.ReadString();
            try
            {
                XmlDocument = new XmlDocument();
                XmlDocument.LoadXml(XmlContent);
            }
            catch
            {
                XmlDocument = null;
            }
        }

        public override void WriteTagData(SwfWriter writer)
        {
            if (XmlContent != null)
            {
                writer.WriteString(XmlContent);
            }
            else if (XmlDocument != null)
            {
                writer.WriteString(XmlDocument.OuterXml);
            }
        }

        public override void DumpBody(XmlWriter writer)
        {
            if (XmlDocument != null)
                XmlDocument.WriteTo(writer);
            else if (XmlContent != null)
                writer.WriteCData(XmlContent);
        }
    }
}