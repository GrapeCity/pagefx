using System.Xml;

namespace DataDynamics.PageFX.FlashLand
{
    public interface ISupportXmlDump
    {
        void DumpXml(XmlWriter writer);
    }
}