using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Xml;

namespace DataDynamics.PageFX.FlashLand.Swf.Tags.Shapes
{
    #region enum SwfFillKind
    public enum SwfFillKind
    {
        Solid = 0x00,

        LinearGradient = 0x10,
        RadialGradient = 0x12,
        FocalGradient = 0x13,

        RepeatingBitmap = 0x40,
        ClippedBitmap = 0x41,
        NonSmoothedRepeatingBitmap = 0x42,
        NonSmoothedClippedBitmap = 0x43,
    }
    #endregion

    #region SwfFillStyle
    public abstract class SwfFillStyle
    {
        public abstract SwfFillKind Kind { get; }

        public abstract void Read(SwfReader reader, SwfTagCode shapeType);

        public void Write(SwfWriter writer, SwfTagCode shapeType)
        {
            writer.WriteUInt8((byte)Kind);
            WriteBody(writer, shapeType);
        }

        protected abstract void WriteBody(SwfWriter writer, SwfTagCode shapeType);

        public void Dump(XmlWriter writer, SwfTagCode shapeType)
        {
            writer.WriteStartElement("fill-style");
            writer.WriteAttributeString("kind", Kind.ToString());
            DumpBody(writer, shapeType);
            writer.WriteEndElement();
        }

        protected abstract void DumpBody(XmlWriter writer, SwfTagCode shapeType);

        public virtual void ImportDependencies(SwfMovie from, SwfMovie to)
        {
        }

        public virtual void GetRefs(IIDList list)
        {
        }

        #region Shared Members
        public static SwfFillStyle Create(SwfFillKind kind)
        {
            switch (kind)
            {
                case SwfFillKind.Solid:
                    return new SwfSolidFillStyle();

                case SwfFillKind.LinearGradient:
                case SwfFillKind.RadialGradient:
                case SwfFillKind.FocalGradient:
                    return new SwfGradientFillStyle(kind);

                case SwfFillKind.RepeatingBitmap:
                case SwfFillKind.ClippedBitmap:
                case SwfFillKind.NonSmoothedRepeatingBitmap:
                case SwfFillKind.NonSmoothedClippedBitmap:
                    return new SwfTextureFillStyle(kind);

                default:
                    throw new ArgumentOutOfRangeException("kind");
            }
        }

        public static SwfFillStyle Create(SwfReader reader, SwfTagCode shapeType)
        {
            var kind = (SwfFillKind)reader.ReadUInt8();
            var fs = Create(kind);
            fs.Read(reader, shapeType);
            return fs;
        }
        #endregion
    }
    #endregion

    #region SwfFillStyles
    public class SwfFillStyles : List<SwfFillStyle>
    {
        public void Read(SwfReader reader, SwfTagCode shapeType)
        {
            int n = reader.ReadUInt8();
            if (n == 255)
                n = reader.ReadUInt16();
            for (int i = 0; i < n; ++i)
            {
                var fs = SwfFillStyle.Create(reader, shapeType);
                Add(fs);
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
            writer.WriteStartElement("fill-styles");
            int n = Count;
            writer.WriteAttributeString("count", n.ToString());
            for (int i = 0; i < n; ++i)
                this[i].Dump(writer, shapeType);
            writer.WriteEndElement();
        }

        public void Add(Color color)
        {
            Add(new SwfSolidFillStyle(color));
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
    #endregion

    #region SwfSolidFillStyle
    public class SwfSolidFillStyle : SwfFillStyle
    {
        public SwfSolidFillStyle()
        {
        }

        public SwfSolidFillStyle(Color color)
        {
            _color = color;
        }

        public Color Color
        {
            get { return _color; }
            set { _color = value; }
        }
        private Color _color;

        public Color EndColor
        {
            get { return _endColor; }
            set { _endColor = value; }
        }
        private Color _endColor;

        public override SwfFillKind Kind
        {
            get { return SwfFillKind.Solid; }
        }

        public override void Read(SwfReader reader, SwfTagCode shapeType)
        {
            bool hasAlpha = SwfShape.HasAlpha(shapeType);
            _color = hasAlpha ? reader.ReadRGBA() : reader.ReadRGB();
            if (SwfShape.IsMorph(shapeType))
                _endColor = reader.ReadRGBA();
        }

        protected override void WriteBody(SwfWriter writer, SwfTagCode shapeType)
        {
            bool hasAlpha = SwfShape.HasAlpha(shapeType);
            if (hasAlpha)
                writer.WriteRGBA(_color);
            else 
                writer.WriteRGB(_color);

            if (SwfShape.IsMorph(shapeType))
                writer.WriteRGBA(_endColor);
        }

        protected override void DumpBody(XmlWriter writer, SwfTagCode shapeType)
        {
            if (SwfShape.IsMorph(shapeType))
            {
                writer.WriteAttributeString("begin-color", _color.ToHtmlHex());
                writer.WriteAttributeString("end-color", _endColor.ToHtmlHex());
            }
            else
            {
                bool hasAlpha = SwfShape.HasAlpha(shapeType);
                writer.WriteAttributeString("color", _color.ToHtmlHex(hasAlpha));
            }
        }
    }
    #endregion

    #region SwfGradientFillStyle
    public class SwfGradientFillStyle : SwfFillStyle
    {
        public SwfGradientFillStyle(SwfFillKind kind)
        {
            _kind = kind;
        }

        private static bool IsMulticolor(LinearGradientBrush brush)
        {
            if (brush == null) return false;
            try
            {
                var cb = brush.InterpolationColors;
                return cb != null;
            }
            catch
            {
                return false;
            }
        }

        public SwfGradientFillStyle(LinearGradientBrush brush)
        {
            _kind = SwfFillKind.LinearGradient;

            if (IsMulticolor(brush))
            {
                _gradient = new SwfGradient(brush.InterpolationColors);
            }
            else
            {
                var colors = brush.LinearColors;
                _gradient = new SwfGradient(colors[0], colors[1]);
            }

            //All gradients are defined in a standard space called the gradient square.
            //The gradient square is centered at (0,0), and extends from (-16384,-16384) to (16384,16384).
            float m = 16384.FromTwips();
            var r = brush.Rectangle;
            var gs = RectangleF.FromLTRB(-m, -m, 2 * m, 2 * m);

            _matrix = brush.Transform;
            if (_matrix.IsIdentity)
                _matrix = new Matrix();

            float sx = r.Width / gs.Width;
            float sy = r.Height / gs.Height;

            _matrix.Translate(r.X + r.Width / 2, r.Y + r.Height / 2);
            _matrix.Scale(sx, sy);
        }

        public override SwfFillKind Kind
        {
            get { return _kind; }
        }
        private readonly SwfFillKind _kind;

        public Matrix Matrix
        {
            get { return _matrix; }
            set { _matrix = value; }
        }
        private Matrix _matrix;

        public Matrix EndMatrix
        {
            get { return _endMatrix; }
            set { _endMatrix = value; }
        }
        private Matrix _endMatrix;

        public SwfGradient Gradient
        {
            get { return _gradient; }
            set { _gradient = value; }
        }
        private SwfGradient _gradient;

        public override void Read(SwfReader reader, SwfTagCode shapeType)
        {
            if (_kind != SwfFillKind.FocalGradient)
            {
                _matrix = reader.ReadMatrix();
                if (SwfShape.IsMorph(shapeType))
                    _endMatrix = reader.ReadMatrix();
                _gradient = new SwfGradient();
                _gradient.Read(reader, shapeType);
            }
            else
            {
                _gradient = new SwfFocalGradient();
                _gradient.Read(reader, shapeType);
            }
        }

        protected override void WriteBody(SwfWriter writer, SwfTagCode shapeType)
        {
            if (_kind != SwfFillKind.FocalGradient)
            {
                writer.WriteMatrix(_matrix);
                if (SwfShape.IsMorph(shapeType))
                    writer.WriteMatrix(_endMatrix);
            }
            _gradient.Write(writer, shapeType);
        }

        protected override void DumpBody(XmlWriter writer, SwfTagCode shapeType)
        {
            writer.WriteElementString("matrix", _matrix.GetMatrixString());
            if (SwfShape.IsMorph(shapeType))
                writer.WriteElementString("end-matrix", _endMatrix.GetMatrixString());
            _gradient.Dump(writer, shapeType);
        }
    }
    #endregion

    #region SwfTextureFillStyle
    public class SwfTextureFillStyle : SwfFillStyle
    {
        public SwfTextureFillStyle(SwfFillKind kind)
        {
            _kind = kind;
        }

        public SwfTextureFillStyle(ushort bmpid, SwfFillKind kind)
        {
            _kind = kind;
            _bmpid = bmpid;
        }

        public ushort BitmapID
        {
            get { return _bmpid; }
            set { _bmpid = value; }
        }
        private ushort _bmpid;

        public Matrix Matrix
        {
            get { return _matrix; }
            set { _matrix = value; }
        }
        private Matrix _matrix;

        public Matrix EndMatrix
        {
            get { return _endMatrix; }
            set { _endMatrix = value; }
        }
        private Matrix _endMatrix;

        public override SwfFillKind Kind
        {
            get { return _kind; }
        }
        private readonly SwfFillKind _kind;

        public override void Read(SwfReader reader, SwfTagCode shapeType)
        {
            _bmpid = reader.ReadUInt16();
            _matrix = reader.ReadMatrix();
            if (SwfShape.IsMorph(shapeType))
                _endMatrix = reader.ReadMatrix();
        }

        protected override void WriteBody(SwfWriter writer, SwfTagCode shapeType)
        {
            writer.WriteUInt16(_bmpid);
            writer.WriteMatrix(_matrix);
            if (SwfShape.IsMorph(shapeType))
                writer.WriteMatrix(_endMatrix);
        }

        protected override void DumpBody(XmlWriter writer, SwfTagCode shapeType)
        {
            writer.WriteElementString("bmp-id", _bmpid.ToString());
            writer.WriteElementString("matrix", _matrix.GetMatrixString());
            if (SwfShape.IsMorph(shapeType))
                writer.WriteElementString("end-matrix", _endMatrix.GetMatrixString());
        }

        public override void ImportDependencies(SwfMovie from, SwfMovie to)
        {
            to.ImportCharacter(from, ref _bmpid);
        }

        public override void GetRefs(IIDList list)
        {
            list.Add(_bmpid);
        }
    }
    #endregion
}