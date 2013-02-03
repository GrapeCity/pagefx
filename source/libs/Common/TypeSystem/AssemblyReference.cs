using System;
using System.Globalization;
using System.Text;

namespace DataDynamics.PageFX.Common.TypeSystem
{
    /// <summary>
    /// Assembly reference
    /// </summary>
    public class AssemblyReference : CustomAttributeProvider, IAssemblyReference
    {
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

		public HashAlgorithmId HashAlgorithm { get; set; }

    	public string FullName
        {
            get { return this.BuildFullName(); }
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
                    	_culture = cultureName.ToLower() == "neutral" ? CultureInfo.InvariantCulture : new CultureInfo(cultureName);
                    }
                	if (pair[0] == "PublicKeyToken")
                    {
                        string keyToken = pair[1].Trim().ToLower();
						if (keyToken == "null")
						{
							PublicKeyToken = null;
						}
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

	    /// <summary>
        /// RTFM
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
				return true;

            var r = obj as IAssemblyReference;
            if (r == null) return false;

		    return this.EqualsTo(r);
        }

        public override int GetHashCode()
        {
	        return this.EvalHashCode();
        }

        public override string ToString()
        {
            return FullName;
        }

	    public string ToString(string format, IFormatProvider formatProvider)
        {
            return FullName;
        }
    }

	public static class AssemblyRefExtensions
	{
		public static bool EqualsTo(this IAssemblyReference x, IAssemblyReference y)
		{
			if (ReferenceEquals(x, y))
				return true;

			if (x.Name != y.Name)
				return false;

			if (x.Culture != y.Culture)
				return false;

			if (!Equals(x.Version, y.Version))
				return false;

			if (x.PublicKeyToken == null || x.PublicKeyToken.Length == 0)
				return y.PublicKeyToken == null || y.PublicKeyToken.Length == 0;

			if (y.PublicKeyToken == null || x.PublicKeyToken.Length == 0)
				return false;

			int n = x.PublicKeyToken.Length;
			if (n != y.PublicKeyToken.Length)
				return false;

			for (int i = 0; i < n; i++)
			{
				if (x.PublicKeyToken[i] != y.PublicKeyToken[i])
					return false;
			}

			return true;
		}

		public static int EvalHashCode(this IAssemblyReference r)
		{
			int hash = r.Name.GetHashCode();
			hash ^= r.Version.GetHashCode();

			if (r.Culture != null)
				hash ^= r.Culture.GetHashCode();

			if (r.PublicKeyToken != null)
			{
				int n = r.PublicKeyToken.Length;
				hash ^= n;
				for (int i = 0; i < n; i++)
					hash ^= r.PublicKeyToken[i];
			}

			return hash;
		}

		public static string BuildFullName(this IAssemblyReference r)
		{
			var s = new StringBuilder();
			s.Append(r.Name);
			s.Append(',');
			s.Append(" Version=");
			s.Append(r.Version.Major);
			s.Append('.');
			s.Append(r.Version.Minor);
			s.Append('.');
			s.Append(r.Version.Build);
			s.Append('.');
			s.Append(r.Version.Revision);
			s.Append(", Culture=");
			s.Append(ReferenceEquals(r.Culture, CultureInfo.InvariantCulture) ? "neutral" : r.Culture.Name);
			if (r.PublicKeyToken != null && r.PublicKeyToken.Length > 0)
			{
				s.Append(", PublicKeyToken=");
				for (int i = 0; i < r.PublicKeyToken.Length; i++)
					s.Append(r.PublicKeyToken[i].ToString("x2"));
			}
			return s.ToString();
		}
	}
}