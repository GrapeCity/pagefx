using System.Linq;
using System.Text;
using System.Xml;
using DataDynamics.PageFX.FlashLand.Swf;

namespace DataDynamics.PageFX.FlashLand.Abc
{
	public sealed class AbcMetaEntry : ISupportXmlDump, ISwfIndexedAtom
	{
		public AbcMetaEntry()
		{    
		}

		public AbcMetaEntry(SwfReader reader)
		{
			Read(reader);
		}

		public int Index
		{
			get { return _index; }
			set { _index = value; }
		}
		int _index = -1;

		/// <summary>
		/// Gets or sets entry name.
		/// </summary>
		public AbcConst<string> Name { get; set; }

		public string NameString
		{
			get { return Name != null ? Name.Value : ""; }
		}

		public KeyValueList Items
		{
			get { return _items; }
		}
		private readonly KeyValueList _items = new KeyValueList();

		public string this[string key]
		{
			get { return _items[key]; }
		}

		public string this[params string[] keys]
		{
			get { return keys.Select(key => _items[(string)key]).FirstOrDefault(value => !string.IsNullOrEmpty(value)); }
		}

		public void Read(SwfReader reader)
		{
			Name = reader.ReadAbcString();
			_items.Read(reader);
		}

		public void Write(SwfWriter writer)
		{
			writer.WriteUIntEncoded((uint)Name.Index);
			_items.Write(writer);
		}

		public void DumpXml(XmlWriter writer)
		{
			writer.WriteStartElement("entry");
			writer.WriteAttributeString("name", Name.Value);
			_items.DumpXml(writer);
			writer.WriteEndElement();
		}

		public override string ToString()
		{
			var s = new StringBuilder();
			s.Append(Name);
			int n = _items.Count;
			if (n > 0)
			{
				s.Append("(");
				s.Append(_items);
				s.Append(")");
			}
			return s.ToString();
		}
	}
}