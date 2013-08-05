using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using DataDynamics.PageFX.Common.Collections;
using DataDynamics.PageFX.FlashLand.Swf;

namespace DataDynamics.PageFX.FlashLand.Abc
{
	using Pair = KeyValuePair<AbcConst<string>, AbcConst<string>>;

	public sealed class KeyValueList : ISwfAtom, IReadOnlyList<Pair>
	{
		private readonly List<AbcConst<string>> _keys = new List<AbcConst<string>>();
		private readonly List<AbcConst<string>> _values = new List<AbcConst<string>>();

		public int Count
		{
			get { return _keys.Count; }
		}

		public Pair this[int index]
		{
			get { return new Pair(_keys[index], _values[index]); }
		}

		public AbcConst<string> GetKey(int index)
		{
			return _keys[index];
		}

		public AbcConst<string> GetValue(int index)
		{
			return _values[index];
		}

		public void Add(AbcConst<string> key, AbcConst<string> value)
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

		public IEnumerator<Pair> GetEnumerator()
		{
			for (int i = 0; i < Count; ++i)
				yield return this[i];
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

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
	}
}