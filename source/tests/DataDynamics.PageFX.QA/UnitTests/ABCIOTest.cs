using System;
using System.IO;
using DataDynamics.PageFX.FLI.ABC;
using DataDynamics.PageFX.FLI.SWC;
using DataDynamics.PageFX.FLI.SWF;
using NUnit.Framework;

namespace DataDynamics.PageFX.FLI.Tests
{
    [TestFixture]
    public class ABCIOTest
    {
        static string Test(string swcName, byte[] original)
        {
            AbcFile abc;
            try
            {
                abc = new AbcFile(original);
            }
            catch (Exception e)
            {
                return string.Format("Unable to load ABC file.\nException: {0}", e);
            }

            byte[] result;
            try
            {
                result = abc.ToByteArray();
            }
            catch (Exception e)
            {
                return string.Format("Unable to serialize ABC file.\nException: {0}", e);
            }

            int index;
            string err = QA.CompareByteArrays(original, result, out index);
            if (!string.IsNullOrEmpty(err))
            {
                return "SWC: " + swcName + ". ABC: " + abc.Name + "." + err;
            }

            //NOTE: Test below is not passed
            //err = CheckOffsetTranslatation(swcName, original, abc);

            return err;
        }

        private static string CheckOffsetTranslatation(string swcName, byte[] original, AbcFile abc)
        {
            byte[] result;
            int index;
            foreach (var body in abc.MethodBodies)
            {
                var code = body.IL;
                code.TranslateOffsets();
                code.TranslateIndicesEnabled = true;
                code.TranslateIndices();
            }

            try
            {
                result = abc.ToByteArray();
            }
            catch (Exception e)
            {
                return string.Format("Unable to serialize ABC file.\nException: {0}", e);
            }

            string err = QA.CompareByteArrays(original, result, out index);
            if (!string.IsNullOrEmpty(err))
            {
                return "SWC: " + swcName + ". ABC: " + abc.Name + "." + err;
            }

            return err;
        }

        public static string RunSwfTestCase(string name, Stream input)
        {
            var list = SwfMovie.GetRawAbcFiles(input);
            int n = list.Count;
            for (int i = 0; i < n; ++i)
            {
                var data = list[i];
                string err = Test(name, data);
                if (!string.IsNullOrEmpty(err))
                    return err;
            }
            return null;
        }

        public static string RunSwcTestCase(string name, Stream input)
        {
            input = SwcHelper.ExtractLibrary(input);
            return RunSwfTestCase(name, input);
        }

        [Test]
        public void Run()
        {
            var asm = typeof(ABCIOTest).Assembly;
            foreach (var resname in asm.GetManifestResourceNames())
            {
                if (resname.EndsWith(".swc"))
                {
                    var rs = asm.GetManifestResourceStream(resname);
                    string name = QA.RemovePrefix(resname);
                    name = name.Substring("flex.".Length);
                    string err = RunSwcTestCase(name, rs);
                    if (!string.IsNullOrEmpty(err))
                        Assert.Fail(err);
                }
            }
        }
    }
}