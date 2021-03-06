//
// System.Xml.XmlWhitespace.cs
//
// Author:
//	Duncan Mak  (duncan@ximian.com)
//
// (C) Ximian, Inc. http://www.ximian.com
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
#if NOT_PFX
using System.Xml.XPath;
#endif

namespace System.Xml
{
	public class XmlWhitespace : XmlCharacterData
	{
		// Constructor
		protected internal XmlWhitespace (string strData, XmlDocument doc)
			: base (strData, doc)
		{
		}
		
		// Properties
		public override string LocalName {
			get { return "#whitespace"; }
		}

		public override string Name {
			get { return "#whitespace"; }
		}

		public override XmlNodeType NodeType {
			get { return XmlNodeType.Whitespace; }
		}

		internal override XPathNodeType XPathNodeType {
			get { return XPathNodeType.Whitespace; }
		}

		public override string Value {
			get { return Data; }
			set {
				if (!XmlChar.IsWhitespace (value))
					throw new ArgumentException ("Invalid whitespace characters.");
				Data = value;
			}
		}

#if NET_2_0
		public override XmlNode ParentNode {
			get { return base.ParentNode; }
		}
#endif

		// Methods
		public override XmlNode CloneNode (bool deep)
		{
			// always return the data value
			return new XmlWhitespace (Data, OwnerDocument); 
		}

		public override void WriteContentTo (XmlWriter w) {}

		public override void WriteTo (XmlWriter w)
		{
			w.WriteWhitespace (Data);
		}
	}
}
