//
// System.Runtime.Remoting.Messaging.MethodReturnDictionary.cs
//
// Author: Lluis Sanchez Gual (lluis@ideary.com)
//
// 2003 (C) Lluis Sanchez Gual
//

//
// Copyright (C) 2004 Novell, Inc (http://www.novell.com)
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

namespace System.Runtime.Remoting.Messaging
{
	internal class MethodReturnDictionary : MethodDictionary
	{
		public static string[] InternalReturnKeys = new string[] {"__Uri", "__MethodName", "__TypeName", "__MethodSignature", "__OutArgs", "__Return", "__CallContext"};
		public static string[] InternalExceptionKeys = new string[] {"__CallContext"};

		public MethodReturnDictionary (IMethodReturnMessage message) : base (message)
		{
			if (message.Exception == null)
				MethodKeys = InternalReturnKeys;
			else
				MethodKeys = InternalExceptionKeys;
		}
	}
}
