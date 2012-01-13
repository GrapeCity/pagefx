using System.Xml;

namespace DataDynamics.PageFX.FLI.SWF
{
    [SwfAction(SwfActionCode.Jump)]
    public class SwfActionJump : SwfAction
    {
        public short Offset
        {
            get { return _offset; }
            set { _offset = value; }
        }
        private short _offset;

        public override SwfActionCode ActionCode
        {
            get { return SwfActionCode.Jump; }
        }

        public override void ReadBody(SwfReader reader)
        {
            _offset = reader.ReadInt16();
        }

        public override void WriteBody(SwfWriter writer)
        {
            writer.WriteInt16(_offset);
        }

        public override void DumpBody(XmlWriter writer)
        {
            writer.WriteElementString("offset", _offset.ToString());
        }
    }
}