using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.Common.TypeSystem;

namespace DataDynamics.PageFX.Common.NUnit
{
	public static class NUnitExtensions
	{
		private const string Namespace = "NUnit.Framework";
		private const string NsPrefix = Namespace + ".";

		private static class Types
		{
			public const string TestCase = NsPrefix + "TestCase";
			public const string Assertion = NsPrefix + "Assertion";
		}

		private static class Attrs
		{
			private const string Suffix = "Attribute";

			public const string TestFixture = NsPrefix + "TestFixture" + Suffix;

			public const string Category = NsPrefix + "Category" + Suffix;
			public const string Description = NsPrefix + "Description" + Suffix;
			public const string Ignore = NsPrefix + "Ignore" + Suffix;

			public const string SetUp = NsPrefix + "SetUp" + Suffix;
			public const string TearDown = NsPrefix + "TearDown" + Suffix;

			public const string TestFixtureSetUp = NsPrefix + "TestFixtureSetUp" + Suffix;
			public const string TestFixtureTearDown = NsPrefix + "TestFixtureTearDown" + Suffix;

			public const string ExpectedException = NsPrefix + "ExpectedException" + Suffix;
		}

		public static bool IsTestFixture(this IType type)
		{
			if (GenericType.HasGenericParams(type))
				return false;

			if (type.HasAttribute(Attrs.TestFixture))
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

		public static string GetTestDescription(this ITypeMember member)
		{
			var attr = member.FindAttribute(Attrs.Description);
			if (attr == null) return null;
			return attr.Arguments[0].Value as string;
		}

		public static string[] GetCategories(this IMethod method)
		{
			if (method == null) return null;
			return (from attr in method.CustomAttributes
			        where attr.TypeName == Attrs.Category
			        select attr.Arguments[0].Value as string
			        into value
			        where !string.IsNullOrEmpty(value)
			        select value).ToArray();
		}

		public static bool HasCategories(this IMethod method, bool ignoreCase, params string[] categories)
		{
			if (categories == null) return false;
			if (categories.Length <= 0) return false;
			var testCategories = method.GetCategories();
			if (testCategories == null) return false;
			if (testCategories.Length <= 0) return false;
			return testCategories.Any(cat => categories.Any(c => string.Compare(cat, c, ignoreCase) == 0));
		}

		private static readonly string[] NotTestAttrs =
			{
				Attrs.SetUp,
				Attrs.TearDown,
				Attrs.TestFixtureSetUp,
				Attrs.TestFixtureTearDown
			};

		public static bool IsIgnored(IMethod method)
		{
			return method.HasAttribute(Attrs.Ignore);
		}

		public static bool IsUnitTest(this IMethod method, bool pfx)
		{
			if (method.IsConstructor) return false;
			if (!method.IsVoid()) return false;
			if (method.Visibility != Visibility.Public) return false;
			//I'm not shure about static methods, need to check
			if (method.IsStatic) return false;
			if (method.Parameters.Count > 0) return false;
			if (GenericType.IsGenericContext(method)) return false;
			if (IsIgnored(method)) return false;
			if (method.HasAttribute(NotTestAttrs)) return false;

			if (pfx)
			{
				if (method.HasCategories(true, "NotDotNet", "NotWorking"))
					return false;
			}

			return true;
		}

		public static bool IsUnitTest(this IMethod method)
		{
			return method.IsUnitTest(true);
		}

		public static bool IsUnitTestSetup(this IMethod method)
		{
			if (method == null) return false;
			return method.HasAttribute(Attrs.SetUp, Attrs.TestFixtureSetUp);
		}

		public static bool IsNUnitMethod(IMethod method)
		{
			if (method == null) return false;
			return method.CustomAttributes.Any(attr => attr.TypeName.StartsWith(NsPrefix));
		}

		public static IMethod GetUnitTestSetup(this IType type)
		{
			return type.Methods.FirstOrDefault(IsUnitTestSetup);
		}

		public static IType FindTestFixture(this IAssembly assembly)
		{
			return assembly.Types.FirstOrDefault(IsTestFixture);
		}

		public static IEnumerable<IType> GetTestFixtures(this IAssembly assembly)
		{
			return assembly.Types.Where(IsTestFixture);
		}

		public static IEnumerable<IMethod> GetUnitTests(this IType fixture, bool pfx)
		{
			var list = new List<IMethod>(fixture.Methods);
			return list.Where(m => m.IsUnitTest(pfx));
		}

		public static IEnumerable<IMethod> GetUnitTests(this IType fixture)
		{
			return fixture.GetUnitTests(true);
		}

		public static IType GetExpectedExceptionType(this IMethod test)
		{
			var attr = test.FindAttribute(Attrs.ExpectedException);
			if (attr == null) return null;

			var ctor = attr.Constructor;
			if (ctor == null)
				return null;

			var arg = attr.Arguments[0];
			if (arg.Type.Is(SystemTypeCode.Type))
				return (IType)arg.Value;

			return null;
		}

		public static string GetExpectedException(this IMethod test)
		{
			var attr = test.FindAttribute(Attrs.ExpectedException);
			if (attr == null) return null;

			var ctor = attr.Constructor;
			if (ctor == null)
				return null;

			var arg = attr.Arguments[0];
			if (arg.Type.Is(SystemTypeCode.String))
				return (string)arg.Value;

			if (arg.Type.Is(SystemTypeCode.Type))
			{
				var type = (IType)arg.Value;
				return type.FullName;
			}

			return null;
		}

		#region GetMonoTestCaseName

		public static string GetMonoTestSuiteName(this IType type)
		{
			string suite = type.FullName;

			const string monons = "MonoTests.";
			if (suite.StartsWith(monons))
				suite = suite.Substring(monons.Length);

			suite = suite.Replace('+', '.');

			return suite;
		}

		public static string GetMonoTestCaseName(this IMethod test)
		{
			var suite = test.DeclaringType.GetMonoTestSuiteName();

			string name = test.Name;
			if (suite == "mono.System.ConvertTest")
			{
				if (!InsertPoint_ConvertTest(ref name))
					name = "Misc." + name;
			}

			return suite + "." + name;
		}

		private static bool InsertPoint(ref string name, string prefix)
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

		private static bool InsertPoint_ConvertTest(ref string name)
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
	}
}