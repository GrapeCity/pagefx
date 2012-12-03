//
// System.Security.Permissions.ResourcePermissionBase.cs
//
// Authors:
//	Jonathan Pryor (jonpryor@vt.edu)
//	Sebastien Pouliot  <sebastien@ximian.com>
//
// (C) 2002
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

using System.Collections;
using System.Globalization;

namespace System.Security.Permissions {

	[Serializable]
	public abstract class ResourcePermissionBase : CodeAccessPermission, IUnrestrictedPermission {

		private const int version = 1;

		private ArrayList _list;
		private bool _unrestricted;
		private Type _type;
		private string[] _tags;

		protected ResourcePermissionBase ()
		{
			_list = new ArrayList ();
		}

		protected ResourcePermissionBase (PermissionState state) : this ()
		{
#if NET_2_0
			PermissionHelper.CheckPermissionState (state, true);
#else
			// there are no validation of the permission state
			// but any invalid value results in a restricted set
#endif
			_unrestricted = (state == PermissionState.Unrestricted);
		}

		public const string Any = "*";
		public const string Local = ".";

		protected Type PermissionAccessType {
			get { return _type; }
			set {
				if (value == null)
					throw new ArgumentNullException ("PermissionAccessType");
				if (!value.IsEnum)
					throw new ArgumentException ("!Enum", "PermissionAccessType");
				_type = value;
			}
		}

		protected string[] TagNames {
			get { return _tags; }
			set {
				if (value == null)
					throw new ArgumentNullException ("TagNames");
				if (value.Length == 0)
					throw new ArgumentException ("Length==0", "TagNames");
				_tags = value;
			}
		}

		protected void AddPermissionAccess (ResourcePermissionBaseEntry entry)
		{
			CheckEntry (entry);
			if (Exists (entry)) {
				string msg = Locale.GetText ("Entry already exists.");
				throw new InvalidOperationException (msg);
			}

			_list.Add (entry);
		}

		protected void Clear ()
		{
			_list.Clear ();
		}

		public override IPermission Copy ()
		{
			ResourcePermissionBase copy = CreateFromType (this.GetType (), _unrestricted);
			if (_tags != null)
				copy._tags = (string[]) _tags.Clone ();
			copy._type = _type;
			// FIXME: shallow or deep copy ?
			copy._list.AddRange (_list);
			return copy;
		}

		[MonoTODO ("incomplete - need more test")]
		public override void FromXml (SecurityElement securityElement)
		{
#if NET_2_0
			if (securityElement == null)
				throw new ArgumentNullException ("securityElement");
#else
			if (securityElement == null)
				throw new NullReferenceException ("securityElement");
#endif
			CheckSecurityElement (securityElement, "securityElement", version, version);
			// Note: we do not (yet) care about the return value 
			// as we only accept version 1 (min/max values)

			_list.Clear ();
			_unrestricted = PermissionHelper.IsUnrestricted (securityElement);
			if ((securityElement.Children == null) || (securityElement.Children.Count < 1))
				return;

			string[] names = new string [1];
			foreach (SecurityElement child in securityElement.Children) {
				// TODO: handle multiple names
				names [0] = child.Attribute ("name");
				int access = (int) Enum.Parse (PermissionAccessType, child.Attribute ("access"));
				ResourcePermissionBaseEntry entry = new ResourcePermissionBaseEntry (access, names);
				AddPermissionAccess (entry);
			}
		}

		protected ResourcePermissionBaseEntry[] GetPermissionEntries ()
		{
			ResourcePermissionBaseEntry[] entries = new ResourcePermissionBaseEntry [_list.Count];
			_list.CopyTo (entries, 0);
			return entries;
		}

		public override IPermission Intersect (IPermission target)
		{
			ResourcePermissionBase rpb = Cast (target);
			if (rpb == null)
				return null;

			bool su = this.IsUnrestricted ();
			bool tu = rpb.IsUnrestricted ();

			// if one is empty we return null (unless the other one is unrestricted)
			if (IsEmpty () && !tu)
				return null;
			if (rpb.IsEmpty () && !su)
				return null;

			ResourcePermissionBase result = CreateFromType (this.GetType (), (su && tu));
			foreach (ResourcePermissionBaseEntry entry in _list) {
				if (tu || rpb.Exists (entry))
					result.AddPermissionAccess (entry);
			}
			foreach (ResourcePermissionBaseEntry entry in rpb._list) {
				// don't add twice
				if ((su || this.Exists (entry)) && !result.Exists (entry))
					result.AddPermissionAccess (entry);
			}
			return result;
		}

		public override bool IsSubsetOf (IPermission target)
		{
			if (target == null) {
#if NET_2_0
				// do not use Cast - different permissions (and earlier Fx) return false :-/
				return true;
#else
				return false;
#endif
			}

			ResourcePermissionBase rpb = (target as ResourcePermissionBase);
			if (rpb == null)
				return false;
			if (rpb.IsUnrestricted ())
				return true;
			if (IsUnrestricted ())
				return rpb.IsUnrestricted ();
			foreach (ResourcePermissionBaseEntry entry in _list) {
				if (!rpb.Exists (entry))
					return false;
			}
			return true;
		}

		public bool IsUnrestricted ()
		{
			return _unrestricted;
		}

		protected void RemovePermissionAccess (ResourcePermissionBaseEntry entry)
		{
			CheckEntry (entry);
			for (int i = 0; i < _list.Count; i++) {
				ResourcePermissionBaseEntry rpbe = (ResourcePermissionBaseEntry) _list [i];
				if (Equals (entry, rpbe)) {
					_list.RemoveAt (i);
					return;
				}
			}
			string msg = Locale.GetText ("Entry doesn't exists.");
			throw new InvalidOperationException (msg);
		}

		public override SecurityElement ToXml ()
		{
			SecurityElement se = PermissionHelper.Element (this.GetType (), version);
			if (IsUnrestricted ()) {
				se.AddAttribute ("Unrestricted", "true");
			}
			else {
				foreach (ResourcePermissionBaseEntry entry in _list) {
					SecurityElement container = se;
					string access = null;
					if (PermissionAccessType != null)
						access = Enum.Format (PermissionAccessType, entry.PermissionAccess, "g");

					for (int i=0; i < _tags.Length; i++) {
						SecurityElement child = new SecurityElement (_tags [i]);
						child.AddAttribute ("name", entry.PermissionAccessPath [i]);
						if (access != null)
							child.AddAttribute ("access", access);
						container.AddChild (child);
						child = container;
					}
				}
			}
			return se;
		}

		public override IPermission Union (IPermission target)
		{
			ResourcePermissionBase rpb = Cast (target);
			if (rpb == null)
				return Copy ();
			if (IsEmpty () && rpb.IsEmpty ())
				return null;
			if (rpb.IsEmpty ())
				return Copy ();
			if (IsEmpty ())
				return rpb.Copy ();

			bool unrestricted = (IsUnrestricted () || rpb.IsUnrestricted ());
			ResourcePermissionBase result = CreateFromType (this.GetType (), unrestricted);
			// strangely unrestricted union doesn't process the elements (while intersect does)
			if (!unrestricted) {
				foreach (ResourcePermissionBaseEntry entry in _list) {
					result.AddPermissionAccess (entry);
				}
				foreach (ResourcePermissionBaseEntry entry in rpb._list) {
					// don't add twice
					if (!result.Exists (entry))
						result.AddPermissionAccess (entry);
				}
			}
			return result;
		}

		// helpers

		private bool IsEmpty ()
		{
			return (!_unrestricted && (_list.Count == 0));
		}

		private ResourcePermissionBase Cast (IPermission target)
		{
			if (target == null)
				return null;

			ResourcePermissionBase rp = (target as ResourcePermissionBase);
			if (rp == null) {
				PermissionHelper.ThrowInvalidPermission (target, typeof (ResourcePermissionBase));
			}

			return rp;
		}

		internal void CheckEntry (ResourcePermissionBaseEntry entry)
		{
			if (entry == null)
				throw new ArgumentNullException ("entry");
			if ((entry.PermissionAccessPath == null) || (entry.PermissionAccessPath.Length != _tags.Length)) {
				string msg = Locale.GetText ("Entry doesn't match TagNames");
				throw new InvalidOperationException (msg);
			}
		}

		internal bool Equals (ResourcePermissionBaseEntry entry1, ResourcePermissionBaseEntry entry2)
		{
			if (entry1.PermissionAccess != entry2.PermissionAccess)
				return false;
			if (entry1.PermissionAccessPath.Length != entry2.PermissionAccessPath.Length)
				return false;
			for (int i=0; i < entry1.PermissionAccessPath.Length; i++) {
				if (entry1.PermissionAccessPath [i] != entry2.PermissionAccessPath [i])
					return false;
			}
			return true;
		}

		internal bool Exists (ResourcePermissionBaseEntry entry)
		{
			if (_list.Count == 0)
				return false;
			foreach (ResourcePermissionBaseEntry rpbe in _list) {
				if (Equals (rpbe, entry))
					return true;
			}
			return false;
		}

		// logic isn't identical to PermissionHelper.CheckSecurityElement
		// - no throw on version mismatch
		internal int CheckSecurityElement (SecurityElement se, string parameterName, int minimumVersion, int maximumVersion) 
		{
			if (se == null)
				throw new ArgumentNullException (parameterName);
#if NET_2_0
			// Tag is case-sensitive
			if (se.Tag != "IPermission") {
				string msg = String.Format (Locale.GetText ("Invalid tag {0}"), se.Tag);
				throw new ArgumentException (msg, parameterName);
			}
#endif
			// Note: we do not care about the class attribute at 
			// this stage (in fact we don't even if the class 
			// attribute is present or not). Anyway the object has
			// already be created, with success, if we're loading it

			// we assume minimum version if no version number is supplied
			int version = minimumVersion;
			string v = se.Attribute ("version");
			if (v != null) {
				try {
					version = Int32.Parse (v);
				}
				catch (Exception e) {
					string msg = Locale.GetText ("Couldn't parse version from '{0}'.");
					msg = String.Format (msg, v);
					throw new ArgumentException (msg, parameterName, e);
				}
			}
#if NET_2_0
			if ((version < minimumVersion) || (version > maximumVersion)) {
				string msg = Locale.GetText ("Unknown version '{0}', expected versions between ['{1}','{2}'].");
				msg = String.Format (msg, version, minimumVersion, maximumVersion);
				throw new ArgumentException (msg, parameterName);
			}
#endif
			return version;
		}

		// static helpers

		private static char[] invalidChars = new char[] { '\t', '\n', '\v', '\f', '\r', ' ', '\\', '\x160' };

		internal static void ValidateMachineName (string name)
		{
			// FIXME: maybe other checks are required (but not documented)
			if ((name == null) || (name.Length == 0) || (name.IndexOfAny (invalidChars) != -1)) {
				string msg = Locale.GetText ("Invalid machine name '{0}'.");
				if (name == null)
					name = "(null)";
				msg = String.Format (msg, name);
				throw new ArgumentException (msg, "MachineName");
			}
		}

		internal static ResourcePermissionBase CreateFromType (Type type, bool unrestricted)
		{
			object[] parameters = new object [1];
			parameters [0] = (object) ((unrestricted) ? PermissionState.Unrestricted : PermissionState.None);
			// we must return the derived type - this is why an empty constructor is required ;-)
			return (ResourcePermissionBase) Activator.CreateInstance (type, parameters);
		}
	}
}
