using System.Diagnostics;
using DataDynamics.PageFX.FlashLand.Swf;

namespace DataDynamics.PageFX.FlashLand.Flv
{
    public sealed class FlvAudioTag : FlvTag
    {
	    public FlvSoundFormat Format { get; set; }

	    public FlvSoundRate Rate { get; set; }

	    public int SampleSize { get; set; }

	    public FlvSoundType SoundType { get; set; }

	    public byte[] SoundData { get; set; }

	    public override FlvTagType Type
        {
            get { return FlvTagType.Audio; }
        }

        protected override void ReadData(SwfReader reader)
        {
            Format = (FlvSoundFormat)reader.ReadUB(4);
            Rate = (FlvSoundRate)reader.ReadUB(2);
            SampleSize = reader.ReadBit() ? 8 : 16;
            SoundType = reader.ReadBit() ? FlvSoundType.Mono : FlvSoundType.Stereo;
            SoundData = reader.ReadToEnd();
        }

        protected override void WriteData(SwfWriter writer)
        {
            Debug.Assert(SampleSize == 8 || SampleSize == 16);
            writer.WriteUB((uint)Format, 4);
            writer.WriteUB((uint)Rate, 2);
            writer.WriteBit(SampleSize == 16);
            writer.WriteBit(SoundType == FlvSoundType.Stereo);
            writer.Write(SoundData);
        }
    }
}