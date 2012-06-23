using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DataDynamics.PageFX
{
    /// <summary>
    /// Contains global PageFX settings
    /// </summary>
    public static class GlobalSettings
    {
        static readonly string[] SysRefs =
        {
            "System",
            "System.Core",
            "System.Xml",
            "System.Data",
            "System.Drawing"
        };

        public static readonly string[] CommonAssemblies;

        static GlobalSettings()
        {
            int n = SysRefs.Length;
            CommonAssemblies = new string[n];
            for (int i = 0; i < n; ++i)
                CommonAssemblies[i] = SysRefs[i] + ".dll";
        }

        #region GetVar
        static string GetVar(string name, params EnvironmentVariableTarget[] targets)
        {
            int n = targets.Length;
            for (int i = 0; i < n; ++i)
            {
                string v = Environment.GetEnvironmentVariable(name, targets[i]);
                if (!string.IsNullOrEmpty(v))
                    return v;
            }
            return null;
        }

        public static string GetVar(string name)
        {
            return GetVar(name, EnvironmentVariableTarget.Process,
                          EnvironmentVariableTarget.User,
                          EnvironmentVariableTarget.Machine);
        }
        #endregion

        #region PageFX Pathes
        static string GetEnvDir(string var)
        {
            string dir = GetVar(var);
            if (!string.IsNullOrEmpty(dir))
            {
                dir = dir.Unquote();
                if (Directory.Exists(dir))
                    return dir;
            }
            return null;
        }

        /// <summary>
        /// Gets PageFX home directory.
        /// </summary>
        public static string HomeDirectory
        {
            get
            {
                //TODO: Read pfx home dir from config
                string dir = GetEnvDir("PFXHOME");
                if (dir != null) return dir;
                return "c:\\pfx";
            }
        }

        /// <summary>
        /// Gets the PageFX directory where binaries are located.
        /// </summary>
        public static string BinDirectory
        {
            get
            {
                return Path.Combine(HomeDirectory, "bin");
            }
        }

        /// <summary>
        /// Gets the PageFX directory with .NET framework libraries
        /// </summary>
        public static string LibsDirectory
        {
            get
            {
                string dir = GetEnvDir("PFXLIB");
                if (!string.IsNullOrEmpty(dir) && Directory.Exists(dir))
                    return dir;
                dir = Path.Combine(HomeDirectory, "framework");
                if (!string.IsNullOrEmpty(dir) && Directory.Exists(dir))
                    return dir;
                dir = Path.Combine(HomeDirectory, "libs");
                return dir;
            }
        }

        /// <summary>
        /// Gets full path to PageFX compiler.
        /// </summary>
        public static string PfcPath
        {
            get
            {
                return Path.Combine(BinDirectory, "pfc.exe");
            }
        }

        public static string ToolsDirectory
        {
            get
            {
                return Path.Combine(HomeDirectory, "tools");
            }
        }
        #endregion

        #region Dirs
        public static class Dirs
        {
            public static string Home
            {
                get { return HomeDirectory; }
            }

            public static string Bin
            {
                get { return BinDirectory; }
            }

            public static string Libs
            {
                get { return LibsDirectory; }
            }

            public static string Tools
            {
                get { return ToolsDirectory; }
            }

            public static string HtmlTemplates
            {
                get { return Path.Combine(HomeDirectory, "HTML Templates"); }
            }
        }
        #endregion

        #region Flex Options
        public static string FlexSdkDirectory
        {
            get
            {
                string dir = GetEnvDir("PFXFLEXSDK");
                if (dir != null) return dir;
                return Path.Combine(HomeDirectory, "flexsdk");
                //return Path.Combine(LibsDirectory, "flex");
            }
        }

        public static string RslsDirectory
        {
            get
            {
                //return Path.Combine(HomeDirectory, "rsls");
                return FlexSdkDirectory;
            }
        }

        public static string LocaleDirectory
        {
            get
            {
                return Path.Combine(FlexSdkDirectory, "locale");
                //return Path.Combine(HomeDirectory, "locale");
            }
        }

        public static string FlexLibsDirectory
        {
            get
            {
                return FlexSdkDirectory;
                //return Path.Combine(LibsDirectory, "flex");
            }
        }

        public static string GetMxLibraryPath()
        {
            return Path.Combine(FlexLibsDirectory, "flex3.dll");
        }
        #endregion

        #region Utils
        public const string CorlibAssemblyName = "mscorlib";
        const string corlibdll = CorlibAssemblyName + ".dll";

        public static string GetCorlibPath(bool check)
        {
            string dir = LibsDirectory;
            if (check && !Directory.Exists(dir))
                throw new DirectoryNotFoundException("PageFX lib directory does not exist");

            string path = Path.Combine(dir, corlibdll);
            if (check && !File.Exists(path))
                throw new FileNotFoundException(
                    string.Format("PageFX {0} does not exist", corlibdll));

            return path;
        }

        public static string GetLibPath(string name)
        {
            return Path.Combine(LibsDirectory, name);
        }

        

        public static void AddCommonReferences(CompilerOptions options)
        {
            options.AddRef(GetCorlibPath(true));
            foreach (var cr in CommonAssemblies)
            {
                string lib = GetLibPath(cr);
                if (File.Exists(lib))
                    options.AddRef(lib);
            }
        }

        public static List<string> GetRefs(CommandLine cl)
        {
            var refs = new List<string>();
            if (cl != null)
            {
                refs.AddRange(cl.GetOptions("r", "ref", "reference"));
                if (cl.HasOption("mx"))
                    refs.Add(GetMxLibraryPath());
            }
            return refs;
        }

        static bool IsSysRef(string r, string sysref)
        {
            string name = Path.GetFileName(r);
            if (string.Compare(name, sysref, true) == 0)
                return true;
            if (string.Compare(name, sysref + ".dll", true) == 0)
                return true;
            return false;
        }

        static bool IsSysRef(string r)
        {
            string name = Path.GetFileName(r);
            foreach (var sr in SysRefs)
            {
                if (string.Compare(name, sr, true) == 0)
                    return true;
                if (string.Compare(name, sr + ".dll", true) == 0)
                    return true;
            }
            return false;
        }

        public static bool HasCorlibRef(IEnumerable<string> refs)
        {
            if (refs == null) return false;
        	return refs.Any(r => IsSysRef(r, CorlibAssemblyName));
        }

        public static bool HasSysRefs(IEnumerable<string> refs)
        {
            if (refs == null) return false;
            return refs.Any(IsSysRef);
        }
        #endregion

        #region Libs
        public static class Libs
        {
            public static string Corlib
            {
                get { return GetCorlibPath(false); }
            }

            public static string VBRuntime
            {
                get { return GetLibPath("Microsoft.VisualBasic.dll"); }
            }
        }
        #endregion

        public static bool EmitDebugInfo = true;
		public static bool EmitDebugDisplay;
        public static bool EncodeDebugFile = true;
        public static bool ReflectionSupport;
        public static bool EnableOptimization = true;

        public static void SetGlobalOptions(CommandLine cl)
        {
            if (cl == null) return;
            if (cl.HasOption("debug-"))
                EmitDebugInfo = false;
            if (cl.HasOption("optimize-"))
                EnableOptimization = false;
            if (cl.HasOption(PFCOptions.GoodDebugFiles))
                EncodeDebugFile = false;
            if (cl.HasOption(PFCOptions.ReflectionSupport))
                ReflectionSupport = true;
        }
    }
}