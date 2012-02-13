using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DataDynamics.PageFX.CodeModel
{
    public static class NUnitHelper
    {
        const string NS = "NUnit.Framework";
        const string NSPrefix = NS + ".";

        static class Types
        {
            public const string TestCase = NSPrefix + "TestCase";
            public const string Assertion = NSPrefix + "Assertion";
        }

        static class Attrs
        {
            public const string Suffix = "Attribute";

            public const string TestFixture = NSPrefix + "TestFixture" + Suffix;

            public const string Category = NSPrefix + "Category" + Suffix;
            public const string Description = NSPrefix + "Description" + Suffix;
            public const string Ignore = NSPrefix + "Ignore" + Suffix;

            public const string SetUp = NSPrefix + "SetUp" + Suffix;
            public const string TearDown = NSPrefix + "TearDown" + Suffix;

            public const string TestFixtureSetUp = NSPrefix + "TestFixtureSetUp" + Suffix;
            public const string TestFixtureTearDown = NSPrefix + "TestFixtureTearDown" + Suffix;

            public const string ExpectedException = NSPrefix + "ExpectedException" + Suffix;
        }

        public static bool IsTestFixture(IType type)
        {
            if (GenericType.HasGenericParams(type))
                return false;

            if (HasAttribute(type, Attrs.TestFixture))
                return true;

            var bt = type.BaseType;
            if (bt != null)
            {
                string fn = bt.FullName;
                if (fn == Types.TestCase || fn == Types.Assertion)
                    return true;
            }

            return false;
        }

        public static string GetDescription(ITypeMember member)
        {
            var attr = FindAttribute(member, Attrs.Description);
            if (attr == null) return null;
            return attr.Arguments[0].Value as string;
        }

        public static string[] GetCategories(IMethod method)
        {
            if (method == null) return null;
        	return (from attr in method.CustomAttributes
					where attr.TypeName == Attrs.Category
					select attr.Arguments[0].Value as string into value
					where !string.IsNullOrEmpty(value) select value).ToArray();
        }

        public static bool HasCategories(IMethod method, bool ignoreCase, params string[] cats)
        {
            if (cats == null) return false;
            if (cats.Length <= 0) return false;
            var mcats = GetCategories(method);
            if (mcats == null) return false;
            if (mcats.Length <= 0) return false;
            return mcats.Any(cat => cats.Any(c => string.Compare(cat, c, ignoreCase) == 0));
        }

        static readonly string[] NotTestAttrs = 
        {
            Attrs.SetUp,
            Attrs.TearDown,
            Attrs.TestFixtureSetUp,
            Attrs.TestFixtureTearDown,
        };

        public static bool IsIgnored(IMethod method)
        {
            return HasAttribute(method, Attrs.Ignore);
        }

        public static bool IsTest(IMethod method, bool pfx)
        {
            if (method.IsConstructor) return false;
            if (!TypeService.IsVoid(method)) return false;
            if (method.Visibility != Visibility.Public) return false;
            //I'm not shure about static methods, need to check
            if (method.IsStatic) return false;
            if (method.Parameters.Count > 0) return false;
            if (GenericType.IsGenericContext(method)) return false;
            if (IsIgnored(method)) return false;
            if (HasAttribute(method, NotTestAttrs)) return false;

            if (pfx)
            {
                if (HasCategories(method, true, "NotDotNet", "NotWorking"))
                    return false;
            }

            return true;
        }

        public static bool IsTest(IMethod method)
        {
            return IsTest(method, true);
        }

        public static bool IsSetup(IMethod method)
        {
            if (method == null) return false;
            return HasAttribute(method, Attrs.SetUp, Attrs.TestFixtureSetUp);
        }

        public static bool IsNUnitMethod(IMethod method)
        {
            if (method == null) return false;
            return method.CustomAttributes.Any(attr => attr.TypeName.StartsWith(NSPrefix));
        }

        public static IMethod FindSetupMethod(IType type)
        {
            return type.Methods.FirstOrDefault(IsSetup);
        }

        public static IType FindTestFixture(IAssembly assembly)
        {
            return assembly.Types.FirstOrDefault(IsTestFixture);
        }

        public static IEnumerable<IType> GetTestFixtures(IAssembly assembly)
        {
            return assembly.Types.Where(IsTestFixture);
        }

        public static IEnumerable<IMethod> GetTests(IType fixture, bool pfx)
        {
            var list = new List<IMethod>(fixture.Methods);
            return list.Where(m => IsTest(m, pfx));
        }

        public static IEnumerable<IMethod> GetTests(IType fixture)
        {
            return GetTests(fixture, true);
        }

        public static IType GetExpectedExceptionType(IMethod test)
        {
            var attr = FindAttribute(test, Attrs.ExpectedException);
            if (attr == null) return null;

            var ctor = attr.Constructor;
            if (ctor == null)
                return null;

            var arg = attr.Arguments[0];
            if (arg.Type == SystemTypes.Type)
                return (IType)arg.Value;

            return null;
        }

        public static string GetExpectedException(IMethod test)
        {
            var attr = FindAttribute(test, Attrs.ExpectedException);
            if (attr == null) return null;

            var ctor = attr.Constructor;
            if (ctor == null)
                return null;

            var arg = attr.Arguments[0];
            if (arg.Type == SystemTypes.String)
                return (string)arg.Value;

            if (arg.Type == SystemTypes.Type)
            {
                var type = (IType)arg.Value;
                return type.FullName;
            }

            return null;
        }

        static ICustomAttribute FindAttribute(ICustomAttributeProvider p, string fullname)
        {
            return p.CustomAttributes.FirstOrDefault(attr => attr.TypeName == fullname);
        }

        static bool HasAttribute(ICustomAttributeProvider p, string fullname)
        {
            return FindAttribute(p, fullname) != null;
        }

        static bool HasAttribute(ICustomAttributeProvider p, params string[] attrs)
        {
            return p.CustomAttributes.Any(attr => attrs.Contains(attr.TypeName));
        }

        #region GetMonoTestCaseName
        public static string GetMonoTestSuiteName(IType type)
        {
            string suite = type.FullName;

            const string monons = "MonoTests.";
            if (suite.StartsWith(monons))
                suite = "mono." + suite.Substring(monons.Length);

            suite = suite.Replace('+', '.');

            return suite;
        }

        public static string GetMonoTestCaseName(IMethod test)
        {
            var suite = GetMonoTestSuiteName(test.DeclaringType);

            string name = test.Name;
            if (suite == "mono.System.ConvertTest")
            {
                if (!InsertPoint_ConvertTest(ref name))
                    name = "Misc." + name;
            }

            return suite + "." + name;
        }

        static bool InsertPoint(ref string name, string prefix)
        {
            if (name.StartsWith(prefix + "_"))
            {
                name = prefix + "." + name.Substring(prefix.Length + 1);
                return true;
            }

            if (name.StartsWith(prefix))
            {
                name = prefix + "." + name.Substring(prefix.Length);
                return true;
            }

            return false;
        }

        static bool InsertPoint_ConvertTest(ref string name)
        {
            if (InsertPoint(ref name, "FromBase64")) return true;
            if (InsertPoint(ref name, "ToBase64")) return true;
            if (InsertPoint(ref name, "ToByte")) return true;
            if (InsertPoint(ref name, "ToInt16")) return true;
            if (InsertPoint(ref name, "ToInt32")) return true;
            if (InsertPoint(ref name, "ToInt64")) return true;
            if (InsertPoint(ref name, "ToSByte")) return true;
            if (InsertPoint(ref name, "ToUInt16")) return true;
            if (InsertPoint(ref name, "ToUInt32")) return true;
            if (InsertPoint(ref name, "ToUInt64")) return true;
            if (InsertPoint(ref name, "TestTo")) return true;
            return false;
        }
        #endregion

        #region Generation of Main
        public class TestRunnerOptions
        {
            public bool Protect { get; set; }
            public string EndMarker { get; set; }
            public string SuccessString { get; set; }
            public string FailString { get; set; }
        }

        public static string GenerateRunnerCode(IMethod test, TestRunnerOptions options)
        {
            using (var writer = new StringWriter())
            {
                WriteRunnerCode(writer, options, test);
                return writer.ToString();
            }
        }

        public static string GenerateRunnerCode(IType testSuite, TestRunnerOptions options)
        {
            using (var writer = new StringWriter())
            {
                WriteRunnerCode(writer, options, testSuite);
                return writer.ToString();
            }
        }

        public static void WriteRunnerCode(TextWriter writer, TestRunnerOptions options, IMethod test)
        {
            WriteRunnerCode(new CodeTextWriter(writer), options, test);
        }

        public static void WriteRunnerCode(CodeTextWriter writer, TestRunnerOptions options, IMethod test)
        {
            if (options == null)
                options = new TestRunnerOptions { Protect = true };

            var testSuite = test.DeclaringType;

            BeginProgram(writer, testSuite);

            BeginMain(writer);

            RunTest(writer, options, test);

            EndMain(writer, options);

            EndProgram(writer);
        }

        public static void WriteRunnerCode(TextWriter writer, TestRunnerOptions options, IType testSuite)
        {
            WriteRunnerCode(new CodeTextWriter(writer), options, testSuite);
        }

        public static void WriteRunnerCode(CodeTextWriter writer, TestRunnerOptions options, IType testSuite)
        {
            if (options == null)
                options = new TestRunnerOptions { Protect = true };

            BeginProgram(writer, testSuite);

            writer.WriteLine("static bool fail = false;");

            var methods = new List<string>();
            foreach (var test in GetTests(testSuite))
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

        static void BeginProgram(CodeTextWriter writer, IType testSuite)
        {
            writer.WriteLine("using System;");
            //writer.WriteLine("using NUnit.Framework;");
            if (!string.IsNullOrEmpty(testSuite.Namespace))
                writer.WriteLine("using {0};", testSuite.Namespace);
            writer.WriteLine();

            writer.WriteLine("class Program");
            writer.WriteLine("{");
            writer.IncreaseIndent();
        }

        static void EndProgram(CodeTextWriter writer)
        {
            writer.EndBlock();
        }

        static void BeginMethod(CodeTextWriter writer, string name)
        {
            writer.WriteLine("static void {0}()", name);
            writer.WriteLine("{");
            writer.IncreaseIndent();
        }

        static void EndMethod(CodeTextWriter writer)
        {
            writer.EndBlock();
        }

        static void BeginMain(CodeTextWriter writer)
        {
            BeginMethod(writer, "Main");
        }

        static void EndMain(CodeTextWriter writer, TestRunnerOptions options)
        {
            if (!string.IsNullOrEmpty(options.EndMarker))
            {
                writer.WriteLine("Console.WriteLine(\"{0}\");", options.EndMarker);
            }

            EndMethod(writer);
        }

        static void RunTest(CodeTextWriter writer, TestRunnerOptions options, IMethod test)
        {
            var type = test.DeclaringType;

            writer.WriteLine("bool fail = false;");

            if (!test.IsStatic)
            {
                writer.WriteLine("{0} obj = new {0}();", type.Name);
            }

            CallSetUp(writer, test);

            writer.WriteLine();

            string expectedException = GetExpectedException(test);
            bool hasExpectedException = !string.IsNullOrEmpty(expectedException);
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
            if (string.IsNullOrEmpty(strFail))
                strFail= "fail";

            string strSuccess = options.SuccessString;
            if (string.IsNullOrEmpty(strSuccess))
                strSuccess = "success";

            writer.WriteLine("Console.WriteLine(fail ? \"{0}\" : \"{1}\");", strFail, strSuccess);
        }

        static void CallSetUp(TextWriter writer, IMethod test)
        {
            var type = test.DeclaringType;

            //NOTE: TestCase.SetUp now is protected method
            //if (QA.IsTestCase(type))
            //{
            //    if (test.IsStatic) return;
            //    writer.WriteLine("{0}obj.SetUp();", indent);
            //    return;
            //}

            var setup = FindSetupMethod(type);
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

        static void CallMethod(TextWriter writer, IMethod test)
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
        #endregion
    }
}