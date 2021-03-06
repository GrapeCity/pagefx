//
// PKCS1MaskGenerationMethod.cs: Handles PKCS#1 mask generation.
//
// Author:
//	Sebastien Pouliot (sebastien@ximian.com)
//
// (C) 2002, 2003 Motus Technologies Inc. (http://www.motus.com)
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

using System.Runtime.InteropServices;
using Mono.Security.Cryptography;

namespace System.Security.Cryptography {

	// References:
	// a.	PKCS#1: RSA Cryptography Standard 
	//	http://www.rsasecurity.com/rsalabs/pkcs/pkcs-1/index.html
	
#if NET_2_0
	[ComVisible (true)]
#endif
	public class PKCS1MaskGenerationMethod : MaskGenerationMethod {

		private string hashName;
	
		public PKCS1MaskGenerationMethod ()
		{
			hashName = "SHA1";
		}
	
		public string HashName 
		{
			get { return hashName; }
			set { hashName = ((value == null) ? "SHA1" : value); }
		}
	
		// This method is not compatible with the one provided by MS in
		// framework 1.0 and 1.1 but IS compliant with PKCS#1 v.2.1 and
		// work for implementing OAEP
		public override byte[] GenerateMask (byte[] mgfSeed, int maskLen)
		{
			HashAlgorithm hash = HashAlgorithm.Create (hashName);
			return PKCS1.MGF1 (hash, mgfSeed, maskLen);
		}
	}
}
