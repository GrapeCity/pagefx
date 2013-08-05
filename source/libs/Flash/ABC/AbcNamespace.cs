using System;
using System.Diagnostics;
using System.Xml;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.FlashLand.Swf;

namespace DataDynamics.PageFX.FlashLand.Abc
{
	public sealed class AbcNamespace : IAbcConst
    {
		public AbcNamespace()
        {
        }

        public AbcNamespace(AbcConst<string> name, AbcConstKind kind)
            : this(name, kind, name.MakeKey(kind))
        {
        }

        public AbcNamespace(AbcConst<string> name, AbcConstKind kind, string key)
        {
            if (!kind.IsNamespace())
                throw new ArgumentException("Invalid namespace kind");
            Name = name;
            _kind = kind;
            _key = key;
        }

		/// <summary>
        /// Gets or sets index of this namespace whithin namespace pool in ABC file.
        /// </summary>
        public int Index
        {
            get { return _index; }
            set { _index = value; }
        }
        private int _index = -1;

        object IAbcConst.Value
        {
            get { return Name; }
            set { throw new NotSupportedException(); }
        }

        /// <summary>
        /// Gets or sets namespace kind.
        /// </summary>
        public AbcConstKind Kind
        {
            get { return _kind; }
            set
            {
                if (!value.IsNamespace())
                    throw new ArgumentException("Invalid namespace kind");
                _kind = value;
            }
        }
        private AbcConstKind _kind;

    	/// <summary>
    	/// Gets or sets namespace name.
    	/// </summary>
    	public AbcConst<string> Name { get; set; }

    	public int NameIndex
        {
            get { return Name != null ? Name.Index : 0; }
        }

        public string NameString
        {
            get { return Name != null ? Name.Value : ""; }
        }

        /// <summary>
        /// Gets the visibility of this namespace.
        /// </summary>
        public Visibility Visibility
        {
            get { return Kind.Visibility(); }
        }

        public bool IsGlobalPackage
        {
            get { return _kind == AbcConstKind.PackageNamespace && string.IsNullOrEmpty(NameString); }
        }

    	public string Key
        {
            get { return _key ?? (_key = Name.MakeKey(_kind)); }
			set { _key = value; }
        }
        private string _key;

        public bool IsAny
        {
            get { return Index == 0; }
        }

	    public void Read(SwfReader reader)
        {
            _kind = (AbcConstKind)reader.ReadUInt8();
            if (!_kind.IsNamespace())
                throw new BadImageFormatException("Invalid namespace kind");
            Name = reader.ReadAbcString();
        }

        public void Write(SwfWriter writer)
        {
            if (_kind == 0)
                throw new InvalidOperationException("Invalid namespace kind");
            writer.WriteUInt8((byte)_kind);
            writer.WriteUIntEncoded((uint)Name.Index);
        }

		public static string GetShortNsKind(AbcConstKind kind)
        {
            switch (kind)
            {
                case AbcConstKind.PrivateNamespace:
                    return "private";
                case AbcConstKind.PublicNamespace:
                    return "public";
                case AbcConstKind.PackageNamespace:
                    return "package";
                case AbcConstKind.InternalNamespace:
                    return "internal";
                case AbcConstKind.ProtectedNamespace:
                    return "protected";
                case AbcConstKind.ExplicitNamespace:
                    return "explicit";
                case AbcConstKind.StaticProtectedNamespace:
                    return "static protected";
            }
            return "";
        }

        public static AbcConstKind FromShortNsKind(string kind)
        {
            switch (kind)
            {
                case "private":
                    return AbcConstKind.PrivateNamespace;
                case "public":
                    return AbcConstKind.PublicNamespace;
                case "package":
                    return AbcConstKind.PackageNamespace;
                case "internal":
                    return AbcConstKind.InternalNamespace;
                case "protected":
                    return AbcConstKind.ProtectedNamespace;
                case "explicit":
                    return AbcConstKind.ExplicitNamespace;
                case "static protected":
                    return AbcConstKind.StaticProtectedNamespace;
                default:
                    throw new ArgumentOutOfRangeException("kind");
            }
        }

        public override string ToString()
        {
            return ToString("f");
        }

        public string ToString(string format)
        {
            if (IsGlobalPackage) return "global";
            if (format == "s")
            {
                if (Name != null)
                    return Name.ToString();
                return "";
            }
            return string.Format("[{0}] {1} {2}", Index, GetShortNsKind(_kind), Name);
        }

        public override int GetHashCode()
        {
        	int h = (int)_kind;
			if (Name != null)
			{
				h ^= Name.GetHashCode();
			}
        	return h;
        }

        public override bool Equals(object obj)
        {
            if (obj == this) return true;
            var ns = obj as AbcNamespace;
            if (ns != null)
            {
                if (ns._kind != _kind) return false;
                if (!Equals(ns.Name, Name)) return false;
                return true;
            }
            string s = obj as string;
            if (s != null)
                return NameString == s;
            return false;
        }

		public void DumpXml(XmlWriter writer)
        {
            writer.WriteStartElement("namespace");
            writer.WriteAttributeString("index", Index.ToString());
            writer.WriteAttributeString("name", NameString);
            writer.WriteAttributeString("kind", Kind.ToString());
            writer.WriteAttributeString("name-index", NameIndex.ToString());
            writer.WriteEndElement();
        }

		internal void Check()
        {
            if (Index == 0)
                Debugger.Break();
            if (Name.Index == 0)
                Debugger.Break();
        }
    }
}