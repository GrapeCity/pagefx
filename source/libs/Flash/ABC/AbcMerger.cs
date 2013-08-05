using System;
using System.Collections.Generic;
using System.IO;
using DataDynamics.PageFX.FlashLand.Swc;
using DataDynamics.PageFX.FlashLand.Swf;

namespace DataDynamics.PageFX.FlashLand.Abc
{
	/// <summary>
	/// Allows to merge multiple abc files into single one.
	/// </summary>
    public static class AbcMerger
    {
		public static void Merge(string[] files, string output)
        {
            if (files == null) return;
            int n = files.Length;
            if (n <= 0) return;

            string path = files[0];
            if (string.IsNullOrEmpty(output))
            {
                if (n == 1)
                {
                    output = Path.Combine(Environment.CurrentDirectory,
                                           Path.GetFileNameWithoutExtension(path) + ".abc");
                }
                else
                {
                    output = Path.Combine(Environment.CurrentDirectory, "result.abc");
                }
            }
            else if (!Path.IsPathRooted(output))
            {
                output = Path.Combine(Environment.CurrentDirectory, output);
            }

            if (n == 1)
            {
                string ext = GetExtension(path);
                if (ext == "swc")
                {
                    MergeSwc(path, output);
                }
            }
            else
            {
                var main = new AbcFile(files[0]);
                for (int i = 1; i < n; ++i)
                {
                    var abc = new AbcFile(files[i]);
                    main.Import(abc);
                }
                main.Save(output);
            }
        }

        public static void Merge(IEnumerable<AbcFile> files, string output)
        {
	        var oldValue = AbcFile.FilterMetadata;
            try
	        {
				AbcFile.FilterMetadata = false;
				MergeCore(files, output);
	        }
	        finally
	        {
				AbcFile.FilterMetadata = oldValue;
	        }
        }

		private static void MergeCore(IEnumerable<AbcFile> files, string output)
		{
			AbcFile main = null;
			foreach (var abc in files)
			{
				if (main == null)
					main = abc;
				else
					main.Import(abc);
			}
			if (main == null)
				throw new InvalidOperationException();
			main.Save(output);
		}

		public static void MergeSwc(string path, string output)
        {
            var lib = path.ExtractSwfLibrary();
            var mov = new SwfMovie(lib);
            var list = mov.GetAbcFiles();
            Merge(list, output);
        }

		private static string GetExtension(string path)
		{
			if (string.IsNullOrEmpty(path)) return "";
			string ext = Path.GetExtension(path);
			if (string.IsNullOrEmpty(ext)) return "";
			return ext[0] == '.' ? ext.Substring(1).ToLower() : ext.ToLower();
		}
    }
}