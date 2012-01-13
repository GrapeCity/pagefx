using System;

namespace DataDynamics.PageFX
{
    public static class ComplexTestCases
    {
        private static string GetParentName(string name, bool ext)
        {
            int i = name.LastIndexOf('.');
            if (ext)
            {
                i = name.LastIndexOf('.', i - 1);
            }
            return name.Substring(0, i);
        }

        private static TestCaseCollection Load(string ext)
        {
            var list = new TestCaseCollection();
            var assembly = typeof(TestCase).Assembly;

            var resnames = assembly.GetManifestResourceNames();
            int n = resnames.Length;
            var processed = new bool[n];

            for (int i = 0; i < n; ++i)
            {
                if (processed[i]) continue;
                string resname = resnames[i];
                if (resname.EndsWith(ext, StringComparison.InvariantCultureIgnoreCase) 
                    && resname.Contains("PageFX.Complex"))
                {
                    //folder
                    string fullname = GetParentName(resname, true);
                    var tc = new TestCase(fullname);

                    for (int j = 0; j < n; ++j)
                    {
                        string name2 = resnames[j];
                        if (name2.StartsWith(fullname))
                        {
                            if (name2.EndsWith(ext, StringComparison.InvariantCultureIgnoreCase))
                            {
                                processed[j] = true;
                                var rs = assembly.GetManifestResourceStream(name2);
                                string text = QA.ReadAllText(rs);
                                string name = name2.Substring(fullname.Length + 1);
                                tc.SourceFiles.Add(name, text);
                            }
                            else if (name2.EndsWith(".xml", StringComparison.InvariantCultureIgnoreCase))
                            {
                                processed[j] = true;
                                var rs = assembly.GetManifestResourceStream(name2);
                                tc.LoadConfig(rs);
                            }
                        }
                    }

                    TestSuite.Register(tc);

                    list.Add(tc);
                }
            }
            list.Sort(delegate(TestCase x, TestCase y)
                          {
                              return x.FullName.CompareTo(y.FullName);
                          });
            return list;
        }

        /// <summary>
        /// Gets all CSharp test cases.
        /// </summary>
        public static TestCaseCollection CSharp
        {
            get
            {
                if (_cs == null)
                    _cs = Load(".cs");
                return _cs;
            }
        }
        private static TestCaseCollection _cs;
    }
}