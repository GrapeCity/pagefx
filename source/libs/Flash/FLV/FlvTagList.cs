using System.Collections.Generic;
using DataDynamics.PageFX.FlashLand.Swf;

namespace DataDynamics.PageFX.FlashLand.Flv
{
	public sealed class FlvTagList : List<FlvTag>
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