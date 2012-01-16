using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Xml;

namespace DataDynamics.PageFX.CodeModel
{
    /// <summary>
    /// Assembly reference
    /// </summary>
    public class AssemblyReference : CustomAttributeProvider, IAssemblyReference, IXmlSerializationFeedback
    {
        #region Constructors
        public AssemblyReference()
            : this("Temp", new Version(0, 0, 0, 0), null, null)
        {
        }

        /// <summary>
        /// ctor.
        /// </summary>
        /// <param name="name">assembly Name</param>
        /// <param name="version">Asembly version</param>
        /// <param name="publicKeyToken">Key token for assembly or null if none</param>
        /// <param name="culture">Culture info or null if none</param>
        public AssemblyReference(string name, Version version,
                                byte[] publicKeyToken, CultureInfo culture)
        {
            Name = name;
            Version = version;
            _culture = culture ?? CultureInfo.InvariantCulture;
            PublicKeyToken = publicKeyToken;
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="fullname">Assembly Full name</param>
        public AssemblyReference(string fullname)
        {
            FullName = fullname;
        }

        public AssemblyReference(IAssemblyReference r)
        {
            Name = r.Name;
            Flags = r.Flags;
            Version = r.Version;
            PublicKey = r.PublicKey;
            PublicKeyToken = r.PublicKeyToken;
            Culture = r.Culture;
        }
        #endregion

        #region IAssemblyReference Members

    	/// <summary>
    	/// Assembly Name
    	/// </summary>
    	public string Name { get; set; }

    	/// <summary>
    	/// Gets or sets assembly version
    	/// </summary>
    	public Version Version { get; set; }

    	public AssemblyFlags Flags { get; set; }

    	/// <summary>
        /// Culture Info
        /// </summary>
        public CultureInfo Culture
        {
            get { return _culture ?? CultureInfo.InvariantCulture; }
    		set { _culture = value ?? CultureInfo.InvariantCulture; }
        }
        private CultureInfo _culture;

    	public byte[] PublicKey { get; set; }

    	/// <summary>
    	/// Public key token
    	/// </summary>
    	/// <remarks>Can be null</remarks>
    	public byte[] PublicKeyToken { get; set; }

    	public byte[] HashValue { get; set; }

    	public string FullName
        {
            get
            {
                var s = new StringBuilder();
                s.Append(Name);
                s.Append(',');
                s.Append(" Version=");
                s.Append(Version.Major);
                s.Append('.');
                s.Append(Version.Minor);
                s.Append('.');
                s.Append(Version.Build);
                s.Append('.');
                s.Append(Version.Revision);
                s.Append(", Culture=");
                if (Culture == CultureInfo.InvariantCulture)
                    s.Append("neutral");
                else
                    s.Append(Culture.Name);
                if (PublicKeyToken != null && PublicKeyToken.Length > 0)
                {
                    s.Append(", PublicKeyToken=");
                    for (int i = 0; i < PublicKeyToken.Length; i++)
                        s.Append(PublicKeyToken[i].ToString("x2"));
                }
                return s.ToString();
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                var parts = value.Split(',');
                Name = parts[0].Trim();
                Version = new Version(0, 0, 0, 0);
                _culture = CultureInfo.InvariantCulture;

                for (int i = 1; i < parts.Length; i++)
                {
                    var pair = parts[i].Split('=');
                    pair[0] = pair[0].Trim();
                    if (pair[0] == "Version")
                        Version = new Version(pair[1].Trim());
                    if (pair[0] == "Culture")
                    {
                        string cultureName = pair[1].Trim();
                        if (cultureName.ToLower() == "neutral")
                            _culture = CultureInfo.InvariantCulture;
                        else
                            _culture = new CultureInfo(cultureName);
                    }
                    if (pair[0] == "PublicKeyToken")
                    {
                        string keyToken = pair[1].Trim().ToLower();
                        if (keyToken == "null")
                            PublicKeyToken = null;
                        else
                        {
                            PublicKeyToken = new byte[keyToken.Length / 2];
                            for (int j = 0; j < PublicKeyToken.Length; j++)
                            {
                                string byteStr = keyToken.Substring(j * 2, 2);
                                PublicKeyToken[j] = byte.Parse(byteStr, NumberStyles.HexNumber);
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region Object Override Members
        /// <summary>
        /// RTFM
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj)) return true;
            var r = obj as IAssemblyReference;
            if (r == null) 
                return false;

            if (Name != r.Name) 
                return false;

            if (Culture != r.Culture) 
                return false;

            if (!Equals(Version, r.Version)) 
                return false;

            if (PublicKeyToken == null)
                return r.PublicKeyToken == null || r.PublicKeyToken.Length == 0;

            int n = PublicKeyToken.Length;
            if (r.PublicKeyToken == null)
                return n == 0;

            if (n != r.PublicKeyToken.Length)
                return false;

            for (int i = 0; i < n; i++)
                if (r.PublicKeyToken[i] != PublicKeyToken[i])
                    return false;

            return true;
        }

        public override int GetHashCode()
        {
            int hash = Name.GetHashCode();
            hash ^= Version.GetHashCode();
            if (Culture != null)
                hash ^= Culture.GetHashCode();
            if (PublicKeyToken != null)
            {
                int n = PublicKeyToken.Length;
                hash ^= n;
                for (int i = 0; i < n; i++)
                    hash ^= PublicKeyToken[i];
            }
            return hash;
        }

        public override string ToString()
        {
            return FullName;
        }
        #endregion

        #region ICodeNode Members

        public CodeNodeType NodeType
        {
            get { return CodeNodeType.AssemblyReference; }
        }

        public virtual IEnumerable<ICodeNode> ChildNodes
        {
            get { return null; }
        }

    	/// <summary>
    	/// Gets or sets user defined data assotiated with this object.
    	/// </summary>
    	public object Tag { get; set; }

    	#endregion

        #region IFormattable Members
        public string ToString(string format, IFormatProvider formatProvider)
        {
            return FullName;
        }
        #endregion

        #region IXmlSerializationFeedback Members
        string IXmlSerializationFeedback.XmlElementName
        {
            get { return null; }
        }

        void IXmlSerializationFeedback.WriteProperties(XmlWriter writer)
        {
            writer.WriteElementString("Name", Name);
            writer.WriteElementString("FullName", FullName);
            writer.WriteElementString("Version", Version.ToString());
            //TODO:
        }
        #endregion
    }
}