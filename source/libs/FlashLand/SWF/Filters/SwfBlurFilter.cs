using System.Xml;

namespace DataDynamics.PageFX.FlashLand.Swf.Filters
{
    public class SwfBlurFilter : SwfFilter
    {
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

        public int Passes
        {
            get { return _passes; }
            set { _passes = value; }
        }
        private int _passes;

        public override SwfFilterID ID
        {
            get { return SwfFilterID.Blur; }
        }

        public override void Read(SwfReader reader)
        {
            _blurX = reader.ReadFixed32();
            _blurY = reader.ReadFixed32();
            _passes = reader.ReadUInt8();
        }

        public override void Write(SwfWriter writer)
        {
            writer.WriteFixed32(_blurX);
            writer.WriteFixed32(_blurY);
            writer.WriteUInt8((byte)_passes);
        }

        public override void DumpBody(XmlWriter writer)
        {
            writer.WriteElementString("blur-x", _blurX.ToString());
            writer.WriteElementString("blur-y", _blurY.ToString());
            writer.WriteElementString("passes", _passes.ToString());
        }
    }
}