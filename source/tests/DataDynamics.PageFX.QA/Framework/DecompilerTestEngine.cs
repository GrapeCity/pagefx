using System;
using System.Collections.Generic;
using System.IO;
using DataDynamics.PageFX.CodeModel;
using NUnit.Framework;

namespace DataDynamics.PageFX.CLI
{
    [TestFixture]
    public class DecompilerTestEngine
    {
        public static void RunTestCase(TestCase tc, TestDriverSettings tds)
        {
            tc.VM = VM.CLR;
            tc.Reset();

            if (!QA.Compile(tc)) return;

            var asm = tc.LoadAssembly();
            if (asm == null) return;
            
            string root = tc.Root;
            string cspath = Path.Combine(root, "src.cs");
            try
            {
                ExportService.ToFile(asm, "c#", cspath);
            }
            catch (Exception e)
            {
                tc.Error = string.Format("Unable to serialize c# source code.\nException:\n{0}", e);
                return;
            }

            if (tds.SetDecompiledCode)
                tc.DecompiledCode = File.ReadAllText(cspath);

            string exePath2 = Path.Combine(root, "dc.exe");
            string err = CompilerConsole.SimpleRun(CompilerLanguage.CSharp, cspath, exePath2, tc.Debug, tc.Optimize);
            if (!string.IsNullOrEmpty(err))
            {
                tc.Error = string.Format("Unable to compile deserialized c# file.\n{0}", err);
                return;
            }

            int exitCode1, exitCode2;
            tc.Output1 = CommandPromt.Run(tc.ExePath, "", out exitCode1);
            tc.Output2 = CommandPromt.Run(exePath2, "", out exitCode2);

            if (tc.CheckExitCode)
            {
                if (exitCode2 != 0)
                {
                    tc.Error = string.Format("{0} returned non zero exit code {1}.",
                                             exePath2, exitCode2);
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
            {
                tc.Error = QA.CompareLines(tc.Output1, tc.Output2, false);
            }
        }

        private static void Run(IEnumerable<TestCase> list, TestDriverSettings tds)
        {
            foreach (var tc in list)
            {
                RunTestCase(tc, tds);
                tds.Report.Add(tc, "optimize-", tc.Error);
            }
            foreach (var tc in list)
            {
                RunTestCase(tc, tds);
                tds.Report.Add(tc, "optimize+", tc.Error);
            }
        }

        //[Test]
        //public void Run_CSharp()
        //{
        //    CIL.ResetCoverage();
        //    TestDriverSettings tds = new TestDriverSettings();
        //    Run(SimpleTestCases.CSharp, tds);
        //    Run(ComplexTestCases.CSharp, tds);
        //    tds.Report.Export("cli.cs.xml", "xml");
        //    CIL.DumpCoverage(QA.GetReportPath("coverage.cli.cs.txt"));
        //}

        //[Test]
        //public void Run_VB()
        //{
        //    CIL.ResetCoverage();
        //    TestDriverSettings tds = new TestDriverSettings();
        //    Run(SimpleTestCases.VB, tds);
        //    tds.Report.Export("cli.vb.xml", "xml");
        //    CIL.DumpCoverage(QA.GetReportPath("coverage.cli.vb.txt"));
        //}

        //[Test]
        //public void Run_Rotor_IL()
        //{
        //    CIL.ResetCoverage();
        //    TestDriverSettings tds = new TestDriverSettings();
        //    Run(RotorTestCases.IL, tds);
        //    tds.Report.Export("cli.rotor.il.xml", "xml");
        //    CIL.DumpCoverage(QA.GetReportPath("coverage.rotor.il.txt"));
        //}
    }
}
