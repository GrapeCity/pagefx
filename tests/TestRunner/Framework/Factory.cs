using System.IO;

namespace DataDynamics.PageFX.TestRunner.Framework
{
	internal static class Factory
	{
		internal static string RootDir
		{
			get { return @"c:\projects\pfx\tests"; }
		}

		internal static string GetFullName(string path)
		{
			return path.Substring(RootDir.Length + 1).Replace('\\', '.').Replace('/', '.');
		}

		internal static TestCase CreateSingleFileTestCase(string path)
		{
			var fullname = GetFullName(path);

			var test = new TestCase(fullname, File.ReadAllText(path));
            
			string configPath = Path.ChangeExtension(path, ".xml");
			if (File.Exists(configPath))
			{
				test.LoadConfig(configPath);
			}

			return test;
		}

		internal static TestCase CreateNUnitTestCase(string name, string path, string mainCode)
		{
			var test = new TestCase(name)
				{
					IsNUnit = true,
					NUnitBasePath = path
				};

			test.SourceFiles.Add("Program.cs", mainCode);

			return test;
		}
	}
}