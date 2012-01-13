//
// System.Security.Permissions.RegistryPermissionAttribute.cs
//
// Authors
//	Duncan Mak <duncan@ximian.com>
//	Sebastien Pouliot  <sebastien@ximian.com>
//
// (C) 2002 Ximian, Inc. http://www.ximian.com
// Portions Copyright (C) 2003 Motus Technologies (http://www.motus.com)
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

namespace System.Security.Permissions {

#if NET_2_0
	[ComVisible (true)]
#endif
	[AttributeUsage (AttributeTargets.Assembly | AttributeTargets.Class |
			 AttributeTargets.Struct | AttributeTargets.Constructor |
			 AttributeTargets.Method, AllowMultiple=true, Inherited=false)]
	[Serializable]
	public sealed class RegistryPermissionAttribute : CodeAccessSecurityAttribute {

		// Fields
		private string create;
		private string read;
		private string write;
#if NET_2_0
		private string changeAccessControl;
		private string viewAccessControl;
		private string viewAndModify;
#endif

		// Constructor
		public RegistryPermissionAttribute (SecurityAction action) : base (action)
		{
		}
		
		// Properties
#if NET_2_0
		[Obsolete ("use newer properties")]
#endif
		public string All {
#if ! NET_1_0
			get { throw new NotSupportedException ("All"); }
#endif
			set { 
				create = value; 
				read = value;
				write = value;
			}
		}
		
		public string Create {
			get { return create; }
			set { create = value; }
		}

		public string Read { 
			get { return read; }
			set { read = value; }
		}

		public string Write {
			get { return write; }
			set { write = value; }
		}

#if NET_2_0
		public string ChangeAccessControl {
			get { return changeAccessControl; }
			set { changeAccessControl = value; }
		}

		public string ViewAccessControl {
			get { return viewAccessControl; }
			set { viewAccessControl = value; }
		}

		public string ViewAndModify {
			get { throw new NotSupportedException (); }	// as documented
			set {
				create = value;
				read = value;
				write = value;
			}
		}
#endif
		// Methods
#if NOT_PFX
		public override IPermission CreatePermission ()
		{
			RegistryPermission perm = null;
			if (this.Unrestricted)
				perm = new RegistryPermission (PermissionState.Unrestricted);
			else {
				perm = new RegistryPermission (PermissionState.None);
				if (create != null)
					perm.AddPathList (RegistryPermissionAccess.Create, create);
				if (read != null)
					perm.AddPathList (RegistryPermissionAccess.Read, read);
				if (write != null)
					perm.AddPathList (RegistryPermissionAccess.Write, write);
			}
			return perm;
		}
#endif
	}
}
