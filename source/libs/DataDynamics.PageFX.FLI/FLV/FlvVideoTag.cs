using DataDynamics.PageFX.FLI.SWF;

namespace DataDynamics.PageFX.FLI.FLV
{
    public class FlvVideoTag : FlvTag
    {
        public FlvFrameType FrameType
        {
            get { return _frameType; }
            set { _frameType = value; }
        }
        private FlvFrameType _frameType;

        public FlvCodecID CodecID
        {
            get { return _codecID; }
            set { _codecID = value; }
        }
        private FlvCodecID _codecID;

        public byte[] VideoPacket
        {
            get { return _videoPacket; }
            set { _videoPacket = value; }
        }
        private byte[] _videoPacket;

        public override FlvTagType Type
        {
            get { return FlvTagType.Video; }
        }

        protected override void ReadData(SwfReader reader)
        {
            _frameType = (FlvFrameType)reader.ReadUB(4);
            _codecID = (FlvCodecID)reader.ReadUB(4);
            _videoPacket = reader.ReadToEnd();
        }

        protected override void WriteData(SwfWriter writer)
        {
            writer.WriteUB((uint)_frameType, 4);
            writer.WriteUB((uint)_codecID, 4);
            writer.Write(_videoPacket);
        }
    }
}