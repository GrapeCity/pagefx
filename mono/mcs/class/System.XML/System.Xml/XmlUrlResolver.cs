// System.Xml.XmlUrlResolver.cs
//
// Author: Duncan Mak (duncan@ximian.com)
//	   Atsushi Enomoto (ginga@kit.hi-ho.ne.jp)
//
// (C) Ximian, Inc.
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

#if NOT_PFX
using System.Net;
#endif
using System.IO;
using System.Text;

namespace System.Xml
{
#if NOT_PFX	
    public 
#else
    internal 
#endif
        class XmlUrlResolver : XmlResolver
	{
#if !NET_2_1
		// Field

#if NOT_PFX
ICredentials credential;
#endif

#endif

		// Constructor
		public XmlUrlResolver ()
			: base ()
		{
		}

#if !NET_2_1
		// Properties		
#if NOT_PFX
		public override ICredentials Credentials
		{
			set { credential = value; }
		}
#endif
#endif

		// Methods
#if NET_2_1
		[MonoTODO ("What should/can we do for non-file URLs, without HttpWebRequest implementation in System.dll?")]
#endif
		public override object GetEntity (Uri absoluteUri, string role, Type ofObjectToReturn)
		{
			if (ofObjectToReturn == null)
				ofObjectToReturn = typeof (Stream);
			if (ofObjectToReturn != typeof (Stream))
				throw new XmlException ("This object type is not supported.");

#if NET_2_0
			if (!absoluteUri.IsAbsoluteUri)
				throw new ArgumentException ("uri must be absolute.", "absoluteUri");
#endif

#if NOT_PFX
if (absoluteUri.Scheme == "file") {
				if (absoluteUri.AbsolutePath == String.Empty)
					throw new ArgumentException ("uri must be absolute.", "absoluteUri");
				return new FileStream (UnescapeRelativeUriBody (absoluteUri.LocalPath), FileMode.Open, FileAccess.Read, FileShare.Read);
			}
#endif
            throw new NotImplementedException();
		    return null;
#if NOT_PFX
#if net_2_1
			// so, what can i do here? httpwebrequest cannot be instantiated within system.dll
			throw new notimplementedexception ();
#else
			// (ms documentation says) parameter role isn't used yet.
			webrequest req = webrequest.create (absoluteuri);
			if (credential != null)
				req.credentials = credential;
			return req.getresponse().getresponsestream();
#endif
#endif
		}

#if NET_2_0
		public override Uri ResolveUri (Uri baseUri, string relativeUri)
		{
			return base.ResolveUri (baseUri, relativeUri);
		}
#endif

		// see also XmlResolver.EscapeRelativeUriBody().
		private string UnescapeRelativeUriBody (string src)
		{
			return src.Replace ("%3C", "<")
				.Replace ("%3E", ">")
				.Replace ("%23", "#")
				.Replace ("%22", "\"")
				.Replace ("%20", " ")
				.Replace ("%25", "%");
		}
	}
}
