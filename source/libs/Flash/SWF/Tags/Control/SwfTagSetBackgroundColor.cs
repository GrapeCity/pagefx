using System.Drawing;
using System.Xml;

namespace DataDynamics.PageFX.Flash.Swf.Tags.Control
{
    [SwfTag(SwfTagCode.SetBackgroundColor)]
    public sealed class SwfTagSetBackgroundColor : SwfTag
    {
        public SwfTagSetBackgroundColor()
        {
	        Color = Color.White;
        }

	    public SwfTagSetBackgroundColor(Color color)
        {
            Color = color;
        }

	    public Color Color { get; set; }

	    public override SwfTagCode TagCode
        {
            get { return SwfTagCode.SetBackgroundColor; }
        }

        public override void ReadTagData(SwfReader reader)
        {
            Color = reader.ReadRGB();
        }

        public override void WriteTagData(SwfWriter writer)
        {
            writer.WriteRGB(Color);
        }

        public override void DumpBody(XmlWriter writer)
        {
            writer.WriteElementString("color", Color.ToHtmlHex());
        }
    }
}