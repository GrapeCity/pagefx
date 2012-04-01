using System.Drawing;
using System.Xml;

namespace DataDynamics.PageFX.FLI.SWF
{
    public class SwfDropShadowFilter : SwfFilter
    {
        public Color Color
        {
            get { return _color; }
            set { _color = value; }
        }
        private Color _color;

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

        public int Passes
        {
            get { return _passes; }
            set { _passes = value; }
        }
        private int _passes;

        public override SwfFilterID ID
        {
            get { return SwfFilterID.DropShadow; }
        }

        public override void Read(SwfReader reader)
        {
            _color = reader.ReadRGBA();
            _blurX = reader.ReadFixed32();
            _blurY = reader.ReadFixed32();
            _distance = reader.ReadFixed32();
            _strength = reader.ReadFixed16();
            _inner = reader.ReadBit();
            _knockout = reader.ReadBit();
            _compositeSource = reader.ReadBit();
            _passes = (int)reader.ReadUB(5);
        }

        public override void Write(SwfWriter writer)
        {
            writer.WriteRGBA(_color);
            writer.WriteFixed32(_blurX);
            writer.WriteFixed32(_blurY);
            writer.WriteFixed32(_distance);
            writer.WriteFixed16(_strength);
            writer.WriteBit(_inner);
            writer.WriteBit(_knockout);
            writer.WriteBit(_compositeSource);
            writer.WriteUB((uint)_passes, 5);
        }

        public override void DumpBody(XmlWriter writer)
        {
            writer.WriteElementString("color", _color.ToHtmlHex());
            writer.WriteElementString("blur-x", _blurX.ToString());
            writer.WriteElementString("blur-y", _blurY.ToString());
            writer.WriteElementString("distance", _distance.ToString());
            writer.WriteElementString("strength", _strength.ToString());
            writer.WriteElementString("inner", _inner.ToString());
            writer.WriteElementString("knockout", _knockout.ToString());
            writer.WriteElementString("composite-source", _compositeSource.ToString());
            writer.WriteElementString("passes", _passes.ToString());
        }
    }
}