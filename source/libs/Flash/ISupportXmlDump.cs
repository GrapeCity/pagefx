using System.Xml;

namespace DataDynamics.PageFX.Flash
{
    public interface ISupportXmlDump
    {
        void DumpXml(XmlWriter writer);
    }
}