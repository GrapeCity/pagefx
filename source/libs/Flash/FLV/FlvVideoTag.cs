using DataDynamics.PageFX.FlashLand.Swf;

namespace DataDynamics.PageFX.FlashLand.Flv
{
    public sealed class FlvVideoTag : FlvTag
    {
	    public FlvFrameType FrameType { get; set; }

	    public FlvCodecID CodecId { get; set; }

	    public byte[] VideoPacket { get; set; }

	    public override FlvTagType Type
        {
            get { return FlvTagType.Video; }
        }

        protected override void ReadData(SwfReader reader)
        {
            FrameType = (FlvFrameType)reader.ReadUB(4);
            CodecId = (FlvCodecID)reader.ReadUB(4);
            VideoPacket = reader.ReadToEnd();
        }

        protected override void WriteData(SwfWriter writer)
        {
            writer.WriteUB((uint)FrameType, 4);
            writer.WriteUB((uint)CodecId, 4);
            writer.Write(VideoPacket);
        }
    }
}