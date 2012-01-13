using System.Drawing;
using System.Drawing.Drawing2D;
using System.Xml;

namespace DataDynamics.PageFX.FLI.SWF
{
    public class SwfGradient
    {
        #region ctors
        public SwfGradient()
        {
        }

        public SwfGradient(Color start, Color end)
        {
            _colors = new[] { start, end };
            _ratios = new byte[] { 0, 255 };
        }

        public SwfGradient(ColorBlend cb)
        {
            int n = cb.Colors.Length;
            _colors = cb.Colors;
            _ratios = new byte[n];
            var pos = cb.Positions;
            for (int i = 0; i < n; ++i)
                _ratios[i] = (byte)(pos[i] * 255);
        }
        #endregion

        #region Properties
        public SwfSpreadMode SpreadMode
        {
            get { return _spreadMode; }
            set { _spreadMode = value; }
        }
        private SwfSpreadMode _spreadMode;

        public SwfInterpolationMode InterpolationMode
        {
            get { return _interpolationMode; }
            set { _interpolationMode = value; }
        }
        private SwfInterpolationMode _interpolationMode;

        public byte[] Ratios
        {
            get { return _ratios; }
            set { _ratios = value; }
        }
        private byte[] _ratios;

        public Color[] Colors
        {
            get { return _colors; }
            set { _colors = value; }
        }
        private Color[] _colors;
        #endregion

        #region IO
        public virtual void Read(SwfReader reader, bool alpha)
        {
            _spreadMode = (SwfSpreadMode)reader.ReadUB(2);
            _interpolationMode = (SwfInterpolationMode)reader.ReadUB(2);
            int n = (int)reader.ReadUB(4);
            _ratios = new byte[n];
            _colors = new Color[n];
            for (int i = 0; i < n; ++i)
            {
                _ratios[i] = reader.ReadUInt8();
                if (alpha)
                    _colors[i] = reader.ReadRGBA();
                else
                    _colors[i] = reader.ReadRGB();
            }
        }

        public void Read(SwfReader reader, SwfTagCode shapeType)
        {
            Read(reader, SwfShape.HasAlpha(shapeType));
        }

        public virtual void Write(SwfWriter writer, bool alpha)
        {
            writer.WriteUB((uint)_spreadMode, 2);
            writer.WriteUB((uint)_interpolationMode, 2);
            int n = _ratios.Length;
            writer.WriteUB((uint)n, 4);
            for (int i = 0; i < n; ++i)
            {
                writer.WriteUInt8(_ratios[i]);
                if (alpha) writer.WriteRGBA(_colors[i]);
                else  writer.WriteRGB(_colors[i]);
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
            writer.WriteAttributeString("spread-mode", _spreadMode.ToString());
            writer.WriteAttributeString("interpolation-mode", _interpolationMode.ToString());
            writer.WriteElementString("ratios", Str.ToString(_ratios));
            writer.WriteElementString("colors", SwfHelper.ToString(_colors, alpha));
        }
        #endregion
    }

    public class SwfFocalGradient : SwfGradient
    {
        public float FocalPoint
        {
            get { return _focalPoint; }
            set { _focalPoint = value; }
        }
        private float _focalPoint;

        public override void Read(SwfReader reader, bool alpha)
        {
            base.Read(reader, alpha);
            _focalPoint = reader.ReadFixed16();
        }

        public override void Write(SwfWriter writer, bool alpha)
        {
            base.Write(writer, alpha);
            writer.WriteFixed16(_focalPoint);
        }

        public override void DumpBody(XmlWriter writer, bool alpha)
        {
            base.DumpBody(writer, alpha);
            writer.WriteElementString("focal-point", _focalPoint.ToString());
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