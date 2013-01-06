using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using DataDynamics.PageFX.Common.Collections;
using DataDynamics.PageFX.FlashLand.Swf;

namespace DataDynamics.PageFX.FlashLand.Abc
{
	public sealed class AbcClassCollection : IReadOnlyList<AbcClass>, ISupportXmlDump
	{
		private readonly List<AbcClass> _list = new List<AbcClass>();

		public int Count
		{
			get { return _list.Count; }
		}

		public AbcClass this[int index]
		{
			get { return _list[index]; }
		}

		public void Add(AbcClass klass)
		{
			if (klass == null)
				throw new ArgumentNullException("klass");

#if DEBUG
			if (IsDefined(klass))
				throw new InvalidOperationException();
#endif
			klass.Index = Count;

			_list.Add(klass);
		}

		public void Clear()
		{
			_list.Clear();
		}

		public bool IsDefined(AbcClass klass)
		{
			if (klass == null) return false;
			int index = klass.Index;
			if (index < 0 || index >= Count)
				return false;
			return this[index] == klass;
		}

		public void Read(int n, SwfReader reader)
		{
			for (int i = 0; i < n; ++i)
			{
				var klass = new AbcClass();
				klass.Read(reader);
				Add(klass);
			}
		}

		public void Write(SwfWriter writer)
		{
			int n = Count;
			for (int i = 0; i < n; ++i)
				this[i].Write(writer);
		}

		public void DumpXml(XmlWriter writer)
		{
			if (!AbcDumpService.DumpInstances) return;
			writer.WriteStartElement("classes");
			writer.WriteAttributeString("count", Count.ToString());
			foreach (var c in this)
				c.DumpXml(writer);
			writer.WriteEndElement();
		}

		public IEnumerator<AbcClass> GetEnumerator()
		{
			return _list.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}