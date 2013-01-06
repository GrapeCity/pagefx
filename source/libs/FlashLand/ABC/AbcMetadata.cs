using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using DataDynamics.PageFX.Common.Collections;
using DataDynamics.PageFX.FlashLand.Swf;

namespace DataDynamics.PageFX.FlashLand.Abc
{
	public sealed class AbcMetadata : IReadOnlyList<AbcMetaEntry>, ISwfAtom, ISupportXmlDump
    {
		private readonly List<AbcMetaEntry> _list = new List<AbcMetaEntry>();

		public int Count
		{
			get { return _list.Count; }
		}

		public AbcMetaEntry this[int index]
		{
			get { return _list[index]; }
		}

        public bool IsDefined(AbcMetaEntry e)
        {
            if (e == null) return false;
            int index = e.Index;
            if (index < 0 || index >= Count) return false;
            if (e != this[index]) return false;
            return true;
        }

        public void Add(AbcMetaEntry e)
        {
            if (e.Index < 0)
                e.Index = Count;
            _list.Add(e);
        }

        public AbcMetaEntry this[string name]
        {
            get { return this.FirstOrDefault(e => e.Name.Value == name); }
        }

		public void Read(SwfReader reader)
        {
            int n = (int)reader.ReadUIntEncoded();
            for (int i = 0; i < n; ++i)
            {
                Add(new AbcMetaEntry(reader));
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
            if (!AbcDumpService.DumpMetadata) return;
            if (Count > 0)
            {
                writer.WriteStartElement("metadata");
                writer.WriteAttributeString("count", Count.ToString());
                foreach (var e in this)
                    e.DumpXml(writer);
                writer.WriteEndElement();
            }
        }

		public IEnumerator<AbcMetaEntry> GetEnumerator()
		{
			return _list.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
    }
}