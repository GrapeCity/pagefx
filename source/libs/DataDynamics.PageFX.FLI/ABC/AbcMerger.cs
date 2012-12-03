using System;
using System.Collections.Generic;
using System.IO;
using DataDynamics.PageFX.Common;
using DataDynamics.PageFX.FLI.SWC;
using DataDynamics.PageFX.FLI.SWF;

namespace DataDynamics.PageFX.FLI.ABC
{
    public static class AbcMerger
    {
		static void EnshureSystemTypes()
		{
			LanguageInfrastructure.CLI.Deserialize(GlobalSettings.GetCorlibPath(true), null);
		}

        private static string GetExt(string path)
        {
            if (string.IsNullOrEmpty(path)) return "";
            string ext = Path.GetExtension(path);
            if (string.IsNullOrEmpty(ext)) return "";
            if (ext[0] == '.')
                ext = ext.Substring(1);
            return ext.ToLower();
        }

        public static void Merge(string[] files, string outpath)
        {
            if (files == null) return;
            int n = files.Length;
            if (n <= 0) return;

            string path = files[0];
            if (string.IsNullOrEmpty(outpath))
            {
                if (n == 1)
                {
                    outpath = Path.Combine(Environment.CurrentDirectory,
                                           Path.GetFileNameWithoutExtension(path) + ".abc");
                }
                else
                {
                    outpath = Path.Combine(Environment.CurrentDirectory, "result.abc");
                }
            }
            else if (!Path.IsPathRooted(outpath))
            {
                outpath = Path.Combine(Environment.CurrentDirectory, outpath);
            }

            if (n == 1)
            {
                string ext = GetExt(path);
                if (ext == "swc")
                {
                    MergeSwc(path, outpath);
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
                main.Save(outpath);
            }
        }

        public static void Merge(IEnumerable<AbcFile> files, string outpath)
        {
        	EnshureSystemTypes();

            AbcFile.FilterMetadata = false;
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
            main.Save(outpath);
            AbcFile.FilterMetadata = true;
        }

        public static void MergeSwc(string path, string outpath)
        {
            var lib = path.ExtractSwfLibrary();
            var mov = new SwfMovie(lib);
            var list = mov.GetAbcFiles();
            Merge(list, outpath);
        }
    }
}