using System.Collections.Generic;

namespace DataDynamics.PageFX.TestRunner.Framework
{
	internal static class Extensions
	{
		public static IEnumerable<ITestItem> GetDescendants(this ITestItem item)
		{
			foreach (var child in item.GetChildren())
			{
				yield return child;

				foreach (var descendant in child.GetDescendants())
				{
					yield return descendant;
				}
			}
		}

		public static int Total(this ITestSuite suite)
		{
			return suite.TotalFailed + suite.TotalPassed;
		}

		public static double Percentage(this ITestSuite suite)
		{
			return suite.TotalPassed / (double)suite.Total();
		}
	}
}