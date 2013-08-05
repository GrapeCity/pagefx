using System;
using System.Collections.Generic;
using System.Xml;

namespace DataDynamics.PageFX.FlashLand.Swf.Filters
{
	public sealed class SwfFilterList : List<SwfFilter>
	{
		public void Read(SwfReader reader)
		{
			int n = reader.ReadUInt8();
			for (int i = 0; i < n; ++i)
			{
				var kind = (SwfFilterKind)reader.ReadUInt8();
				var f = SwfFilter.Create(kind);
				f.Read(reader);
				Add(f);
			}
		}

		public void Write(SwfWriter writer)
		{
			int n = Count;
			if (n > byte.MaxValue)
				throw new InvalidOperationException();
			writer.WriteUInt8((byte)n);
			for (int i = 0; i < n; ++i)
			{
				var f = this[i];
				writer.WriteUInt8((byte)f.Kind);
				f.Write(writer);
			}
		}

		public void Dump(XmlWriter writer)
		{
			int n = Count;
			writer.WriteStartElement("filters");
			writer.WriteAttributeString("count", n.ToString());
			for (int i = 0; i < n; ++i)
				this[i].Dump(writer);
			writer.WriteEndElement();
		}
	}
}