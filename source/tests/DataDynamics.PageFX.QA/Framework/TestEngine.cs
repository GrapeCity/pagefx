using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using DataDynamics.PageFX.CodeModel;
using DataDynamics.PageFX.FLI.ABC;
using NUnit.Framework;

namespace DataDynamics.PageFX.FLI
{
    public class TestEngine
    {
        #region RunTestCase
        public static void RunTestCase(string name, string format)
        {
            var tc = SimpleTestCases.Find(name);
            if (tc == null)
                Assert.Fail("Unable to find given test case {0}", name);

            var tds = new TestDriverSettings
            {
                UpdateReport = false,
                OutputFormat = format
            };
            tc.Optimize = true;
            RunTestCase(tc, tds);

            if (tc.IsFailed)
                Assert.Fail(tc.Error);
        }

        public static void RunTestCase(TestCase tc, TestDriverSettings tds)
        {
            if (tds == null)
                tds = new TestDriverSettings();

#if DEBUG
            DebugService.LogInfo("TestCase {0} started", tc.Name);
            DebugService.DoCancel();
#endif

            tc.Reset();
            CLI.Infrastructure.ClearCache();

            try
            {
                RunCore(tc, tds);
            }
            finally
            {
                CLI.Infrastructure.ClearCache();
            }

            if (tds.UpdateReport)
                UpdateReport(tc);
        }
        #endregion

        #region RunCore
        static void RunCore(TestCase tc, TestDriverSettings tds)
        {
            if (QA.IsNUnitSession)
            {
                tc.Debug = false;
                GlobalSettings.EmitDebugInfo = false;
                //we use only JIT mode
                AvmShell.Options.InterpretDefaultValue = false;
            }
            else
            {
                GlobalSettings.EmitDebugInfo = tc.Debug = QA.TestDebugSupport;
            }

            if (!Compile(tc)) return;
            if (tds.IsCancel) return;
            if (!GenerateApp(tc, tds)) return;
            Execute(tc, tds);
        }
        #endregion

        #region Compile
        static bool Compile(TestCase tc)
        {
            if (tc.CompileAVM)
            {
                tc.VM = VM.AVM;
                if (!QA.Compile(tc))
                    return false;
            }

            if (tc.CompileCLR)
            {
                tc.VM = VM.CLR;
                if (!QA.Compile(tc))
                    return false;
            }
#if DEBUG
            DebugService.LogInfo("TestCase '{0}' has been compiled", tc.FullName);
            DebugService.DoCancel();
#endif

            return true;
        }
        #endregion

        #region GenerateApp
        static bool GenerateApp(TestCase tc, TestDriverSettings tds)
        {
            tc.VM = VM.AVM;
            tc.OutputPath = Path.Combine(tc.Root, "test." + tds.OutputExtension);

            bool refl = tc.FullName.Contains("Reflection");
            GlobalSettings.ReflectionSupport = refl;

            if (tc.UsePfc)
            {
                var options = new PfxCompilerOptions
                                  {
                                      Nologo = true,
                                      Input = tc.ExePath,
                                      Output = tc.OutputPath,
                                      Reflection = refl
                                  };
                string err = PfxCompiler.Run(options);
                if (CompilerConsole.HasErrors(err))
                {
                    throw new InvalidOperationException("Unable to compile " + tc.Name + ".\n" + err);
                }
            }
            else
            {
                IAssembly asm;
                if (!LoadAssembly(tc, tds, out asm)) return false;
                
                try
                {
                    string cl = string.Format("/format:{0}", tds.OutputFormat);
                    
                    if (tds.IsSWF)
                    {
                        cl += " /framesize:100 /fp:10 /nohtml /exception-break";
                    }

                    Infrastructure.Serialize(asm, tc.OutputPath, cl);
                }
                catch (Exception e)
                {
                    tc.Error = string.Format("Unable to generate {0} file.\nException: {1}", tds.OutputFormat, e);
                    return false;
                }

#if DEBUG
                DebugService.DoCancel();
#endif
            }
            
            return true;
        }
        #endregion

        #region LoadAssembly
        static bool LoadAssembly(TestCase tc, TestDriverSettings tds, out IAssembly asm)
        {
            asm = tc.LoadAssembly();
            if (asm == null) return false;

#if DEBUG
            DebugService.LogInfo("TestCase assembly was deserialized");
            DebugService.DoCancel();
#endif

            if (tds.IsCancel) return false;

            if (tds.ExportCSharpFile)
            {
                string cspath = Path.Combine(tc.Root, "src.cs");
                QA.ToCSharp(asm, cspath);
            }

            if (tds.IsCancel) return false;

            if (tds.SetDecompiledCode)
                tc.DecompiledCode = ExportService.ToString(asm, "c#");

            if (tds.IsCancel) return false;

#if DEBUG
            DebugService.DoCancel();
#endif
            return true;
        }
        #endregion

        #region Execute
        static void Execute(TestCase tc, TestDriverSettings tds)
        {
            if (tc.IsBenchmark)
            {
                if (tds.IsSWF)
                {
                    var results = FlashPlayer.Run(tc.OutputPath);
                    tc.Output2 = results.Output;
                    return;
                }
                else
                {
                    throw new NotImplementedException();
                }
            }

            int exitCode1 = 0;
            if (!tc.HasOutput)
            {
                tc.VM = VM.CLR;
                tc.Output1 = CommandPromt.Run(tc.ExePath, "", out exitCode1);
            }
            else
            {
                tc.Output1 = tc.Output;
            }

            int exitCode2;
            if (tds.IsABC)
            {
                var avmOpts = new AvmShell.Options();
                tc.Output2 = AvmShell.Run(avmOpts, out exitCode2, tc.OutputPath);
            }
            else if (tds.IsSWF)
            {
                //tc.Output2 = FlashShell.Run(outpath, out exitCode2);
                FlashPlayer.Path = @"c:\pfx\tools\fp10.exe";
                var results = FlashPlayer.Run(tc.OutputPath);
                exitCode2 = results.ExitCode;
                tc.Output2 = results.Output;
            }
            else
            {
                throw new NotImplementedException();
            }

            CreateDebugHooks(tc, tds, tc.OutputPath);

            if (tc.CheckExitCode)
            {
                if (exitCode2 != 0)
                {
                    tc.Error = string.Format("{0} returned non zero exit code {1}.",
                                             tc.OutputPath, exitCode2);
                    return;
                }
                if (exitCode1 != 0)
                {
                    tc.Error = string.Format("{0} returned non zero exit code {1}.",
                                             tc.ExePath, exitCode1);
                    return;
                }
            }

            if (tc.CompareOutputs)
                tc.Error = QA.CompareLines(tc.Output1, tc.Output2, true);
        }
        #endregion

        #region CreateDebugHooks
        static void CreateDebugHooks(TestCase tc, TestDriverSettings tds, string outpath)
        {
            if (QA.IsNUnitSession) return;

#if DEBUG
            if (DebugService.AbcDump)
                Dump(outpath);
#endif

            if (tds.IsCancel) return;

#if DEBUG
            tc.AvmDump = "";
            if (DebugService.AvmDump)
                tc.AvmDump = PlayAbc(outpath);

#endif

            string dir = tc.Root;
            //WriteFile(dir, "play.bat", "avmplus.exe -Dinterp -Dverbose %1");
            WriteFile(dir, "run.bat", "avmplus.exe -Dinterp test.abc");
            WriteFile(dir, "avmdump.bat", "avmplus.exe -Dinterp -Dverbose test.abc > avmdump.txt");
        }

        static void WriteFile(string dir, string name, params string[] lines)
        {
            string path = Path.Combine(dir, name);
            using (var writer = new StreamWriter(path, false, Encoding.ASCII))
            {
                foreach (var line in lines)
                    writer.WriteLine(line);
            }
        }
        #endregion

        #region UpdateReport
        static TestResult testResults;

        static void UpdateReport(TestCase tc)
        {
            string path = QA.TestResultsReportPath;

            if (testResults == null)
            {
                testResults = new TestResult();
                testResults.Suites = new List<TestSuite>();
            }

            var ts = tc.Suite;
            while (ts != null && !ts.IsRoot)
            {
                if (!testResults.HasSuite(ts))
                    testResults.Suites.Add(ts);
                if (tc.IsFailed)
                    ts.TotalFailed++;
                else
                    ts.TotalPassed++;
                ts = ts.Parent;
            }

            testResults.Sort();
            testResults.GenerateHtmlReport(path);
        }
        #endregion
        
        #region Run
        public static void Run(IEnumerable<TestCase> list, TestDriverSettings tds)
        {
            var report = new TestReport();
            foreach (var tc in list)
            {
                tc.Optimize = false;
                RunTestCase(tc, tds);
                report.Add(tc, "optimize-", tc.Error);
            }
            foreach (var tc in list)
            {
                tc.Optimize = true;
                RunTestCase(tc, tds);
                report.Add(tc, "optimize+", tc.Error);
            }
            report.Export("fli.abc.xml", "xml");
        }
        #endregion

        #region Utils
        #region Dump
        static void Dump(string abcPath)
        {
            try
            {
                if (File.Exists(abcPath))
                {
#if DEBUG
                    DebugService.DoCancel();
                    DebugService.LogInfo("AbcDump started");
                    int start = Environment.TickCount;
#endif

                    AbcDumpService.Dump(abcPath);

#if DEBUG
                    int end = Environment.TickCount;
                    DebugService.LogInfo("AbcDump succeeded. Ellapsed Time: {0}", (end - start) + "ms");
                    DebugService.DoCancel();
#endif
                }
            }
            catch
            {
            }
        }
        #endregion

        #region PlayAbc
        static string PlayAbc(string path)
        {
#if DEBUG
            DebugService.DoCancel();
            DebugService.LogInfo("AvmDump started");
            int start = Environment.TickCount;
#endif

            var opts = new AvmShell.Options();
            opts.Verbose = true;

            int exitCode;
            string cout = AvmShell.Run(opts, out exitCode, path);

            string dir = Path.GetDirectoryName(path);
            File.WriteAllText(Path.Combine(dir, "avmdump.txt"), cout);

            //if (exitCode != 0)
            //{
            //    return string.Format("Unable to play ABC file {0}", path);
            //}

#if DEBUG
            int end = Environment.TickCount;
            DebugService.LogInfo("AvmDump succeeded. Ellapsed Time: {0}", (end - start) + "ms");
            DebugService.DoCancel();
#endif

            return cout;
        }
        #endregion
        #endregion
    }
}