//
// X509StoreManager.cs: X.509 store manager.
//
// Author:
//	Sebastien Pouliot  <sebastien@ximian.com>
//
// (C) 2004 Novell (http://www.novell.com)
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
using System.Collections;
using System.IO;

using Mono.Security.X509.Extensions;

namespace Mono.Security.X509 {

#if INSIDE_CORLIB
	internal
#else
	public 
#endif
	sealed class X509StoreManager {

		static private X509Stores _userStore;
		static private X509Stores _machineStore;

		private X509StoreManager ()
		{
		}

		static public X509Stores CurrentUser {
			get { 
				if (_userStore == null) {
					string _userPath = Path.Combine (
						Environment.GetFolderPath (Environment.SpecialFolder.ApplicationData),
						".mono");
					_userPath = Path.Combine (_userPath, "certs");

					_userStore = new X509Stores (_userPath);
				}
				return _userStore;
			}
		}

		static public X509Stores LocalMachine {
			get {
				if (_machineStore == null) {
					string _machinePath = Path.Combine (
						Environment.GetFolderPath (Environment.SpecialFolder.CommonApplicationData),
						".mono");
					_machinePath = Path.Combine (_machinePath, "certs");

					_machineStore = new X509Stores (_machinePath);
				}
				return _machineStore;
			}
		}

		// Merged stores collections
		// we need to look at both the user and the machine (entreprise)
		// certificates/CRLs when building/validating a chain

		static public X509CertificateCollection IntermediateCACertificates {
			get { 
				X509CertificateCollection intermediateCerts = new X509CertificateCollection ();
				intermediateCerts.AddRange (CurrentUser.IntermediateCA.Certificates);
				intermediateCerts.AddRange (LocalMachine.IntermediateCA.Certificates);
				return intermediateCerts; 
			}
		}

		static public ArrayList IntermediateCACrls {
			get { 
				ArrayList intermediateCRLs = new ArrayList ();
				intermediateCRLs.AddRange (CurrentUser.IntermediateCA.Crls);
				intermediateCRLs.AddRange (LocalMachine.IntermediateCA.Crls);
				return intermediateCRLs; 
			}
		}

		static public X509CertificateCollection TrustedRootCertificates {
			get { 
				X509CertificateCollection trustedCerts = new X509CertificateCollection ();
				trustedCerts.AddRange (CurrentUser.TrustedRoot.Certificates);
				trustedCerts.AddRange (LocalMachine.TrustedRoot.Certificates);
				return trustedCerts; 
			}
		}

		static public ArrayList TrustedRootCACrls {
			get { 
				ArrayList trustedCRLs = new ArrayList ();
				trustedCRLs.AddRange (CurrentUser.TrustedRoot.Crls);
				trustedCRLs.AddRange (LocalMachine.TrustedRoot.Crls);
				return trustedCRLs; 
			}
		}

		static public X509CertificateCollection UntrustedCertificates {
			get { 
				X509CertificateCollection untrustedCerts = new X509CertificateCollection ();
				untrustedCerts.AddRange (CurrentUser.Untrusted.Certificates);
				untrustedCerts.AddRange (LocalMachine.Untrusted.Certificates);
				return untrustedCerts; 
			}
		}
	}
}
