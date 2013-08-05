using System.Xml;
using DataDynamics.PageFX.Flash.Abc;

namespace DataDynamics.PageFX.Flash.Swf.Tags.Actions
{
    [SwfTag(SwfTagCode.DoAbc)]
    public class SwfTagDoAbc : SwfTag
    {
        public AbcFile ByteCode
        {
            get { return _abc; }
            set { _abc = value; }
        }
        private AbcFile _abc;

        public override SwfTagCode TagCode
        {
            get { return SwfTagCode.DoAbc; }
        }

        public override void ReadTagData(SwfReader reader)
        {
            _abc = new AbcFile(reader);
        }

        public override void WriteTagData(SwfWriter writer)
        {
            _abc.Write(writer);
        }

        public override void DumpBody(XmlWriter writer)
        {
            if (_abc != null)
            {
                //NOTE: This info is not interest
                //using (SwfWriter wr = new SwfWriter())
                //{
                //    _abc.Write(wr);
                //    byte[] data = wr.ToByteArray();
                //    writer.WriteElementString("abcsize", data.Length.ToString());
                //}
                writer.WriteStartElement("ABC");
                writer.WriteAttributeString("version", _abc.Version.ToString());
                writer.WriteElementString("instance-count", _abc.Instances.Count.ToString());
                writer.WriteElementString("method-count", _abc.Methods.Count.ToString());
                writer.WriteElementString("body-count", _abc.MethodBodies.Count.ToString());
                writer.WriteElementString("script-count", _abc.Scripts.Count.ToString());
                writer.WriteElementString("interface-count", _abc.InterfaceCount.ToString());
                writer.WriteStartElement("const-pool");
                writer.WriteElementString("int-count", (_abc.IntPool.Count - 1).ToString());
                writer.WriteElementString("double-count", (_abc.DoublePool.Count - 1).ToString());
                writer.WriteElementString("string-count", (_abc.StringPool.Count - 1).ToString());
                writer.WriteElementString("namespace-count", (_abc.Namespaces.Count - 1).ToString());
                writer.WriteElementString("nsset-count", (_abc.NamespaceSets.Count - 1).ToString());
                writer.WriteElementString("multiname-count", (_abc.Multinames.Count - 1).ToString());
                writer.WriteEndElement();
                writer.WriteEndElement();
                
            }
        }
    }
}