using System.Xml;

namespace DataDynamics.PageFX.FlashLand.Swf.Tags.Control
{
    /// <summary>
    /// The ImportAssets2 tag replaces the ImportAssets tag for SWF 8 and later.
    /// </summary>
    [SwfTag(SwfTagCode.ImportAssets2)]
    public class SwfTagImportAssets2 : SwfTagImportAssets
    {
    	public byte MajorVersion { get; set; }

    	public byte MinorVersion { get; set; }

    	public override SwfTagCode TagCode
        {
            get { return SwfTagCode.ImportAssets2; }
        }

        public override void ReadTagData(SwfReader reader)
        {
            URL = reader.ReadString();
            MajorVersion = reader.ReadUInt8();
            MinorVersion = reader.ReadUInt8();
            Assets.Read(reader, SwfAssetFlags.Imported);
        }

        public override void WriteTagData(SwfWriter writer)
        {
            writer.WriteString(URL);
            writer.WriteUInt8(MajorVersion);
            writer.WriteUInt8(MinorVersion);
            Assets.Write(writer);
        }

        public override void DumpBody(XmlWriter writer)
        {
            writer.WriteElementString("url", URL);
            writer.WriteElementString("major-version", MajorVersion.ToString());
            writer.WriteElementString("minor-version", MinorVersion.ToString());
            Assets.DumpXml(writer);
        }
    }
}