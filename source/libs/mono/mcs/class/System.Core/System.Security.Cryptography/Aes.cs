//
// System.Security.Cryptography.Aes.cs
//	based on mcs/class/corlib/System.Security.Cryptography/Rijndael.cs
//
// Authors:
//	Dan Lewis (dihlewis@yahoo.co.uk)
//	Andrew Birkett (andy@nobugs.org)
//	Sebastien Pouliot  <sebastien@ximian.com>
//
// (C) 2002
// Copyright (C) 2004-2006,2008 Novell, Inc (http://www.novell.com)
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

using System.Runtime.InteropServices;

namespace System.Security.Cryptography {

	// References:
	// a.	FIPS PUB 197: Advanced Encryption Standard
	//	http://csrc.nist.gov/publications/fips/fips197/fips-197.pdf

	public abstract class Aes : SymmetricAlgorithm {

		public static new Aes Create () 
		{
			return Create ("System.Security.Cryptography.Aes");
		}

		public static new Aes Create (string algName) 
		{
			return (Aes) CryptoConfig.CreateFromName (algName);
		}

		protected Aes ()
		{
			KeySizeValue = 256;
			BlockSizeValue = 128;
#if !NET_2_1
			// Silverlight 2.0 only supports CBC mode (i.e. no feedback)
			FeedbackSizeValue = 128;
#endif
			LegalKeySizesValue = new KeySizes [1];
			LegalKeySizesValue [0] = new KeySizes (128, 256, 64);

			LegalBlockSizesValue = new KeySizes [1];
			LegalBlockSizesValue [0] = new KeySizes (128, 128, 0);
		}
	}
}
