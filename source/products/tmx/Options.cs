using System;
using System.ComponentModel;
using System.IO;
using DataDynamics;

namespace tmx
{
    class Options
    {
        [Description("Specifies output directory")]
        public static readonly CLOption Opt_Out = new CLOption("out", "outdir;dir");

        [Description("Specifies namespace for generated classes")]
        [DefaultValue(Const.DefaultNamespace)]
        public static readonly CLOption Opt_Namespace = new CLOption("ns", "namespace");

        [Description("Brings translator to generate partial classes")]
        public static readonly CLOption Opt_Partial = new CLOption("partial");

        public static readonly CLOption[] AllOptions = new[]
            {
                Opt_Out,
                Opt_Namespace,
                Opt_Partial
            };

        static Options()
        {
            CLOption.Init(typeof(Options));
        }

        public static void Usage()
        {
            Console.WriteLine("tmx.exe [options] <input-files>");
            Console.WriteLine();
            Console.WriteLine("\t\t\tOptions");
            Console.WriteLine();
            CommandLine.Usage(AllOptions, "\t\t");
        }

        public static void Init(CommandLine cl)
        {
            GetOutDir(cl);

            Namespace = cl.GetOption(Opt_Namespace);
            if (string.IsNullOrEmpty(Namespace))
            {
                Namespace = Const.DefaultNamespace;
            }
            else
            {
                //TODO: check namespace
            }
        }

        public static string OutDir;
        public static string Namespace;
        public static bool Partial;

        static void GetOutDir(CommandLine cl)
        {
            try
            {
                string outdir = cl.GetOption(Opt_Out);

                if (string.IsNullOrEmpty(outdir))
                {
                    outdir = Environment.CurrentDirectory;
                }
                else if (!Path.IsPathRooted(outdir))
                {
                    outdir = Path.Combine(Environment.CurrentDirectory, outdir);
                }

                Directory.CreateDirectory(outdir);

                OutDir = outdir;
            }
            catch (Exception e)
            {
                Errors.OutDir(e);
                return;
            }
        }
    }
}