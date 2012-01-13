using System.Diagnostics;
using DataDynamics.PageFX.FLI.SWF;

namespace DataDynamics.PageFX.FLI.FLV
{
    public class FlvAudioTag : FlvTag
    {
        public FlvSoundFormat Format
        {
            get { return _format; }
            set { _format = value; }
        }
        private FlvSoundFormat _format;

        public FlvSoundRate Rate
        {
            get { return _rate; }
            set { _rate = value; }
        }
        private FlvSoundRate _rate;

        public int SampleSize
        {
            get { return _sampleSize; }
            set { _sampleSize = value; }
        }
        private int _sampleSize;

        public FlvSoundType SoundType
        {
            get { return _soundType; }
            set { _soundType = value; }
        }
        private FlvSoundType _soundType;

        public byte[] SoundData
        {
            get { return _soundData; }
            set { _soundData = value; }
        }
        private byte[] _soundData;

        public override FlvTagType Type
        {
            get { return FlvTagType.Audio; }
        }

        protected override void ReadData(SwfReader reader)
        {
            _format = (FlvSoundFormat)reader.ReadUB(4);
            _rate = (FlvSoundRate)reader.ReadUB(2);
            _sampleSize = reader.ReadBit() ? 8 : 16;
            _soundType = reader.ReadBit() ? FlvSoundType.Mono : FlvSoundType.Stereo;
            _soundData = reader.ReadToEnd();
        }

        protected override void WriteData(SwfWriter writer)
        {
            Debug.Assert(_sampleSize == 8 || _sampleSize == 16);
            writer.WriteUB((uint)_format, 4);
            writer.WriteUB((uint)_rate, 2);
            writer.WriteBit(_sampleSize == 16);
            writer.WriteBit(_soundType == FlvSoundType.Stereo);
            writer.Write(_soundData);
        }
    }
}