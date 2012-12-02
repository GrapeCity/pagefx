using System.Xml;

namespace DataDynamics
{
    public interface ISupportXmlDump
    {
        void DumpXml(XmlWriter writer);
    }
}