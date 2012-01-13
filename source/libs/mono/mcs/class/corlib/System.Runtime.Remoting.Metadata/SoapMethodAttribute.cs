//
// System.Runtime.Remoting.Metadata.SoapMethodAttribute.cs
//
// Author: Duncan Mak  (duncan@ximian.com)
//         Lluis Sanchez Gual (lluis@ximian.com)
//
// 2002 (C) Copyright, Ximian, Inc.
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
using System.Reflection;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Metadata;

namespace System.Runtime.Remoting.Metadata {

	[AttributeUsage (AttributeTargets.Method)]
#if NET_2_0
	[System.Runtime.InteropServices.ComVisible (true)]
#endif
	public sealed class SoapMethodAttribute : SoapAttribute
	{
		string _responseElement;
		string _responseNamespace;
		string _returnElement;
		string _soapAction;
		bool _useAttribute;
		string _namespace;
		
		public SoapMethodAttribute ()
		{
		}

		public string ResponseXmlElementName {
			get {
				return _responseElement;
			}

			set {
				_responseElement = value;
			}
		}
		
		public string ResponseXmlNamespace {
			get {
				return _responseNamespace;
			}

			set {
				_responseNamespace = value;
			}
		}

		public string ReturnXmlElementName {
			get {
				return _returnElement;
			}

			set {
				_returnElement = value;
			}
		}

		public string SoapAction {
			get {
				return _soapAction;
			}

			set {
				_soapAction = value;
			}
		}

		public override bool UseAttribute {
			get {
				return _useAttribute;
			}

			set {
				_useAttribute = value;
			}
		}

		public override string XmlNamespace {
			get {
				return _namespace;
			}

			set {
				_namespace = value;
			}
		}
		
		internal override void SetReflectionObject (object reflectionObject)
		{
			MethodBase mb = (MethodBase) reflectionObject;
			
			if (_responseElement == null)
				_responseElement = mb.Name + "Response";
				
			if (_responseNamespace == null)
				_responseNamespace = SoapServices.GetXmlNamespaceForMethodResponse (mb);
				
			if (_returnElement == null)
				_returnElement = "return";
				
			if (_soapAction == null) {
				_soapAction = SoapServices.GetXmlNamespaceForMethodCall (mb) + "#" + mb.Name;
			}
			
			if (_namespace == null)
				_namespace = SoapServices.GetXmlNamespaceForMethodCall (mb);
		}
	}
}
