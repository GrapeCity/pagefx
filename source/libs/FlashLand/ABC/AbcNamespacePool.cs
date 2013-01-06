using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using DataDynamics.PageFX.Common.Collections;
using DataDynamics.PageFX.Common.Extensions;
using DataDynamics.PageFX.FlashLand.Swf;

namespace DataDynamics.PageFX.FlashLand.Abc
{
	public sealed class AbcNamespacePool : ISwfAtom, ISupportXmlDump, IAbcConstPool, IReadOnlyList<AbcNamespace>
	{
		private readonly AbcFile _abc;
		private readonly AbcConstList<AbcNamespace> _list = new AbcConstList<AbcNamespace>();
        
		public AbcNamespacePool(AbcFile abc)
		{
			if (abc == null)
				throw new ArgumentNullException("abc");

			_abc = abc;

			var ns = new AbcNamespace {Key = "*"};
			Add(ns);
		}

		public void Read(SwfReader reader)
		{
			int n = (int)reader.ReadUIntEncoded();
			for (int i = 1; i < n; ++i)
			{
				var ns = new AbcNamespace();
				ns.Read(reader);
				Add(ns);
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
				{
					this[i].Write(writer);
				}
			}
		}

		public void DumpXml(XmlWriter writer)
		{
			writer.WriteStartElement("namespaces");
			writer.WriteAttributeString("count", Count.ToString());
			for (int i = 0; i < Count; ++i)
			{
				this[i].DumpXml(writer, "item");
			}
			writer.WriteEndElement();
		}

		public int Count
		{
			get { return _list.Count; }
		}

		public void Add(AbcNamespace ns)
		{
			_list.Add(ns);
		}

		public AbcNamespace this[int index]
		{
			get { return _list[index]; }
		}

		public AbcNamespace this[string key]
		{
			get { return _list[key]; }
		}

		public AbcNamespace this[AbcConst<string> name, AbcConstKind kind]
		{
			get
			{
				string key = name.MakeKey(kind);
				return _list[key];
			}
		}

		public override string ToString()
		{
			return this.Join<AbcNamespace>("\n");
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
			return _list.IsDefined((AbcNamespace)c);
		}

		/// <summary>
		/// Imports given namespace.
		/// </summary>
		/// <param name="ns">namespace to import.</param>
		/// <returns>imported namespace.</returns>
		public AbcNamespace Import(AbcNamespace ns)
		{
			if (ns == null) return null;
			if (ns.IsAny) return this[0];
			if (IsDefined(ns)) return ns;

			var name = ns.Name;
			var kind = ns.Kind;
			string key = name.MakeKey(kind);
			var ns2 = this[key];
			if (ns2 != null) return ns2;

			name = _abc.ImportConst(name);
			ns = new AbcNamespace(name, kind, key);
			Add(ns);
			return ns;
		}

		/// <summary>
		/// Imports given constant.
		/// </summary>
		/// <param name="c">constant to import.</param>
		/// <returns>imported constant.</returns>
		public IAbcConst Import(IAbcConst c)
		{
			return Import((AbcNamespace)c);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return _list.GetEnumerator();
		}

		public IEnumerator<AbcNamespace> GetEnumerator()
		{
			return _list.GetEnumerator();
		}

		IEnumerator<IAbcConst> IEnumerable<IAbcConst>.GetEnumerator()
		{
			return _list.Cast<IAbcConst>().GetEnumerator();
		}
	}
}