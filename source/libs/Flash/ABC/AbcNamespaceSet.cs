using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;
using DataDynamics.PageFX.Common.Collections;
using DataDynamics.PageFX.Common.Extensions;
using DataDynamics.PageFX.FlashLand.Swf;

namespace DataDynamics.PageFX.FlashLand.Abc
{
    /// <summary>
    /// Represents namespace set
    /// </summary>
    public sealed class AbcNamespaceSet : List<AbcNamespace>, IAbcConst, IReadOnlyList<AbcNamespace>
    {
	    public AbcNamespaceSet()
        {
        }

        internal AbcNamespaceSet(IEnumerable<AbcNamespace> collection, string key)
            : base(collection)
        {
            _key = key;
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
}