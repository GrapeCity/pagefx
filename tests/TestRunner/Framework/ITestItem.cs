using System.Collections.Generic;

namespace DataDynamics.PageFX.TestRunner.Framework
{
	public interface ITestItem
	{
		string Name { get; }
		string FullName { get; }
		IEnumerable<ITestItem> GetChildren();
	}
}