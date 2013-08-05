using System.Collections.Generic;
using System.Xml;

namespace DataDynamics.PageFX.Flash.Swf.Actions
{
	public sealed class SwfActionList : List<SwfAction>, ISupportXmlDump
	{
		private const byte ActionsWithData = 0x80;

		public void Read(SwfReader reader)
		{
			while (true)
			{
				byte code = reader.ReadUInt8();
				if (code == 0) break;
				if (code >= ActionsWithData)
				{
					int len = reader.ReadUInt16();
					var data = reader.ReadUInt8(len);
					var action = SwfActionFactory.Create((SwfActionCode)code);
					if (action == null)
					{
						action = new SwfUnknownAction((SwfActionCode)code, data);
					}
					else
					{
						action.ReadBody(new SwfReader(data));
					}
					Add(action);
				}
				else
				{
					Add(new SwfSimpleAction((SwfActionCode)code));
				}
			}
		}

		public void Write(SwfWriter writer)
		{
			foreach (var action in this)
			{
				byte code = (byte)action.ActionCode;
				writer.WriteUInt8(code);
				if (code >= ActionsWithData)
				{
					var data = action.GetData();
					writer.WriteUInt16((ushort)data.Length);
					writer.Write(data);
				}
			}
			if (Count <= 0 || this[Count - 1].ActionCode != SwfActionCode.End)
				writer.WriteUInt8(0);
		}

		public void DumpXml(XmlWriter writer)
		{
			writer.WriteStartElement("actions");
			foreach (var action in this)
				action.DumpXml(writer);
			writer.WriteEndElement();
		}
	}
}