using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using DataDynamics.PageFX.Common.Collections;
using DataDynamics.PageFX.FlashLand.Swf;

namespace DataDynamics.PageFX.FlashLand.Abc
{
	public sealed class AbcMethodCollection : IReadOnlyList<AbcMethod>, ISwfAtom, ISupportXmlDump
	{
		private readonly AbcFile _abc;
		private readonly List<AbcMethod> _list = new List<AbcMethod>();

		public AbcMethodCollection(AbcFile abc)
		{
			_abc = abc;
		}

		public int Count
		{
			get { return _list.Count; }
		}

		public AbcMethod this[int index]
		{
			get { return _list[index]; }
		}

		public void Add(AbcMethod method)
		{
			if (method == null)
				throw new ArgumentNullException("method");

			if (method.Abc != null)
				throw new InvalidOperationException();

			method.Abc = _abc;
			method.Index = Count;

			_list.Add(method);
		}

		public void Read(SwfReader reader)
		{
			int n = (int)reader.ReadUIntEncoded();
			for (int i = 0; i < n; ++i)
			{
				var method = new AbcMethod();
				method.Read(reader);
				Add(method);
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
			if (!AbcDumpService.DumpFunctions) return;
			writer.WriteStartElement("methods");
			writer.WriteAttributeString("count", Count.ToString());
			foreach (var m in this)
			{
				if (m.Trait != null) continue;
				if (m.IsInitializer) continue;
				m.DumpXml(writer);
			}
			writer.WriteEndElement();
		}

		public void Dump(TextWriter writer, string tab, bool isStatic)
		{
			int n = Count;
			for (int i = 0; i < n; ++i)
			{
				if (i > 0) writer.WriteLine();
				this[i].Dump(writer, tab, isStatic);
			}
		}

		public IEnumerator<AbcMethod> GetEnumerator()
		{
			return _list.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}