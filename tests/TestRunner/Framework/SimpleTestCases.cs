using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DataDynamics.PageFX.Common.Collections;
using DataDynamics.PageFX.Common.CompilerServices;
using DataDynamics.PageFX.Common.TypeSystem;

namespace DataDynamics.PageFX.TestRunner.Framework
{
    public static class SimpleTestCases
    {
	    private static IEnumerable<string> GetFiles(string dir, string ext)
	    {
		    string pattern = string.IsNullOrEmpty(ext) ? "*.*" : "*" + ext;
		    return Directory.GetFiles(dir, pattern)
		                    .Concat(Directory.GetDirectories(dir)
		                                     .SelectMany(subdir => GetFiles(subdir, ext)));
	    }

	    public static IEnumerable<TestCase> Load(string testDir, string ext)
		{
			return GetFiles(Path.Combine(Factory.RootDir, testDir), ext).Select(path => Factory.CreateSingleFileTestCase(path));
        }

	    private static ITestSuite LoadNUnitSuite(string path)
        {
            string fullpath = path + ".avm.dll";
	        if (!File.Exists(fullpath))
		        return null;
	        if (!GlobalOptions.LoadNUnitTests)
		        return null;

            string name = Path.GetFileNameWithoutExtension(path);
            string tcroot = Path.Combine(QA.Root, name);

            string error = "";
            IAssembly asm;

            bool oldVal = GlobalSettings.EmitDebugInfo;
            try
            {
                GlobalSettings.EmitDebugInfo = false;
                asm = AssemblyLoader.Load(fullpath, VM.AVM, tcroot, ref error);
            }
            finally
            {
                GlobalSettings.EmitDebugInfo = oldVal;
            }

	        if (asm == null)
		        return null;

			return new NUnitSuite(path, asm);
        }

	    public static IReadOnlyList<TestCase> GetAllTestCases()
	    {
		    return _all ?? (_all = GetSuite().GetDescendants().OfType<TestCase>().Memoize());
	    }

	    private static IReadOnlyList<TestCase> _all;

		public static ITestSuite GetSuite()
		{
			var suite1 = new SingleFileSuite(Path.Combine(Factory.RootDir, "Simple"));
			if (GlobalOptions.LoadNUnitTests)
			{
				string dir = QA.NUnitTestsDirectory;
				var suite2 = LoadNUnitSuite(Path.Combine(dir, "mono"));
				if (suite2 != null)
				{
					return new SimpleSuite("PFX", new[] {suite1, suite2});
				}
			}
			return suite1;
		}

        public static TestCase Find(string name)
        {
			return GetAllTestCases().FirstOrDefault(tc => string.Equals(tc.FullName, name, StringComparison.CurrentCultureIgnoreCase));
        }
    }
}