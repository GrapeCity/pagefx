namespace DataDynamics.PageFX.TestRunner.Framework
{
	public interface ITestSuite : ITestItem
	{
		int TotalFailed { get; set; }
		int TotalPassed { get; set; }
	}
}