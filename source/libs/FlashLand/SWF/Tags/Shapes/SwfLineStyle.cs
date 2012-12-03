using System.Collections.Generic;
using System.Drawing;
using System.Xml;

namespace DataDynamics.PageFX.FlashLand.Swf.Tags.Shapes
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
        #region ctors
        public SwfLineStyle()
        {
        }

        public SwfLineStyle(Color color, float width)
        {
            _color = color;
            _width = width;
        }
        #endregion

        #region Properties
        public float Width
        {
            get { return _width; }
            set { _width = value; }
        }
        private float _width;

        public Color Color
        {
            get { return _color; }
            set { _color = value; }
        }
        private Color _color;

        public float EndWidth
        {
            get { return _endWidth; }
            set { _endWidth = value; }
        }
        private float _endWidth;

        public Color EndColor
        {
            get { return _endColor; }
            set { _endColor = value; }
        }
        private Color _endColor;

        public SwfCapStyle StartCapStyle
        {
            get { return _startCapStyle; }
            set { _startCapStyle = value; }
        }
        private SwfCapStyle _startCapStyle;

        public SwfCapStyle EndCapStyle
        {
            get { return _endCapStyle; }
            set { _endCapStyle = value; }
        }
        private SwfCapStyle _endCapStyle;

        public SwfJoinStyle JoinStyle
        {
            get { return _joinStyle; }
            set { _joinStyle = value; }
        }
        private SwfJoinStyle _joinStyle;

        public float MiterLimit
        {
            get { return _miterLimit; }
            set { _miterLimit = value; }
        }
        private float _miterLimit;

        public SwfFillStyle Fill
        {
            get { return _fill; }
            set { _fill = value; }
        }
        private SwfFillStyle _fill;

        public SwfLineFlags Flags
        {
            get { return _flags; }
            set { _flags = value; }
        }
        private SwfLineFlags _flags;
        #endregion

        #region Read
        public void Read(SwfReader reader, SwfTagCode shapeType)
        {
            _width = reader.ReadTwipU16();
            if (shapeType == SwfTagCode.DefineMorphShape)
            {
                _endWidth = reader.ReadTwipU16();
                _color = reader.ReadRGBA();
                _endColor = reader.ReadRGBA();
            }
            else if (shapeType == SwfTagCode.DefineShape4 || shapeType == SwfTagCode.DefineMorphShape2)
            {
                bool isMorph = shapeType == SwfTagCode.DefineMorphShape2;
                if (isMorph)
                    _endWidth = reader.ReadTwipU16();

                _startCapStyle = (SwfCapStyle)reader.ReadUB(2);
                _joinStyle = (SwfJoinStyle)reader.ReadUB(2);
                bool hasFill = reader.ReadBit();
                bool noHScale = reader.ReadBit();
                bool noVScale = reader.ReadBit();
                bool pixelHinting = reader.ReadBit();

                reader.ReadUB(5); //reserved
                bool noClose = reader.ReadBit();
                _endCapStyle = (SwfCapStyle)reader.ReadUB(2);

                if (noHScale) _flags |= SwfLineFlags.NoHScale;
                if (noVScale) _flags |= SwfLineFlags.NoVScale;
                if (pixelHinting) _flags |= SwfLineFlags.PixelHinting;
                if (noClose) _flags |= SwfLineFlags.NoClose;

                if (_joinStyle == SwfJoinStyle.Miter)
                    _miterLimit = reader.ReadFixed16();

                if (hasFill)
                {
                    _fill = SwfFillStyle.Create(reader, shapeType);
                }
                else
                {
                    _color = reader.ReadRGBA();
                    if (isMorph)
                        _endColor = reader.ReadRGBA();
                }
            }
            else if (shapeType == SwfTagCode.DefineShape3)
            {
                _color = reader.ReadRGBA();
            }
            else
            {
                _color = reader.ReadRGB();
            }
        }
        #endregion

        #region Write
        public void Write(SwfWriter writer, SwfTagCode shapeType)
        {
            writer.WriteTwipU16(_width);
            if (shapeType == SwfTagCode.DefineMorphShape)
            {
                writer.WriteTwipU16(_endWidth);
                writer.WriteRGBA(_color);
                writer.WriteRGBA(_endColor);
            }
            else if (shapeType == SwfTagCode.DefineShape4 || shapeType == SwfTagCode.DefineMorphShape2)
            {
                bool isMorph = shapeType == SwfTagCode.DefineMorphShape2;
                if (isMorph)
                    writer.WriteTwipU16(_endWidth);

                writer.WriteUB((uint)_startCapStyle, 2);
                writer.WriteUB((uint)_joinStyle, 2);

                writer.WriteBit(_fill != null); //hasFill
                writer.WriteBit((_flags & SwfLineFlags.NoHScale) != 0);
                writer.WriteBit((_flags & SwfLineFlags.NoVScale) != 0);
                writer.WriteBit((_flags & SwfLineFlags.PixelHinting) != 0);
                writer.WriteUB(0, 5); //reserved
                writer.WriteBit((_flags & SwfLineFlags.NoClose) != 0);

                writer.WriteUB((uint)_endCapStyle, 2);

                if (_joinStyle == SwfJoinStyle.Miter)
                    writer.WriteFixed16(_miterLimit);

                if (_fill != null)
                {
                    _fill.Write(writer, shapeType);
                }
                else
                {
                    writer.WriteRGBA(_color);
                    if (isMorph)
                        writer.WriteRGBA(_endColor);
                }
            }
            else if (shapeType == SwfTagCode.DefineShape3)
            {
                writer.WriteRGBA(_color);
            }
            else
            {
                writer.WriteRGB(_color);
            }
        }
        #endregion

        #region Dump
        public void Dump(XmlWriter writer, SwfTagCode shapeType)
        {
            writer.WriteStartElement("line-style");
            writer.WriteAttributeString("width", _width.ToString());
            if (shapeType == SwfTagCode.DefineMorphShape)
            {
                writer.WriteAttributeString("end-width", _endWidth.ToString());
                writer.WriteAttributeString("color", _color.ToHtmlHex());
                writer.WriteAttributeString("end-color", _endColor.ToHtmlHex());
            }
            else if (shapeType == SwfTagCode.DefineShape4 || shapeType == SwfTagCode.DefineMorphShape2)
            {
                bool isMorph = shapeType == SwfTagCode.DefineMorphShape2;
                if (isMorph)
                    writer.WriteAttributeString("end-width", _endWidth.ToString());

                writer.WriteAttributeString("start-cap", _startCapStyle.ToString());
                writer.WriteAttributeString("end-cap", _endCapStyle.ToString());
                writer.WriteAttributeString("flags", _flags.ToString());
                
                if (_joinStyle == SwfJoinStyle.Miter)
                    writer.WriteAttributeString("miter-limit", _miterLimit.ToString());

                if (_fill != null)
                {
                    _fill.Dump(writer, shapeType);
                }
                else
                {
                    writer.WriteAttributeString("color", _color.ToHtmlHex());
                    if (isMorph)
                        writer.WriteAttributeString("end-color", _endColor.ToHtmlHex());
                }
            }
            else
            {
                writer.WriteAttributeString("color", _color.ToHtmlHex(shapeType == SwfTagCode.DefineShape3));
            }
            writer.WriteEndElement();
        }
        #endregion

        public void ImportDependencies(SwfMovie from, SwfMovie to)
        {
            if (_fill != null)
                _fill.ImportDependencies(from, to);
        }

        public void GetRefs(IIDList list)
        {
            if (_fill != null)
                _fill.GetRefs(list);
        }
    }

    public class SwfLineStyles : List<SwfLineStyle>
    {
        #region IO
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
        #endregion

        public void Add(Color color, float width)
        {
            Add(new SwfLineStyle(color, width));
        }

        public void ImportDependencies(SwfMovie from, SwfMovie to)
        {
            foreach (var style in this)
                style.ImportDependencies(from, to);
        }

        public void GetRefs(IIDList list)
        {
            foreach (var style in this)
                style.GetRefs(list);
        }
    }
}