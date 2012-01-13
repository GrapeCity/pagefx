using System.Xml;

namespace DataDynamics.PageFX.FLI.SWF
{
    [SwfTag(SwfTagCode.Metadata)]
    public class SwfTagMetadata : SwfTag
    {
        public SwfTagMetadata()
        {    
        }

        public SwfTagMetadata(string xml)
        {
            _xml = xml;
        }

        public string MetadataXml
        {
            get { return _xml; }
            set { _xml = value; }
        }
        private string _xml;

        public XmlDocument Metadata
        {
            get { return _metadoc; }
            set { _metadoc = value; }
        }
        private XmlDocument _metadoc;

        public override SwfTagCode TagCode
        {
            get { return SwfTagCode.Metadata; }
        }

        public override void ReadTagData(SwfReader reader)
        {
            _xml = reader.ReadString();
            try
            {
                _metadoc = new XmlDocument();
                _metadoc.LoadXml(_xml);
            }
            catch
            {
                _metadoc = null;
            }
        }

        public override void WriteTagData(SwfWriter writer)
        {
            if (_xml != null)
            {
                writer.WriteString(_xml);
            }
            else if (_metadoc != null)
            {
                writer.WriteString(_metadoc.OuterXml);
            }
        }

        public override void DumpBody(XmlWriter writer)
        {
            if (_metadoc != null)
                _metadoc.WriteTo(writer);
            else if (_xml != null)
                writer.WriteCData(_xml);
        }
    }
}