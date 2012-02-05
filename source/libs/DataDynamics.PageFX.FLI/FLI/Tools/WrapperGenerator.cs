using System;
using System.IO;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.FLI
{
    public static class WrapperGenerator
    {
        static void EnshureSystemTypes()
        {
            //Infrastructure.Deserialize(typeof(int).Assembly.Location, null);
            LanguageInfrastructure.CLI.Deserialize(GlobalSettings.GetCorlibPath(true), null);
        }

        public static void Wrap(string commandLine)
        {
            if (string.IsNullOrEmpty(commandLine))
                throw new ArgumentException("Command line is null or empty", "commandLine");
            var cl = CommandLine.Parse(commandLine);
            if (cl == null)
                throw new InvalidOperationException(
                    string.Format("Unable to parse command line: {0}", commandLine));
            Wrap(cl);
        }

        public static void Wrap(string path, string outpath, CommandLine cl)
        {
            Wrap(path, outpath, cl.ToString());
        }

        public static void Wrap(string path, string outpath, string cls)
        {
            try
            {
                EnshureSystemTypes();
                Generate(path, outpath, cls);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Environment.Exit(-1);
            }
        }

        public static void Wrap(CommandLine cl)
        {
            cl.RemoveOptions("wrap");
            var files = cl.GetInputFiles();
            string outpath = cl.GetOption("", "out", "output");
            foreach (var file in files)
                Wrap(file, outpath, cl);
        }

        static string GetExt(string path)
        {
            if (string.IsNullOrEmpty(path)) return "";
            string ext = Path.GetExtension(path);
            if (string.IsNullOrEmpty(ext)) return "";
            if (ext[0] == '.')
                ext = ext.Substring(1);
            return ext.ToLower();
        }

        static void CopyResourceFile(string dir, string res, string name)
        {
            Directory.CreateDirectory(dir);
            var rs = ResourceHelper.GetStream(typeof(WrapperGenerator), res);
            string path = Path.Combine(dir, name);
            Stream2.SaveStream(rs, path);
        }

        static void CopyAbcAttributes(string dir)
        {
            CopyResourceFile(dir, "abc.cs", "abc.cs");
        }

        /// <summary>
        /// Generates wrapper for specified ABC/SWC file.
        /// </summary>
        /// <param name="path">path to ABC file</param>
        /// <param name="outpath">output path. Can be null.</param>
        /// <param name="format">options</param>
        public static void Generate(string path, string outpath, string format)
        {
            if (!Path.IsPathRooted(path))
                path = Path.Combine(Environment.CurrentDirectory, path);

            if (string.IsNullOrEmpty(path) || !File.Exists(path))
                throw new ArgumentException("Input path is invalid");

            var cl = CommandLine.Parse(format);
            if (cl == null)
                cl = new CommandLine();

            var asm = BuildAssembly(path, cl);

            if (string.IsNullOrEmpty(outpath))
                outpath = Path.ChangeExtension(path, ".dll");
            else if (!Path.IsPathRooted(outpath))
                outpath = Path.Combine(Environment.CurrentDirectory, outpath);

            asm.Name = Path.GetFileName(outpath);

            var copts = new CompilerOptions();

            bool srconly = cl.GetBoolOption(false, "srconly");

            var refs = GlobalSettings.GetRefs(cl);

            bool hasCorlibRef = GlobalSettings.HasCorlibRef(refs);

            string srcdir = cl.GetOption(null, "dir");
            if (string.IsNullOrEmpty(srcdir))
                srcdir = Path.GetFileName(path) + "_src";

            if (!Path.IsPathRooted(srcdir))
                srcdir = Path.Combine(Environment.CurrentDirectory, srcdir);

            ExportService.ToDirectory(asm, "c#", srcdir);
            copts.Recurse.Add(srcdir + "\\*.cs");

			if (!hasCorlibRef)
			{
				CopyAbcAttributes(srcdir);
			}

        	if (!srconly)
            {
                copts.AddRes(path);

                if (path.EndsWith(".swc", StringComparison.InvariantCultureIgnoreCase))
                {
                    string swcdep = Path.ChangeExtension(path, ".swcdep");
                    if (File.Exists(swcdep))
                        copts.AddRes(swcdep);
                }

                copts.Optimize = true;
                copts.Debug = true;

                if (hasCorlibRef)
                {
                    copts.NoConfig = true;
                    copts.NoStdlib = true;
                }

                copts.AddRefs(refs);

                copts.Output = outpath;
                copts.Target = CompilerTarget.Library;

                Compile(copts, path);
            }
        }

        private static IAssembly BuildAssembly(string path, CommandLine cl)
        {
            var builder = new AssemblyBuilder(cl);
            IAssembly asm;
            string ext = GetExt(path);
            switch (ext)
            {
                case "abc":
                    asm = builder.FromFile(path);
                    break;

                case "swc":
                    asm = builder.FromSwc(path);
                    break;

                default:
                    throw new NotSupportedException();
            }
            return asm;
        }

        static void Compile(CompilerOptions copts, string path)
        {
            string olddir = Environment.CurrentDirectory;
            try
            {
                Environment.CurrentDirectory = Path.GetDirectoryName(path);
                string cout = CompilerConsole.Run(copts, true);
                var errors = CompilerConsole.ParseOutput(cout);
				if (errors.HasErrors)
				{
					throw new InvalidOperationException(string.Format("Unable to generate wrapper.\n{0}", cout));
				}
				if (errors.HasWarnings)
				{
					Console.WriteLine(cout);
				}
            }
            finally
            {
                Environment.CurrentDirectory = olddir;
            }
        }
    }
}