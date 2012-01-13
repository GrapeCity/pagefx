//
// System.Net.NetworkCredential.cs
//
// Author: Duncan Mak (duncan@ximian.com)
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

namespace System.Net
{
	public class NetworkCredential : ICredentials
#if NET_2_0
					, ICredentialsByHost
#endif
	{
		// Fields
		string userName;
		string password;
		string domain;
		
		// Constructors
		public NetworkCredential ()
			: base ()
		{
		}

		public NetworkCredential (string userName, string password)
		{
			this.userName = userName;
			this.password = password;
		}

		public NetworkCredential (string userName, string password, string domain)
		{
			this.userName = userName;
			this.password = password;
			this.domain = domain;
		}

		// Properties

		public string Domain
		{
			get { return domain; }
			set { domain = value; }
		}

		public string UserName
		{
			get { return userName; }
			set { userName = value; }			
		}

		public string Password
		{
			get { return password; }
			set { password = value; }
		}

		public NetworkCredential GetCredential (Uri uri, string authType)
		{
			return this;
		}

#if NET_2_0
		public NetworkCredential GetCredential (string host, int port, string authenticationType)
		{
			return this;
		}
#endif
	}
}
