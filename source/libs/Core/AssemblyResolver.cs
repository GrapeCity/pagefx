using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DataDynamics.PageFX.Common.IO;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Core.Metadata;

namespace DataDynamics.PageFX.Core
{
    internal static class AssemblyResolver
    {
        private static readonly PathResolver PathResolver;

        static AssemblyResolver()
        {
            PathResolver = new PathResolver();
            PathResolver.AddEnvironmentVariable("LIB");
        }

	    private static string GetCacLocation(IAssemblyReference id)
	    {
		    try
		    {
			    return Gac.ResolvePath(id);
		    }
		    catch (Exception e)
		    {
			    Console.WriteLine(e);
		    }
		    return null;
	    }

	    private static string ResolvePath(string path, string refpath)
        {
            string dir = Path.GetDirectoryName(refpath);
            string fullpath = Path.Combine(dir, path);
            if (File.Exists(fullpath)) return fullpath;
            path = PathResolver.Resolve(path);
            if (!string.IsNullOrEmpty(path))
                return path;
            return null;
        }

        private static IEnumerable<string> GetLibDirs()
        {
            var dirs = new[]
                {
                    GlobalSettings.LibsDirectory,
                    GlobalSettings.FlexLibsDirectory
                };
        	return dirs.Where(Directory.Exists).ToArray();
        }

        static string GetAssemblyLocation(IAssemblyReference asmref, string refpath)
        {
            string path;

            string refname = asmref.Name;

            if (CommonLanguageInfrastructure.SubstituteFrameworkAssemblies && IsFrameworkAssembly(asmref))
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
            if (string.Equals(refname, GlobalSettings.CorlibAssemblyName, StringComparison.OrdinalIgnoreCase))
                return GlobalSettings.GetCorlibPath(true);

            var libdirs = GetLibDirs();
            if (libdirs != null)
            {
            	return libdirs.Select(libdir => Path.Combine(libdir, refname + ".dll")).FirstOrDefault(File.Exists);
            }

            return null;
        }

        private static readonly Dictionary<IAssemblyReference, IAssembly> _cache = new Dictionary<IAssemblyReference, IAssembly>();

        public static IAssembly GetFromCache(IAssemblyReference id)
        {
            IAssembly assembly;
            return id != null && _cache.TryGetValue(id, out assembly) ? assembly : null;
        }

        public static void AddToCache(IAssembly assembly)
        {
            var id = new AssemblyReference(assembly);
            if (!_cache.ContainsKey(id))
            {
                _cache.Add(id, assembly);
            }
        }

	    private static readonly string[] SysRefs =
		    {
			    "mscorlib",
			    "System",
			    "Microsoft.VisualBasic",
			    "NUnit.Framework"
		    };

        public static bool IsFrameworkAssembly(IAssemblyReference asmref)
        {
            string refname = asmref.Name;
            if (Array.FindIndex(SysRefs, sr => string.Equals(refname, sr, StringComparison.OrdinalIgnoreCase)) >= 0)
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