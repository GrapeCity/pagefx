using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using DataDynamics.PageFX.Common.Collections;
using DataDynamics.PageFX.FlashLand.Swf;

namespace DataDynamics.PageFX.FlashLand.Abc
{
	public sealed class AbcNssetPool : ISwfAtom, ISupportXmlDump, IAbcConstPool, IReadOnlyList<AbcNamespaceSet>
	{
		private readonly AbcFile _abc;
		private readonly AbcConstList<AbcNamespaceSet> _list = new AbcConstList<AbcNamespaceSet>();

		public AbcNssetPool(AbcFile abc)
		{
			if (abc == null)
				throw new ArgumentNullException("abc");

			_abc = abc;

			Add(new AbcNamespaceSet {Key = "*"});
		}

		public int Count
		{
			get { return _list.Count; }
		}

		public AbcNamespaceSet this[int index]
		{
			get { return _list[index]; }
		}

		public AbcNamespaceSet this[string key]
		{
			get { return _list[key]; }
		}

		public void Add(AbcNamespaceSet item)
		{
			_list.Add(item);
		}

		public void Read(SwfReader reader)
		{
			int n = (int)reader.ReadUIntEncoded();
			for (int i = 1; i < n; ++i)
			{
				var item = new AbcNamespaceSet();
				item.Read(reader);
				Add(item);
			}
		}

		public void Write(SwfWriter writer)
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
					this[i].Write(writer);
			}
		}

		public void DumpXml(XmlWriter writer)
		{
			writer.WriteStartElement("ns-set-pool");
			writer.WriteAttributeString("count", Count.ToString());
			foreach (var set in this)
			{
				set.DumpXml(writer);
			}
			writer.WriteEndElement();
		}

		IAbcConst IAbcConstPool.this[int index]
		{
			get { return _list[index]; }
		}

		/// <summary>
		/// Determines whether given constant is defined in this pool.
		/// </summary>
		/// <param name="c">constant to check.</param>
		/// <returns>true if defined; otherwise, false</returns>
		public bool IsDefined(IAbcConst c)
		{
			return _list.IsDefined((AbcNamespaceSet)c);
		}

		/// <summary>
		/// Imports given namespace set.
		/// </summary>
		/// <param name="nss">namespace set to import.</param>
		/// <returns>imported namespace set.</returns>
		public AbcNamespaceSet Import(AbcNamespaceSet nss)
		{
			if (nss == null) return null;
			if (IsDefined(nss)) return nss;

			string key = nss.Key;
			var set = _list[key];
			if (set != null) return set;

			int n = nss.Count;
			var namespaces = new AbcNamespace[n];
			for (int i = 0; i < n; ++i)
				namespaces[i] = _abc.ImportConst(nss[i]);

			set = new AbcNamespaceSet(namespaces, key);
			_list.Add(set);

			return set;
		}

		/// <summary>
		/// Imports given constant.
		/// </summary>
		/// <param name="c">constant to import.</param>
		/// <returns>imported constant.</returns>
		public IAbcConst Import(IAbcConst c)
		{
			return Import((AbcNamespaceSet)c);
		}

		public IEnumerator<AbcNamespaceSet> GetEnumerator()
		{
			return _list.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return _list.GetEnumerator();
		}

		IEnumerator<IAbcConst> IEnumerable<IAbcConst>.GetEnumerator()
		{
			return _list.Cast<IAbcConst>().GetEnumerator();
		}
	}
}