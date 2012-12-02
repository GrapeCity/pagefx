using System.IO;
using Ionic.Zip;

namespace DataDynamics.PageFX
{
    public static class RotorTestCases
    {
        public static TestCaseCollection IL
        {
            get
            {
                if (_il == null)
                {
                    _il = new TestCaseCollection();
                    var stream = typeof(RotorTestCases).GetResourceStream("Rotor.il_bvt.zip");
                    if (stream != null)
                    {
                        var zip = ZipFile.Read(stream);
                        foreach (var zipEntry in zip)
                        {
                            string name = zipEntry.FileName;
                            if (name.EndsWith(".il"))
                            {
                                stream = zipEntry.OpenReader().ToMemoryStream();
                                string text;
                                using (var reader = new StreamReader(stream))
                                    text = reader.ReadToEnd();

                                int i = name.IndexOf('/');
                                if (i < 0) i = name.IndexOf('\\');
                                name = name.Substring(i + 1);
                                name = name.Replace('/', '.');
                                name = name.Replace('\\', '.');
                                var tc = new TestCase("Rotor.CIL." + name, text);
                                TestSuite.Register(tc);
                                _il.Add(tc);
                            }
                        }
                    }
                }
                return _il;
            }
        }
        private static TestCaseCollection _il;
    }
}