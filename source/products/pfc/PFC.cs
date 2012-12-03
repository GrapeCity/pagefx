using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Linq;
using DataDynamics.PageFX.Common;
using DataDynamics.PageFX.Common.Services;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Common.Utilities;
using DataDynamics.PageFX.Ecma335;
using DataDynamics.PageFX.Flash;
using DataDynamics.PageFX.FLI;
using DataDynamics.Tools;

namespace DataDynamics.PageFX
{
    partial class PFC
    {
        #region Shared Fields
        const string DefaultOutputFormat = "swf";
        static CommandLine cl;
        static string asmpath;
        static string outpath;
        static bool nologo;
        #endregion

        #region Logo
        static void Logo()
        {
            CommandLine.Logo(typeof(PFC).Assembly);
        }
        #endregion

        #region Usage
        static void Usage()
        {
            if (!nologo)
                Logo();

            Console.WriteLine("Usage: pfc.exe [.NET compiler options] [options] <input-files>");
            Console.WriteLine("  where:");
            Console.WriteLine("    <input-files>           - managed assembly (.dll;.exe) files or source code files");
            Console.WriteLine("    [.NET compiler options] - valid .NET compiler options");
            Console.WriteLine("    [options]               - PageFX compiler options listed below");
            Console.WriteLine();
            Console.WriteLine("                      PageFX Compiler Options");
            Console.WriteLine();
            PFCOptions.Usage();
            //ConsoleTestRunner.Usage();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("NOTE: All relative pathes are resolved relative to current directory.");
        }
        #endregion

        #region Main
        [STAThread]
        static int Main(string[] args)
        {
            InitInfrastructures();

            if (args.Length <= 0)
            {
#if DEBUG
                if (RunTests())
                {
                    Wait();
                    return 0;
                }
                if (RunSample())
                {
                    Wait();
                    return 0;
                }
#endif
                Usage();
                return 1;
            }

            cl = CommandLine.Parse(args);
            if (cl == null)
            {
                Usage();
                LogError(Errors.InvalidCommandLine);
                return -1;
            }

            return MainCore();
        }
#endregion

        #region MainCore
        static int MainCore()
        {
            if (cl.HasOption("?", "help"))
            {
                Usage();
                return 0;
            }

			if (cl.HasOption("-rv", "--rv"))
			{
				return RV.Run(cl);
			}

        	if (cl.HasOption(PFCOptions.Wrap))
            {
                WrapperGenerator.Wrap(cl);
                return 0;
            }

            //if (cl.HasOption(PFCOptions.NUnit))
            //{
            //    ConsoleTestRunner.Run(cl);
            //    return 0;
            //}

#if PERF
            int start = Environment.TickCount;
#endif
			if (cl.HasOption("dot"))
			{
				DebugHooks.MethodName = cl.GetOption("", "dot");
			}

            RunCore();

#if PERF
            CLI.Infrastructure.DumpPerfStat();
            Infrastructure.DumpPerfStat();
            Console.WriteLine((Environment.TickCount - start) + "ms");
#endif
            return 0;
        }
        #endregion
        
        #region RunSample, RunIn
#if DEBUG
        static bool RunSample()
        {
            cl = DebugInput.GetCommandLine();
            if (cl == null) return false;
            string dir = Path.GetDirectoryName(DebugInput.SamplePath);
#if PERF
            int start = Environment.TickCount;
#endif
            RunIn(dir);
#if PERF
            CLI.Infrastructure.DumpPerfStat();
            Infrastructure.DumpPerfStat();
            Console.WriteLine((Environment.TickCount - start) + "ms");
#endif
            return true;
        }
#endif

        static void RunIn(string dir)
        {
            RunCodeIn(dir, RunCore);
        }
        #endregion
        
        #region RunCore
        static void RunCore()
        {
            nologo = cl.HasOption("nologo");

            if (cl.HasOption("h", "help", "?"))
            {
                Usage();
                return;
            }

            GlobalSettings.SetGlobalOptions(cl);

            ProcessInputFiles();

            string format = GetOutputFormat();

            var asm = LoadAssembly(asmpath, true);

            outpath = string.IsNullOrEmpty(outpath)
                          ? Path.ChangeExtension(asmpath, "." + format)
                          : Path.ChangeExtension(outpath, "." + format);

            if (!Path.IsPathRooted(outpath))
                outpath = Path.Combine(Environment.CurrentDirectory, outpath);

            Serialize(asm, format);

            FlashPlayerTrust.AddPath(Path.GetDirectoryName(outpath));
        }
        #endregion

        #region STEP1: ProcessInputFiles
        static void ProcessInputFiles()
        {
            string recurse = cl.GetOption(null, "recurse");
            bool hasRecurse = !string.IsNullOrEmpty(recurse);
            var inputFiles = cl.GetInputFiles();
            if (inputFiles.Length <= 0 && !hasRecurse)
            {
                Usage();
                Errors.NoInputFiles.LogConsole();
                Environment.Exit(-1);
            }

            if (!nologo) Logo();

            CompilerReport.CollectErrors = true;

            outpath = cl.GetOption(null, "out");

            if (HasSourceFiles(inputFiles) || hasRecurse)
            {
                string target = cl.GetOption("exe", "target", "t");

                string oldpath = outpath;

                if (string.IsNullOrEmpty(outpath))
                {
                    outpath = Path.ChangeExtension(inputFiles[0], GetTargetExtension(target));
                }

                //change extension according to target
                FixOutputExtension(target);

                asmpath = outpath;

                try
                {
                    CompileAssembly();
                }
                catch (Exception e)
                {
                    LogException(e, Errors.UnableToCompileSourceFiles);
                    Environment.Exit(-1);
                }

                outpath = oldpath;
            }
            else
            {
                if (inputFiles.Length != 1)
                {
                    Usage();
                    LogError(Errors.TwoManyInputAssemblies);
                    Environment.Exit(-1);
                }
                asmpath = inputFiles[0];
                if (!Path.IsPathRooted(asmpath))
                    asmpath = Path.Combine(Environment.CurrentDirectory, asmpath);
            }
        }

        static void FixOutputExtension(string target)
        {
            switch (target.ToLower())
            {
                case "exe":
                case "winexe":
                    outpath = Path.ChangeExtension(outpath, ".exe");
                    break;

                case "library":
                    outpath = Path.ChangeExtension(outpath, ".dll");
                    break;

                default:
                    {
                        Console.WriteLine("error PFC101: Target {0} is not supported.", target);
                        Environment.Exit(-1);
                    }
                    break;
            }
        }
        #endregion

        #region STEP2: CompileAssembly - if input is source files
        static readonly string[] SysRefs = new[]
            {
                GlobalSettings.CorlibAssemblyName,
                "System",
                "System.Core",
                "System.Xml",
                "System.Data"
            };

        static bool IsSysRefCore(string r, string sysRef)
        {
            if (string.Compare(r, sysRef, true) == 0)
                return true;
            if (string.Compare(r, sysRef + ".dll", true) == 0)
                return true;
            return false;
        }

        private static bool IsSysRef(string r)
        {
            r = Path.GetFileName(r);
        	return SysRefs.Any(sysRef => IsSysRefCore(r, sysRef));
        }

        static void CompileAssembly()
        {
            var options = new CompilerOptions();

            //redirect cl options to compiler
            if (!RedirectOptions(options))
            {
                LogError(Errors.InvalidCommandLine);
                Environment.Exit(-1);
            }

            //remove system references
            options.References.RemoveAll(IsSysRef);

            //add reference to pagefx corlib
            AddCorlibRef(options);
            AddCommonoRefs(options);

            if (cl.HasOption(PFCOptions.MX))
            {
                string mx = GlobalSettings.GetMxLibraryPath();
                if (!File.Exists(mx))
                {
                    //TODO: raise error
                }
                options.AddRef(mx);
            }

            var rsls = RslList.Parse(cl);
            foreach (var rsl in rsls)
                options.AddRef(rsl.Library);

            options.NoLogo = true;
            options.NoStdlib = true;
            options.NoConfig = true;
            options.Output = asmpath;

            string cout = CompilerConsole.Run(options, true);

            var errors = CompilerConsole.ParseOutput(cout);

            if (NoMain(errors))
            {
                asmpath = Path.ChangeExtension(asmpath, ".dll");
                options.Output = asmpath;
                options.Target = CompilerTarget.Library;

                cout = CompilerConsole.Run(options, true);
                errors = CompilerConsole.ParseOutput(cout);
            }

            if (errors.HasErrors)
            {
                Console.WriteLine(cout);
                Environment.Exit(-1);
            }
        }

        static void AddCorlibRef(CompilerOptions options)
        {
            options.AddRef(GlobalSettings.GetCorlibPath(true));
        }

        static int CompareRef(string r1, string r2)
        {
            string n1 = Path.GetFileName(r1);
            string n2 = Path.GetFileName(r2);
            return string.Compare(n1, n2, true);
        }

        static void AddCommonoRef(CompilerOptions options, string cr)
        {
            int i = options.References.FindIndex(r => CompareRef(cr, r) == 0);
            if (i >= 0)
                options.References.RemoveAt(i);
            options.References.Add(GlobalSettings.GetLibPath(cr));
        }

        static void AddCommonoRefs(CompilerOptions options)
        {
            foreach (var cr in GlobalSettings.CommonAssemblies)
                AddCommonoRef(options, cr);
        }

        static bool NoMain(CompilerErrorCollection errors)
        {
            foreach (CompilerError err in errors)
            {
                if (err.IsWarning) continue;
                if (err.ErrorNumber == "CS5001") //NOTE: No Main Error
                    return true;
                return false;
            }
            return false;
        }

        static bool IsPfcOption(CommandLine.Item item)
        {
            return PFCOptions.Contains(item.Name);
        }

        static bool RedirectOptions(CompilerOptions options)
        {
            foreach (var item in cl.Items)
            {
                switch (item.Type)
                {
                    case CommandLine.ItemType.InputFile:
                        options.Input.Add(item.Value);
                        break;

                    case CommandLine.ItemType.ResponseFile:
                        options.ResponseFiles.Add(item.Value);
                        break;

                    case CommandLine.ItemType.Option:
                        var opt = PFCOptions.Find(item.Name);
                        if (opt == null || opt.DotNetCompiler)
                        {
                            if (!options.SetOption(item.Name, item.Value))
                            {
                                LogError(Errors.InvalidCommandLineOption, item);
                                return false;
                            }
                        }
                        break;
                }
            }
            return true;
        }
        #endregion

        #region STEP3: LoadAssembly
        public static IAssembly LoadAssembly(string path, bool delbins)
        {
            try
            {
                if (!Path.IsPathRooted(path))
                    path = Path.Combine(Environment.CurrentDirectory, path);

                var asm = CommonLanguageInfrastructure.Deserialize(path, null);
                if (asm == null)
                {
                    LogError(Errors.UnableToLoadAssembly, path);
                    Environment.Exit(-1);
                }
                return asm;
            }
            catch (Exception e)
            {
                LogException(e, Errors.UnableToLoadAssembly, path);
                Environment.Exit(-1);
            }
            finally
            {
                if (delbins)
                    DeleteBins(path);
            }
            return null;
        }
        #endregion

        #region STEP4: Serialize
        static void Serialize(IAssembly asm, string format)
        {
            try
            {
                string f = FilterCommandLine();
                f += string.Format(" /format:{0}", format);

                FlashLanguageInfrastructure.Serialize(asm, outpath, f);

                if (CompilerReport.HasErrors)
                {
                    CompilerReport.Log();
                    Environment.Exit(-1);
                }
            }
            catch (Exception e)
            {
                LogException(e, Errors.UnableToTranslateAssembly, asmpath);
                Environment.Exit(-1);
            }
            finally
            {
                DeleteBins(asmpath);
            }
        }

        static string FilterCommandLine()
        {
            return cl.Filter(item =>
	            {
		            if (!item.IsOption) return false;
		            if (!IsPfcOption(item)) return false;
		            return true;
	            });
        }

        static bool doNotDeleteBins = true;

        static void DeleteBins(string path)
        {
            if (doNotDeleteBins) return;
            try
            {
                File.Delete(path);
                string pdbpath = Path.ChangeExtension(path, ".pdb");
                if (File.Exists(pdbpath))
                    File.Delete(pdbpath);
            }
            catch
            {
            }
        }
        #endregion

        #region GetOutputFormat
        static string GetOutputFormat()
        {
            string format = cl.GetOption(PFCOptions.Format);

            if (string.IsNullOrEmpty(format))
            {
                //try to determine output format using extension of output file.
                string ext = GetExt(outpath);
                switch (ext)
                {
                    case "abc":
                        return "abc";

                    case "swf":
                        return "swf";

                    case "swc":
                        return "swc";

                    default:
                        return DefaultOutputFormat;
                }
            }

            //check format
            return format;
        }
        #endregion

        #region Utils
        static void RunCodeIn(string dir, Action action)
        {
            string curdir = Environment.CurrentDirectory;
            try
            {
                Environment.CurrentDirectory = dir;
                action();
            }
            finally
            {
                Environment.CurrentDirectory = curdir;
            }
        }

        const bool wait = true;

        static void Wait()
        {
            if (wait)
            {
                Console.WriteLine("Done!");
                Console.ReadKey();
            }
        }

        static void InitInfrastructures()
        {
            CommonLanguageInfrastructure.Init();
            FlashLanguageInfrastructure.Init();
        }

        static string GetTargetExtension(string target)
        {
            switch (target.ToLower())
            {
                case "exe":
                case "winexe":
                    return ".exe";

                case "lib":
                case "library":
                    return ".dll";

                default:
                    {
                        Console.WriteLine("error PFC101: Target {0} is not supported.", target);
                        Environment.Exit(-1);
                        return null;
                    }
            }
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

        static bool IsSourceExtension(string ext)
        {
            switch (ext)
            {
                case "cs":
                case "csc":
                case "vb":
                case "vbc":
                case "js":
                case "jsc":
                    return true;
            }
            return false;
        }

        static bool HasSourceFiles(string[] files)
        {
            if (files == null) return false;
            int n = files.Length;
            for (int i = 0; i < n; ++i)
            {
                string file = files[i];
                string ext = GetExt(file);
                if (IsSourceExtension(ext))
                    return true;
            }
            return false;
        }
        #endregion

        #region Error Reporting
        static void LogException(Exception e, Error err, params object[] args)
        {
            var ce = e as CompilerException;
            if (ce != null)
            {
                Console.WriteLine(ce.Message);
                return;
            }

            err.LogConsole(args);
            Console.WriteLine("Exception:\n{0}", e);
        }

        static void LogError(Error err, params object[] args)
        {
            err.LogConsole(args);
        }
        #endregion
    }
}
