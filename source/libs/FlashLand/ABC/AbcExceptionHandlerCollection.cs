using System.Collections;
using System.Collections.Generic;
using System.Xml;
using DataDynamics.PageFX.Common.Collections;
using DataDynamics.PageFX.FlashLand.Swf;

namespace DataDynamics.PageFX.FlashLand.Abc
{
	public sealed class AbcExceptionHandlerCollection : IReadOnlyList<AbcExceptionHandler>, ISwfAtom, ISupportXmlDump
	{
		private readonly List<AbcExceptionHandler> _list = new List<AbcExceptionHandler>();

		public int Count
		{
			get { return _list.Count; }
		}

		public AbcExceptionHandler this[int index]
		{
			get { return _list[index]; }
		}

		public void Sort()
		{
			_list.Sort((x, y)=>
				{
					int c = x.Target - y.Target;
					if (c != 0) return c;

					c = x.From - y.From;
					if (c != 0) return c;

					return 0;
				});

			for (int i = 0; i < Count; i++)
			{
				this[i].Index = i;
			}
		}

		public void Add(AbcExceptionHandler e)
		{
			e.Index = Count;
			_list.Add(e);
		}

		public void Read(SwfReader reader)
		{
			int n = (int)reader.ReadUIntEncoded();
			for (int i = 0; i < n; ++i)
			{
				var handler = new AbcExceptionHandler();
				handler.Read(reader);
				Add(handler);
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
			if (Count > 0)
			{
				writer.WriteStartElement("exceptions");
				foreach (var h in this)
					h.DumpXml(writer);
				writer.WriteEndElement();
			}
		}

		public IEnumerator<AbcExceptionHandler> GetEnumerator()
		{
			return _list.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}