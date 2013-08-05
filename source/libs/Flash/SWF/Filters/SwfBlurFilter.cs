using System.Xml;

namespace DataDynamics.PageFX.Flash.Swf.Filters
{
    public sealed class SwfBlurFilter : SwfFilter
    {
	    public float BlurX { get; set; }

	    public float BlurY { get; set; }

	    public int Passes { get; set; }

	    public override SwfFilterKind Kind
        {
            get { return SwfFilterKind.Blur; }
        }

        public override void Read(SwfReader reader)
        {
            BlurX = reader.ReadFixed32();
            BlurY = reader.ReadFixed32();
            Passes = reader.ReadUInt8();
        }

        public override void Write(SwfWriter writer)
        {
            writer.WriteFixed32(BlurX);
            writer.WriteFixed32(BlurY);
            writer.WriteUInt8((byte)Passes);
        }

        public override void DumpBody(XmlWriter writer)
        {
            writer.WriteElementString("blur-x", BlurX.ToString());
            writer.WriteElementString("blur-y", BlurY.ToString());
            writer.WriteElementString("passes", Passes.ToString());
        }
    }
}