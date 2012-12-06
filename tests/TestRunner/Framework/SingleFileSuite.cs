using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DataDynamics.PageFX.Common.Collections;

namespace DataDynamics.PageFX.TestRunner.Framework
{
	internal sealed class SingleFileSuite : ITestSuite
	{
		private IReadOnlyList<ITestItem> _kids;
		private readonly string _path;

		public SingleFileSuite(string path)
		{
			_path = path;
			FullName = Factory.GetFullName(path);
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

		public string FullName { get; private set; }

		public IEnumerable<ITestItem> GetChildren()
		{
			return _kids ?? (_kids = Populate().Memoize());
		}

		public int TotalFailed { get; set; }

		public int TotalPassed { get; set; }

		public override string ToString()
		{
			return Name;
		}

		private IEnumerable<ITestItem> Populate()
		{
			var cases = Directory.GetFiles(_path)
			                     .Where(file => !Path.GetExtension(file).Equals(".xml", StringComparison.OrdinalIgnoreCase))
			                     .Select(file => (ITestItem)Factory.CreateSingleFileTestCase(file));
			var suites = Directory.GetDirectories(_path)
			                      .Select(dir => (ITestItem)new SingleFileSuite(dir));
			return cases.Concat(suites);
		}
	}
}