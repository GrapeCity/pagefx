using System.Drawing;
using System.Xml;

namespace DataDynamics.PageFX.Flash.Swf.Tags.Control
{
    [SwfTag(SwfTagCode.DefineScalingGrid)]
    public sealed class SwfTagDefineScalingGrid : SwfTag
    {
	    /// <summary>
	    /// ID of sprite or button character upon which the scaling grid will be applied. 
	    /// </summary>
	    public ushort Id { get; set; }

	    /// <summary>
	    /// Center region of 9-slice grid
	    /// </summary>
	    public RectangleF Splitter { get; set; }

	    public override SwfTagCode TagCode
        {
            get { return SwfTagCode.DefineScalingGrid; }
        }

        public override void ReadTagData(SwfReader reader)
        {
            Id = reader.ReadUInt16();
            Splitter = reader.ReadRectF();
        }

        public override void WriteTagData(SwfWriter writer)
        {
            writer.WriteUInt16(Id);
            writer.WriteRectTwip(Splitter);
        }

        public override void DumpBody(XmlWriter writer)
        {
            writer.WriteElementString("idref", Id.ToString());
            writer.WriteElementString("splitter", Splitter.ToString());
        }
    }
}