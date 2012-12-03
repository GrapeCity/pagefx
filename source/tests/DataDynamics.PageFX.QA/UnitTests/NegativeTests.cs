using System;
using System.IO;
using System.Linq;
using DataDynamics.PageFX.Common.Extensions;
using DataDynamics.PageFX.Common.Utilities;
using DataDynamics.PageFX.FlashLand;
using NUnit.Framework;

namespace DataDynamics.PageFX.Tests
{
    [TestFixture]
    public class NegativeTests
    {
        private void Compile(string resname)
        {
            string sourceCode = GetType().GetTextResource(resname);
            string dir = Path.Combine(QA.RootTestCases, "NegativeTests");
            Directory.CreateDirectory(dir);
            string csfile = Path.Combine(dir, "src.cs");
            File.WriteAllText(csfile, sourceCode);

            string asmpath = Path.Combine(dir, "out.dll");

        	var options = new CompilerOptions
        	              	{
        	              		NoLogo = true,
        	              		NoConfig = true,
        	              		NoStdlib = true,
        	              		Target = CompilerTarget.Library,
        	              		Output = asmpath
        	              	};
        	GlobalSettings.AddCommonReferences(options);

            options.Input.Add(csfile);

            string err = CompilerConsole.Run(options);
            if (!string.IsNullOrEmpty(err))
                throw new InvalidOperationException("Unable to compile " + resname);

            var asm = QA.LoadAssembly(asmpath, VM.AVM, dir, ref err);

            string swfpath = Path.Combine(dir, "out.swf");
            FlashLanguageInfrastructure.Serialize(asm, swfpath, "/format:swf");
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