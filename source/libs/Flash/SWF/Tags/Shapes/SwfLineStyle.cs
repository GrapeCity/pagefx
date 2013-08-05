using System.Collections.Generic;
using System.Drawing;
using System.Xml;

namespace DataDynamics.PageFX.Flash.Swf.Tags.Shapes
{
    //NOTE: Before the introduction of LINESTYLE2 in SWF 8, all lines in the SWF file format have
    //rounded joins and round caps. Different join styles and end styles can be simulated with
    //a very narrow shape that looks identical to the desired stroke.

    //NOTE: The SWF file format has no native support for dashed or dotted line styles. A dashed line
    //can be simulated by breaking up the path into a series of short lines.

    //NOTE: In order to use LINESTYLE2, the shape must be defined with DefineShape4 
    // — not DefineShape, DefineShape2, or DefineShape3.

    public sealed class SwfLineStyle
    {
	    public SwfLineStyle()
        {
        }

        public SwfLineStyle(Color color, float width)
        {
            Color = color;
            Width = width;
        }

	    public float Width { get; set; }

	    public Color Color { get; set; }

	    public float EndWidth { get; set; }

	    public Color EndColor { get; set; }

	    public SwfCapStyle StartCapStyle { get; set; }

	    public SwfCapStyle EndCapStyle { get; set; }

	    public SwfJoinStyle JoinStyle { get; set; }

	    public float MiterLimit { get; set; }

	    public SwfFillStyle Fill { get; set; }

	    public SwfLineFlags Flags { get; set; }

	    public void Read(SwfReader reader, SwfTagCode shapeType)
        {
            Width = reader.ReadTwipU16();
            if (shapeType == SwfTagCode.DefineMorphShape)
            {
                EndWidth = reader.ReadTwipU16();
                Color = reader.ReadRGBA();
                EndColor = reader.ReadRGBA();
            }
            else if (shapeType == SwfTagCode.DefineShape4 || shapeType == SwfTagCode.DefineMorphShape2)
            {
                bool isMorph = shapeType == SwfTagCode.DefineMorphShape2;
                if (isMorph)
                    EndWidth = reader.ReadTwipU16();

                StartCapStyle = (SwfCapStyle)reader.ReadUB(2);
                JoinStyle = (SwfJoinStyle)reader.ReadUB(2);
                bool hasFill = reader.ReadBit();
                bool noHScale = reader.ReadBit();
                bool noVScale = reader.ReadBit();
                bool pixelHinting = reader.ReadBit();

                reader.ReadUB(5); //reserved
                bool noClose = reader.ReadBit();
                EndCapStyle = (SwfCapStyle)reader.ReadUB(2);

                if (noHScale) Flags |= SwfLineFlags.NoHScale;
                if (noVScale) Flags |= SwfLineFlags.NoVScale;
                if (pixelHinting) Flags |= SwfLineFlags.PixelHinting;
                if (noClose) Flags |= SwfLineFlags.NoClose;

                if (JoinStyle == SwfJoinStyle.Miter)
                    MiterLimit = reader.ReadFixed16();

                if (hasFill)
                {
                    Fill = SwfFillStyle.Create(reader, shapeType);
                }
                else
                {
                    Color = reader.ReadRGBA();
                    if (isMorph)
                        EndColor = reader.ReadRGBA();
                }
            }
            else if (shapeType == SwfTagCode.DefineShape3)
            {
                Color = reader.ReadRGBA();
            }
            else
            {
                Color = reader.ReadRGB();
            }
        }

	    public void Write(SwfWriter writer, SwfTagCode shapeType)
        {
            writer.WriteTwipU16(Width);
            if (shapeType == SwfTagCode.DefineMorphShape)
            {
                writer.WriteTwipU16(EndWidth);
                writer.WriteRGBA(Color);
                writer.WriteRGBA(EndColor);
            }
            else if (shapeType == SwfTagCode.DefineShape4 || shapeType == SwfTagCode.DefineMorphShape2)
            {
                bool isMorph = shapeType == SwfTagCode.DefineMorphShape2;
                if (isMorph)
                    writer.WriteTwipU16(EndWidth);

                writer.WriteUB((uint)StartCapStyle, 2);
                writer.WriteUB((uint)JoinStyle, 2);

                writer.WriteBit(Fill != null); //hasFill
                writer.WriteBit((Flags & SwfLineFlags.NoHScale) != 0);
                writer.WriteBit((Flags & SwfLineFlags.NoVScale) != 0);
                writer.WriteBit((Flags & SwfLineFlags.PixelHinting) != 0);
                writer.WriteUB(0, 5); //reserved
                writer.WriteBit((Flags & SwfLineFlags.NoClose) != 0);

                writer.WriteUB((uint)EndCapStyle, 2);

                if (JoinStyle == SwfJoinStyle.Miter)
                    writer.WriteFixed16(MiterLimit);

                if (Fill != null)
                {
                    Fill.Write(writer, shapeType);
                }
                else
                {
                    writer.WriteRGBA(Color);
                    if (isMorph)
                        writer.WriteRGBA(EndColor);
                }
            }
            else if (shapeType == SwfTagCode.DefineShape3)
            {
                writer.WriteRGBA(Color);
            }
            else
            {
                writer.WriteRGB(Color);
            }
        }

	    public void Dump(XmlWriter writer, SwfTagCode shapeType)
        {
            writer.WriteStartElement("line-style");
            writer.WriteAttributeString("width", Width.ToString());
            if (shapeType == SwfTagCode.DefineMorphShape)
            {
                writer.WriteAttributeString("end-width", EndWidth.ToString());
                writer.WriteAttributeString("color", Color.ToHtmlHex());
                writer.WriteAttributeString("end-color", EndColor.ToHtmlHex());
            }
            else if (shapeType == SwfTagCode.DefineShape4 || shapeType == SwfTagCode.DefineMorphShape2)
            {
                bool isMorph = shapeType == SwfTagCode.DefineMorphShape2;
                if (isMorph)
                    writer.WriteAttributeString("end-width", EndWidth.ToString());

                writer.WriteAttributeString("start-cap", StartCapStyle.ToString());
                writer.WriteAttributeString("end-cap", EndCapStyle.ToString());
                writer.WriteAttributeString("flags", Flags.ToString());
                
                if (JoinStyle == SwfJoinStyle.Miter)
                    writer.WriteAttributeString("miter-limit", MiterLimit.ToString());

                if (Fill != null)
                {
                    Fill.Dump(writer, shapeType);
                }
                else
                {
                    writer.WriteAttributeString("color", Color.ToHtmlHex());
                    if (isMorph)
                        writer.WriteAttributeString("end-color", EndColor.ToHtmlHex());
                }
            }
            else
            {
                writer.WriteAttributeString("color", Color.ToHtmlHex(shapeType == SwfTagCode.DefineShape3));
            }
            writer.WriteEndElement();
        }

	    public void ImportDependencies(SwfMovie from, SwfMovie to)
        {
            if (Fill != null)
                Fill.ImportDependencies(from, to);
        }

        public void GetRefs(SwfRefList list)
        {
            if (Fill != null)
                Fill.GetRefs(list);
        }
    }

    public sealed class SwfLineStyles : List<SwfLineStyle>
    {
	    public void Read(SwfReader reader, SwfTagCode shapeType)
        {
            int n = reader.ReadUInt8();
            if (n == 255)
                n = reader.ReadUInt16();
            for (int i = 0; i < n; ++i)
            {
                var ls = new SwfLineStyle();
                ls.Read(reader, shapeType);
                Add(ls);
            }
        }

        public void Write(SwfWriter writer, SwfTagCode shapeType)
        {
            int n = Count;
            if (n >= 255)
            {
                writer.WriteUInt8(255);
                writer.WriteUInt16((ushort)n);
            }
            else
            {
                writer.WriteUInt8((byte)n);
            }
            for (int i = 0; i < n; ++i)
                this[i].Write(writer, shapeType);
        }

        public void Dump(XmlWriter writer, SwfTagCode shapeType)
        {
            writer.WriteStartElement("line-styles");
            int n = Count;
            writer.WriteAttributeString("count", n.ToString());
            for (int i = 0; i < n; ++i)
                this[i].Dump(writer, shapeType);
            writer.WriteEndElement();
        }

	    public void Add(Color color, float width)
        {
            Add(new SwfLineStyle(color, width));
        }

        public void ImportDependencies(SwfMovie from, SwfMovie to)
        {
            foreach (var style in this)
                style.ImportDependencies(from, to);
        }

        public void GetRefs(SwfRefList list)
        {
            foreach (var style in this)
                style.GetRefs(list);
        }
    }
}