using System.Drawing;
using System.Xml;

namespace DataDynamics.PageFX.FlashLand.Swf.Tags.Control
{
    [SwfTag(SwfTagCode.SetBackgroundColor)]
    public class SwfTagSetBackgroundColor : SwfTag
    {
        public SwfTagSetBackgroundColor()
        {
        }

        public SwfTagSetBackgroundColor(Color color)
        {
            _color = color;
        }

        public Color Color
        {
            get { return _color; }
            set { _color = value; }
        }
        private Color _color = Color.White;

        public override SwfTagCode TagCode
        {
            get { return SwfTagCode.SetBackgroundColor; }
        }

        public override void ReadTagData(SwfReader reader)
        {
            _color = reader.ReadRGB();
        }

        public override void WriteTagData(SwfWriter writer)
        {
            writer.WriteRGB(_color);
        }

        public override void DumpBody(XmlWriter writer)
        {
            writer.WriteElementString("color", _color.ToHtmlHex());
        }
    }
}