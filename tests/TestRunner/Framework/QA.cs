using System;
using System.IO;
using DataDynamics.PageFX.Flash.Swf;
using NUnit.Framework;
using GlobalSettings = DataDynamics.PageFX.Common.CompilerServices.GlobalSettings;

namespace DataDynamics.PageFX.TestRunner.Framework
{
    public static class QA
    {
	    public static string BinDir
        {
            get { return Path.GetDirectoryName(typeof(QA).Assembly.Location); }
        }

        public static string TestResultsReportPath
        {
            get { return Path.Combine(BinDir, "report.htm"); }
        }

        public static string Root
        {
            get { return Path.Combine(GlobalOptions.BaseDir, "PageFX"); }
        }

        public static string RootTestCases
        {
            get { return Path.Combine(Root, "TestCases"); }
        }

        public static string RootReports
        {
            get { return Path.Combine(Root, "Reports"); }
        }

        public static string CommonDirectory
        {
            get { return GetTestCasePath("common"); }
        }

        public static string GetTestCasePath(string subpath)
        {
            return Path.Combine(RootTestCases, subpath);
        }

        public static string GetReportPath(string subpath)
        {
            return Path.Combine(RootReports, subpath);
        }

        public static string NUnitTestsDirectory
        {
            get { return Path.Combine(GlobalSettings.HomeDirectory, "tests"); }
        }

        public static void CopyNUnitTests()
        {
            string dir = NUnitTestsDirectory;
            if (Directory.Exists(dir))
            {
                try
                {
                    string dstDir = CommonDirectory;
                    Directory.CreateDirectory(dstDir);
                    GetNUnitFrameworkPath(dstDir);
                    foreach (var srcFile in Directory.GetFiles(dir))
                    {
                        string dstFile = Path.Combine(dstDir, Path.GetFileName(srcFile));
                        File.Copy(srcFile, dstFile, true);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

	    public static void SaveSwf(SwfMovie swf, string name)
        {
            string dir = Path.Combine(Root, "swf");
            Directory.CreateDirectory(dir);
            swf.Save(Path.Combine(dir, name));
        }

	    internal static string GetNUnitFrameworkPath(string targetDir)
        {
            string path = typeof(TestAttribute).Assembly.Location;

            if (string.IsNullOrEmpty(targetDir))
                return path;

	        Directory.CreateDirectory(targetDir);

	        string fileName = Path.Combine(targetDir, Path.GetFileName(path));
	        if (!File.Exists(fileName))
		        File.Copy(path, fileName, true);

	        return fileName;
        }

        internal const string NUnitFrameworkDll = "NUnit.Framework.dll";
    }
}