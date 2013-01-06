using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml;
using DataDynamics.PageFX.Common.Extensions;
using DataDynamics.PageFX.FlashLand.Swf;

namespace DataDynamics.PageFX.FlashLand.Abc
{
    /// <summary>
    /// Represents namespace set
    /// </summary>
    public sealed class AbcNamespaceSet : List<AbcNamespace>, ISupportXmlDump, IAbcConst
    {
	    public AbcNamespaceSet()
        {
        }

        internal AbcNamespaceSet(IEnumerable<AbcNamespace> collection, string key)
            : base(collection)
        {
            this._key = key;
        }

        internal AbcNamespaceSet(SwfReader reader)
        {
            Read(reader);
        }

	    public int Index
        {
            get { return _index; }
            set { _index = value; }
        }
        private int _index = -1;

        object IAbcConst.Value
        {
            get { throw new NotSupportedException(); }
            set { throw new NotSupportedException(); }
        }

        public AbcConstKind Kind
        {
            get { return AbcConstKind.NamespaceSet; }
        }

        public static string KeyOf(IEnumerable<AbcNamespace> set)
        {
            string key = "";
            foreach (var ns in set)
            {
                if (key.Length > 0) key += ",";
                key += ns.Key;
            }
            return key;
        }

        public string Key
        {
            get { return _key ?? (_key = KeyOf(this)); }
			internal set { _key = value; }
        }
        private string _key;

	    public void Read(SwfReader reader)
        {
            int n = (int)reader.ReadUIntEncoded();
            if (n <= 0)
                throw new BadImageFormatException();
            for (int i = 0; i < n; ++i)
            {
                int nsIndex = (int)reader.ReadUIntEncoded();
                var ns = reader.ABC.Namespaces[nsIndex];
                Add(ns);
            }
        }

        public void Write(SwfWriter writer)
        {
            int n = Count;
            writer.WriteUIntEncoded((uint)n);
            for (int i = 0; i < n; ++i)
                writer.WriteUIntEncoded((uint)this[i].Index);
        }

	    public void DumpXml(XmlWriter writer)
        {
            writer.WriteStartElement("nsset");
            writer.WriteAttributeString("index", _index.ToString());
            for (int i = 0; i < Count; ++i)
            {
                writer.WriteStartElement("item");
                writer.WriteAttributeString("index", i.ToString());
                writer.WriteAttributeString("nsindex", this[i].Index.ToString());
                writer.WriteAttributeString("ns", this[i].NameString);
                writer.WriteAttributeString("nskind", this[i].Kind.ToString());
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
        }

	    public override bool Equals(object obj)
        {
            if (obj == this) return true;
            var nss = obj as AbcNamespaceSet;
            if (nss == null) return false;
            return this.EqualsTo(nss);
        }

        public override int GetHashCode()
        {
            return this.EvalHashCode();
        }

        public override string ToString()
        {
            return this.Join("\n");
        }

	    internal void Check()
        {
            if (Index == 0)
                Debugger.Break();
            foreach (var ns in this)
                ns.Check();
        }
    }

    public sealed class AbcNssetPool : ISwfAtom, ISupportXmlDump, IAbcConstPool, IEnumerable<AbcNamespaceSet>
    {
        private readonly AbcFile _abc;
        private readonly AbcConstList<AbcNamespaceSet> _list = new AbcConstList<AbcNamespaceSet>();

        public AbcNssetPool(AbcFile abc)
	    {
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
                Add(new AbcNamespaceSet(reader));
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