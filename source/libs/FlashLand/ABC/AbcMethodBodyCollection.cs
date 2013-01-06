using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using DataDynamics.PageFX.Common.Collections;
using DataDynamics.PageFX.FlashLand.Swf;

namespace DataDynamics.PageFX.FlashLand.Abc
{
	public sealed class AbcMethodBodyCollection : IReadOnlyList<AbcMethodBody>, ISwfAtom, ISupportXmlDump
	{
		private readonly List<AbcMethodBody> _list = new List<AbcMethodBody>();

		public int Count
		{
			get { return _list.Count; }
		}

		public AbcMethodBody this[int index]
		{
			get { return _list[index]; }
		}

		public void Add(AbcMethodBody body)
		{
			if (body == null)
				throw new ArgumentNullException("body");

			_list.Add(body);
		}

		public void Read(SwfReader reader)
		{
			int n = (int)reader.ReadUIntEncoded();
			for (int i = 0; i < n; ++i)
			{
				var body = new AbcMethodBody();
				body.Read(reader);
				Add(body);
			}
		}

		public void Write(SwfWriter writer)
		{
			int n = Count;
			writer.WriteUIntEncoded((uint)n);
			for (int i = 0; i < n; ++i)
				this[i].Write(writer);
		}

		public void DumpXml(XmlWriter writer)
		{
			writer.WriteStartElement("method-bodies");
			writer.WriteAttributeString("count", Count.ToString());
			foreach (var body in this)
				body.DumpXml(writer);
			writer.WriteEndElement();
		}

		public IEnumerator<AbcMethodBody> GetEnumerator()
		{
			return _list.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}