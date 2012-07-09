using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using DataDynamics;
using DataDynamics.PageFX.CLI;
using DataDynamics.PageFX.FLI;
using DataDynamics.PageFX.FLI.ABC;
using DataDynamics.PageFX.FLI.SWC;
using DataDynamics.PageFX.FLI.SWF;

namespace abc
{
    internal partial class Program
    {
        #region Usage
        static void Usage()
        {
            Console.WriteLine("Usage:");
        }
        #endregion

        #region Main
        const string FileFilter = "Flash Files|*.abc;*.swf;*.swc";

        [STAThread]
        static int Main(string[] args)
        {
            Application.EnableVisualStyles();

            InitInfrastructures();

            if (args.Length < 1)
            {
                Usage();
                var sel = Utils.SelectOperations("Select Operation...", "Dump", "Wrap", "Merge");
                if (sel != null)
                {
                    foreach (var i in sel)
                        RunTask(i);
                }
                return 0;
            }

            try
            {
                var cl = CommandLine.Parse(args);
                if (cl == null)
                {
                    Console.WriteLine("error PFX0100: Unable to parse command line.");
                    return -1;
                }

                Run(cl);
                return 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return -1;
            }
        }

        static void InitInfrastructures()
        {
            CommonLanguageInfrastructure.Init();
            FlashLanguageInfrastructure.Init();
        }

        static void Run(CommandLine cl)
        {
            if (cl.HasOption("w", "wrap"))
            {
                WrapperGenerator.Wrap(cl);
                return;
            }

            if (cl.HasOption("m", "merge"))
            {
                AbcMerger.Merge(cl.GetInputFiles(), null);
                return;
            }

            if (cl.HasOption("l", "list"))
            {
                List(cl);
                return;
            }

            if (cl.HasOption("removeExports"))
            {
                RemoveExports(cl);
                return;
            }

            Dump(cl);
        }
        #endregion

        #region RunTask
        const int TaskDump = 0;
        const int TaskWrap = 1;
        const int TaskMerge = 2;

        static void RunTask(int tid)
        {
            switch (tid)
            {
                case TaskDump:
                    using (var dlg = new DumpForm())
                        dlg.ShowDialog();
                    break;
                case TaskWrap:
                    using (var dlg = new WrapForm())
                        dlg.ShowDialog();
                    break;
                case TaskMerge:
                    using (var dlg = new OpenFileDialog())
                    {
                        dlg.Filter = FileFilter;
                        dlg.Multiselect = true;
                        if (dlg.ShowDialog() != DialogResult.OK)
                            return;
                        AbcMerger.Merge(dlg.FileNames, null);
                    }
                    break;
            }
        }
        #endregion

        #region Excludes
        public static Hashtable LoadExcludes(IEnumerable<string> excludes)
        {
            var filter = new Hashtable();
            if (excludes != null)
            {
                foreach (var exclude in excludes)
                {
                    string path = Utils.ResolvePath(exclude);
                    LoadExclude(filter, path);
                }
            }
            return filter;
        }

        public static void LoadExclude(IDictionary filter, string path)
        {
            string ext = Utils.GetExt(path);
            switch (ext)
            {
                case "abc":
                    {
                        var abc = new AbcFile(path);
                        AddExcludes(filter, abc);
                    }
                    break;

                case "swf":
                    {
                        var swf = new SwfMovie(path);
                        var list = swf.GetAbcFiles();
                        foreach (var abc in list)
                            AddExcludes(filter, abc);
                    }
                    break;

                case "swc":
                    {
                        var swc = new SwcFile(path);
                        foreach (var abc in swc.GetAbcFiles())
                            AddExcludes(filter, abc);
                    }
                    break;
            }
        }

        public static void AddExcludes(IDictionary filter, AbcFile abc)
        {
            foreach (var instance in abc.Instances)
            {
                filter[instance.FullName] = instance.FullName;
            }
        }

        public static Hashtable LoadStandardExcludes()
        {
            var filter = new Hashtable();
            //TODO: Remove hardcoded dependency. Use some config or environment variable.
            string dir = @"E:\sdk\flex.3.0.0\libs";
            if (Directory.Exists(dir))
            {
                foreach (var swc in Directory.GetFiles(dir, "*.swc"))
                    LoadExclude(filter, swc);
            }
            return filter;
        }
        #endregion

        #region RemoveExports
        static void RemoveExports(CommandLine cl)
        {
            foreach (var path in cl.GetInputFiles())
            {
                RemoveExports(path);
            }
        }

        static void RemoveExports(string path)
        {
            var swf = new SwfMovie();
            swf.Load(path, SwfTagDecodeOptions.DonotDecodeTags);
            for (int i = 0; i < swf.Tags.Count; ++i)
            {
                var tag = swf.Tags[i];
                if (tag.TagCode == SwfTagCode.ExportAssets)
                {
                    swf.Tags.RemoveAt(i);
                    --i;
                }
            }
            swf.Save(path + ".noexports.swf");
        }
        #endregion
    }
}