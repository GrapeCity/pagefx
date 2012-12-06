using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using DataDynamics.PageFX.Common.Tools;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Common.Utilities;
using DataDynamics.PageFX.Ecma335;
using DataDynamics.PageFX.Ecma335.Execution;
using DataDynamics.PageFX.Ecma335.JavaScript;
using DataDynamics.PageFX.FlashLand;
using DataDynamics.PageFX.FlashLand.Abc;
using DataDynamics.PageFX.FlashLand.Core.Tools;
using DataDynamics.PageFX.TestRunner.Tools;
using NUnit.Framework;

namespace DataDynamics.PageFX.TestRunner.Framework
{
    public class TestEngine
    {
        #region RunTestCase
        public static void RunTestCase(string name, string format)
        {
            var test = SimpleTestCases.Find(name);
            if (test == null)
                Assert.Fail("Unable to find given test case {0}", name);

        	var settings = new TestDriverSettings
        	          	{
        	          		OutputFormat = format
        	          	};
            test.Optimize = true;
            RunTestCase(test, settings);

			if (test.IsFailed)
			{
				Assert.Fail(test.Error);
			}
        }

        public static void RunTestCase(TestCase test, TestDriverSettings settings)
        {
            if (settings == null)
                settings = new TestDriverSettings();

#if DEBUG
            DebugService.LogInfo("TestCase {0} started", test.Name);
            DebugService.DoCancel();
#endif

            test.Reset();
            CommonLanguageInfrastructure.ClearCache();

            try
            {
                RunCore(test, settings);
            }
            finally
            {
                CommonLanguageInfrastructure.ClearCache();
            }
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

            if (!Compile(tc) || tds.IsCancel) return;
            
			if (!(tds.IsClrEmulation))
			{
				if (!GenerateApp(tc, tds)) return;
			}

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
        static bool GenerateApp(TestCase test, TestDriverSettings tds)
        {
			test.VM = VM.AVM;
			test.OutputPath = Path.Combine(test.Root, "test." + tds.OutputExtension);

			if (tds.IsJavaScript)
			{
				try
				{
					var compiler = new JsCompiler(new FileInfo(test.ExePath));
					compiler.Compile(new FileInfo(test.OutputPath));
					return true;
				}
				catch (Exception e)
				{
					test.Error = string.Format("Unable to generate {0} file.\nException: {1}", tds.OutputFormat, e);
					return false;
				}				
			}

            bool refl = test.FullName.Contains("Reflection");
            GlobalSettings.ReflectionSupport = refl;

            if (test.UsePfc)
            {
                var options = new PfxCompilerOptions
                                  {
                                      Nologo = true,
                                      Input = test.ExePath,
                                      Output = test.OutputPath,
                                      Reflection = refl
                                  };
                string err = PfxCompiler.Run(options);
                if (CompilerConsole.HasErrors(err))
                {
                    throw new InvalidOperationException("Unable to compile " + test.Name + ".\n" + err);
                }
            }
            else
            {
                IAssembly asm;
                if (!LoadAssembly(test, tds, out asm)) return false;
                
                try
                {
                    string cl = string.Format("/format:{0}", tds.OutputFormat);
                    
                    if (tds.IsSWF)
                    {
                        cl += " /framesize:100 /fp:10 /nohtml /exception-break";
                    }

                    FlashLanguageInfrastructure.Serialize(asm, test.OutputPath, cl);
                }
                catch (Exception e)
                {
                    test.Error = string.Format("Unable to generate {0} file.\nException: {1}", tds.OutputFormat, e);
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

#if DEBUG
            DebugService.DoCancel();
#endif
            return true;
        }
        #endregion

        #region Execute
        static void Execute(TestCase test, TestDriverSettings settings)
        {
            if (test.IsBenchmark)
            {
                if (settings.IsSWF)
                {
                    var results = FlashPlayer.Run(test.OutputPath);
                    test.Output2 = results.Output;
                    return;
                }
                else
                {
                    throw new NotImplementedException();
                }
            }

            int exitCode1 = 0;
            if (!test.HasOutput)
            {
                test.VM = VM.CLR;
                test.Output1 = CommandPromt.Run(test.ExePath, "", out exitCode1);
            }
            else
            {
                test.Output1 = test.Output;
            }

            int exitCode2;
			if (settings.IsClrEmulation)
			{
				try
				{
					var console = new StringWriter();
					var vm = new VirtualMachine(console);
					exitCode2 = vm.Run(test.ExePath, "", new string[0]);
					test.Output2 = console.ToString();
				}
				catch (Exception e)
				{
					exitCode2 = 0;
					test.Output2 = e.ToString();
				}
			}
			else if (settings.IsJavaScript)
			{
				try
				{
					test.Output2 = JsRunner.Run(test.OutputPath, null, out exitCode2);
				}
				catch (Exception e)
				{
					exitCode2 = 0;
					test.Output2 = e.ToString();
				}
			}
            else if (settings.IsABC)
            {
                var avmOpts = new AvmShell.Options();
                test.Output2 = AvmShell.Run(avmOpts, out exitCode2, test.OutputPath);
            }
            else if (settings.IsSWF)
            {
                //tc.Output2 = FlashShell.Run(outpath, out exitCode2);
                FlashPlayer.Path = @"c:\pfx\tools\fp10.exe";
                var results = FlashPlayer.Run(test.OutputPath);
                exitCode2 = results.ExitCode;
                test.Output2 = results.Output;
            }
			else
            {
                throw new NotImplementedException();
            }

            CreateDebugHooks(test, settings, test.OutputPath);

            if (test.CheckExitCode)
            {
                if (exitCode2 != 0)
                {
                    test.Error = string.Format("{0} returned non zero exit code {1}.",
                                             test.OutputPath, exitCode2);
                    return;
                }
                if (exitCode1 != 0)
                {
                    test.Error = string.Format("{0} returned non zero exit code {1}.",
                                             test.ExePath, exitCode1);
                    return;
                }
            }

            if (test.CompareOutputs)
                test.Error = QA.CompareLines(test.Output1, test.Output2, true);
        }
        #endregion

        #region CreateDebugHooks
        static void CreateDebugHooks(TestCase test, TestDriverSettings settings, string outpath)
        {
            if (QA.IsNUnitSession) return;

#if DEBUG
            if (DebugService.AbcDump)
                Dump(outpath);
#endif

            if (settings.IsCancel) return;

#if DEBUG
            test.AvmDump = "";
            if (DebugService.AvmDump)
                test.AvmDump = AvmShell.Dump(outpath);
#endif

            string dir = test.Root;
            //WriteFile(dir, "play.bat", "avmplus.exe -Dinterp -Dverbose %1");
            WriteFile(dir, "run.bat", "avmplus.exe -Dinterp test.abc");
            WriteFile(dir, "avmdump.bat", "avmplus.exe -Dinterp -Dverbose test.abc > avmdump.txt");
			WriteFile(dir, "test.js.html", @"<!DOCTYPE html>
<html>
<head>
<script type=""text/javascript"" src=""test.js""></script>
</head>
<body>
</body>
</html>
");
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

        #endregion
    }
}