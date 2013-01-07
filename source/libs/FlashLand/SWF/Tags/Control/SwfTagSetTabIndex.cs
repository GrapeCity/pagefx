using System.Xml;

namespace DataDynamics.PageFX.FlashLand.Swf.Tags.Control
{
    [SwfTag(SwfTagCode.SetTabIndex)]
    public sealed class SwfTagSetTabIndex : SwfTag
    {
	    public ushort Depth { get; set; }

	    public ushort TabIndex { get; set; }

	    public override SwfTagCode TagCode
        {
            get { return SwfTagCode.SetTabIndex; }
        }

        public override void ReadTagData(SwfReader reader)
        {
            Depth = reader.ReadUInt16();
            TabIndex = reader.ReadUInt16();
        }

        public override void WriteTagData(SwfWriter writer)
        {
            writer.WriteUInt16(Depth);
            writer.WriteUInt16(TabIndex);
        }

        public override void DumpBody(XmlWriter writer)
        {
            writer.WriteElementString("depth", Depth.ToString());
            writer.WriteElementString("tab-index", TabIndex.ToString());
        }
    }
}