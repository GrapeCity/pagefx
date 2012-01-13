//
// System.Xml.XmlSecureResolver.cs
//
// Author: Atsushi Enomoto (ginga@kit.hi-ho.ne.jp)
//
// (C) 2003 Atsushi Enomoto
// Copyright (C) 2005 Novell, Inc (http://www.novell.com)
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
using System.Security.Policy;
#endif
using System.Security;
using System.Security.Permissions;

namespace System.Xml
{
	public class XmlSecureResolver : XmlResolver
	{

#region Static Members

#if NOT_PFX
public static Evidence CreateEvidenceForUrl (string securityUrl)
		{
			Evidence e = new Evidence ();

			if ((securityUrl != null) && (securityUrl.Length > 0)) {
				try {
					Url url = new Url (securityUrl);
					e.AddHost (url);
				} catch (ArgumentException) {
				}

				try {
					Zone zone = Zone.CreateFromUrl (securityUrl);
					e.AddHost (zone);
				} catch (ArgumentException) {
				}

				try {
					Site site = Site.CreateFromUrl (securityUrl);
					e.AddHost (site);
				} catch (ArgumentException) {
				}
			}

			return e;
		}
#endif
#endregion

		XmlResolver resolver;
#if NOT_PFX
PermissionSet permissionSet;
#endif

#region .ctor and Finalizer

		public XmlSecureResolver (
			XmlResolver resolver
#if NOT_PFX
, Evidence evidence
#endif
            )
		{
			this.resolver = resolver;
#if NOT_PFX
		if (SecurityManager.SecurityEnabled) {
				this.permissionSet = SecurityManager.ResolvePolicy (evidence);
			}
#endif
		}

#if NOT_PFX
public XmlSecureResolver (
			XmlResolver resolver, PermissionSet permissionSet)
		{
			this.resolver = resolver;
			this.permissionSet = permissionSet;
		}

#endif
		public XmlSecureResolver (
			XmlResolver resolver, string securityUrl)
		{
			this.resolver = resolver;
#if NOT_PFX
if (SecurityManager.SecurityEnabled) {
				this.permissionSet = SecurityManager.ResolvePolicy (CreateEvidenceForUrl (securityUrl));
			}
#endif
		}
#endregion

#region Property

#if !NET_2_1
#if NOT_PFX
public override ICredentials Credentials {
			set { resolver.Credentials = value; }
		}
#endif
#endif

#endregion

#region Methods

		[MonoTODO]
		// FIXME: imperative PermitOnly isn't supported
		public override object GetEntity (
			Uri absoluteUri, string role, Type ofObjectToReturn)
		{
#if NOT_PFX
			if (SecurityManager.SecurityEnabled) {
				// in case the security manager was switched after the constructor was called
				if (permissionSet == null) {
					throw new SecurityException (Locale.GetText (
						"Security Manager wasn't active when instance was created."));
				}
				permissionSet.PermitOnly ();
			}
#endif
			return resolver.GetEntity (absoluteUri, role, ofObjectToReturn);
		}

		public override Uri ResolveUri (Uri baseUri, string relativeUri)
		{
			return resolver.ResolveUri (baseUri, relativeUri);
		}
#endregion

	}
}
