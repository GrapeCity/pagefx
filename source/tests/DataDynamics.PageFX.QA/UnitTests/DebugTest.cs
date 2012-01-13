using System;
using System.IO;
using NUnit.Framework;

namespace DataDynamics.PageFX.FLI.Tests
{
    [TestFixture]
    public class DebugTest
    {
        //[Test]
        //public void TestConditionalBreak()
        //{
        //    Test("DebugTest.input1.txt",
        //        "DebugTest.output1.txt",
        //        "samples\\simple\\flash\\FadedCircle\\"); // check conditional break
        //}

        [Test]
        public void TestSteppingAndLocalsInfo()
        {
            Test("DebugTest.input2.txt",
                "DebugTest.output2.txt",
                "samples\\simple\\flash\\Filters\\DropShadow\\"); // check stepping and locals info
        }

        private static void Test(string inputFile, string outputFile, string path)
        {
            int exitCode;
            string input1 = ResourceHelper.GetText(typeof (DebugTest), inputFile);
            string output1 = ResourceHelper.GetText(typeof(DebugTest), outputFile);
            string workdir = Path.Combine(GlobalSettings.HomeDirectory, path);
            if (!Directory.Exists(workdir))
                throw new DirectoryNotFoundException(workdir);
            string[] a = output1.Split('\n');
            string output = FdbProcessHost.Run("", Str.GetLines(input1), workdir, out exitCode);
            string[] b = Str.GetLines(output);
            
            if (b.Length > 3)
            {
                b[b.Length - 1] = "";
                b[b.Length - 2] = "";
                b[b.Length - 3] = "";
            }
            
            if (a.Length > 1)
                a[a.Length - 1] = "";
            
            if (exitCode != 0)
                throw new AssertionException(output);
            else if (Str.ToString(a, "\n").Equals(Str.ToString(b, "\n")))
            //else if (!output.Equals(output1))
                throw new AssertionException("input and ouput not equals"
                    +"\n expected: \n"
                    + "------------------------------------------------\n " 
                    + Str.ToString(a, "\n")
                    + "\n actually: \n"
                    + "------------------------------------------------\n " 
                    + Str.ToString(b, "\n")
                    );
        }
    }
}