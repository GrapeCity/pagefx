//CHANGED

//
// System.Security.Cryptography.AsymmetricAlgorithm Class implementation
//
// Authors:
//	Thomas Neidhart (tome@sbox.tugraz.at)
//	Sebastien Pouliot (sebastien@ximian.com)
//
// Portions (C) 2002, 2003 Motus Technologies Inc. (http://www.motus.com)
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

using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography {

#if NET_2_0
	[ComVisible (true)]
#endif
	public abstract class AsymmetricAlgorithm : IDisposable	{

		protected int KeySizeValue;
		protected KeySizes[] LegalKeySizesValue; 

		protected AsymmetricAlgorithm ()
		{
		}
		
		public abstract string KeyExchangeAlgorithm {
			get;
		}
		
		public virtual int KeySize {
			get { return this.KeySizeValue; }
			set {
				if (!KeySizes.IsLegalKeySize (this.LegalKeySizesValue, value))
					throw new CryptographicException (Locale.GetText ("Key size not supported by algorithm."));
				
				this.KeySizeValue = value;
			}
		}

		public virtual KeySizes[] LegalKeySizes {
			get { return this.LegalKeySizesValue; }
		}

		public abstract string SignatureAlgorithm {
			get;
		}

		void IDisposable.Dispose () 
		{
			Dispose (true);
			//GC.SuppressFinalize (this);  // Finalization is now unnecessary
		}

		public void Clear () 
		{
			Dispose (false);
		}

		protected abstract void Dispose (bool disposing);

		public abstract void FromXmlString (string xmlString);
		
		public abstract string ToXmlString (bool includePrivateParameters);		
		
		public static AsymmetricAlgorithm Create () 
		{
			return Create ("System.Security.Cryptography.AsymmetricAlgorithm");
		}
	
		public static AsymmetricAlgorithm Create (string algName) 
		{
			//return (AsymmetricAlgorithm) CryptoConfig.CreateFromName (algName);
            throw new NotImplementedException();
		}
	}
}
