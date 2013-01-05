using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using DataDynamics.PageFX.Common.Extensions;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.FlashLand.Swf;

namespace DataDynamics.PageFX.FlashLand.Abc
{
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
            Kind = 0;
        }

        public AbcMultiname(AbcConstKind kind)
        {
            Kind = kind;
        }

        public AbcMultiname(AbcConstKind kind, AbcNamespace ns, AbcConst<string> name)
        {
            Kind = kind;
            Namespace = ns;
            Name = name;
        }

        public AbcMultiname(AbcConstKind kind, AbcNamespaceSet nss, AbcConst<string> name)
        {
            Kind = kind;
            NamespaceSet = nss;
            Name = name;
        }

        public AbcMultiname(AbcConstKind kind, AbcConst<string> name)
        {
            Kind = kind;
            Name = name;
        }

        public AbcMultiname(AbcConstKind kind, AbcNamespaceSet nss)
        {
            Kind = kind;
            NamespaceSet = nss;
        }

        public AbcMultiname(AbcMultiname type, AbcMultiname param)
        {
            if (type == null)
                throw new ArgumentNullException("type");
            if (param == null)
                throw new ArgumentNullException("param");
            Kind = AbcConstKind.TypeName;
            Type = type;
            TypeParameter = param;
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

    	public AbcConstKind Kind { get; set; }

    	public bool IsAny
        {
            get { return Index == 0; }
        }

        public bool IsRuntime
        {
            get
            {
                switch (Kind)
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
                switch (Kind)
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

    	public AbcNamespace Namespace { get; set; }

    	public AbcNamespaceSet NamespaceSet { get; set; }

    	public string NamespaceString
        {
            get
            {
                if (Type != null)
                    return Type.NamespaceString;
                if (Namespace != null)
                    return Namespace.NameString;
                if (NamespaceSet != null && NamespaceSet.Count > 0)
                    return NamespaceSet[0].Name.Value;
                return string.Empty;
            }
        }

        public Visibility Visibility
        {
            get
            {
                if (Type != null)
                    return Type.Visibility;
                if (Namespace != null)
                    return Namespace.Visibility;
                return Visibility.Private;
            }
        }

        #region Name

		public AbcConst<string> Name { get; set; }

		public string NameString
        {
            get
            {
                if (Type != null)
                    return Type.NameString;
                if (Name != null)
                    return Name.Value;
                return "";
            }
        }

        public string FullName
        {
            get
            {
                if (IsParameterizedType)
                {
                    if (Type != null)
                    {
                        string s = Type.FullName;
                        if (TypeParameter != null)
                            s += "$" + TypeParameter.FullName;
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
            if (NamespaceSet != null)
            {
                string name = NameString;
                foreach (var ns in NamespaceSet)
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
            get { return Namespace != null && Namespace.IsGlobalPackage; }
        }

        public bool IsGlobalName(string name)
        {
            if (Namespace == null || Name == null)
                return false;
            if (!Namespace.IsGlobalPackage)
                return false;
            return Name.Value == name;
        }

        /// <summary>
        /// Determines whether the name is global::Object (name of native object type).
        /// </summary>
        public bool IsObject
        {
            get { return IsGlobalName("Object"); }
        }

        #region ParameterizedType

		public AbcMultiname Type { get; set; }

		public AbcMultiname TypeParameter { get; set; }

		public bool IsParameterizedType
        {
            get { return Kind == AbcConstKind.TypeName; }
        }
        #endregion
        #endregion

        #region Key
        public static string KeyOf(AbcConstKind kind, AbcNamespace ns, AbcConst<string> name)
        {
            return ns.Key + "." + name.Value + (int)kind;
        }

        public static string KeyOf(AbcConstKind kind, AbcNamespaceSet nss, AbcConst<string> name)
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
                    switch (Kind)
                    {
                        case AbcConstKind.QName:
                        case AbcConstKind.QNameA:
                            key = KeyOf(Kind, Namespace, Name);
                            break;

                        case AbcConstKind.RTQNameL:
                        case AbcConstKind.RTQNameLA:
                            key = ((int)Kind).ToString();
                            break;

                        case AbcConstKind.RTQName:
                        case AbcConstKind.RTQNameA:
                            key = Name.Value + ((int)Kind);
                            break;

                        case AbcConstKind.Multiname:
                        case AbcConstKind.MultinameA:
                            key = KeyOf(Kind, NamespaceSet, Name);
                            break;

                        case AbcConstKind.MultinameL:
                        case AbcConstKind.MultinameLA:
                            key = KeyOf(Kind, NamespaceSet);
                            break;

                        case AbcConstKind.TypeName:
                            key = Type.Key + "<" + TypeParameter.Key + ">";
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
            switch (Kind)
            {
                case AbcConstKind.QName:
                case AbcConstKind.QNameA:
                    Namespace.Check();
                    CheckConst(Name);
                    break;

                case AbcConstKind.RTQNameL:
                case AbcConstKind.RTQNameLA:
                    break;

                case AbcConstKind.RTQName:
                case AbcConstKind.RTQNameA:
                    CheckConst(Name);
                    break;

                case AbcConstKind.Multiname:
                case AbcConstKind.MultinameA:
                    NamespaceSet.Check();
                    CheckConst(Name);
                    break;

                case AbcConstKind.MultinameL:
                case AbcConstKind.MultinameLA:
                    NamespaceSet.Check();
                    break;

                case AbcConstKind.TypeName:
                    Type.Check();
                    TypeParameter.Check();
                    break;
            }
        }
        #endregion

        #region IO
        public void Read(SwfReader reader)
        {
            Kind = (AbcConstKind)reader.ReadUInt8();
            switch (Kind)
            {
                    //U30 ns_index
                    //U30 name_index
                case AbcConstKind.QName:
                case AbcConstKind.QNameA:
                    Namespace = reader.ReadAbcNamespace();
                    Name = reader.ReadAbcString();
                    break;

                    //U30 name_index
                case AbcConstKind.RTQName:
                case AbcConstKind.RTQNameA:
                    Name = reader.ReadAbcString();
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
                        Name = reader.ReadAbcString();

                        int index = (int)reader.ReadUIntEncoded(); //ns_set
                        if (index == 0)
                            throw new BadImageFormatException("The value of ns_set cannot be zero.");
                        NamespaceSet = reader.ABC.NamespaceSets[index];
                    }
                    break;

                    //U30 ns_set_index
                case AbcConstKind.MultinameL:
                case AbcConstKind.MultinameLA:
                    {
                        int index = (int)reader.ReadUIntEncoded(); //ns_set
                        if (index == 0)
                            throw new BadImageFormatException("The value of ns_set cannot be zero.");
                        NamespaceSet = reader.ABC.NamespaceSets[index];
                    }
                    break;

                case AbcConstKind.TypeName:
                    {
                        int typeIndex = (int)reader.ReadUIntEncoded();

                        if (typeIndex == 0 || typeIndex >= reader.MultinameCount)
                            throw new BadImageFormatException(string.Format("TypeIndex {0} is out of range", typeIndex));

                        Type = reader.ABC.Multinames[typeIndex];

                        int one = (int)reader.ReadUIntEncoded();
                        if (one != 1)
                            throw new BadImageFormatException("TypeName constant must have 1 after type");

                        //NOTE: In Tamarin AbcParser does not check paramIndex
                        //Therefore param multiname can be not read yet.
                        int paramIndex = (int)reader.ReadUIntEncoded();
                        TypeParameter = new AbcMultiname {Index = paramIndex};
                    }
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Write(SwfWriter writer)
        {
            writer.WriteUInt8((byte)Kind);
            switch(Kind)
            {
                case AbcConstKind.QName:
                case AbcConstKind.QNameA:
                    {
                        writer.WriteUIntEncoded((uint)Namespace.Index);
                        writer.WriteUIntEncoded((uint)Name.Index);
                    }
                    break;

                case AbcConstKind.RTQName:
                case AbcConstKind.RTQNameA:
                    {
                        writer.WriteUIntEncoded((uint)Name.Index);
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
                        writer.WriteUIntEncoded((uint)Name.Index);
                        writer.WriteUIntEncoded((uint)NamespaceSet.Index);
                    }
                    break;

                case AbcConstKind.MultinameL:
                case AbcConstKind.MultinameLA:
                    {
                        writer.WriteUIntEncoded((uint)NamespaceSet.Index);
                    }
                    break;

                case AbcConstKind.TypeName:
                    {
                        writer.WriteUIntEncoded((uint)Type.Index);
                        writer.WriteByte(1);
                        writer.WriteUIntEncoded((uint)TypeParameter.Index);
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
			return new object[] { Namespace, NamespaceSet, Name }.EvalHashCode() ^ (int)Kind;
        }

        private bool HasNamespace(AbcNamespace ns)
        {
            if (NamespaceSet == null) return false;
            return ((IEnumerable<AbcNamespace>)NamespaceSet).Contains(ns);
        }

        public bool IsQName
        {
            get { return Kind == AbcConstKind.QName || Kind == AbcConstKind.QNameA; }
        }

        public bool IsMultiname
        {
            get { return Kind == AbcConstKind.Multiname || Kind == AbcConstKind.MultinameA; }
        }

        public override bool Equals(object obj)
        {
            if (obj == this) return true;

            var mn = obj as AbcMultiname;
            if (mn != null)
            {
                if (mn.Kind == Kind)
                {
	                return Equals(mn.Name, Name)
	                       && Equals(mn.Namespace, Namespace)
	                       && Equals(mn.NamespaceSet, NamespaceSet);
                }
                if (IsQName && mn.IsMultiname)
                {
	                return Equals(mn.Name, Name) && mn.HasNamespace(Namespace);
                }
	            if (mn.IsQName && IsMultiname)
	            {
		            return Equals(mn.Name, Name) && HasNamespace(mn.Namespace);
	            }
	            return false;
            }

            var s = obj as string;
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
            if (Kind == 0) return "*";
            var sb = new StringBuilder();
            if (format == "s")
            {
                string fullname = FullName;
            	sb.Append(string.IsNullOrEmpty(fullname) ? Kind.ToString() : fullname);
            	return sb.ToString();
            }

            if (IsQName)
            {
                sb.Append(FullName);
                return sb.ToString();
            }

            sb.Append(Kind.ToString());
            sb.AppendFormat("[{0}] ", _index);
            sb.Append("{");

            if (IsParameterizedType)
            {
                if (Type != null)
                {
                    sb.Append("type = ");
                    sb.Append(Type.ToString());
                    if (TypeParameter != null)
                    {
                        sb.Append(", param = ");
                        sb.Append(TypeParameter.ToString());
                    }
                }
            }
            else
            {
                if (NamespaceSet != null)
                {
                    sb.Append("nsset[");
                    sb.Append(NamespaceSet.Index);
                    sb.Append("]{");
                    int n = NamespaceSet.Count;
                    for (int i = 0; i < n; ++i)
                    {
                        if (i > 0) sb.Append("; ");
                        sb.Append(NamespaceSet[i].ToString("f"));
                    }
                    sb.Append("} ");
                }
                else if (Namespace != null)
                {
                    sb.Append("ns[");
                    sb.Append(Namespace.Index);
                    sb.Append("]{");
                    sb.Append(Namespace.ToString("f"));
                    sb.Append("} ");
                }
                if (Name != null)
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
            writer.WriteAttributeString("kind", Kind.ToString());
            if (IsParameterizedType)
            {
                if (Type != null)
                    writer.WriteAttributeString("type", Type.ToString());
                if (TypeParameter != null)
                    writer.WriteAttributeString("param", TypeParameter.ToString());
            }
            else
            {
                if (Name != null)
                    writer.WriteAttributeString("name", Name.Value);
                if (Namespace != null)
                {
                    writer.WriteAttributeString("ns", Namespace.NameString);
                    writer.WriteAttributeString("nskind", Namespace.Kind.ToString());
                    writer.WriteAttributeString("nsindex", Namespace.Index.ToString());
                }
                if (NamespaceSet != null)
                {
                    writer.WriteAttributeString("nsset", NamespaceSet.Index.ToString());
                    //_nsset.DumpXml(writer);
                }
            }
            writer.WriteEndElement();
        }
        #endregion

        #region Utils
        public bool Equals(string fullname)
        {
            if (NamespaceSet != null) return false;
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
            get { return !IsRuntime && HasGlobalPackage && GlobalTypes.Contains(NameString); }
        }

    	private static readonly HashSet<string> GlobalTypes = new HashSet<string>(
    		new[]
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
    				"VerifyError"
    			});

    	#endregion
    }
}