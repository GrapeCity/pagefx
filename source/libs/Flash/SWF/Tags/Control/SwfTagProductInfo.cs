using System.Xml;

namespace DataDynamics.PageFX.Flash.Swf.Tags.Control
{
    [SwfTag(SwfTagCode.ProductInfo)]
    public sealed class SwfTagProductInfo : SwfTag
    {
	    public uint ProductId { get; set; }

	    public uint Edition { get; set; }

	    public byte MajorVersion { get; set; }

	    public byte MinorVersion { get; set; }

	    public ulong BuildNumber { get; set; }

	    public ulong BuildDate { get; set; }

	    public override SwfTagCode TagCode
        {
            get { return SwfTagCode.ProductInfo; }
        }

        public override void ReadTagData(SwfReader reader)
        {
            ProductId = reader.ReadUInt32();
            Edition = reader.ReadUInt32();
            MajorVersion = reader.ReadUInt8();
            MinorVersion = reader.ReadUInt8();
            BuildNumber = reader.ReadUInt64();
            BuildDate = reader.ReadUInt64();
        }

        public override void WriteTagData(SwfWriter writer)
        {
            writer.WriteUInt32(ProductId);
            writer.WriteUInt32(Edition);
            writer.WriteUInt8(MajorVersion);
            writer.WriteUInt8(MinorVersion);
            writer.WriteUInt64(BuildNumber);
            writer.WriteUInt64(BuildDate);
        }

        public override void DumpBody(XmlWriter writer)
        {
            writer.WriteElementString("product-id", ProductId.ToString());
            writer.WriteElementString("edition", Edition.ToString());
            writer.WriteElementString("major-version", MajorVersion.ToString());
            writer.WriteElementString("minor-version", MinorVersion.ToString());
            writer.WriteElementString("build-number", BuildNumber.ToString());
            writer.WriteElementString("build-date", BuildDate.ToString());
        }
    }
}