using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.Common.Collections;
using DataDynamics.PageFX.Common.NUnit;
using DataDynamics.PageFX.Common.Tools;
using DataDynamics.PageFX.Common.TypeSystem;

namespace DataDynamics.PageFX.TestRunner.Framework
{
	internal sealed class NUnitSuite : ITestSuite
	{
		private readonly string _path;
		private readonly IAssembly _assembly;

		public NUnitSuite(string path, IAssembly assembly)
		{
			_path = path;
			_assembly = assembly;
		}

		public string Name
		{
			get { return "Mono"; }
		}

		public string FullName
		{
			get { return Name; }
		}

		public IEnumerable<ITestItem> GetChildren()
		{
			if (QA.RunSuiteAsOneTest)
			{
				var cases = from fixture in _assembly.Types.Where(x => x.IsTestFixture())
				            select CreateCase(fixture);

				return cases.GroupBy(x => GetSuiteName(x))
				            .Select(g => (ITestItem)new SimpleSuite(g.Key, g));
			}

			return from fixture in _assembly.Types.Where(x => x.IsTestFixture())
			       select (ITestItem)new NUnitFixture(_path, fixture);
		}

		public int TotalFailed { get; set; }
		public int TotalPassed { get; set; }

		private ITestItem CreateCase(IType fixture)
		{
			var main = fixture.GenerateTestRunnerCode(QA.TestRunnerOptions);
			var name = fixture.GetMonoTestSuiteName();
			return Factory.CreateNUnitTestCase(name, _path, main);
		}

		private static string GetSuiteName(ITestItem item)
		{
			return item.FullName.Substring(0, item.FullName.Length - item.Name.Length - 1);
		}
	}

	internal sealed class NUnitFixture : ITestSuite
	{
		private readonly string _path;
		private readonly IType _suite;
		private IReadOnlyList<ITestItem> _kids;

		public NUnitFixture(string path, IType suite)
		{
			_path = path;
			_suite = suite;
		}

		public string Name
		{
			get
			{
				int i = FullName.LastIndexOf('.');
				if (i >= 0)
					return FullName.Substring(i + 1);
				return FullName;
			}
		}

		public string FullName
		{
			get { return _suite.GetMonoTestSuiteName(); }
		}

		public IEnumerable<ITestItem> GetChildren()
		{
			return _kids ?? (_kids = Populate().Memoize());
		}

		public int TotalFailed { get; set; }
		public int TotalPassed { get; set; }

		private IEnumerable<ITestItem> Populate()
		{
			return from test in _suite.GetUnitTests()
			       let main = test.GenerateTestRunnerCode(QA.TestRunnerOptions)
			       let name = test.GetMonoTestCaseName()
			       select (ITestItem)Factory.CreateNUnitTestCase(name, _path, main);
		}
	}
}