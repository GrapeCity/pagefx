using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml;
using DataDynamics.PageFX.CodeModel;
using DataDynamics.PageFX.FLI.SWF;

namespace DataDynamics.PageFX.FLI.ABC
{
    #region class AbcNamespace
    public class AbcNamespace : IAbcConst
    {
        #region Constructors
        public AbcNamespace()
        {
        }

        public AbcNamespace(AbcConst<string> name, AbcConstKind kind)
            : this(name, kind, KeyOf(name, kind))
        {
        }

        public AbcNamespace(AbcConst<string> name, AbcConstKind kind, string key)
        {
            if (!kind.IsNamespace())
                throw new ArgumentException("Invalid namespace kind");
            _name = name;
            _kind = kind;
            this.key = key;
        }

        public AbcNamespace(SwfReader reader)
        {
            Read(reader);
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets index of this namespace whithin namespace pool in ABC file.
        /// </summary>
        public int Index
        {
            get { return _index; }
            set { _index = value; }
        }
        int _index = -1;

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
        AbcConstKind _kind;

        /// <summary>
        /// Gets or sets namespace name.
        /// </summary>
        public AbcConst<string> Name
        {
            get { return _name; }
            set { _name = value; }
        }
        AbcConst<string> _name;

        public int NameIndex
        {
            get
            {
                if (Name != null)
                    return Name.Index;
                return 0;
            }
        }

        public string NameString

        {
            get
            {
                if (Name != null)
                    return Name.Value;
                return "";
            }
        }

        /// <summary>
        /// Gets the visibility of this namespace.
        /// </summary>
        public Visibility Visibility
        {
            get
            {
                switch (Kind)
                {
                    case AbcConstKind.PrivateNamespace:
                        return Visibility.Private;
                    case AbcConstKind.PublicNamespace:
                        return Visibility.Public;
                    case AbcConstKind.PackageNamespace:
                        return Visibility.Public;
                    case AbcConstKind.InternalNamespace:
                        return Visibility.Internal;
                    case AbcConstKind.ProtectedNamespace:
                        return Visibility.Protected;
                    case AbcConstKind.StaticProtectedNamespace:
                        return Visibility.Protected;
                    default:
                        return Visibility.Private;
                }
            }
        }

        public bool IsGlobalPackage
        {
            get
            {
                if (_kind == AbcConstKind.PackageNamespace)
                    return _name == null || string.IsNullOrEmpty(_name.Value);
                return false;
            }
        }

        public static string KeyOf(AbcConst<string> name, AbcConstKind kind)
        {
            return name.Value + (int)kind;
        }

        public string Key
        {
            get { return key ?? (key = _kind == 0 ? "*" : KeyOf(_name, _kind)); }
        }
        internal string key;

        public bool IsAny
        {
            get { return Index == 0; }
        }
        #endregion

        #region IAbcAtom Members
        public void Read(SwfReader reader)
        {
            _kind = (AbcConstKind)reader.ReadUInt8();
            if (!_kind.IsNamespace())
                throw new BadFormatException("Invalid namespace kind");
            _name = AbcIO.ReadString(reader);
        }

        public void Write(SwfWriter writer)
        {
            if (_kind == 0)
                throw new InvalidOperationException("Invalid namespace kind");
            writer.WriteUInt8((byte)_kind);
            writer.WriteUIntEncoded((uint)_name.Index);
        }
        #endregion

        #region Object Overrides
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
                if (_name != null)
                    return _name.ToString();
                return "";
            }
            return string.Format("[{0}] {1} {2}", Index, GetShortNsKind(_kind), _name);
        }

        public override int GetHashCode()
        {
            return Algorithms.GetHashCode(_kind, _name);
        }

        public override bool Equals(object obj)
        {
            if (obj == this) return true;
            var ns = obj as AbcNamespace;
            if (ns != null)
            {
                if (ns._kind != _kind) return false;
                if (!Equals(ns._name, _name)) return false;
                return true;
            }
            string s = obj as string;
            if (s != null)
                return NameString == s;
            return false;
        }
        #endregion

        #region Dump
        public void DumpXml(XmlWriter writer, string elementName)
        {
            writer.WriteStartElement(elementName);
            writer.WriteAttributeString("index", Index.ToString());
            writer.WriteAttributeString("name", NameString);
            writer.WriteAttributeString("kind", Kind.ToString());
            writer.WriteAttributeString("name-index", NameIndex.ToString());
            writer.WriteEndElement();
        }
        #endregion

        internal void Check()
        {
            if (Index == 0)
                Debugger.Break();
            if (_name.Index == 0)
                Debugger.Break();
        }
    }
    #endregion

    #region class AbcNamespacePool
    public class AbcNamespacePool : ISwfAtom, ISupportXmlDump, IAbcConstPool, IEnumerable<AbcNamespace>
    {
        readonly AbcFile _abc;
        readonly AbcConstList<AbcNamespace> _list = new AbcConstList<AbcNamespace>();
        
        public AbcNamespacePool(AbcFile abc)
        {
            _abc = abc;
            var ns = new AbcNamespace {key = "*"};
            Add(ns);
        }

        #region IAbcAtom Members
        private int _begin;
        private int _end;

        public void Read(SwfReader reader)
        {
            _begin = (int)reader.Position;
            int n = (int)reader.ReadUIntEncoded();
            for (int i = 1; i < n; ++i)
                Add(new AbcNamespace(reader));
            _end = (int)reader.Position;
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

        public string FormatOffset(AbcFile file, int offset)
        {
            var list = new List<AbcNamespace>(this);
            return AbcHelper.FormatOffset(file, offset, list, _begin, _end, "Namespace Pool", true, true);
        }
        #endregion

        #region Dump
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
        #endregion

        #region Public Members
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
                string key = AbcNamespace.KeyOf(name, kind);
                return _list[key];
            }
        }
        #endregion

        #region Object Override Methods
        public override string ToString()
        {
            return Str.ToString<AbcNamespace>(this, "\n");
        }
        #endregion

        #region IAbcConstPool Members
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
            string key = AbcNamespace.KeyOf(name, kind);
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
        #endregion

        #region IEnumerable Members

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

    	#endregion
    }
    #endregion

    #region class AbcNamespaceList
    public class AbcNamespaceList
    {
        private readonly List<string> _names = new List<string>();
        private readonly List<AbcNamespace> _list = new List<AbcNamespace>();
        private readonly Dictionary<string, int> _index = new Dictionary<string, int>();

        public static AbcNamespaceList Global
        {
            get { return _global ?? (_global = new AbcNamespaceList()); }
        }
        private static AbcNamespaceList _global;

        public int Count
        {
            get { return _list.Count; }
        }

        public AbcNamespace this[int index]
        {
            get { return _list[index]; }
        }

        public int IndexOf(string name)
        {
            int v;
            if (_index.TryGetValue(name, out v))
                return v;
            return -1;
        }

        public AbcNamespace this[string name]
        {
            get 
            {
                int i = IndexOf(name);
                if (i >= 0) return _list[i];
                return null;
            }
        }

        public string GetName(int index)
        {
            return _names[index];
        }

        public void Add(string name, AbcNamespace ns)
        {
            if (_index.ContainsKey(name))
                return;
            int i = _list.Count;
            _list.Add(ns);
            _names.Add(name);
            _index.Add(name, i);
        }

        public void Read(SwfReader reader)
        {
            int n = (int)reader.ReadUIntEncoded();
            for (int i = 0; i < n; ++i)
            {
                string name = reader.ReadString();
                var kind = (AbcConstKind)reader.ReadByte();
                string ns = reader.ReadString();
                Add(name, new AbcNamespace(new AbcConst<string>(ns), kind));
            }
        }

        public void Write(SwfWriter writer)
        {
            int n = Count;
            writer.WriteUIntEncoded(n);
            for (int i = 0; i < n; ++i)
            {
                string name = _names[i];
                var ns = _list[i];
                writer.WriteString(name);
                writer.WriteByte((byte)ns.Kind);
                writer.WriteString(ns.NameString);
            }
        }
    }
    #endregion
}