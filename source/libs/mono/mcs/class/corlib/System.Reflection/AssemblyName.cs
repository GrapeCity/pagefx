//CHANGED
//
// System.Reflection/AssemblyName.cs
//
// Authors:
//	Paolo Molaro (lupus@ximian.com)
//	Sebastien Pouliot  <sebastien@ximian.com>
//
// (C) 2001 Ximian, Inc.  http://www.ximian.com
// Portions (C) 2002 Motus Technologies Inc. (http://www.motus.com)
// Copyright (C) 2004-2005 Novell, Inc (http://www.novell.com)
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System.Configuration.Assemblies;
using System.Globalization;
#if NOT_PFX
using System.Runtime.Serialization;
using System.Security.Cryptography;
#endif
using System.Text;
using System.IO;

//using Mono.Security;

namespace System.Reflection {

// References:
// a.	Uniform Resource Identifiers (URI): Generic Syntax
//	http://www.ietf.org/rfc/rfc2396.txt

	[Serializable]
	public sealed class AssemblyName  : ICloneable
    {
		#region Synch with object-internals.h
		string name;
		string codebase;
		int major, minor, build, revision;
		CultureInfo cultureinfo;
		AssemblyNameFlags flags;
		AssemblyHashAlgorithm hashalg;
#if NOT_PFX
		StrongNameKeyPair keypair;
#endif
		byte[] publicKey;
		byte[] keyToken;
		AssemblyVersionCompatibility versioncompat;
		Version version;
#if NET_2_0
		ProcessorArchitecture processor_architecture = ProcessorArchitecture.MSIL;
#else
		int processor_architecture;
#endif
        #endregion
		
		public AssemblyName ()
		{
			// defaults
			versioncompat = AssemblyVersionCompatibility.SameMachine;
		}

#if NET_2_0 || BOOTSTRAP_NET_2_0
		[MethodImpl (MethodImplOptions.InternalCall)]
		static extern bool ParseName (AssemblyName aname, string assemblyName);
		
		public AssemblyName (string assemblyName)
		{
			if (assemblyName == null)
				throw new ArgumentNullException ("assemblyName");
			if (assemblyName.Length < 1)
				throw new ArgumentException ("assemblyName cannot have zero length.");
			
			if (!ParseName (this, assemblyName))
				throw new FileLoadException ("The assembly name is invalid.");
		}
#endif
		
#if NET_2_0
		[MonoTODO ("Not used, as the values are too limited;  Mono supports more")]
		public ProcessorArchitecture ProcessorArchitecture {
			get {
				return processor_architecture;
			}
			set {
				processor_architecture = value;
			}
		}
#endif

		public string Name {
			get { return name; }
			set { name = value; }
		}

		public string CodeBase {
			get { return codebase; }
			set { codebase = value; }
		}

		public string EscapedCodeBase 
        {
			get 
            {
				if (codebase == null)
					return null;
                //TODO:
				//return Uri.EscapeString (codebase, false, true, true);
                return "";
			}
		}

		public CultureInfo CultureInfo {
			get { return cultureinfo; }
			set { cultureinfo = value; }
		}

		public AssemblyNameFlags Flags {
			get { return flags; }
			set { flags = value; }
		}

		public string FullName {
			get {
				if (name == null)
					return null;
				StringBuilder fname = new StringBuilder ();
				fname.Append (name);
				if (Version != null) {
					fname.Append (", Version=");
					fname.Append (Version.ToString ());
				}
				if (cultureinfo != null) {
					fname.Append (", Culture=");
					if (cultureinfo.LCID == CultureInfo.InvariantCulture.LCID)
						fname.Append ("neutral");
					else
						fname.Append (cultureinfo.Name);
				}
				byte[] pub_tok = GetPublicKeyToken ();
				if (pub_tok != null) {
					if (pub_tok.Length == 0)
						fname.Append (", PublicKeyToken=null");
					else {
						fname.Append (", PublicKeyToken=");
						for (int i = 0; i < pub_tok.Length; i++)
							fname.Append (pub_tok[i].ToString ("x2"));
					}
				}
				return fname.ToString ();
			}
		}

		public AssemblyHashAlgorithm HashAlgorithm {
			get { return hashalg; }
			set { hashalg = value; }
		}

#if NOT_PFX
        public StrongNameKeyPair KeyPair
        {
            get { return keypair; }
            set { keypair = value; }
        }
#endif

		public Version Version {
			get {
				return version;
			}

			set {
				version = value;
				if (value == null)
					major = minor = build = revision = 0;
				else {
					major = value.Major;
					minor = value.Minor;
					build = value.Build;
					revision = value.Revision;
				}
			}
		}

		public AssemblyVersionCompatibility VersionCompatibility {
			get { return versioncompat; }
			set { versioncompat = value; }
		}
		
		public override string ToString ()
		{
			string name = FullName;
			return (name != null) ? name : base.ToString ();
		}

		public byte[] GetPublicKey() 
		{
			return publicKey;
			// FIXME: In some cases MS implementation returns 
			// "new byte [0]" instead of null
		}

		public byte[] GetPublicKeyToken() 
		{
            //if (keyToken != null)
            //    return keyToken;
            //else if (publicKey == null)
            //    return null;
            //else {
            //    HashAlgorithm ha = null;
            //    switch (hashalg) {
            //        case AssemblyHashAlgorithm.MD5:
            //            ha = MD5.Create ();
            //            break;
            //        default:
            //            // None default to SHA1
            //            ha = SHA1.Create ();
            //            break;
            //    }
            //    byte[] hash = ha.ComputeHash (publicKey);
            //    // we need the last 8 bytes in reverse order
            //    keyToken = new byte [8];
            //    Array.Copy (hash, (hash.Length - 8), keyToken, 0, 8);
            //    Array.Reverse (keyToken, 0, 8);
            //    return keyToken;
            //}
            throw new NotImplementedException();
		}

#if NET_2_0
		[MonoTODO]
		public static bool ReferenceMatchesDefinition (AssemblyName reference, AssemblyName definition)
		{
			throw new NotImplementedException ();
		}
#endif

		public void SetPublicKey (byte[] publicKey) 
		{
			flags = AssemblyNameFlags.PublicKey;
			this.publicKey = publicKey;
		}

		public void SetPublicKeyToken (byte[] publicKeyToken) 
		{
			keyToken = publicKeyToken;
		}

		public object Clone() 
		{
			AssemblyName an = new AssemblyName ();
			an.name = name;
			an.codebase = codebase;
			an.major = major;
			an.minor = minor;
			an.build = build;
			an.revision = revision;
			an.version = version;
			an.cultureinfo = cultureinfo;
			an.flags = flags;
			an.hashalg = hashalg;
			//an.keypair = keypair;
			an.publicKey = publicKey;
			an.keyToken = keyToken;
			an.versioncompat = versioncompat;
			return an;
		}

		public void OnDeserialization (object sender) 
		{
			Version = version;
		}

		public static AssemblyName GetAssemblyName (string assemblyFile) 
		{
			throw new NotImplementedException();
		}
	}
}
