using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using DataDynamics.PageFX.Common.Collections;
using DataDynamics.PageFX.FlashLand.Swf;

namespace DataDynamics.PageFX.FlashLand.Abc
{
	public sealed class AbcScriptCollection : IReadOnlyList<AbcScript>, ISwfAtom, ISupportXmlDump
	{
		private readonly List<AbcScript> _list = new List<AbcScript>();
		private readonly AbcFile _abc;

		public AbcScriptCollection(AbcFile abc)
		{
			_abc = abc;
		}

		public int Count
		{
			get { return _list.Count; }
		}

		public AbcScript this[int index]
		{
			get { return _list[index]; }
		}

		public void Add(AbcScript script)
		{
			if (script == null)
				throw new ArgumentNullException("script");

			script.Index = Count;
			script.Abc = _abc;
			_list.Add(script);
		}

		public bool IsDefined(AbcScript script)
		{
			if (script == null) return false;
			int index = script.Index;
			if (index < 0 || index >= Count) return false;
			return this[index] == script;
		}

		public void Read(SwfReader reader)
		{
			int n = (int)reader.ReadUIntEncoded();
			for (int i = 0; i < n; ++i)
			{
				var script = new AbcScript();
				script.Read(reader);
				Add(script);
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
			writer.WriteStartElement("scripts");
			writer.WriteAttributeString("count", Count.ToString());
			foreach (var s in this)
				s.DumpXml(writer);
			writer.WriteEndElement();
		}

		public void Dump(TextWriter writer)
		{
			bool eol = false;
			foreach (var script in this)
			{
				if (eol) writer.WriteLine();
				script.Dump(writer);
				eol = true;
			}
		}

		public IEnumerator<AbcScript> GetEnumerator()
		{
			return _list.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public void Sort(IComparer<AbcScript> comparer)
		{
			_list.Sort(comparer);
		}
	}
}