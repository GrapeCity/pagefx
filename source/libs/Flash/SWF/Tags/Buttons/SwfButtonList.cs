using System.Collections.Generic;
using System.Xml;

namespace DataDynamics.PageFX.FlashLand.Swf.Tags.Buttons
{
	public sealed class SwfButtonList : List<SwfButton>
	{
		public void Read(SwfReader reader, SwfTagCode tagCode)
		{
			while (true)
			{
				byte state = reader.ReadUInt8();
				if (state == 0) break;
				var btn = new SwfButton((SwfButtonState)state);
				btn.Read(reader, tagCode);
				Add(btn);
			}
		}

		public void Write(SwfWriter writer, SwfTagCode tagCode)
		{
			int n = Count;
			for (int i = 0; i < n; ++i)
				this[i].Write(writer, tagCode);
			writer.WriteUInt8(0);
		}

		public void Dump(XmlWriter writer, SwfTagCode tagCode)
		{
			writer.WriteStartElement("buttons");
			int n = Count;
			writer.WriteAttributeString("count", n.ToString());
			for (int i = 0; i < n; ++i)
				this[i].Dump(writer, tagCode);
			writer.WriteEndElement();
		}
	}
}