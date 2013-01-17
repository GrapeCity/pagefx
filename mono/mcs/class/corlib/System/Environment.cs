//CHANGED

//------------------------------------------------------------------------------
// 
// System.Environment.cs 
//
// Copyright (C) 2001 Moonlight Enterprises, All Rights Reserved
// 
// Author:         Jim Richardson, develop@wtfo-guru.com
//                 Dan Lewis (dihlewis@yahoo.co.uk)
// Created:        Saturday, August 11, 2001 
//
//------------------------------------------------------------------------------
//
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

using System.IO;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Security.Permissions;
using System.Text;

namespace System
{
#if NET_2_0
	public static class Environment {
#else
    public sealed class Environment
    {

        private Environment()
        {
        }
#endif
        /*
		 * This is the version number of the corlib-runtime interface. When
		 * making changes to this interface (by changing the layout
		 * of classes the runtime knows about, changing icall signature or
		 * semantics etc), increment this variable. Also increment the
		 * pair of this variable in the runtime in metadata/appdomain.c.
		 * Changes which are already detected at runtime, like the addition
		 * of icalls, do not require an increment.
		 */
        private const int mono_corlib_version = 58;

        public enum SpecialFolder
        {	// TODO: Determine if these windoze style folder identifiers 
            //       have unix/linux counterparts
#if NET_2_0
			MyDocuments = 0x05,
#endif
#if NET_1_1
            Desktop = 0x00,
            MyComputer = 0x11,
#endif
            Programs = 0x02,
            Personal = 0x05,
            Favorites = 0x06,
            Startup = 0x07,
            Recent = 0x08,
            SendTo = 0x09,
            StartMenu = 0x0b,
            MyMusic = 0x0d,
            DesktopDirectory = 0x10,
            Templates = 0x15,
            ApplicationData = 0x1a,
            LocalApplicationData = 0x1c,
            InternetCache = 0x20,
            Cookies = 0x21,
            History = 0x22,
            CommonApplicationData = 0x23,
            System = 0x25,
            ProgramFiles = 0x26,
            MyPictures = 0x27,
            CommonProgramFiles = 0x2b,
        }

        /// <summary>
        /// Gets the command line for this process
        /// </summary>
        internal static string CommandLine
        {
            // note: security demand inherited from calling GetCommandLineArgs
            get
            {
                // FIXME: we may need to quote, but any sane person
                // should use GetCommandLineArgs () instead.
                return String.Join(" ", GetCommandLineArgs());
            }
        }

        /// <summary>
        /// Gets or sets the current directory. Actually this is supposed to get
        /// and/or set the process start directory acording to the documentation
        /// but actually test revealed at beta2 it is just Getting/Setting the CurrentDirectory
        /// </summary>

        public static string CurrentDirectory
        {
            get
            {
#if NOT_PFX
                return Directory.GetCurrentDirectory();
#else
                return PfxIO.CurrentDirectory;
#endif
            }
            set
            {
#if NOT_PFX
                Directory.SetCurrentDirectory(value);
#else
                PfxIO.CurrentDirectory = value;
#endif
            }
        }


        /// <summary>
        /// Gets the standard new line value
        /// </summary>
        public static string NewLine
        {
            get
            {
                return PfxIO.NewLine;
            }
        }

        //
        // Support methods and fields for OSVersion property
        //
        static OperatingSystem os;

        internal static PlatformID Platform
        {
            get
            {
                return PlatformID.Win32Windows;
            }
        }

        internal static string GetOSVersionString()
        {
            return "1.0";
        }

        /// <summary>
        /// Gets the current OS version information
        /// </summary>
        internal static OperatingSystem OSVersion
        {
            get
            {
                if (os == null)
                {
                    Version v = Version.CreateFromString(GetOSVersionString());
                    PlatformID p = Platform;
#if NET_2_0
					if ((int) p == 128)
						p = PlatformID.Unix;
#endif
                    os = new OperatingSystem(p, v);
                }
                return os;
            }
        }

        /// <summary>
        /// Get StackTrace
        /// </summary>
        internal static string StackTrace
        {
            get
            {
                //if (flash.system.Capabilities.isDebugger)
                //{
                //    try
                //    {
                //        ThrowAvmError();
                //    }
                //    catch (Avm.Error err)
                //    {
                //        //TODO: Trancate trace.
                //        return err.getStackTrace();
                //    }
                //}
                return "";
            }
        }

        //static void ThrowAvmError()
        //{
        //    throw new Avm.Error("");
        //}

        /// <summary>
        /// Get a fully qualified path to the system directory
        /// </summary>
        internal static string SystemDirectory
        {
            get
            {
                return GetFolderPath(SpecialFolder.System);
            }
        }

        /// <summary>
        /// Get the number of milliseconds that have elapsed since the system was booted
        /// </summary>
        public static int TickCount
        {
            get
            {
                if (avm.IsFlashPlayer)
                    return avm.getTimer();
                return 0;
            }
        }
        
        /// <summary>
        /// Get the version of the common language runtime 
        /// </summary>
        public static Version Version
        {
            get
            {
                return new Version(Consts.FxFileVersion);
            }
        }

        /// <summary>
        /// Get the amount of physical memory mapped to process
        /// </summary>
        internal static long WorkingSet
        {
            //[EnvironmentPermission(SecurityAction.Demand, Unrestricted = true)]
            get { return 0; }
        }

        [SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
        public static void Exit(int exitCode)
        {
            avm.exit(exitCode);
        }

        /// <summary>
        /// Substitute environment variables in the argument "name"
        /// </summary>
        internal static string ExpandEnvironmentVariables(string name)
        {
            if (name == null)
                throw new ArgumentNullException("name");

            int off1 = name.IndexOf('%');
            if (off1 == -1)
                return name;

            int len = name.Length;
            int off2 = 0;
            if (off1 == len - 1 || (off2 = name.IndexOf('%', off1 + 1)) == -1)
                return name;

            StringBuilder result = new StringBuilder();
            result.Append(name, 0, off1);
            Hashtable tbl = null;
            do
            {
                string var = name.Substring(off1 + 1, off2 - off1 - 1);
                string value = GetEnvironmentVariable(var);
                if (value == null && Environment.IsRunningOnWindows)
                {
                    // On windows, env. vars. are case insensitive
                    if (tbl == null)
                        tbl = GetEnvironmentVariablesNoCase();

                    value = tbl[var] as string;
                }

                // If value not found, add %FOO to stream,
                //  and use the closing % for the next iteration.
                // If value found, expand it in place of %FOO%
                if (value == null)
                {
                    result.Append('%');
                    result.Append(var);
                    off2--;
                }
                else
                {
                    result.Append(value);
                }
                int oldOff2 = off2;
                off1 = name.IndexOf('%', off2 + 1);
                // If no % found for off1, don't look for one for off2
                off2 = (off1 == -1 || off2 > len - 1) ? -1 : name.IndexOf('%', off1 + 1);
                // textLen is the length of text between the closing % of current iteration
                //  and the starting % of the next iteration if any. This text is added to output
                int textLen;
                // If no new % found, use all the remaining text
                if (off1 == -1 || off2 == -1)
                    textLen = len - oldOff2 - 1;
                // If value found in current iteration, use text after current closing % and next %
                else if (value != null)
                    textLen = off1 - oldOff2 - 1;
                // If value not found in current iteration, but a % was found for next iteration,
                //  use text from current closing % to the next %.
                else
                    textLen = off1 - oldOff2;
                if (off1 >= oldOff2 || off1 == -1)
                    result.Append(name, oldOff2 + 1, textLen);
            } while (off2 > -1 && off2 < len);

            return result.ToString();

        }

        /// <summary>
        /// Return an array of the command line arguments of the current process
        /// </summary>
        //[EnvironmentPermissionAttribute(SecurityAction.Demand, Read = "PATH")]
        internal static string[] GetCommandLineArgs()
        {
            return new string[0];
        }

        /// <summary>
        /// Return a string containing the value of the environment
        /// variable identifed by parameter "variable"
        /// </summary>
        internal static string GetEnvironmentVariable(string name)
        {
            if (string.Compare(name, "SYSTEMROOT", true) == 0)
            {
                if (IsRunningOnWindows)
                    return "C:\\Windows";
            }
            return "";
        }

        static Hashtable GetEnvironmentVariablesNoCase()
        {
            Hashtable vars = new Hashtable(CaseInsensitiveHashCodeProvider.Default,
                            CaseInsensitiveComparer.Default);

            foreach (string name in GetEnvironmentVariableNames())
            {
                vars[name] = GetEnvironmentVariable(name);
            }

            return vars;
        }

        /// <summary>
        /// Return a set of all environment variables and their values
        /// </summary>
        //[EnvironmentPermission(SecurityAction.Demand, Unrestricted = true)]
        internal static IDictionary GetEnvironmentVariables()
        {
            Hashtable vars = new Hashtable();
            foreach (string name in GetEnvironmentVariableNames())
            {
                vars[name] = GetEnvironmentVariable(name);
            }
            return vars;
        }

        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        private extern static string GetWindowsFolderPath(int folder);

        /// <summary>
        /// Returns the fully qualified path of the
        /// folder specified by the "folder" parameter
        /// </summary>
        internal static string GetFolderPath(SpecialFolder folder)
        {
            string dir = null;

            if (Environment.IsRunningOnWindows)
            {
                dir = GetWindowsFolderPath((int)folder);
            }
            else
            {
                dir = InternalGetFolderPath(folder);
            }

            //if ((dir != null) && (dir.Length > 0) && SecurityManager.SecurityEnabled)
            //{
            //    new FileIOPermission(FileIOPermissionAccess.PathDiscovery, dir).Demand();
            //}
            return dir;
        }

        private static string ReadXdgUserDir(string config_dir, string home_dir,
            string key, string fallback)
        {
            //string env_path = GetEnvironmentVariable(key);
            //if (env_path != null && env_path != String.Empty)
            //{
            //    return env_path;
            //}

            //string user_dirs_path = Path.Combine(config_dir, "user-dirs.dirs");

            //if (!File.Exists(user_dirs_path))
            //{
            //    return Path.Combine(home_dir, fallback);
            //}

            //try
            //{
            //    using (StreamReader reader = new StreamReader(user_dirs_path))
            //    {
            //        string line;
            //        while ((line = reader.ReadLine()) != null)
            //        {
            //            line = line.Trim();
            //            int delim_index = line.IndexOf('=');
            //            if (delim_index > 8 && line.Substring(0, delim_index) == key)
            //            {
            //                return Path.Combine(home_dir, line.Substring(delim_index + 1));
            //            }
            //        }
            //    }
            //}
            //catch (FileNotFoundException)
            //{
            //}

            //return Path.Combine(home_dir, fallback);
            throw new NotImplementedException("Environment.ReadXdgUserDir");
        }


        // the security runtime (and maybe other parts of corlib) needs the
        // information to initialize themselves before permissions can be checked
        internal static string InternalGetFolderPath(SpecialFolder folder)
        {
            string home = internalGetHome();

            // http://freedesktop.org/Standards/basedir-spec/basedir-spec-0.6.html

            // note: skip security check for environment variables
            string data = GetEnvironmentVariable("XDG_DATA_HOME");
            if ((data == null) || (data == String.Empty))
            {
                data = Path.Combine(home, ".local");
                data = Path.Combine(data, "share");
            }

            // note: skip security check for environment variables
            string config = GetEnvironmentVariable("XDG_CONFIG_HOME");
            if ((config == null) || (config == String.Empty))
            {
                config = Path.Combine(home, ".config");
            }

            switch (folder)
            {
#if NET_1_1
                // MyComputer is a virtual directory
                case SpecialFolder.MyComputer:
                    return String.Empty;
#endif
                // personal == ~
                case SpecialFolder.Personal:
                    return home;
                // use FDO's CONFIG_HOME. This data will be synced across a network like the windows counterpart.
                case SpecialFolder.ApplicationData:
                    return config;
                //use FDO's DATA_HOME. This is *NOT* synced
                case SpecialFolder.LocalApplicationData:
                    return data;
#if NET_1_1
                case SpecialFolder.Desktop:
#endif
                case SpecialFolder.DesktopDirectory:
                    return ReadXdgUserDir(config, home, "XDG_DESKTOP_DIR", "Desktop");

                case SpecialFolder.MyMusic:
                    return ReadXdgUserDir(config, home, "XDG_MUSIC_DIR", "Music");

                case SpecialFolder.MyPictures:
                    return ReadXdgUserDir(config, home, "XDG_PICTURES_DIR", "Pictures");

                // these simply dont exist on Linux
                // The spec says if a folder doesnt exist, we
                // should return ""
                case SpecialFolder.Favorites:
                case SpecialFolder.Programs:
                case SpecialFolder.SendTo:
                case SpecialFolder.StartMenu:
                case SpecialFolder.Startup:
                case SpecialFolder.Templates:
                case SpecialFolder.Cookies:
                case SpecialFolder.History:
                case SpecialFolder.InternetCache:
                case SpecialFolder.Recent:
                case SpecialFolder.CommonProgramFiles:
                case SpecialFolder.ProgramFiles:
                case SpecialFolder.System:
                    return String.Empty;
                // This is where data common to all users goes
                case SpecialFolder.CommonApplicationData:
                    return "/usr/share";
                default:
                    throw new ArgumentException("Invalid SpecialFolder");
            }
        }

        //[EnvironmentPermission(SecurityAction.Demand, Unrestricted = true)]
        internal static string[] GetLogicalDrives()
        {
            return GetLogicalDrivesInternal();
        }

        // FIXME: Anyone using this anywhere ?
        static internal string GetResourceString(string s) { return String.Empty; }



#if NET_2_0
#if NOT_PFX
		public static string GetEnvironmentVariable (string variable, EnvironmentVariableTarget target)
		{
			switch (target) {
			case EnvironmentVariableTarget.Process:
				return GetEnvironmentVariable (variable);
			case EnvironmentVariableTarget.Machine:
				new EnvironmentPermission (PermissionState.Unrestricted).Demand ();
				if (!IsRunningOnWindows)
					return null;
				using (Microsoft.Win32.RegistryKey env = Microsoft.Win32.Registry.LocalMachine.OpenSubKey (@"SYSTEM\CurrentControlSet\Control\Session Manager\Environment")) {
					return env.GetValue (variable).ToString ();
				}
			case EnvironmentVariableTarget.User:
				new EnvironmentPermission (PermissionState.Unrestricted).Demand ();
				if (!IsRunningOnWindows)
					return null;
				using (Microsoft.Win32.RegistryKey env = Microsoft.Win32.Registry.CurrentUser.OpenSubKey ("Environment", false)) {
					return env.GetValue (variable).ToString ();
				}
			default:
				throw new ArgumentException ("target");
			}
		}


		public static IDictionary GetEnvironmentVariables (EnvironmentVariableTarget target)
		{
			IDictionary variables = (IDictionary)new Hashtable ();
			switch (target) {
			case EnvironmentVariableTarget.Process:
				variables = GetEnvironmentVariables ();
				break;
			case EnvironmentVariableTarget.Machine:
				new EnvironmentPermission (PermissionState.Unrestricted).Demand ();
				if (IsRunningOnWindows) {
					using (Microsoft.Win32.RegistryKey env = Microsoft.Win32.Registry.LocalMachine.OpenSubKey (@"SYSTEM\CurrentControlSet\Control\Session Manager\Environment")) {
						string[] value_names = env.GetValueNames ();
						foreach (string value_name in value_names)
							variables.Add (value_name, env.GetValue (value_name));
					}
				}
				break;
			case EnvironmentVariableTarget.User:
				new EnvironmentPermission (PermissionState.Unrestricted).Demand ();
				if (IsRunningOnWindows) {
					using (Microsoft.Win32.RegistryKey env = Microsoft.Win32.Registry.CurrentUser.OpenSubKey ("Environment")) {
						string[] value_names = env.GetValueNames ();
						foreach (string value_name in value_names)
							variables.Add (value_name, env.GetValue (value_name));
					}
				}
				break;
			default:
				throw new ArgumentException ("target");
			}
			return variables;
		}

        public static void SetEnvironmentVariable (string variable, string value)
		{
			SetEnvironmentVariable (variable, value, EnvironmentVariableTarget.Process);
		}

		public static void SetEnvironmentVariable (string variable, string value, EnvironmentVariableTarget target)
		{
			if (variable == null)
				throw new ArgumentNullException ("variable");
			if (variable == String.Empty)
				throw new ArgumentException ("String cannot be of zero length.", "variable");
			if (variable.IndexOf ('=') != -1)
				throw new ArgumentException ("Environment variable name cannot contain an equal character.", "variable");
			if (variable[0] == '\0')
				throw new ArgumentException ("The first char in the string is the null character.", "variable");

			switch (target) {
			case EnvironmentVariableTarget.Process:
				InternalSetEnvironmentVariable (variable, value);
				break;
			case EnvironmentVariableTarget.Machine:
				if (!IsRunningOnWindows)
					return;
				using (Microsoft.Win32.RegistryKey env = Microsoft.Win32.Registry.LocalMachine.OpenSubKey (@"SYSTEM\CurrentControlSet\Control\Session Manager\Environment", true)) {
					if (value == null || value.Length == 0)
						env.DeleteValue (variable, false);
					else
						env.SetValue (variable, value);
				}
				break;
			case EnvironmentVariableTarget.User:
				if (!IsRunningOnWindows)
					return;
				using (Microsoft.Win32.RegistryKey env = Microsoft.Win32.Registry.CurrentUser.OpenSubKey ("Environment", true)) {
					if (value == null || value.Length == 0)
						env.DeleteValue (variable, false);
					else
						env.SetValue (variable, value);
				}
				break;
			default:
				throw new ArgumentException ("target");
			}
		}
#endif

		[MethodImplAttribute (MethodImplOptions.InternalCall)]
		internal static extern void InternalSetEnvironmentVariable (string variable, string value);

		
        public static extern int ProcessorCount {
			[MethodImplAttribute (MethodImplOptions.InternalCall)]
			get;			
		}


		[MonoTODO ("Not implemented")]
		[SecurityPermission (SecurityAction.LinkDemand, UnmanagedCode=true)]
		public static void FailFast (string message)
		{
			throw new NotImplementedException ();
		}
#endif

        // private methods

        internal static bool IsRunningOnWindows
        {
            get { return ((int)Platform != 128); }
        }

        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        private extern static string[] GetLogicalDrivesInternal();

        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        private extern static string[] GetEnvironmentVariableNames();

        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        internal extern static string GetMachineConfigPath();

        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        internal extern static string internalGetHome();
    }
}

