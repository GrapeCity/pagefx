using System.Collections;
using System.Collections.Generic;
using System.Xml;
using DataDynamics.PageFX.Common.Collections;
using DataDynamics.PageFX.Flash.Swf;

namespace DataDynamics.PageFX.Flash.Abc
{
	public abstract class AbcConstList<T> : IReadOnlyList<T>, ISupportXmlDump, IAbcConstPool
        where T : class, IAbcConst, new()
    {
        private readonly List<T> _list = new List<T>();
        private readonly Hashtable _index = new Hashtable();

        public int Count
        {
            get { return _list.Count; }
        }

        public T this[int index]
        {
            get { return _list[index]; }
        }

        public T this[string key]
        {
            get
            {
                int i = IndexOf(key);
                if (i >= 0)
                    return _list[i];
                return null;
            }
        }

        public void Add(T item)
        {
            int i = _list.Count;
            item.Index = i;

            _list.Add(item);
            _index[item.Key] = i;

			OnAdded(item);
        }

		protected void UpdateIndex(T item)
		{
			_index[item.Key] = item.Index;
		}

		protected virtual void OnAdded(T item)
		{
		}

        public int IndexOf(string key)
        {
            var i = _index[key];
            if (i != null) return (int)i;
            return -1;
        }

        public bool IsDefined(T item)
        {
            if (item == null) return false;
            int i = item.Index;
            if (i < 0 || i >= _list.Count) return false;
            return ReferenceEquals(_list[i], item);
        }

		public virtual void Read(SwfReader reader)
		{
			int n = (int)reader.ReadUIntEncoded();
			for (int i = 1; i < n; ++i)
			{
				var item = new T();
				item.Read(reader);
				Add(item);
			}
		}

		public virtual void Write(SwfWriter writer)
		{
			int n = Count;
			if (n <= 1)
			{
				writer.WriteUInt8(0);
			}
			else
			{
				writer.WriteUIntEncoded((uint)n);
				for (int i = 1; i < n; ++i)
				{
					this[i].Write(writer);
				}
			}
		}

	    protected virtual string DumpElementName
	    {
			get { return typeof(T).Name + "pool"; }
	    }

		public void DumpXml(XmlWriter writer)
		{
			if (Count <= 1) return;

			DumpXml(writer, DumpElementName);
		}

		public void DumpXml(XmlWriter writer, string elementName)
		{
			if (Count <= 1) return;
			writer.WriteStartElement(elementName);
			writer.WriteAttributeString("count", Count.ToString());
			foreach (var item in _list)
			{
				item.DumpXml(writer);
			}
			writer.WriteEndElement();
		}

	    public IEnumerator<T> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

	    IEnumerator IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator();
        }

		IAbcConst IAbcConstPool.this[int index]
		{
			get { return this[index]; }
		}

		bool IAbcConstPool.IsDefined(IAbcConst c)
		{
			return IsDefined((T)c);
		}

		IAbcConst IAbcConstPool.Import(IAbcConst c)
		{
			return Import((T)c);
		}

		public abstract T Import(T item);
    }
}