//
// System.Net.EndPointManager
//
// Author:
//	Gonzalo Paniagua Javier (gonzalo@ximian.com)
//
// Copyright (c) 2005 Novell, Inc. (http://www.novell.com)
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

#if NET_2_0 && SECURITY_DEP

using System.Collections;
using System.Collections.Generic;
namespace System.Net {
	sealed class EndPointManager
	{
		static Dictionary<IPAddress, Dictionary<int, EndPointListener>> ip_to_endpoints =
				new Dictionary<IPAddress, Dictionary<int, EndPointListener>> ();
		
		private EndPointManager ()
		{
		}

		public static void AddListener (HttpListener listener)
		{
			List<string> added = new List<string> ();
			try {
				lock (ip_to_endpoints) {
					foreach (string prefix in listener.Prefixes) {
						AddPrefixInternal (prefix, listener);
						added.Add (prefix);
					}
				}
			} catch {
				foreach (string prefix in added) {
					RemovePrefixInternal (prefix, listener);
				}
				throw;
			}
		}

		public static void AddPrefix (string prefix, HttpListener listener)
		{
			lock (ip_to_endpoints) {
				AddPrefixInternal (prefix, listener);
			}
		}

		static void AddPrefixInternal (string p, HttpListener listener)
		{
			ListenerPrefix lp = new ListenerPrefix (p);
			if (lp.Path.IndexOf ('%') != -1)
				throw new HttpListenerException (400, "Invalid path.");

			if (lp.Path.IndexOf ("//") != -1) // TODO: Code?
				throw new HttpListenerException (400, "Invalid path.");

			// Always listens on all the interfaces, no matter the host name/ip used.
			EndPointListener epl = GetEPListener (IPAddress.Any, lp.Port, listener, lp.Secure);
			epl.AddPrefix (lp, listener);
		}

		static EndPointListener GetEPListener (IPAddress addr, int port, HttpListener listener, bool secure)
		{
			Dictionary<int, EndPointListener> p = null;
			if (ip_to_endpoints.ContainsKey (addr)) {
				p = ip_to_endpoints [addr];
			} else {
				p = new Dictionary<int, EndPointListener> ();
				ip_to_endpoints [addr] = p;
			}

			EndPointListener epl = null;
			if (p.ContainsKey (port)) {
				epl = p [port];
			} else {
				epl = new EndPointListener (addr, port, secure);
				p [port] = epl;
			}

			return epl;
		}

		public static void RemoveEndPoint (EndPointListener epl, IPEndPoint ep)
		{
			lock (ip_to_endpoints) {
				Dictionary<int, EndPointListener> p = null;
				p = ip_to_endpoints [ep.Address];
				p.Remove (ep.Port);
				epl.Close ();
			}
		}

		public static void RemoveListener (HttpListener listener)
		{
			lock (ip_to_endpoints) {
				foreach (string prefix in listener.Prefixes) {
					RemovePrefixInternal (prefix, listener);
				}
			}
		}

		public static void RemovePrefix (string prefix, HttpListener listener)
		{
			lock (ip_to_endpoints) {
				RemovePrefixInternal (prefix, listener);
			}
		}

		static void RemovePrefixInternal (string prefix, HttpListener listener)
		{
			ListenerPrefix lp = new ListenerPrefix (prefix);
			if (lp.Path.IndexOf ('%') != -1)
				return;

			if (lp.Path.IndexOf ("//") != -1)
				return;

			EndPointListener epl = GetEPListener (IPAddress.Any, lp.Port, listener, lp.Secure);
			epl.RemovePrefix (lp, listener);
		}
	}
}
#endif

