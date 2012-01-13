using System.Xml;

namespace DataDynamics.PageFX.FLI.SWF
{
    [SwfAction(SwfActionCode.GotoFrame)]
    public class SwfActionGotoFrame : SwfAction
    {
        public ushort FrameIndex
        {
            get { return _frameIndex; }
            set { _frameIndex = value; }
        }
        private ushort _frameIndex;

        public override SwfActionCode ActionCode
        {
            get { return SwfActionCode.GotoFrame; }
        }

        public override void ReadBody(SwfReader reader)
        {
            _frameIndex = reader.ReadUInt16();
        }

        public override void WriteBody(SwfWriter writer)
        {
            writer.WriteUInt16(_frameIndex);
        }

        public override void DumpBody(XmlWriter writer)
        {
            writer.WriteElementString("frame-index", _frameIndex.ToString());
        }
    }
}