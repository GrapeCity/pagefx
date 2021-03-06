using System.Drawing;
using System.Drawing.Drawing2D;
using System.Xml;
using DataDynamics.PageFX.Common.Extensions;
using DataDynamics.PageFX.Flash.Swf.Tags;
using DataDynamics.PageFX.Flash.Swf.Tags.Shapes;

namespace DataDynamics.PageFX.Flash.Swf
{
    public class SwfGradient
    {
    	public SwfGradient()
        {
        }

        public SwfGradient(Color start, Color end)
        {
            Colors = new[] { start, end };
            Ratios = new byte[] { 0, 255 };
        }

        public SwfGradient(ColorBlend cb)
        {
            int n = cb.Colors.Length;
            Colors = cb.Colors;
            Ratios = new byte[n];
            var pos = cb.Positions;
            for (int i = 0; i < n; ++i)
                Ratios[i] = (byte)(pos[i] * 255);
        }

    	public SwfSpreadMode SpreadMode { get; set; }

    	public SwfInterpolationMode InterpolationMode { get; set; }

    	public byte[] Ratios { get; set; }

    	public Color[] Colors { get; set; }

    	public virtual void Read(SwfReader reader, bool alpha)
        {
            SpreadMode = (SwfSpreadMode)reader.ReadUB(2);
            InterpolationMode = (SwfInterpolationMode)reader.ReadUB(2);
            int n = (int)reader.ReadUB(4);
            Ratios = new byte[n];
            Colors = new Color[n];
            for (int i = 0; i < n; ++i)
            {
            	Ratios[i] = reader.ReadUInt8();
            	Colors[i] = alpha ? reader.ReadRGBA() : reader.ReadRGB();
            }
        }

        public void Read(SwfReader reader, SwfTagCode shapeType)
        {
            Read(reader, SwfShape.HasAlpha(shapeType));
        }

        public virtual void Write(SwfWriter writer, bool alpha)
        {
            writer.WriteUB((uint)SpreadMode, 2);
            writer.WriteUB((uint)InterpolationMode, 2);
            int n = Ratios.Length;
            writer.WriteUB((uint)n, 4);
            for (int i = 0; i < n; ++i)
            {
                writer.WriteUInt8(Ratios[i]);
                if (alpha) writer.WriteRGBA(Colors[i]);
                else  writer.WriteRGB(Colors[i]);
            }
        }

        public void Write(SwfWriter writer, SwfTagCode shapeType)
        {
            Write(writer, SwfShape.HasAlpha(shapeType));
        }

        public void Dump(XmlWriter writer, SwfTagCode shapeType)
        {
            Dump(writer, SwfShape.HasAlpha(shapeType));
        }

        public void Dump(XmlWriter writer, bool alpha)
        {
            writer.WriteStartElement("gradient");
            DumpBody(writer, alpha);
            writer.WriteEndElement();
        }

        public virtual void DumpBody(XmlWriter writer, bool alpha)
        {
            writer.WriteAttributeString("spread-mode", SpreadMode.ToString());
            writer.WriteAttributeString("interpolation-mode", InterpolationMode.ToString());
            writer.WriteElementString("ratios", Ratios.Join());
            writer.WriteElementString("colors", Colors.ToHtmlHex(alpha));
        }
    }

	public enum SwfSpreadMode
    {
        Pad,
        Reflect,
        Repeat,
    }

    public enum SwfInterpolationMode
    {
        Normal,
        Linear,
    }
}