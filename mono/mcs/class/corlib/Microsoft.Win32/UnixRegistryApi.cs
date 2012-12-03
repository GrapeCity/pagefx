//
// Microsoft.Win32/UnixRegistryApi.cs
//
// Authors:
//	Miguel de Icaza (miguel@gnome.org)
//	Gert Driesen (drieseng@users.sourceforge.net)
//
// (C) 2005, 2006 Novell, Inc (http://www.novell.com)
// 
// MISSING:
//   It would be useful if we do case-insensitive expansion of variables,
//   the registry is very windows specific, so we probably should default to
//   those semantics in expanding environment variables, for example %path%
//
//   We should use an ordered collection for storing the values (instead of
//   a Hashtable).
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
using System.Globalization;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Security;
using System.Threading;

namespace Microsoft.Win32 {

	class ExpandString {
		string value;
		
		public ExpandString (string s)
		{
			value = s;
		}

		public override string ToString ()
		{
			return value;
		}

		public string Expand ()
		{
			StringBuilder sb = new StringBuilder ();

			for (int i = 0; i < value.Length; i++){
				if (value [i] == '%'){
					int j = i + 1;
					for (; j < value.Length; j++){
						if (value [j] == '%'){
							string key = value.Substring (i + 1, j - i - 1);

							sb.Append (Environment.GetEnvironmentVariable (key));
							i += j;
							break;
						}
					}
					if (j == value.Length){
						sb.Append ('%');
					}
				} else {
					sb.Append (value [i]);
				}
			}
			return sb.ToString ();
		}
	}

	class KeyHandler
	{
		static Hashtable key_to_handler = new Hashtable ();
		static Hashtable dir_to_handler = new Hashtable (
			new CaseInsensitiveHashCodeProvider (), new CaseInsensitiveComparer ());
		public string Dir;

		Hashtable values;
		string file;
		bool dirty;

		KeyHandler (RegistryKey rkey, string basedir)
		{
			if (!Directory.Exists (basedir)){
				try {
					Directory.CreateDirectory (basedir);
				} catch (UnauthorizedAccessException){
					throw new SecurityException ("No access to the given key");
				}
			}
			Dir = basedir;
			file = Path.Combine (Dir, "values.xml");
			Load ();
		}

		public void Load ()
		{
			values = new Hashtable ();
			if (!File.Exists (file))
				return;
			
			try {
				using (FileStream fs = File.OpenRead (file)){
					StreamReader r = new StreamReader (fs);
					string xml = r.ReadToEnd ();
					if (xml.Length == 0)
						return;
					
					SecurityElement tree = SecurityElement.FromString (xml);
					if (tree.Tag == "values" && tree.Children != null){
						foreach (SecurityElement value in tree.Children){
							if (value.Tag == "value"){
								LoadKey (value);
							}
						}
					}
				}
			} catch (UnauthorizedAccessException){
				values.Clear ();
				throw new SecurityException ("No access to the given key");
			} catch (Exception e){
				Console.Error.WriteLine ("While loading registry key at {0}: {1}", file, e);
				values.Clear ();
			}
		}

		void LoadKey (SecurityElement se)
		{
			Hashtable h = se.Attributes;
			try {
				string name = (string) h ["name"];
				if (name == null)
					return;
				string type = (string) h ["type"];
				if (type == null)
					return;
				
				switch (type){
				case "int":
					values [name] = Int32.Parse (se.Text);
					break;
				case "bytearray":
					Convert.FromBase64String (se.Text);
					break;
				case "string":
					values [name] = se.Text;
					break;
				case "expand":
					values [name] = new ExpandString (se.Text);
					break;
				case "qword":
					values [name] = Int64.Parse (se.Text);
					break;
				case "string-array":
					ArrayList sa = new ArrayList ();
					if (se.Children != null){
						foreach (SecurityElement stre in se.Children){
							sa.Add (stre.Text);
						}
					}
					values [name] = sa.ToArray (typeof (string));
					break;
				}
			} catch {
				// We ignore individual errors in the file.
			}
		}
		
		public RegistryKey Ensure (RegistryKey rkey, string extra, bool writable)
		{
			lock (typeof (KeyHandler)){
				string f = Path.Combine (Dir, extra);
				KeyHandler kh = (KeyHandler) dir_to_handler [f];
				if (kh == null)
					kh = new KeyHandler (rkey, f);
				RegistryKey rk = new RegistryKey (kh, CombineName (rkey, extra), writable);
				key_to_handler [rk] = kh;
				dir_to_handler [f] = kh;
				return rk;
			}
		}

		public RegistryKey Probe (RegistryKey rkey, string extra, bool writable)
		{
			RegistryKey rk = null;

			lock (typeof (KeyHandler)){
				string f = Path.Combine (Dir, extra);
				KeyHandler kh = (KeyHandler) dir_to_handler [f];
				if (kh != null) {
					rk = new RegistryKey (kh, CombineName (rkey,
						extra), writable);
					key_to_handler [rk] = kh;
				} else if (Directory.Exists (f)) {
					kh = new KeyHandler (rkey, f);
					rk = new RegistryKey (kh, CombineName (rkey, extra),
						writable);
					dir_to_handler [f] = kh;
					key_to_handler [rk] = kh;
				}
				return rk;
			}
		}

		static string CombineName (RegistryKey rkey, string extra)
		{
			if (extra.IndexOf ('/') != -1)
				extra = extra.Replace ('/', '\\');
			
			return String.Concat (rkey.Name, "\\", extra);
		}
		
		public static KeyHandler Lookup (RegistryKey rkey, bool createNonExisting)
		{
			lock (typeof (KeyHandler)){
				KeyHandler k = (KeyHandler) key_to_handler [rkey];
				if (k != null)
					return k;

				// when a non-root key is requested for no keyhandler exist
				// then that key must have been marked for deletion
				if (!rkey.IsRoot || !createNonExisting)
					return null;

				RegistryHive x = (RegistryHive) rkey.Hive;
				switch (x){
				case RegistryHive.CurrentUser:
					string userDir = Path.Combine (UserStore, x.ToString ());
					k = new KeyHandler (rkey, userDir);
					dir_to_handler [userDir] = k;
					break;
				case RegistryHive.CurrentConfig:
				case RegistryHive.ClassesRoot:
				case RegistryHive.DynData:
				case RegistryHive.LocalMachine:
				case RegistryHive.PerformanceData:
				case RegistryHive.Users:
					string machine_dir = Path.Combine (MachineStore, x.ToString ());
					k = new KeyHandler (rkey, machine_dir);
					dir_to_handler [machine_dir] = k;
					break;
				default:
					throw new Exception ("Unknown RegistryHive");
				}
				key_to_handler [rkey] = k;
				return k;
			}
		}

		public static void Drop (RegistryKey rkey)
		{
			lock (typeof (KeyHandler)) {
				KeyHandler k = (KeyHandler) key_to_handler [rkey];
				if (k == null)
					return;
				key_to_handler.Remove (rkey);

				// remove cached KeyHandler if no other keys reference it
				int refCount = 0;
				foreach (DictionaryEntry de in key_to_handler)
					if (de.Value == k)
						refCount++;
				if (refCount == 0)
					dir_to_handler.Remove (k.Dir);
			}
		}

		public static void Drop (string dir)
		{
			lock (typeof (KeyHandler)) {
				KeyHandler kh = (KeyHandler) dir_to_handler [dir];
				if (kh == null)
					return;

				dir_to_handler.Remove (dir);

				// remove (other) references to keyhandler
				ArrayList keys = new ArrayList ();
				foreach (DictionaryEntry de in key_to_handler)
					if (de.Value == kh)
						keys.Add (de.Key);

				foreach (object key in keys)
					key_to_handler.Remove (key);
			}
		}

		public object GetValue (string name, RegistryValueOptions options)
		{
			if (IsMarkedForDeletion)
				return null;

			if (name == null)
				name = string.Empty;
			object value = values [name];
			ExpandString exp = value as ExpandString;
			if (exp == null)
				return value;
			if ((options & RegistryValueOptions.DoNotExpandEnvironmentNames) == 0)
				return exp.Expand ();

			return exp.ToString ();
		}

		public void SetValue (string name, object value)
		{
			AssertNotMarkedForDeletion ();

			if (name == null)
				name = string.Empty;

			// immediately convert non-native registry values to string to avoid
			// returning it unmodified in calls to UnixRegistryApi.GetValue
			if (value is int || value is string || value is byte[] || value is string[])
				values[name] = value;
			else
				values[name] = value.ToString ();
			SetDirty ();
		}

		public string [] GetValueNames ()
		{
			AssertNotMarkedForDeletion ();

			ICollection keys = values.Keys;

			string [] vals = new string [keys.Count];
			keys.CopyTo (vals, 0);
			return vals;
		}

#if NET_2_0
		//
		// This version has to do argument validation based on the valueKind
		//
		public void SetValue (string name, object value, RegistryValueKind valueKind)
		{
			SetDirty ();
			switch (valueKind){
			case RegistryValueKind.String:
				if (value is string){
					values [name] = value;
					return;
				}
				break;
			case RegistryValueKind.ExpandString:
				if (value is string){
					values [name] = new ExpandString ((string)value);
					return;
				}
				break;
				
			case RegistryValueKind.Binary:
				if (value is byte []){
					values [name] = value;
					return;
				}
				break;
				
			case RegistryValueKind.DWord:
				if (value is long &&
				    (((long) value) < Int32.MaxValue) &&
				    (((long) value) > Int32.MinValue)){
					values [name] = (int) ((long)value);
					return;
				}
				if (value is int){
					values [name] = value;
					return;
				}
				break;
				
			case RegistryValueKind.MultiString:
				if (value is string []){
					values [name] = value;
					return;
				}
				break;
				
			case RegistryValueKind.QWord:
				if (value is int){
					values [name] = (long) ((int) value);
					return;
				}
				if (value is long){
					values [name] = value;
					return;
				}
				break;
			default:
				throw new ArgumentException ("unknown value", "valueKind");
			}
			throw new ArgumentException ("Value could not be converted to specified type", "valueKind");
		}
#endif

		void SetDirty ()
		{
			lock (typeof (KeyHandler)){
				if (dirty)
					return;
				dirty = true;
				new Timer (DirtyTimeout, null, 3000, Timeout.Infinite);
			}
		}

		public void DirtyTimeout (object state)
		{
			Flush ();
		}

		public void Flush ()
		{
			lock (typeof (KeyHandler)) {
				if (dirty) {
					Save ();
					dirty = false;
				}
			}
		}

		public bool ValueExists (string name)
		{
			if (name == null)
				name = string.Empty;

			return values.Contains (name);
		}

		public int ValueCount {
			get {
				return values.Keys.Count;
			}
		}

		public bool IsMarkedForDeletion {
			get {
				return !dir_to_handler.Contains (Dir);
			}
		}

		public void RemoveValue (string name)
		{
			AssertNotMarkedForDeletion ();

			values.Remove (name);
			SetDirty ();
		}

		~KeyHandler ()
		{
			Flush ();
		}
		
		void Save ()
		{
			if (IsMarkedForDeletion)
				return;

			if (!File.Exists (file) && values.Count == 0)
				return;

			SecurityElement se = new SecurityElement ("values");
			
			foreach (DictionaryEntry de in values){
				object val = de.Value;
				SecurityElement value = new SecurityElement ("value");
				value.AddAttribute ("name", (string) de.Key);
				
				if (val is string){
					value.AddAttribute ("type", "string");
					value.Text = (string) val;
				} else if (val is int){
					value.AddAttribute ("type", "int");
					value.Text = val.ToString ();
				} else if (val is long) {
					value.AddAttribute ("type", "qword");
					value.Text = val.ToString ();
				} else if (val is byte []){
					value.AddAttribute ("type", "bytearray");
					value.Text = Convert.ToBase64String ((byte[]) val);
				} else if (val is ExpandString){
					value.AddAttribute ("type", "expand");
					value.Text = val.ToString ();
				} else if (val is string []){
					value.AddAttribute ("type", "string-array");

					foreach (string ss in (string[]) val){
						SecurityElement str = new SecurityElement ("string");
						str.Text = ss; 
						value.AddChild (str);
					}
				}
				se.AddChild (value);
			}

			using (FileStream fs = File.Create (file)){
				StreamWriter sw = new StreamWriter (fs);

				sw.Write (se.ToString ());
				sw.Flush ();
			}
		}

		private void AssertNotMarkedForDeletion ()
		{
			if (IsMarkedForDeletion)
				throw RegistryKey.CreateMarkedForDeletionException ();
		}

		private static string UserStore {
			get {
				return Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Personal),
					".mono/registry");
			}
		}

		private static string MachineStore {
			get {
				string s;

				s = Environment.GetEnvironmentVariable ("MONO_REGISTRY_PATH");
				if (s != null)
					return s;
				s = Environment.GetMachineConfigPath ();
				int p = s.IndexOf ("machine.config");
				return Path.Combine (Path.Combine (s.Substring (0, p-1), ".."), "registry");
			}
		}
	}
	
	internal class UnixRegistryApi : IRegistryApi {

		static string ToUnix (string keyname)
		{
			if (keyname.IndexOf ('\\') != -1)
				keyname = keyname.Replace ('\\', '/');
			return keyname.ToLower ();
		}

		static bool IsWellKnownKey (string parentKeyName, string keyname)
		{
			// FIXME: Add more keys if needed
			if (parentKeyName == Registry.CurrentUser.Name ||
				parentKeyName == Registry.LocalMachine.Name)
				return (0 == String.Compare ("software", keyname, true, CultureInfo.InvariantCulture));

			return false;
		}

		public RegistryKey CreateSubKey (RegistryKey rkey, string keyname)
		{
			return CreateSubKey (rkey, keyname, true);
		}

		public RegistryKey OpenRemoteBaseKey (RegistryHive hKey, string machineName)
		{
			throw new NotImplementedException ();
		}

		public RegistryKey OpenSubKey (RegistryKey rkey, string keyname, bool writable)
		{
			KeyHandler self = KeyHandler.Lookup (rkey, true);
			if (self == null) {
				// return null if parent is marked for deletion
				return null;
			}

			RegistryKey result = self.Probe (rkey, ToUnix (keyname), writable);
			if (result == null && IsWellKnownKey (rkey.Name, keyname)) {
				// create the subkey even if its parent was opened read-only
				result = CreateSubKey (rkey, keyname, false);
			}

			return result;
		}
		
		public void Flush (RegistryKey rkey)
		{
			KeyHandler self = KeyHandler.Lookup (rkey, false);
			if (self == null) {
				// we do not need to flush changes as key is marked for deletion
				return;
			}
			self.Flush ();
		}
		
		public void Close (RegistryKey rkey)
		{
			KeyHandler.Drop (rkey);
		}

		public object GetValue (RegistryKey rkey, string name, object default_value, RegistryValueOptions options)
		{
			KeyHandler self = KeyHandler.Lookup (rkey, true);
			if (self == null) {
				// key was removed since it was opened
				return default_value;
			}

			if (self.ValueExists (name))
				return self.GetValue (name, options);
			return default_value;
		}
		
		public void SetValue (RegistryKey rkey, string name, object value)
		{
			KeyHandler self = KeyHandler.Lookup (rkey, true);
			if (self == null)
				throw RegistryKey.CreateMarkedForDeletionException ();
			self.SetValue (name, value);
		}

#if NET_2_0
		public void SetValue (RegistryKey rkey, string name, object value, RegistryValueKind valueKind)
		{
			KeyHandler self = KeyHandler.Lookup (rkey, true);
			if (self == null)
				throw RegistryKey.CreateMarkedForDeletionException ();
			self.SetValue (name, value, valueKind);
		}
#endif

		public int SubKeyCount (RegistryKey rkey)
		{
			KeyHandler self = KeyHandler.Lookup (rkey, true);
			if (self == null)
				throw RegistryKey.CreateMarkedForDeletionException ();
			return Directory.GetDirectories (self.Dir).Length;
		}
		
		public int ValueCount (RegistryKey rkey)
		{
			KeyHandler self = KeyHandler.Lookup (rkey, true);
			if (self == null)
				throw RegistryKey.CreateMarkedForDeletionException ();
			return self.ValueCount;
		}
		
		public void DeleteValue (RegistryKey rkey, string name, bool throw_if_missing)
		{
			KeyHandler self = KeyHandler.Lookup (rkey, true);
			if (self == null) {
				// if key is marked for deletion, report success regardless of
				// throw_if_missing
				return;
			}

			if (throw_if_missing && !self.ValueExists (name))
				throw new ArgumentException ("the given value does not exist", "name");

			self.RemoveValue (name);
		}
		
		public void DeleteKey (RegistryKey rkey, string keyname, bool throw_if_missing)
		{
			KeyHandler self = KeyHandler.Lookup (rkey, true);
			if (self == null) {
				// key is marked for deletion
				if (!throw_if_missing)
					return;
				throw new ArgumentException ("the given value does not exist", "value");
			}

			string dir = Path.Combine (self.Dir, ToUnix (keyname));
			
			if (Directory.Exists (dir)){
				Directory.Delete (dir, true);
				KeyHandler.Drop (dir);
			} else if (throw_if_missing)
				throw new ArgumentException ("the given value does not exist", "value");
		}
		
		public string [] GetSubKeyNames (RegistryKey rkey)
		{
			KeyHandler self = KeyHandler.Lookup (rkey, true);
			DirectoryInfo selfDir = new DirectoryInfo (self.Dir);
			DirectoryInfo[] subDirs = selfDir.GetDirectories ();
			string[] subKeyNames = new string[subDirs.Length];
			for (int i = 0; i < subDirs.Length; i++) {
				DirectoryInfo subDir = subDirs[i];
				subKeyNames[i] = subDir.Name;
			}
			return subKeyNames;
		}
		
		public string [] GetValueNames (RegistryKey rkey)
		{
			KeyHandler self = KeyHandler.Lookup (rkey, true);
			if (self == null)
				throw RegistryKey.CreateMarkedForDeletionException ();
			return self.GetValueNames ();
		}

		public string ToString (RegistryKey rkey)
		{
			return rkey.Name;
		}

		private RegistryKey CreateSubKey (RegistryKey rkey, string keyname, bool writable)
		{
			KeyHandler self = KeyHandler.Lookup (rkey, true);
			if (self == null)
				throw RegistryKey.CreateMarkedForDeletionException ();
			return self.Ensure (rkey, ToUnix (keyname), writable);
		}
	}
}
