using System.Collections.Generic;

namespace DataDynamics.PageFX.FlashLand.Abc
{
	public static class AbcInstanceExtensions
	{
		public static IEnumerable<AbcInstance> BaseInstances(this AbcInstance instance)
		{
			var b = instance.BaseInstance;
			while (b != null)
			{
				yield return b;
				b = b.BaseInstance;
			}
		}
	}
}