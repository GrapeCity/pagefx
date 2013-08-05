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

        public virtual void GetRefs(SwfRefList list)
        {
        }

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
    }
    #endregion

    #region SwfFillStyles
    public sealed class SwfFillStyles : List<SwfFillStyle>
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

        public void GetRefs(SwfRefList list)
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
            Color = color;
        }

	    public Color Color { get; set; }

	    public Color EndColor { get; set; }

	    public override SwfFillKind Kind
        {
            get { return SwfFillKind.Solid; }
        }

        public override void Read(SwfReader reader, SwfTagCode shapeType)
        {
            bool hasAlpha = SwfShape.HasAlpha(shapeType);
            Color = hasAlpha ? reader.ReadRGBA() : reader.ReadRGB();
            if (SwfShape.IsMorph(shapeType))
                EndColor = reader.ReadRGBA();
        }

        protected override void WriteBody(SwfWriter writer, SwfTagCode shapeType)
        {
            bool hasAlpha = SwfShape.HasAlpha(shapeType);
            if (hasAlpha)
                writer.WriteRGBA(Color);
            else 
                writer.WriteRGB(Color);

            if (SwfShape.IsMorph(shapeType))
                writer.WriteRGBA(EndColor);
        }

        protected override void DumpBody(XmlWriter writer, SwfTagCode shapeType)
        {
            if (SwfShape.IsMorph(shapeType))
            {
                writer.WriteAttributeString("begin-color", Color.ToHtmlHex());
                writer.WriteAttributeString("end-color", EndColor.ToHtmlHex());
            }
            else
            {
                bool hasAlpha = SwfShape.HasAlpha(shapeType);
                writer.WriteAttributeString("color", Color.ToHtmlHex(hasAlpha));
            }
        }
    }
    #endregion

    #region SwfGradientFillStyle
    public sealed class SwfGradientFillStyle : SwfFillStyle
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
                Gradient = new SwfGradient(brush.InterpolationColors);
            }
            else
            {
                var colors = brush.LinearColors;
                Gradient = new SwfGradient(colors[0], colors[1]);
            }

            //All gradients are defined in a standard space called the gradient square.
            //The gradient square is centered at (0,0), and extends from (-16384,-16384) to (16384,16384).
            float m = 16384.FromTwips();
            var r = brush.Rectangle;
            var gs = RectangleF.FromLTRB(-m, -m, 2 * m, 2 * m);

            Matrix = brush.Transform;
            if (Matrix.IsIdentity)
                Matrix = new Matrix();

            float sx = r.Width / gs.Width;
            float sy = r.Height / gs.Height;

            Matrix.Translate(r.X + r.Width / 2, r.Y + r.Height / 2);
            Matrix.Scale(sx, sy);
        }

        public override SwfFillKind Kind
        {
            get { return _kind; }
        }
        private readonly SwfFillKind _kind;

	    public Matrix Matrix { get; set; }

	    public Matrix EndMatrix { get; set; }

	    public SwfGradient Gradient { get; set; }

	    public override void Read(SwfReader reader, SwfTagCode shapeType)
        {
            if (_kind != SwfFillKind.FocalGradient)
            {
                Matrix = reader.ReadMatrix();
                if (SwfShape.IsMorph(shapeType))
                    EndMatrix = reader.ReadMatrix();
                Gradient = new SwfGradient();
                Gradient.Read(reader, shapeType);
            }
            else
            {
                Gradient = new SwfFocalGradient();
                Gradient.Read(reader, shapeType);
            }
        }

        protected override void WriteBody(SwfWriter writer, SwfTagCode shapeType)
        {
            if (_kind != SwfFillKind.FocalGradient)
            {
                writer.WriteMatrix(Matrix);
                if (SwfShape.IsMorph(shapeType))
                    writer.WriteMatrix(EndMatrix);
            }
            Gradient.Write(writer, shapeType);
        }

        protected override void DumpBody(XmlWriter writer, SwfTagCode shapeType)
        {
            writer.WriteElementString("matrix", Matrix.GetMatrixString());
            if (SwfShape.IsMorph(shapeType))
                writer.WriteElementString("end-matrix", EndMatrix.GetMatrixString());
            Gradient.Dump(writer, shapeType);
        }
    }
    #endregion

    #region SwfTextureFillStyle
    public sealed class SwfTextureFillStyle : SwfFillStyle
    {
        public SwfTextureFillStyle(SwfFillKind kind)
        {
            _kind = kind;
        }

        public SwfTextureFillStyle(ushort bmpid, SwfFillKind kind)
        {
            _kind = kind;
            BitmapId = bmpid;
        }

	    public ushort BitmapId { get; set; }

	    public Matrix Matrix { get; set; }

	    public Matrix EndMatrix { get; set; }

	    public override SwfFillKind Kind
        {
            get { return _kind; }
        }
        private readonly SwfFillKind _kind;

        public override void Read(SwfReader reader, SwfTagCode shapeType)
        {
            BitmapId = reader.ReadUInt16();
            Matrix = reader.ReadMatrix();
            if (SwfShape.IsMorph(shapeType))
                EndMatrix = reader.ReadMatrix();
        }

        protected override void WriteBody(SwfWriter writer, SwfTagCode shapeType)
        {
            writer.WriteUInt16(BitmapId);
            writer.WriteMatrix(Matrix);
            if (SwfShape.IsMorph(shapeType))
                writer.WriteMatrix(EndMatrix);
        }

        protected override void DumpBody(XmlWriter writer, SwfTagCode shapeType)
        {
            writer.WriteElementString("bmp-id", BitmapId.ToString());
            writer.WriteElementString("matrix", Matrix.GetMatrixString());
            if (SwfShape.IsMorph(shapeType))
                writer.WriteElementString("end-matrix", EndMatrix.GetMatrixString());
        }

        public override void ImportDependencies(SwfMovie from, SwfMovie to)
        {
	        var cid = BitmapId;
            to.ImportCharacter(from, ref cid);
	        BitmapId = cid;
        }

        public override void GetRefs(SwfRefList list)
        {
            list.Add(BitmapId);
        }
    }
    #endregion
}