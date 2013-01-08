using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DataDynamics.PageFX.TestRunner.Framework
{
	internal static class NUnitFixtureGeneratpr
	{
		//TODO: provide enum
		internal static string TestSet = "all";

		private static string GetNUnitMethodName(TestCase tc)
		{
			string s = tc.FullName;
			if (!tc.IsNUnit)
			{
				s = RemoveSuffix(s, tc.FileExtension);
			}
			s = RemovePrefix(s, "PageFX.");
			s = RemovePrefix(s, "Simple.");
			s = s.Replace('-', '_');
			s = s.Replace('.', '_');
			return s;
		}

		private static readonly string[] CriticalSubstrings =
			{
				"mono.System.ArrayTest",
				"mono.System.StringTest",
				"mono.System.ConvertTest",
				"CSharp.Delegates.",
				"CSharp.Generics.",
				"CSharp.Nullable."
			};

		private static bool IsCriticalTest(TestCase tc)
		{
			string fullName = tc.FullName;
			if (CriticalSubstrings.Any(fullName.Contains))
			{
				if (fullName.Contains("CSharp.Generics.Casting"))
					return false;
				return true;
			}
			return false;
		}

		private static bool IsFrameworkTest(TestCase tc)
		{
			if (IsCriticalTest(tc)) return false;
			string fullName = tc.FullName;
			if (fullName.Contains("mono.VB.")) return false;
			if (fullName.Contains("mono.")) return true;
			return false;
		}

		private static bool ShouldInclude(TestCase tc)
		{
			if (String.IsNullOrEmpty(TestSet)
			    || String.Equals(TestSet, "all", StringComparison.OrdinalIgnoreCase))
				return true;

			if (String.Equals(TestSet, "critical", StringComparison.OrdinalIgnoreCase))
				return IsCriticalTest(tc);

			if (String.Equals(TestSet, "framework", StringComparison.OrdinalIgnoreCase))
				return IsFrameworkTest(tc);

			if (String.Equals(TestSet, "rest", StringComparison.OrdinalIgnoreCase))
			{
				if (IsCriticalTest(tc)) return false;
				if (IsFrameworkTest(tc)) return false;
				return true;
			}

			return true;
		}

		public static void GenerateNUnitTestFixture(string path, string ns, string classname, string format, IEnumerable<TestCase> testCases)
		{
			string dir = Path.GetDirectoryName(path);
			Directory.CreateDirectory(dir);

			if (String.IsNullOrEmpty(ns))
				ns = "DataDynamics.PageFX.FLI.Tests";

			if (String.IsNullOrEmpty(classname))
				classname = "SWFALL";

			using (var writer = new StreamWriter(path))
			{
				writer.WriteLine("//WARNING: This file is auto generated!!!");
				writer.WriteLine();

				writer.WriteLine("using NUnit.Framework;");
				writer.WriteLine();

				writer.WriteLine("namespace {0}", ns);
				writer.WriteLine("{");

				writer.WriteLine("\t[TestFixture]");
				writer.WriteLine("\tpublic class {0}", classname);
				writer.WriteLine("\t{");

				writer.WriteLine("\t\t[SetUp]");
				writer.WriteLine("\t\tpublic void SetUp()");
				writer.WriteLine("\t\t{");
				writer.WriteLine("\t\t\tQA.IsNUnitSession = true;");
				if (GlobalOptions.RunSuiteAsOneTest)
					writer.WriteLine("\t\t\tQA.RunSuiteAsOneTest = true;");
				writer.WriteLine("\t\t}");

				writer.WriteLine("\t\tstatic void Run(string fullname)");
				writer.WriteLine("\t\t{");
				writer.WriteLine("\t\t\tTestEngine.RunTestCase(fullname, \"{0}\");", format);
				writer.WriteLine("\t\t}");

				foreach (var tc in testCases)
				{
					if (!ShouldInclude(tc)) continue;

					writer.WriteLine("\t\t[Test]");
					writer.WriteLine("\t\tpublic void {0}()", GetNUnitMethodName(tc));
					writer.WriteLine("\t\t{");
					writer.WriteLine("\t\t\tRun(\"{0}\");", tc.FullName);
					writer.WriteLine("\t\t}");
				}

				writer.WriteLine("\t}");

				writer.WriteLine("}");
			}
		}

		public static void GenerateNUnitTestFixture(string path, string ns, string classname, string format)
		{
			GenerateNUnitTestFixture(path, ns, classname, format, SimpleTestCases.GetAllTestCases());
		}

		private static string RemovePrefix(string s, string prefix)
		{
			if (s.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
				return s.Substring(prefix.Length);
			return s;
		}

		private static string RemoveSuffix(string s, string suffix)
		{
			if (s.EndsWith(suffix, StringComparison.OrdinalIgnoreCase))
				return s.Substring(0, s.Length - suffix.Length);
			return s;
		}
	}
}
