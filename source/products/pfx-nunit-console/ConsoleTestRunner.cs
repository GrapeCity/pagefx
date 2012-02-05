//#define INSIDE_PFC

using System;
using System.Collections.Generic;
using System.IO;
using DataDynamics.PageFX.CodeModel;
using DataDynamics.PageFX.FLI;

namespace DataDynamics.PageFX.NUnit
{
    class ConsoleTestRunner
    {
        #region Logo
#if !INSIDE_PFC
        public static void Logo()
        {
            CommandLine.Logo(typeof(ConsoleTestRunner).Assembly);
        }
#endif
        #endregion

        #region Usage
        public static void Usage()
        {
#if !INSIDE_PFC
            if (cl == null || !cl.HasOption("nologo"))
                Logo();
#endif

            Console.WriteLine();
#if INSIDE_PFC
            Console.WriteLine("NUnit Test Runner Options (available when /nunit switch is set):");
#else
            Console.WriteLine("pfx-nunit-console [options] <*.dll|*.swf>");
            Console.WriteLine();
            Console.WriteLine("Options:");
#endif
            Console.WriteLine("/fixture=STR         Fixture to test");
            //Console.WriteLine("/config=STR          Project configuration to load");
            Console.WriteLine("/xml=STR             Name of XML output file");
            Console.WriteLine("/transform=STR       Name of transform file");
            Console.WriteLine("/xmlConsole          Display XML to the console");
            //Console.WriteLine("/output=STR          File to receive test output (Short format: /out=STR)");
            //Console.WriteLine("/framework=STR       .NET Framework version to execute with (eg 'v1.0.3705')");
            //Console.WriteLine("/err=STR             File to receive test error output");
            //Console.WriteLine("/labels              Label each test in stdOut");
            Console.WriteLine("/include=STR         List of categories to include");
            Console.WriteLine("/exclude=STR         List of categories to exclude");
            //Console.WriteLine("/domain=X            AppDomain Usage for Tests");
            //Console.WriteLine("/noshadow            Disable shadow copy when running in separate domain");
            //Console.WriteLine("/nothread            Disable use of a separate thread for tests");
            Console.WriteLine("/wait                Wait for input before closing console window");
#if !INSIDE_PFC
            Console.WriteLine("/nologo              Do not display the logo");
#endif
            //Console.WriteLine("/nodots              Do not display progress");
#if !INSIDE_PFC
            Console.WriteLine("/help                Display help (Short format: /?)");
#endif
            Console.WriteLine("/transformOut=STR    Name of XSLT results file");
            Console.WriteLine("/fp=STR              Path to Flash Player to run swf files");
        }
        #endregion

        #region Fields
        internal static CommandLine cl;
        static string Fixture;
        static Report Report;
        static bool UseSharedReport;
        static bool XmlConsole;
        static string XmlPath;
        static string XsltPath;
        #endregion

        #region Run - Entry Point
        public static void Run(CommandLine commandLine)
        {
            if (commandLine == null)
                throw new ArgumentNullException("commandLine");
            cl = commandLine;
            Run();
        }

        public static void Run()
        {
            if (cl == null)
            {
                Console.WriteLine("No input assemblies");
                Environment.Exit(-1);
            }

            string[] input = cl.GetInputFiles();
            if (input == null || input.Length <= 0)
            {
                Console.WriteLine("No input assemblies");
                Environment.Exit(-1);
            }

            Init();

            int n = input.Length;
            if (n > 0)
            {
                string path = input[0];
                if (path.EndsWith(".swf", StringComparison.InvariantCultureIgnoreCase))
                {
                    RunSwf(path);
                    return;
                }
            }

            for (int i = 0; i < n; ++i)
            {
                string path = input[i];
                if (path.EndsWith(".dll", StringComparison.InvariantCultureIgnoreCase))
                    RunAssembly(path);
            }

            if (UseSharedReport)
            {
                if (XmlConsole)
                {
                    Report.Write(Console.Out);
                    Console.WriteLine();
                }
                else
                {
                    SaveReport(XmlPath);
                }
            }

            if (cl.HasOption("wait"))
                Console.ReadKey();
        }
        #endregion

        #region Init
        static void Init()
        {
            SetFlashPlayerPath();
            InitSharedReport();
            InitFilter();
        }

        static void SetFlashPlayerPath()
        {
            string path = cl.GetPath(null, "fp", "flash-player", "flash_player");
            if (string.IsNullOrEmpty(path)) return;
            if (!File.Exists(path))
            {
                Console.WriteLine("warning PFC0100: Bad flash player path. File {0} does not exists", path);
                return;
            }
            FlashPlayer.Path = path;
        }

        static void InitSharedReport()
        {
            UseSharedReport = false;
            XmlConsole = cl.HasOption("xmlConsole");
            if (XmlConsole)
            {
                UseSharedReport = true;
                Report = new Report();
            }
            else
            {
                XmlPath = cl.GetPath(null, "xml");
                if (!string.IsNullOrEmpty(XmlPath))
                {
                    Report = new Report();
                    UseSharedReport = true;
                }

                XsltPath = cl.GetPath(null, "transform");
            }
        }

        static void InitFilter()
        {
            Fixture = cl.GetOption(null, "fixture");
            CategoryFilter.Clear();
            SetFilter("exclude", false);
            SetFilter("include", true);
        }

        static void SetFilter(string optname, bool include)
        {
            string[] opts = cl.GetOptions(optname);
            if (include)
                CategoryFilter.Include(opts);
            else
                CategoryFilter.Exclude(opts);
        }
        #endregion

        #region RunAssembly
        static string _asmpath; //path to current assembly
        static List<Test> _tests;

        static void RunAssembly(string path)
        {
            if (!Path.IsPathRooted(path))
                path = Path.Combine(Environment.CurrentDirectory, path);

            _asmpath = path;

            if (!UseSharedReport)
                Report = new Report { Name = _asmpath };

            var asm = LoadAssembly(path);

            _tests = new List<Test>();

            var list = new List<IType>(NUnitHelper.GetTestFixtures(asm));
            foreach (var tf in list)
                RegisterFixture(tf);

            foreach (var test in _tests)
                RunTest(test);

            if (!UseSharedReport)
            {
                SaveReport(_asmpath + "-results.xml");
                Report = null;
            }

            _asmpath = null;
        }
        #endregion

        #region RegisterFixture
        static void RegisterFixture(IType type)
        {
            if (!string.IsNullOrEmpty(Fixture) && type.FullName != Fixture)
                return;

            var list = new List<IMethod>(NUnitHelper.GetTests(type, false));
            foreach (var test in list)
                RegisterTest(test);
        }
        #endregion

        #region RegisterTest
        static void RegisterTest(IMethod method)
        {
            string[] cats = NUnitHelper.GetCategories(method);
            if (!CategoryFilter.IsIncluded(cats)) return;

            var nunitOptions = new NUnitHelper.TestRunnerOptions
                                   {
                                       Protect = true,
                                       EndMarker = FlashPlayer.MarkerEnd,
                                       FailString = FlashPlayer.MarkerFail,
                                       SuccessString = FlashPlayer.MarkerSuccess
                                   };

            var testFixture = method.DeclaringType;

            var test = new Test
                           {
                               Name = method.FullName,
                               Description = NUnitHelper.GetDescription(method),
                               SuiteName = testFixture.FullName,
                               SuiteDescription = NUnitHelper.GetDescription(testFixture),
                               AssemblyPath = _asmpath,
                               MainCode = NUnitHelper.GenerateRunnerCode(method, nunitOptions)
                           };

            if (cats != null && cats.Length > 0)
                test.Categories.AddRange(cats);
            
            _tests.Add(test);
        }

        
        #endregion

        #region RunTest
        static void RunTest(Test test)
        {
            string wd = Environment.CurrentDirectory;
            
            const string prefix = "__pfx_nunit";
            const string name_cs = prefix + ".cs";
            const string name_exe = prefix + ".exe";
            const string name_swf = prefix + ".swf";

            string mainFile = Path.Combine(wd, name_cs);
            File.WriteAllText(mainFile, test.MainCode);

            var compilerOptions = new CompilerOptions
            {
                Target = CompilerTarget.ConsoleApp,
                Output = name_exe,
                NoLogo = true,
                NoConfig = true,
                NoStdlib = true
            };

            GlobalSettings.AddCommonReferences(compilerOptions);
            compilerOptions.AddRef(_asmpath);
            compilerOptions.Input.Add(name_cs);

            string cout = CompilerConsole.Run(compilerOptions);
            if (CompilerConsole.HasErrors(cout))
            {
                Console.WriteLine(cout);
                Environment.Exit(-1);
            }

            string path_exe = Path.Combine(wd, name_exe);
            string path_swf = Path.Combine(wd, name_swf);

            var asm = LoadAssembly(path_exe);

            MakeSwf(asm, path_swf);
            RunPlayer(test, path_swf);
            Report.AddTest(test);
        }
        #endregion

        #region MakeSwf
        const string FLIOptions = "/format:swf /nohtml /debug- /fp:10 /framesize:50";

        static void MakeSwf(IAssembly assembly, string path_swf)
        {
            try
            {
                Infrastructure.Serialize(assembly, path_swf, FLIOptions);
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc);
                Environment.Exit(-1);
            }
        }
        #endregion

        #region RunPlayer
        static void RunPlayer(Test test, string path_swf)
        {
            var results = FlashPlayer.Run(path_swf);
            test.Output = TrimOutput(results.Output);
            test.Success = results.Success;
            test.Time = results.Time / 1000.0;
            test.Executed = !results.NotRun;
            test.ExitCode = results.ExitCode;
        }

        static readonly string[] Markers =
            {
                FlashPlayer.MarkerSuccess,
                FlashPlayer.MarkerFail,
                FlashPlayer.MarkerEnd
            };

        static string TrimOutput(string s)
        {
            if (string.IsNullOrEmpty(s)) return s;
            var lines = Str.GetLines(s);
            var list = new List<string>(Algorithms.Filter(lines, l => !Algorithms.Contains(Markers, l)));
            return string.Join("\n", list.ToArray());
        }
        #endregion

        #region SaveReport
        static void SaveReport(string path)
        {
            Report.Save(path);
            Transform(path);
        }

        static void Transform(string path)
        {
            string resultsFile = cl.GetPath(null, "transformOut");
            if (string.IsNullOrEmpty(resultsFile))
                resultsFile = Path.ChangeExtension(path, ".htm");

            Report.Transform(path, resultsFile, XsltPath);
        }
        #endregion

        #region RunSwf
        static void RunSwf(string path)
        {
            if (!Path.IsPathRooted(path))
                path = Path.Combine(Environment.CurrentDirectory, path);

            var results = FlashPlayer.Run(path);
            if (results.Timeout)
            {
                Console.WriteLine("TIMEOUT!");
                Environment.Exit(-1);
            }

            string output = TrimOutput(results.Output);
            if (XmlConsole)
            {
                Console.WriteLine(output);
            }
            else
            {
                if (string.IsNullOrEmpty(XmlPath))
                    XmlPath = Path.ChangeExtension(path, ".xml");
                File.WriteAllText(XmlPath, output);
                Transform(XmlPath);
            }
        }
        #endregion

        #region Utils
        static IAssembly LoadAssembly(string path)
        {
            CLI.Infrastructure.ClearCache();

            try
            {
                if (!Path.IsPathRooted(path))
                    path = Path.Combine(Environment.CurrentDirectory, path);

                var asm = CLI.Infrastructure.Deserialize(path, null);
                if (asm == null)
                {
                    Console.WriteLine("Unable to load assembly '{0}'", path);
                    Environment.Exit(-1);
                }
                return asm;
            }
            catch (Exception e)
            {
                Console.WriteLine("Unable to load assembly '{0}'", path);
                Environment.Exit(-1);
            }
            return null;
        }
        #endregion
    }
}