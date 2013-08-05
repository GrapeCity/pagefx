using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using DataDynamics.PageFX.Common.Collections;
using DataDynamics.PageFX.Flash.Swf;

namespace DataDynamics.PageFX.Flash.Abc
{
	public class AbcAtomList<T> : IReadOnlyList<T>, ISupportXmlDump
		where T:class, ISupportXmlDump, ISwfIndexedAtom, new()
	{
		private readonly List<T> _list = new List<T>();

		public int Count
		{
			get { return _list.Count; }
		}

		public T this[int index]
		{
			get { return _list[index]; }
		}

		public void Add(T item)
		{
			if (item == null)
				throw new ArgumentNullException("item");

#if DEBUG
			if (IsDefined(item))
				throw new InvalidOperationException();
#endif

			item.Index = Count;

			_list.Add(item);

			OnAdded(item);
		}

		protected virtual void OnAdded(T item)
		{
		}

		public void Sort(Comparison<T> comparison)
		{
			_list.Sort(comparison);
		}

		public void Sort(IComparer<T> comparer)
		{
			_list.Sort(comparer);
		}

		public virtual void Clear()
		{
			_list.Clear();
		}

		public bool IsDefined(T item)
		{
			if (item == null)
				return false;

			int index = item.Index;
			if (index < 0 || index >= Count)
				return false;

			return this[index] == item;
		}

		protected virtual bool DumpDisabled
		{
			get { return false; }
		}

		protected virtual string DumpElementName
		{
			get { return GetType().Name; }
		}

		public virtual void DumpXml(XmlWriter writer)
		{
			if (DumpDisabled || Count == 0) return;

			writer.WriteStartElement(DumpElementName);
			writer.WriteAttributeString("count", XmlConvert.ToString(Count));

			foreach (var c in this)
				c.DumpXml(writer);

			writer.WriteEndElement();
		}

		public virtual void Read(SwfReader reader)
		{
			int n = (int)reader.ReadUIntEncoded();
			Read(n, reader);
		}

		public void Read(int n, SwfReader reader)
		{
			for (int i = 0; i < n; ++i)
			{
				var item = new T();
				item.Read(reader);
				Add(item);
			}
		}

		public virtual void Write(SwfWriter writer)
		{
			int n = Count;

			//TODO: to always write count in client code
			WriteCount(writer);

			for (int i = 0; i < n; ++i)
				this[i].Write(writer);
		}

		protected virtual void WriteCount(SwfWriter writer)
		{
			writer.WriteUIntEncoded((uint)Count);
		}

		public IEnumerator<T> GetEnumerator()
		{
			return _list.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}