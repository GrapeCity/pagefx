using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DataDynamics.PageFX.Common.NUnit;
using DataDynamics.PageFX.Common.Tools;
using DataDynamics.PageFX.Common.TypeSystem;

namespace DataDynamics.PageFX.TestRunner.Framework
{
    public static class SimpleTestCases
    {
        private static string ReadText(string path)
        {
	        return File.ReadAllText(path);
        }

        private static List<TestCase> Load(string ext)
        {
            return Load("Simple", ext);
        }

	    private static string RootDir
	    {
			get { return @"c:\projects\pfx\tests"; }
	    }

		private static IEnumerable<string> GetFiles(string dir, string ext)
		{
			foreach (var file in Directory.GetFiles(dir, "*" + ext))
			{
				yield return file;
			}

			foreach (var subdir in Directory.GetDirectories(dir))
			{
				foreach (var file in GetFiles(subdir, ext))
				{
					yield return file;
				}
			}
		}

		public static List<TestCase> Load(string testDir, string ext)
        {
            var list = new List<TestCase>();

            foreach (var path in GetFiles(Path.Combine(RootDir, testDir), ext))
            {
				var tc = CreateSimpleTestCase(path);
				TestSuite.Register(tc);
				list.Add(tc);
            }

            return list;
        }

        #region LoadNUnitTests
        private static void LoadNUnitTests(ICollection<TestCase> list, string path)
        {
            string fullpath = path + ".avm.dll";
            if (!File.Exists(fullpath)) return;

            if (!QA.LoadNUnitTests) return;

            string name = Path.GetFileNameWithoutExtension(path);
            string tcroot = Path.Combine(QA.Root, name);

            string error = "";
            IAssembly asm;

            bool oldVal = GlobalSettings.EmitDebugInfo;
            try
            {
                GlobalSettings.EmitDebugInfo = false;
                asm = QA.LoadAssembly(fullpath, VM.AVM, tcroot, ref error);
            }
            finally
            {
                GlobalSettings.EmitDebugInfo = oldVal;
            }

            if (asm == null) return;

            foreach (var type in asm.Types)
            {
                if (type.IsTestFixture())
                    LoadNUnitTests(list, type, path);
            }
        }

        private static void LoadNUnitTests(ICollection<TestCase> list, IType testSuite, string path)
        {
            if (QA.RunSuiteAsOneTest)
            {
                string main = testSuite.GenerateTestRunnerCode(QA.TestRunnerOptions);

                string name = testSuite.GetMonoTestSuiteName();

                RegisterNUnitTest(list, name, path, main);
            }
            else
            {
                foreach (var test in testSuite.GetUnitTests())
                {
                    string main = test.GenerateTestRunnerCode(QA.TestRunnerOptions);
                    string name = test.GetMonoTestCaseName();

                    RegisterNUnitTest(list, name, path, main);
                }
            }
        }

        private static void RegisterNUnitTest(ICollection<TestCase> list, string name, string path, string mainCode)
        {
            var tc = new TestCase("Simple." + name)
                         {
                             IsNUnit = true,
                             NUnitBasePath = path
                         };

            tc.SourceFiles.Add("Program.cs", mainCode);
            list.Add(tc);

            TestSuite.Register(tc);
        }
        #endregion

        private static TestCase CreateSimpleTestCase(string path)
        {
			var fullname = path.Substring(RootDir.Length + 1).Replace('\\', '.').Replace('/', '.');
            var tc = new TestCase(fullname, ReadText(path));
            
            string configPath = Path.ChangeExtension(path, ".xml");
            if (File.Exists(configPath))
            {
                tc.LoadConfig(configPath);
            }

            return tc;
        }

        public static List<TestCase> All
        {
            get
            {
                if (_all == null)
                {
                    _all = new List<TestCase>();
                    _all.AddRange(CSharp);
                    _all.AddRange(VB);
                }
                return _all;
            }
        }

        private static List<TestCase> _all;

        public static TestCase Find(string name)
        {
			return All.FirstOrDefault(tc => string.Equals(tc.FullName, name, StringComparison.CurrentCultureIgnoreCase));
        }

        /// <summary>
        /// Gets all CSharp test cases.
        /// </summary>
        public static List<TestCase> CSharp
        {
            get
            {
                if (_cs == null)
                {
                    _cs = Load(".cs");
                    if (QA.LoadNUnitTests)
                    {
                        string dir = QA.NUnitTestsDirectory;
                        LoadNUnitTests(_cs, Path.Combine(dir, "mono"));
                        if (QA.IsNUnitSession)
                            QA.CopyNUnitTests();
                    }
                    if (QA.SortTests)
                        _cs.Sort((x, y) => x.FullName.CompareTo(y.FullName));
                }
                return _cs;
            }
        }
        private static List<TestCase> _cs;

        /// <summary>
        /// Gets all VB test cases.
        /// </summary>
        public static List<TestCase> VB
        {
            get { return _vb ?? (_vb = Load(".vb")); }
        }
        private static List<TestCase> _vb;
    }
}