using System;
using DataDynamics.PageFX.FlashLand.Swf;

namespace DataDynamics.PageFX.FlashLand.Flv
{
    public abstract class FlvTag
    {
        public abstract FlvTagType Type { get; }

	    public int TimeStamp { get; set; }

	    public int TimeStampExtended { get; set; }

	    public int StreamId { get; set; }

	    public void Read(SwfReader reader)
        {
            int size = (int)reader.ReadUInt24BE();
            TimeStamp = (int)reader.ReadUInt24BE();
            TimeStampExtended = reader.ReadUInt8();
            StreamId = (int)reader.ReadUInt24BE();
            var data = reader.ReadUInt8(size);
            var tagReader = new SwfReader(data, reader);
            ReadData(tagReader);
        }

        protected abstract void ReadData(SwfReader reader);

        public void Write(SwfWriter writer)
        {
            writer.WriteUInt8((byte)Type);
            var data = GetData();
            writer.WriteUInt24BE((uint)data.Length);
            writer.WriteUInt24BE((uint)TimeStamp);
            writer.WriteUInt8((byte)TimeStampExtended);
            writer.WriteUInt24BE((uint)StreamId);
            writer.Write(data);
        }

        public byte[] GetData()
        {
            var writer = new SwfWriter();
            WriteData(writer);
            return writer.ToByteArray();
        }

        protected abstract void WriteData(SwfWriter writer);

        public static FlvTag Create(FlvTagType type)
        {
            switch (type)
            {
                case FlvTagType.Audio:
                    return new FlvAudioTag();
                case FlvTagType.Video:
                    return new FlvVideoTag();
                case FlvTagType.ScriptData:
                    return null;
                default:
                    throw new NotSupportedException();
            }
        }
    }
}