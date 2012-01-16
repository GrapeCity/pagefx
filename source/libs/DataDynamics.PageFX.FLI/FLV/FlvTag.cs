using System;
using System.Collections.Generic;
using DataDynamics.PageFX.FLI.SWF;

namespace DataDynamics.PageFX.FLI.FLV
{
    public abstract class FlvTag
    {
        public abstract FlvTagType Type { get; }

    	public int TimeStamp { get; set; }

    	public int TimeStampExtended { get; set; }

    	public int StreamID { get; set; }

    	public void Read(SwfReader reader)
        {
            int size = (int)reader.ReadUInt24BE();
            TimeStamp = (int)reader.ReadUInt24BE();
            TimeStampExtended = reader.ReadUInt8();
            StreamID = (int)reader.ReadUInt24BE();
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
            writer.WriteUInt24BE((uint)StreamID);
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

    public class FlvTagList : List<FlvTag>
    {
        public void Read(SwfReader reader)
        {
            long len = reader.Length;
            while (reader.Position < len)
            {
                uint prevSize = reader.ReadUInt32BE();
                var type = (FlvTagType)reader.ReadUInt8();
                var tag = FlvTag.Create(type);
                tag.Read(reader);
                Add(tag);
            }
        }

        public void Write(SwfWriter writer)
        {
            int n = Count;
            for (int i = 0; i < n; ++i)
            {
                var tag = this[i];
            	var tagWriter = new SwfWriter {FileVersion = writer.FileVersion};
            	tag.Write(tagWriter);
                var tagData = tagWriter.ToByteArray();
                writer.WriteUInt32BE((uint)tagData.Length);
                writer.Write(tagData);
            }
        }
    }
}