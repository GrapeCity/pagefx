using System.Drawing;
using System.Xml;

namespace DataDynamics.PageFX.Flash.Swf.Filters
{
    public sealed class SwfGlowFilter : SwfFilter
    {
	    public Color Color { get; set; }

	    public float BlurX { get; set; }

	    public float BlurY { get; set; }

	    public float Strength { get; set; }

	    public bool Inner { get; set; }

	    public bool Knockout { get; set; }

	    public bool CompositeSource { get; set; }

	    public int Passes { get; set; }

	    public override SwfFilterKind Kind
        {
            get { return SwfFilterKind.Glow; }
        }

        public override void Read(SwfReader reader)
        {
            Color = reader.ReadRGBA();
            BlurX = reader.ReadFixed32();
            BlurY = reader.ReadFixed32();
            Strength = reader.ReadFixed16();
            Inner = reader.ReadBit();
            Knockout = reader.ReadBit();
            CompositeSource = reader.ReadBit();
            Passes = (int)reader.ReadUB(5);
        }

        public override void Write(SwfWriter writer)
        {
            writer.WriteRGBA(Color);
            writer.WriteFixed32(BlurX);
            writer.WriteFixed32(BlurY);
            writer.WriteFixed16(Strength);
            writer.WriteBit(Inner);
            writer.WriteBit(Knockout);
            writer.WriteBit(CompositeSource);
            writer.WriteUB((uint)Passes, 5);
        }

        public override void DumpBody(XmlWriter writer)
        {
            writer.WriteElementString("color", Color.ToHtmlHex());
            writer.WriteElementString("blur-x", BlurX.ToString());
            writer.WriteElementString("blur-y", BlurY.ToString());
            writer.WriteElementString("strength", Strength.ToString());
            writer.WriteElementString("inner", Inner.ToString());
            writer.WriteElementString("knockout", Knockout.ToString());
            writer.WriteElementString("composite-source", CompositeSource.ToString());
            writer.WriteElementString("passes", Passes.ToString());
        }
    }
}