using System.Collections.Generic;

namespace DataDynamics.PageFX.TestRunner.Framework
{
	internal sealed class SimpleSuite : ITestSuite
	{
		private readonly IEnumerable<ITestItem> _items;

		public SimpleSuite(string name, IEnumerable<ITestItem> items)
		{
			_items = items;
			Name = name;
		}

		public string Name { get; set; }

		public string FullName
		{
			get { return Name; }
		}

		public IEnumerable<ITestItem> GetChildren()
		{
			return _items;
		}

		public int TotalFailed { get; set; }
		public int TotalPassed { get; set; }
	}
}