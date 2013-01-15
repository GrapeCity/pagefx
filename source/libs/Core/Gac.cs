using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Common.Utilities;

namespace DataDynamics.PageFX.Core
{
	// TODO: support mono gac
	// TODO: support short assembly versions?
	// TODO: caching by public token + version

	/// <summary>
	/// Managed imlementation of resolving assemblies from gac
	/// </summary>
	internal static class Gac
	{
		private static IEnumerable<string> RootDirs()
		{
			var roots = new[]
				{
					Path.GetDirectoryName(Environment.GetFolderPath(Environment.SpecialFolder.System)),
					Path.GetDirectoryName(FrameworkInfo.InstallRoot.TrimEnd('\\', '/'))
				}
				.Where(x => !string.IsNullOrEmpty(x))
				.ToArray();

			return roots.Select(root => Path.Combine(root, "assembly"))
			            .Where(dir => Directory.Exists(dir));
		}

		public static string ResolvePath(IAssemblyReference id)
		{
			if (id == null)
				throw new ArgumentNullException("id");

			// only signed assemblies
			if (id.PublicKeyToken == null)
				return null;

			string token = string.Join("", id.PublicKeyToken.Select(x => x.ToString("x2")).ToArray());

			return RootDirs()
				.Select(root => ResolvePath(id, token, root))
				.FirstOrDefault(x => x != null);
		}

		private static readonly string[] GacDirs =
			{
				"GAC_MSIL",
				"GAC",
				"GAC_32",
			};

		private static string ResolvePath(IAssemblyReference id, string token, string root)
		{
			//TODO: mono has no subdirs
			return GetDirs(root, GacDirs)
				.Select(dir => ResolveImpl(id, token, dir)	)
				.FirstOrDefault(x => x != null);
		}

		private static string ResolveImpl(IAssemblyReference id, string token, string dir)
		{
			string libdir = Path.Combine(dir, id.Name);
			if (!Directory.Exists(libdir)) return null;

			foreach (var vdir in Directory.GetDirectories(libdir))
			{
				var vname = Path.GetFileName(vdir);
				if (vname == null) continue;
				if (!vname.EndsWith(token, StringComparison.InvariantCultureIgnoreCase)) continue;

				vname = vname.Substring(0, vname.Length - token.Length);
				vname = vname.TrimEnd('_');
				var v = vname.Split('.', '_');
				if (v.Length < 4) continue; // TODO: support short versions?

				v = TakeLast(v, 4).ToArray();
				if (v.Any(x =>
					{
						int r;
						return !int.TryParse(x, out r);
					})) continue;

				var ver = new Version(int.Parse(v[0]), int.Parse(v[1]), int.Parse(v[2]), int.Parse(v[3]));
				if (ver >= id.Version)
				{
					string path = Path.Combine(vdir, id.Name + ".dll");
					if (File.Exists(path))
						return path;
				}
			}

			return null;
		}

		private static IEnumerable<string> GetDirs(string root, IEnumerable<string> dirs)
		{
			return dirs.Select(x => Path.Combine(root, x)).Where(x => Directory.Exists(x));
		}

		private static IEnumerable<T> TakeLast<T>(T[] array, int count)
		{
			for (var i = array.Length - count; i < array.Length ; i++)
			{
				if (i < 0) continue;
				yield return array[i];
			}
		}
	}
}
