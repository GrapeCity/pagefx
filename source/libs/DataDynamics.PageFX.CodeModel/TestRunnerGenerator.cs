using System;
using System.Collections.Generic;
using System.IO;
using DataDynamics.PageFX.Common.TypeSystem;

namespace DataDynamics.PageFX.Common
{
	public sealed class TestRunnerGeneratorOptions
	{
		public bool Protect { get; set; }
		public string EndMarker { get; set; }
		public string SuccessString { get; set; }
		public string FailString { get; set; }
	}

	public static class TestRunnerGenerator
	{
		public static string GenerateTestRunnerCode(this IMethod test, TestRunnerGeneratorOptions options)
		{
			using (var writer = new StringWriter())
			{
				test.WriteRunnerCode(writer, options);
				return writer.ToString();
			}
		}

		public static string GenerateTestRunnerCode(this IType testSuite, TestRunnerGeneratorOptions options)
		{
			using (var writer = new StringWriter())
			{
				testSuite.WriteRunnerCode(writer, options);
				return writer.ToString();
			}
		}

		private static void WriteRunnerCode(this IMethod test, TextWriter writer, TestRunnerGeneratorOptions options)
		{
			test.WriteRunnerCode(new CodeTextWriter(writer), options);
		}

		private static void WriteRunnerCode(this IMethod test, CodeTextWriter writer, TestRunnerGeneratorOptions options)
		{
			if (options == null)
				options = new TestRunnerGeneratorOptions { Protect = true };

			var testSuite = test.DeclaringType;

			BeginProgram(writer, testSuite);

			BeginMain(writer);

			RunTest(writer, options, test);

			EndMain(writer, options);

			EndProgram(writer);
		}

		private static void WriteRunnerCode(this IType testSuite, TextWriter writer, TestRunnerGeneratorOptions options)
		{
			testSuite.WriteRunnerCode(new CodeTextWriter(writer), options);
		}

		private static void WriteRunnerCode(this IType testSuite, CodeTextWriter writer, TestRunnerGeneratorOptions options)
		{
			if (options == null)
				options = new TestRunnerGeneratorOptions { Protect = true };

			BeginProgram(writer, testSuite);

			writer.WriteLine("static bool fail = false;");

			var methods = new List<string>();
			foreach (var test in testSuite.GetUnitTests())
			{
				string name = "Run_" + test.Name;
				BeginMethod(writer, name);
				RunTest(writer, options, test);
				EndMethod(writer);
				methods.Add(name);
			}

			BeginMain(writer);

			foreach (var method in methods)
			{
				writer.WriteLine("Console.WriteLine(\"{0}\");", method);
				writer.WriteLine("{0}();", method);
			}

			EndMain(writer, options);

			EndProgram(writer);
		}

		private static void BeginProgram(CodeTextWriter writer, IType testSuite)
		{
			writer.WriteLine("using System;");
			//writer.WriteLine("using NUnit.Framework;");
			if (!String.IsNullOrEmpty(testSuite.Namespace))
				writer.WriteLine("using {0};", testSuite.Namespace);
			writer.WriteLine();

			writer.WriteLine("class Program");
			writer.WriteLine("{");
			writer.IncreaseIndent();
		}

		private static void EndProgram(CodeTextWriter writer)
		{
			writer.EndBlock();
		}

		private static void BeginMethod(CodeTextWriter writer, string name)
		{
			writer.WriteLine("static void {0}()", name);
			writer.WriteLine("{");
			writer.IncreaseIndent();
		}

		private static void EndMethod(CodeTextWriter writer)
		{
			writer.EndBlock();
		}

		private static void BeginMain(CodeTextWriter writer)
		{
			BeginMethod(writer, "Main");
		}

		private static void EndMain(CodeTextWriter writer, TestRunnerGeneratorOptions options)
		{
			if (!String.IsNullOrEmpty(options.EndMarker))
			{
				writer.WriteLine("Console.WriteLine(\"{0}\");", options.EndMarker);
			}

			EndMethod(writer);
		}

		private static void RunTest(CodeTextWriter writer, TestRunnerGeneratorOptions options, IMethod test)
		{
			var type = test.DeclaringType;

			writer.WriteLine("bool fail = false;");

			if (!test.IsStatic)
			{
				writer.WriteLine("{0} obj = new {0}();", type.Name);
			}

			CallSetUp(writer, test);

			writer.WriteLine();

			string expectedException = test.GetExpectedException();
			bool hasExpectedException = !String.IsNullOrEmpty(expectedException);
			bool hasTry = hasExpectedException || options.Protect;

			if (hasTry)
			{
				writer.WriteLine("try");
				writer.WriteLine("{");
				writer.IncreaseIndent();
			}

			CallMethod(writer, test);

			if (hasExpectedException)
			{
				writer.WriteLine("fail = true;");
				writer.WriteLine("Console.WriteLine(\"No expected exception: {0}\");", expectedException);
			}

			if (hasTry)
			{
				writer.DecreaseIndent();
				writer.WriteLine("}"); //end of try
			}

			if (hasExpectedException)
			{
				//catch for expected exception
				writer.WriteLine("catch ({0})", expectedException);
				writer.WriteLine("{");
				writer.IncreaseIndent();
				writer.WriteLine("fail = false;");
				writer.DecreaseIndent();
				writer.WriteLine("}");
			}

			if (hasTry)
			{
				writer.WriteLine("catch (Exception e)");
				writer.WriteLine("{");
				writer.IncreaseIndent();
				writer.WriteLine("fail = true;");
				writer.WriteLine("Console.WriteLine(\"Unexpected exception: \" + e);");
				writer.DecreaseIndent();
				writer.WriteLine("}");
			}

			writer.WriteLine();

			string strFail = options.FailString;
			if (String.IsNullOrEmpty(strFail))
				strFail= "fail";

			string strSuccess = options.SuccessString;
			if (String.IsNullOrEmpty(strSuccess))
				strSuccess = "success";

			writer.WriteLine("Console.WriteLine(fail ? \"{0}\" : \"{1}\");", strFail, strSuccess);
		}

		private static void CallSetUp(TextWriter writer, IMethod test)
		{
			var type = test.DeclaringType;

			//NOTE: TestCase.SetUp now is protected method
			//if (QA.IsTestCase(type))
			//{
			//    if (test.IsStatic) return;
			//    writer.WriteLine("{0}obj.SetUp();", indent);
			//    return;
			//}

			var setup = type.GetUnitTestSetup();
			if (setup == null) return;
			if (setup.Visibility != Visibility.Public) return;

			if (setup.IsStatic)
			{
				writer.WriteLine("{0}.{1}();", type.Name, setup.Name);
				return;
			}

			if (!test.IsStatic)
			{
				writer.WriteLine("obj.{0}();", setup.Name);
			}
		}

		private static void CallMethod(TextWriter writer, IMethod test)
		{
			if (test.IsStatic)
			{
				writer.WriteLine("{0}.{1}();", test.DeclaringType.Name, test.Name);
			}
			else
			{
				writer.WriteLine("obj.{0}();", test.Name);
			}
		}
	}
}