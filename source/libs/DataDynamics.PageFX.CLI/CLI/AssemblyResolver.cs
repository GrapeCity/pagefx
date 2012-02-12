using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using DataDynamics.PageFX.CLI.GAC;
using DataDynamics.PageFX.CLI.Metadata;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI
{
    internal static class AssemblyResolver
    {
        static readonly PathResolver _pathResolver;

        static AssemblyResolver()
        {
            _pathResolver = new PathResolver();
            _pathResolver.AddEnvironmentVariable("LIB");
        }

        #region GetCacLocation
        static string GetCacLocation(IAssemblyReference id)
        {
            IAssemblyCache cache = null;
            ASSEMBLY_INFO asmInfo;

            try
            {
                cache = AssemblyCache.CreateAssemblyCache();
                using (var e = new GacEnum(id.Name))
                {
                    string displayName = null;
                    foreach (var name in e)
                    {
                        if (name.Version >= id.Version)
                            displayName = name.DisplayName;
                        name.Dispose();
                        if (displayName != null)
                            break;
                    }

                    asmInfo = new ASSEMBLY_INFO();
                    int hr;
                    try
                    {
                        hr = cache.QueryAssemblyInfo((uint)AssemblyInfoFlag.Validate, displayName, ref asmInfo);
                        if (0 != hr && (uint)hr != AssemblyCache.HResults.E_INSUFFICIENT_BUFFER)
                            return string.Empty;
                    }
                    catch
                    {
                        return string.Empty;
                    }

                    if (asmInfo.cchBuf != 0)
                    {
                        asmInfo.pszCurrentAssemblyPathBuf = new string(new char[asmInfo.cchBuf]);
                        hr = cache.QueryAssemblyInfo((uint)AssemblyInfoFlag.Validate, displayName, ref asmInfo);
                        if (0 != hr)
                            return string.Empty;
                        return asmInfo.pszCurrentAssemblyPathBuf;
                    }
                }
            }
            catch (SecurityException)
            {
            }
            finally
            {
                if (cache != null)
                    Marshal.ReleaseComObject(cache);
            }
            return string.Empty;
        }
        #endregion

        static string ResolvePath(string path, string refpath)
        {
            string dir = Path.GetDirectoryName(refpath);
            string fullpath = Path.Combine(dir, path);
            if (File.Exists(fullpath)) return fullpath;
            path = _pathResolver.Resolve(path);
            if (!string.IsNullOrEmpty(path))
                return path;
            return null;
        }

        private static IEnumerable<string> GetLibDirs()
        {
            var dirs = new[]
                {
                    GlobalSettings.LibsDirectory,
                    GlobalSettings.FlexLibsDirectory,
                };
        	return dirs.Where(Directory.Exists).ToArray();
        }

        static string GetAssemblyLocation(IAssemblyReference asmref, string refpath)
        {
            string path;

            string refname = asmref.Name;

            if (Infrastructure.SubstituteFrameworkAssemblies && IsFrameworkAssembly(asmref))
            {
                path = GetPfxLocation(refname);
                if (!string.IsNullOrEmpty(path))
                    return path;
            }

            path = ResolvePath(refname + ".dll", refpath);
            if (!string.IsNullOrEmpty(path))
                return path;

            path = ResolvePath(refname + ".exe", refpath);
            if (!string.IsNullOrEmpty(path))
                return path;

            if (asmref.PublicKeyToken != null)
            {
                path = GetCacLocation(asmref);
                if (!string.IsNullOrEmpty(path))
                    return path;
            }

            return GetPfxLocation(refname);
        }

        static string GetPfxLocation(string refname)
        {
            if (string.Compare(refname, GlobalSettings.CorlibAssemblyName, true) == 0)
                return GlobalSettings.GetCorlibPath(true);

            var libdirs = GetLibDirs();
            if (libdirs != null)
            {
            	return libdirs.Select(libdir => Path.Combine(libdir, refname + ".dll")).FirstOrDefault(File.Exists);
            }

            return null;
        }

        static readonly Dictionary<IAssemblyReference, IAssembly> _cache = new Dictionary<IAssemblyReference, IAssembly>();

        public static IAssembly GetFromCache(IAssemblyReference id)
        {
            IAssembly asm;
            if (_cache.TryGetValue(id, out asm))
                return asm;
            return null;
        }

        public static void AddToCache(IAssembly asm)
        {
            var id = new AssemblyReference(asm);
            if (!_cache.ContainsKey(id))
            {
                _cache.Add(id, asm);
            }
        }

        static readonly string[] SysRefs = 
        {
            "mscorlib",
            "System",
            "Microsoft.VisualBasic",
            "NUnit.Framework",
        };

        public static bool IsFrameworkAssembly(IAssemblyReference asmref)
        {
            string refname = asmref.Name;
            if (Array.FindIndex(SysRefs,
                                sr => string.Compare(refname, sr, true) == 0) >= 0)
                return true;
            return refname.StartsWith("System.", StringComparison.InvariantCultureIgnoreCase);
        }

        public static IAssembly ResolveAssembly(IAssemblyReference asmref, string refpath)
        {
            var asm = GetFromCache(asmref);
            if (asm != null) return asm;
            string path = GetAssemblyLocation(asmref, refpath);
            if (string.IsNullOrEmpty(path))
                throw new BadMetadataException(string.Format("Unable to resole assembly {0}", asmref));
            if (!File.Exists(path))
                throw new BadMetadataException(string.Format("Unable to resole assembly {0}", asmref));
            return AssemblyLoader.Load(path);
        }

        public static void ClearCache()
        {
            _cache.Clear();
        }
    }
}