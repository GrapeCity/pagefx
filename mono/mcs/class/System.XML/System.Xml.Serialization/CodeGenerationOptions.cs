// 
// System.Xml.Serialization.CodeGenerationOptions 
//
// Author:
//   Lluis Sanchez Gual (lluis@ximian.com)
//
// Copyright (C) Novell, Inc., 2004
//

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

using System;

namespace System.Xml.Serialization 
{
	[Flags]
#if NET_2_0
	public
#else
	[Serializable]
	internal
#endif
	enum CodeGenerationOptions
	{
		[XmlIgnore]
		None = 0,
		[XmlEnum ("properties")]
		GenerateProperties = 1,
		[XmlEnum ("newAsync")]
		GenerateNewAsync = 2,
		[XmlEnum ("oldAsync")]
		GenerateOldAsync = 4,
		[XmlEnum ("order")]
		GenerateOrder = 8,
		[XmlEnum ("enableDataBinding")]
		EnableDataBinding = 16,
		
	}
}
