using System.Collections.Generic;
using System.Xml;

namespace DataDynamics.PageFX.FlashLand.Swf.Tags.Control
{
	public sealed class SwfAssetCollection : List<SwfAsset>, ISupportXmlDump
	{
		public void Add(ISwfCharacter obj, string name)
		{
			Add(new SwfAsset(obj, name));
		}

		public void Add(ushort id, string name)
		{
			Add(new SwfAsset(id, name));
		}

		public void Read(SwfReader reader, SwfAssetFlags flags)
		{
			int n = reader.ReadUInt16();
			for (int i = 0; i < n; ++i)
			{
				Add(new SwfAsset(reader) {Flags = flags});
			}
		}

		public void Write(SwfWriter writer)
		{
			writer.WriteUInt16((ushort)Count);
			foreach (var obj in this)
				obj.Write(writer);
		}

		public void DumpXml(XmlWriter writer, string name, string assetName)
		{
			writer.WriteStartElement(name);
			foreach (var obj in this)
				obj.DumpXml(writer, assetName);
			writer.WriteEndElement();
		}

		public void DumpXml(XmlWriter writer)
		{
			DumpXml(writer, "assets", "asset");
		}
	}
}