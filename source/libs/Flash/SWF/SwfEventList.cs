using System.Collections.Generic;

namespace DataDynamics.PageFX.Flash.Swf
{
	public sealed class SwfEventList : List<SwfEvent>
	{
		public SwfEventFlags AllEventFlags { get; set; }

		public void Read(SwfReader reader)
		{
			reader.ReadUInt16(); //reserved
			AllEventFlags = SwfEvent.ReadFlags(reader);

			int ver = reader.FileVersion;
			while (true)
			{
				uint flags = ver >= 6 ? reader.ReadUInt32() : reader.ReadUInt16();
				if (flags == 0) break;
				var e = new SwfEvent {Flags = SwfEvent.ToFlags(flags, ver)};
				e.Read(reader);
			}
		}

		public void Write(SwfWriter writer)
		{
			writer.WriteUInt16(0); //reserved
			SwfEvent.WriteFlags(writer, AllEventFlags);

			int n = Count;
			for (int i = 0; i < n; ++i)
				this[i].Write(writer);

			if (writer.FileVersion >= 6)
				writer.WriteUInt32(0);
			else
				writer.WriteUInt16(0);
		}
	}
}