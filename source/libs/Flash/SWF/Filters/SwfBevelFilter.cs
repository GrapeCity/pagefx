using System.Drawing;
using System.Xml;

namespace DataDynamics.PageFX.Flash.Swf.Filters
{
    public class SwfBevelFilter : SwfFilter
    {
        public Color ShadowColor
        {
            get { return _shadowColor; }
            set { _shadowColor = value; }
        }
        private Color _shadowColor;

        public Color HighlightColor
        {
            get { return _highlightColor; }
            set { _highlightColor = value; }
        }
        private Color _highlightColor;

        public float BlurX
        {
            get { return _blurX; }
            set { _blurX = value; }
        }
        private float _blurX;

        public float BlurY
        {
            get { return _blurY; }
            set { _blurY = value; }
        }
        private float _blurY;

        public float Angle
        {
            get { return _angle; }
            set { _angle = value; }
        }
        private float _angle;

        public float Distance
        {
            get { return _distance; }
            set { _distance = value; }
        }
        private float _distance;

        public float Strength
        {
            get { return _strength; }
            set { _strength = value; }
        }
        private float _strength;

        public bool Inner
        {
            get { return _inner; }
            set { _inner = value; }
        }
        private bool _inner;

        public bool Knockout
        {
            get { return _knockout; }
            set { _knockout = value; }
        }
        private bool _knockout;

        public bool CompositeSource
        {
            get { return _compositeSource; }
            set { _compositeSource = value; }
        }
        private bool _compositeSource;

        public bool OnTop
        {
            get { return _onTop; }
            set { _onTop = value; }
        }
        private bool _onTop;

        public int Passes
        {
            get { return _passes; }
            set { _passes = value; }
        }
        private int _passes;

        public override SwfFilterKind Kind
        {
            get { return SwfFilterKind.Bevel; }
        }

        public override void Read(SwfReader reader)
        {
            _shadowColor = reader.ReadRGBA();
            _highlightColor = reader.ReadRGBA();
            _blurX = reader.ReadFixed32();
            _blurY = reader.ReadFixed32();
            _angle = reader.ReadFixed32();
            _distance = reader.ReadFixed32();
            _strength = reader.ReadFixed16();
            _inner = reader.ReadBit();
            _knockout = reader.ReadBit();
            _compositeSource = reader.ReadBit();
            _onTop = reader.ReadBit();
            _passes = (int)reader.ReadUB(4);
        }

        public override void Write(SwfWriter writer)
        {
            writer.WriteRGBA(_shadowColor);
            writer.WriteRGBA(_highlightColor);
            writer.WriteFixed32(_blurX);
            writer.WriteFixed32(_blurY);
            writer.WriteFixed32(_angle);
            writer.WriteFixed32(_distance);
            writer.WriteFixed16(_strength);
            writer.WriteBit(_inner);
            writer.WriteBit(_knockout);
            writer.WriteBit(_compositeSource);
            writer.WriteBit(_onTop);
            writer.WriteUB((uint)_passes, 4);
        }

        public override void DumpBody(XmlWriter writer)
        {
            writer.WriteElementString("shadow-color", _shadowColor.ToHtmlHex());
            writer.WriteElementString("highlight-color", _highlightColor.ToHtmlHex());
            writer.WriteElementString("blur-x", _blurX.ToString());
            writer.WriteElementString("blur-y", _blurY.ToString());
            writer.WriteElementString("angle", _angle.ToString());
            writer.WriteElementString("distance", _distance.ToString());
            writer.WriteElementString("strength", _strength.ToString());
            writer.WriteElementString("inner", _inner.ToString());
            writer.WriteElementString("knockout", _knockout.ToString());
            writer.WriteElementString("composite-source", _compositeSource.ToString());
            writer.WriteElementString("passes", _passes.ToString());
        }
    }
}