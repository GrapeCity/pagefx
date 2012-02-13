using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace DataDynamics.PageFX.Tests
{
    [TestFixture]
    public class NegativeTests
    {
        private void Compile(string resname)
        {
            string sourceCode = ResourceHelper.GetText(GetType(), resname);
            string dir = Path.Combine(QA.RootTestCases, "NegativeTests");
            Directory.CreateDirectory(dir);
            string csfile = Path.Combine(dir, "src.cs");
            File.WriteAllText(csfile, sourceCode);

            string asmpath = Path.Combine(dir, "out.dll");

            var options = new CompilerOptions();
            options.NoLogo = true;
            options.NoConfig = true;
            options.NoStdlib = true;
            options.Target = CompilerTarget.Library;
            options.Output = asmpath;
            GlobalSettings.AddCommonReferences(options);

            options.Input.Add(csfile);

            string err = CompilerConsole.Run(options);
            if (!string.IsNullOrEmpty(err))
                throw new InvalidOperationException("Unable to compile " + resname);

            var asm = QA.LoadAssembly(asmpath, VM.AVM, dir, ref err);

            string swfpath = Path.Combine(dir, "out.swf");
            FLI.Infrastructure.Serialize(asm, swfpath, "/format:swf");
        }

        private void Run(string resname, params string[] errorcodes)
        {
            try
            {
                Compile(resname);
            }
            catch (CompilerException ce)
            {
                Assert.IsTrue(errorcodes.Contains(ce.ErrorCode), ce.Message);
            }
            catch (Exception e)
            {
                Assert.Fail("Unexpected exception {0}", e);
            }
        }

        [Test]
        public void TestIncompatibleCall()
        {
            Run("IncompatibleCall.cs", Errors.ABC.IncompatibleCall.Code);
        }
    }
}