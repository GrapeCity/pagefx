using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using DataDynamics.PageFX.FLI.SWF;

namespace DataDynamics.PageFX.FLI.ABC
{
    using AbcString = AbcConst<string>;
    using Pair = KeyValuePair<AbcConst<string>, AbcConst<string>>;

    public class AbcMetaEntry : ISupportXmlDump, ISwfIndexedAtom
    {
        #region Constructors
        public AbcMetaEntry()
        {    
        }

        public AbcMetaEntry(SwfReader reader)
        {
            Read(reader);
        }
        #endregion

        #region Properties

        public int Index
        {
            get { return _index; }
            set { _index = value; }
        }
        int _index = -1;

    	/// <summary>
    	/// Gets or sets entry name.
    	/// </summary>
    	public AbcString Name { get; set; }

    	public string NameString
        {
            get { return Name != null ? Name.Value : ""; }
        }

        public KeyValueList Items
        {
            get { return _items; }
        }
        readonly KeyValueList _items = new KeyValueList();

        public string this[string key]
        {
            get { return _items[key]; }
        }

        public string this[params string[] keys]
        {
            get { return keys.Select(key => _items[key]).FirstOrDefault(value => !string.IsNullOrEmpty(value)); }
        }

        #endregion

        #region IAbcAtom Members
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
        #endregion

        #region Dump
        public void DumpXml(XmlWriter writer)
        {
            writer.WriteStartElement("entry");
            writer.WriteAttributeString("name", Name.Value);
            _items.DumpXml(writer);
            writer.WriteEndElement();
        }
        #endregion

        #region Object Override Members
        public override string ToString()
        {
            var s = new StringBuilder();
            s.Append(Name);
            int n = _items.Count;
            if (n > 0)
            {
                s.Append("(");
                s.Append(_items.ToString());
                s.Append(")");
            }
            return s.ToString();
        }
        #endregion
    }

    #region class AbcMetadata
    public class AbcMetadata : List<AbcMetaEntry>, ISwfAtom, ISupportXmlDump
    {
        public bool IsDefined(AbcMetaEntry e)
        {
            if (e == null) return false;
            int index = e.Index;
            if (index < 0 || index >= Count) return false;
            if (e != this[index]) return false;
            return true;
        }

        public new void Add(AbcMetaEntry e)
        {
            if (e.Index < 0)
                e.Index = Count;
            base.Add(e);
        }

        public AbcMetaEntry this[string name]
        {
            get
            {
                return Find(e => e.Name.Value == name);
            }
        }

        #region IAbcAtom Members

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

    	#endregion

        #region Dump
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
        #endregion
    }
    #endregion

    public class KeyValueList : ISwfAtom, IEnumerable<Pair>
    {
        #region Fields
        readonly List<AbcString> _keys = new List<AbcString>();
        readonly List<AbcString> _values = new List<AbcString>();
        #endregion

        #region Public Members
        public int Count
        {
            get { return _keys.Count; }
        }

        public Pair this[int index]
        {
            get
            {
                return new Pair(_keys[index], _values[index]);
            }
        }

        public AbcString GetKey(int index)
        {
            return _keys[index];
        }

        public AbcString GetValue(int index)
        {
            return _values[index];
        }

        public void Add(AbcString key, AbcString value)
        {
            _keys.Add(key);
            _values.Add(value);
        }

        public int IndexOf(string key)
        {
            return _keys.FindIndex(k => k != null && k.Value == key);
        }

        public string this[string key]
        {
            get
            {
                 int i = IndexOf(key);
                 if (i >= 0)
                     return _values[i].Value;
                return null;
            }
        }
        #endregion

        #region IAbcAtom Members
        public void Read(SwfReader reader)
        {
            int n = (int)reader.ReadUIntEncoded();
            if (n > 0)
            {
                for (int i = 0; i < n; ++i)
                {
                    var s = reader.ReadAbcString();
                    _keys.Add(s);
                }
                for (int i = 0; i < n; ++i)
                {
                    var s = reader.ReadAbcString();
                    _values.Add(s);
                }
            }
        }

        public void Write(SwfWriter writer)
        {
            int n = Count;
            writer.WriteUIntEncoded((uint)n);
            if (n > 0)
            {
                for (int i = 0; i < n; ++i)
                {
                    writer.WriteUIntEncoded((uint)_keys[i].Index);
                }
                for (int i = 0; i < n; ++i)
                {
                    writer.WriteUIntEncoded((uint)_values[i].Index);
                }
            }
        }
        #endregion

        #region IEnumerable<Pair> Members
        public IEnumerator<Pair> GetEnumerator()
        {
            int n = _keys.Count;
            for (int i = 0; i < n; ++i)
                yield return this[i];
        }
        #endregion

        #region IEnumerable Members
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion

        #region Dump
        public void DumpXml(XmlWriter writer)
        {
            int n = _keys.Count;
            for (int i = 0; i < n; ++i)
            {
                writer.WriteStartElement("item");
                writer.WriteAttributeString("key", _keys[i].Value);
                writer.WriteAttributeString("value", _values[i].Value);
                writer.WriteEndElement();
            }
        }
        #endregion

        #region Object Override Methods
        public override string ToString()
        {
            var sb = new StringBuilder();
            int n = _keys.Count;
            for (int i = 0; i < n; ++i)
            {
                if (i > 0) sb.Append(", ");
                if (string.IsNullOrEmpty(_keys[i].Value))
                {
                    sb.Append(_values[i].Value);
                }
                else
                {
                    sb.AppendFormat("{0} = {1}", _keys[i].Value, _values[i].Value);
                }
            }
            return sb.ToString();
        }
        #endregion
    }
}