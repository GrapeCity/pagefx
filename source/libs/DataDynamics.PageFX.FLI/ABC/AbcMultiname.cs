using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml;
using DataDynamics.PageFX.CodeModel;
using DataDynamics.PageFX.FLI.SWF;

namespace DataDynamics.PageFX.FLI.ABC
{
    using AbcString = AbcConst<string>;

    /// <summary>
    /// Represents multiname - a combination of an unqualified name and one or more namespaces.
    /// </summary>
    public class AbcMultiname : IAbcConst
    {
        #region Constructors
        /// <summary>
        /// Initializes new instance of <see cref="AbcMultiname"/> class.
        /// </summary>
        public AbcMultiname()
        {
            _kind = 0;
        }

        public AbcMultiname(AbcConstKind kind)
        {
            _kind = kind;
        }

        public AbcMultiname(AbcConstKind kind, AbcNamespace ns, AbcString name)
        {
            _kind = kind;
            _ns = ns;
            _name = name;
        }

        public AbcMultiname(AbcConstKind kind, AbcNamespaceSet nss, AbcString name)
        {
            _kind = kind;
            _nsset = nss;
            _name = name;
        }

        public AbcMultiname(AbcConstKind kind, AbcString name)
        {
            _kind = kind;
            _name = name;
        }

        public AbcMultiname(AbcConstKind kind, AbcNamespaceSet nss)
        {
            _kind = kind;
            _nsset = nss;
        }

        public AbcMultiname(AbcMultiname type, AbcMultiname param)
        {
            if (type == null)
                throw new ArgumentNullException("type");
            if (param == null)
                throw new ArgumentNullException("param");
            _kind = AbcConstKind.TypeName;
            _type = type;
            _typeParam = param;
        }

        internal AbcMultiname(SwfReader reader)
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

        public AbcConstKind Kind
        {
            get { return _kind; }
            set { _kind = value; }
        }
        AbcConstKind _kind;

        public bool IsAny
        {
            get { return Index == 0; }
        }

        public bool IsRuntime
        {
            get
            {
                switch (_kind)
                {
                    case AbcConstKind.RTQName:
                    case AbcConstKind.RTQNameA:
                    case AbcConstKind.RTQNameL:
                    case AbcConstKind.RTQNameLA:
                    case AbcConstKind.MultinameL:
                    case AbcConstKind.MultinameLA:
                        return true;
                }
                return false;
            }
        }

        object IAbcConst.Value
        {
            get { return Name; }
            set { throw new NotSupportedException(); }
        }

        /// <summary>
        /// Gets number of pop operations required for this multiname.
        /// </summary>
        public int StackPop
        {
            get
            {
                switch (_kind)
                {
                        //U30 name_index
                    case AbcConstKind.RTQName:
                    case AbcConstKind.RTQNameA:
                    case AbcConstKind.MultinameL:
                    case AbcConstKind.MultinameLA:
                        return 1;

                    case AbcConstKind.RTQNameL:
                    case AbcConstKind.RTQNameLA:
                        return 2;
                }
                return 0;
            }
        }

        public AbcNamespace Namespace
        {
            get { return _ns; }
            set { _ns = value; }
        }
        AbcNamespace _ns;

        public AbcNamespaceSet NamespaceSet
        {
            get { return _nsset; }
            set { _nsset = value; }
        }
        AbcNamespaceSet _nsset;

        public string NamespaceString
        {
            get
            {
                if (_type != null)
                    return _type.NamespaceString;
                if (_ns != null)
                    return _ns.NameString;
                if (_nsset != null && _nsset.Count > 0)
                    return _nsset[0].Name.Value;
                return string.Empty;
            }
        }

        public Visibility Visibility
        {
            get
            {
                if (_type != null)
                    return _type.Visibility;
                if (_ns != null)
                    return _ns.Visibility;
                return Visibility.Private;
            }
        }

        #region Name
        public AbcString Name
        {
            get { return _name; }
            set { _name = value; }
        }
        AbcString _name;

        public string NameString
        {
            get
            {
                if (_type != null)
                    return _type.NameString;
                if (_name != null)
                    return _name.Value;
                return "";
            }
        }

        public string FullName
        {
            get
            {
                if (IsParameterizedType)
                {
                    if (_type != null)
                    {
                        string s = _type.FullName;
                        if (_typeParam != null)
                            s += "$" + _typeParam.FullName;
                        return s;
                    }
                    return "";
                }
                string ns = NamespaceString;
                if (string.IsNullOrEmpty(ns))
                    return NameString;
                return ns + "." + NameString;
            }
        }

        public IEnumerable<string> GetFullNames()
        {
            if (_nsset != null)
            {
                string name = NameString;
                foreach (var ns in _nsset)
                {
                    yield return MakeFullName(ns.NameString, name);
                }
            }
            else
            {
                yield return FullName;
            }
        }
        #endregion

        public AbcFile ABC { get; set; }

        public bool HasGlobalPackage
        {
            get { return _ns != null && _ns.IsGlobalPackage; }
        }

        public bool IsGlobalName(string name)
        {
            if (_ns == null || _name == null)
                return false;
            if (!_ns.IsGlobalPackage)
                return false;
            return _name.Value == name;
        }

        /// <summary>
        /// Determines whether the name is global::Object (name of native object type).
        /// </summary>
        public bool IsObject
        {
            get
            {
                return IsGlobalName("Object");
            }
        }

        #region ParameterizedType
        public AbcMultiname Type
        {
            get { return _type; }
            set { _type = value; }
        }
        AbcMultiname _type;

        public AbcMultiname TypeParameter
        {
            get { return _typeParam; }
            set { _typeParam = value; }
        }
        AbcMultiname _typeParam;

        public bool IsParameterizedType
        {
            get { return _kind == AbcConstKind.TypeName; }
        }
        #endregion
        #endregion

        #region Key
        public static string KeyOf(AbcConstKind kind, AbcNamespace ns, AbcString name)
        {
            return ns.Key + "." + name.Value + (int)kind;
        }

        public static string KeyOf(AbcConstKind kind, AbcNamespaceSet nss, AbcString name)
        {
            return "{" + nss.Key + "}." + name.Value + (int)kind;
        }

        public static string KeyOf(AbcConstKind kind, AbcNamespaceSet nss)
        {
            return nss.Key + (int)kind;
        }

        public string Key
        {
            get
            {
                if (key == null)
                {
                    switch (_kind)
                    {
                        case AbcConstKind.QName:
                        case AbcConstKind.QNameA:
                            key = KeyOf(_kind, _ns, _name);
                            break;

                        case AbcConstKind.RTQNameL:
                        case AbcConstKind.RTQNameLA:
                            key = ((int)_kind).ToString();
                            break;

                        case AbcConstKind.RTQName:
                        case AbcConstKind.RTQNameA:
                            key = _name.Value + ((int)_kind);
                            break;

                        case AbcConstKind.Multiname:
                        case AbcConstKind.MultinameA:
                            key = KeyOf(_kind, _nsset, _name);
                            break;

                        case AbcConstKind.MultinameL:
                        case AbcConstKind.MultinameLA:
                            key = KeyOf(_kind, _nsset);
                            break;

                        case AbcConstKind.TypeName:
                            key = _type.Key + "<" + _typeParam.Key + ">";
                            break;
                    }
                }
                return key;
            }
        }
        internal string key;

        internal static void CheckConst(IAbcConst c)
        {
            //if (c.Index == 0)
            //    Debugger.Break();
        }

        internal void Check()
        {
            switch (_kind)
            {
                case AbcConstKind.QName:
                case AbcConstKind.QNameA:
                    _ns.Check();
                    CheckConst(_name);
                    break;

                case AbcConstKind.RTQNameL:
                case AbcConstKind.RTQNameLA:
                    break;

                case AbcConstKind.RTQName:
                case AbcConstKind.RTQNameA:
                    CheckConst(_name);
                    break;

                case AbcConstKind.Multiname:
                case AbcConstKind.MultinameA:
                    _nsset.Check();
                    CheckConst(_name);
                    break;

                case AbcConstKind.MultinameL:
                case AbcConstKind.MultinameLA:
                    _nsset.Check();
                    break;

                case AbcConstKind.TypeName:
                    _type.Check();
                    _typeParam.Check();
                    break;
            }
        }
        #endregion

        #region IO
        public void Read(SwfReader reader)
        {
            _kind = (AbcConstKind)reader.ReadUInt8();
            switch (_kind)
            {
                    //U30 ns_index
                    //U30 name_index
                case AbcConstKind.QName:
                case AbcConstKind.QNameA:
                    _ns = AbcIO.ReadNamespace(reader);
                    _name = AbcIO.ReadString(reader);
                    break;

                    //U30 name_index
                case AbcConstKind.RTQName:
                case AbcConstKind.RTQNameA:
                    _name = AbcIO.ReadString(reader);
                    break;

                case AbcConstKind.RTQNameL:
                case AbcConstKind.RTQNameLA:
                    //This kind has no associated data.
                    break;

                    //U30 name_index
                    //U30 ns_set_index
                case AbcConstKind.Multiname:
                case AbcConstKind.MultinameA:
                    {
                        _name = AbcIO.ReadString(reader);

                        int index = (int)reader.ReadUIntEncoded(); //ns_set
                        if (index == 0)
                            throw new BadFormatException("The value of ns_set cannot be zero.");
                        _nsset = reader.ABC.NamespaceSets[index];
                    }
                    break;

                    //U30 ns_set_index
                case AbcConstKind.MultinameL:
                case AbcConstKind.MultinameLA:
                    {
                        int index = (int)reader.ReadUIntEncoded(); //ns_set
                        if (index == 0)
                            throw new BadFormatException("The value of ns_set cannot be zero.");
                        _nsset = reader.ABC.NamespaceSets[index];
                    }
                    break;

                case AbcConstKind.TypeName:
                    {
                        int typeIndex = (int)reader.ReadUIntEncoded();

                        if (typeIndex == 0 || typeIndex >= reader.MultinameCount)
                            throw new BadFormatException(string.Format("TypeIndex {0} is out of range", typeIndex));

                        _type = reader.ABC.Multinames[typeIndex];

                        int one = (int)reader.ReadUIntEncoded();
                        if (one != 1)
                            throw new BadFormatException("TypeName constant must have 1 after type");

                        //NOTE: In Tamarin AbcParser does not check paramIndex
                        //Therefore param multiname can be not read yet.
                        int paramIndex = (int)reader.ReadUIntEncoded();
                        _typeParam = new AbcMultiname {Index = paramIndex};
                    }
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Write(SwfWriter writer)
        {
            writer.WriteUInt8((byte)_kind);
            switch(_kind)
            {
                case AbcConstKind.QName:
                case AbcConstKind.QNameA:
                    {
                        writer.WriteUIntEncoded((uint)_ns.Index);
                        writer.WriteUIntEncoded((uint)_name.Index);
                    }
                    break;

                case AbcConstKind.RTQName:
                case AbcConstKind.RTQNameA:
                    {
                        writer.WriteUIntEncoded((uint)_name.Index);
                    }
                    break;

                case AbcConstKind.RTQNameL:
                case AbcConstKind.RTQNameLA:
                    {
                        //This kind has no associated data.
                    }
                    break;

                case AbcConstKind.Multiname:
                case AbcConstKind.MultinameA:
                    {
                        writer.WriteUIntEncoded((uint)_name.Index);
                        writer.WriteUIntEncoded((uint)_nsset.Index);
                    }
                    break;

                case AbcConstKind.MultinameL:
                case AbcConstKind.MultinameLA:
                    {
                        writer.WriteUIntEncoded((uint)_nsset.Index);
                    }
                    break;

                case AbcConstKind.TypeName:
                    {
                        writer.WriteUIntEncoded((uint)_type.Index);
                        writer.WriteByte(1);
                        writer.WriteUIntEncoded((uint)_typeParam.Index);
                    }
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        #endregion

        #region Object Overrides
        public override int GetHashCode()
        {
            return Algorithms.EvalHashCode(_ns, _nsset, _name) ^ (int)_kind;
        }

        bool HasNamespace(AbcNamespace ns)
        {
            if (_nsset == null) return false;
            return ((IEnumerable<AbcNamespace>)_nsset).Contains(ns);
        }

        public bool IsQName
        {
            get
            {
                return _kind == AbcConstKind.QName || _kind == AbcConstKind.QNameA; 
            }
        }

        public bool IsMultiname
        {
            get
            {
                return _kind == AbcConstKind.Multiname
                    || _kind == AbcConstKind.MultinameA;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == this) return true;
            var mn = obj as AbcMultiname;
            if (mn != null)
            {
                if (mn._kind == _kind)
                {
                    if (!Equals(mn._name, _name)) return false;
                    if (!Equals(mn._ns, _ns)) return false;
                    if (!Equals(mn._nsset, _nsset)) return false;
                    return true;
                }
                if (IsQName && mn.IsMultiname)
                {
                    if (!Equals(mn._name, _name)) return false;
                    return mn.HasNamespace(_ns);
                }
                if (mn.IsQName && IsMultiname)
                {
                    if (!Equals(mn._name, _name)) return false;
                    return HasNamespace(mn._ns);
                }
                return false;
            }
            string s = obj as string;
            return s != null && Equals(s);
        }

        public static string MakeFullName(string ns, string name)
        {
            if (name == null)
                name = string.Empty;
            if (string.IsNullOrEmpty(ns))
                return name;
            if (name.Length > 0)
                return ns + "." + name;
            return ns;
        }

        public string ToString(string format)
        {
            if (_kind == 0) return "*";
            var sb = new StringBuilder();
            if (format == "s")
            {
                string fullname = FullName;
            	sb.Append(string.IsNullOrEmpty(fullname) ? _kind.ToString() : fullname);
            	return sb.ToString();
            }

            if (IsQName)
            {
                sb.Append(FullName);
                return sb.ToString();
            }

            sb.Append(_kind.ToString());
            sb.AppendFormat("[{0}] ", _index);
            sb.Append("{");

            if (IsParameterizedType)
            {
                if (_type != null)
                {
                    sb.Append("type = ");
                    sb.Append(_type.ToString());
                    if (_typeParam != null)
                    {
                        sb.Append(", param = ");
                        sb.Append(_typeParam.ToString());
                    }
                }
            }
            else
            {
                if (_nsset != null)
                {
                    sb.Append("nsset[");
                    sb.Append(_nsset.Index);
                    sb.Append("]{");
                    int n = _nsset.Count;
                    for (int i = 0; i < n; ++i)
                    {
                        if (i > 0) sb.Append("; ");
                        sb.Append(_nsset[i].ToString("f"));
                    }
                    sb.Append("} ");
                }
                else if (_ns != null)
                {
                    sb.Append("ns[");
                    sb.Append(_ns.Index);
                    sb.Append("]{");
                    sb.Append(_ns.ToString("f"));
                    sb.Append("} ");
                }
                if (_name != null)
                {
                    sb.Append("name = ");
                    sb.Append(Name);
                }
            }
            sb.Append("}");

            return sb.ToString();
        }

        public override string ToString()
        {
            return ToString("f");
        }
        #endregion

        #region Dump
        public void DumpXml(XmlWriter writer, string elementName)
        {
            writer.WriteStartElement(elementName);
            writer.WriteAttributeString("index", _index.ToString());
            writer.WriteAttributeString("kind", _kind.ToString());
            if (IsParameterizedType)
            {
                if (_type != null)
                    writer.WriteAttributeString("type", _type.ToString());
                if (_typeParam != null)
                    writer.WriteAttributeString("param", _typeParam.ToString());
            }
            else
            {
                if (_name != null)
                    writer.WriteAttributeString("name", _name.Value);
                if (_ns != null)
                {
                    writer.WriteAttributeString("ns", _ns.NameString);
                    writer.WriteAttributeString("nskind", _ns.Kind.ToString());
                    writer.WriteAttributeString("nsindex", _ns.Index.ToString());
                }
                if (_nsset != null)
                {
                    writer.WriteAttributeString("nsset", _nsset.Index.ToString());
                    //_nsset.DumpXml(writer);
                }
            }
            writer.WriteEndElement();
        }
        #endregion

        #region Utils
        public bool Equals(string fullname)
        {
            if (_nsset != null) return false;
            int i = fullname.LastIndexOf('.');
            if (i >= 0)
            {
                string ns = fullname.Substring(0, i);
                string name = fullname.Substring(i + 1);
                return NamespaceString == ns && NameString == name;
            }
            return NamespaceString == "" && NameString == fullname;
        }
        #endregion
        
        #region IsGlobalType
        public bool IsGlobalType
        {
            get
            {
                if (IsRuntime) return false;
                if (!HasGlobalPackage) return false;
                return GlobalTypes.Contains(NameString);
            }
        }

        static readonly string[] GlobalTypes =
            {
                "void",
                "int",
                "uint",
                "Number",
                "String",
                "Object",
                "Boolean",
                "Array",
                "Date",
                "Class",
                "Function",
                "Math",
                "Namespace",
                "QName",
                "XML",
                "XMLList",
                "RegExp",
                "Error",
                "EvalError",
                "ReferenceError",
                "ArgumentError",
                "DefinitionError",
                "RangeError",
                "SecurityError",
                "SyntaxError",
                "TypeError",
                "URIError",
                "VerifyError",
            };
        #endregion
    }

    #region class AbcMemberName
    public class AbcMemberName
    {
        public AbcMemberName(AbcMultiname type, AbcMultiname name)
        {
            _type = type;
            _name = name;
        }

        public AbcMultiname Type
        {
            get { return _type; }
        }
        readonly AbcMultiname _type;

        public AbcMultiname Name
        {
            get { return _name; }
        }
        readonly AbcMultiname _name;
    }
    #endregion
}