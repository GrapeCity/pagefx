using System.Collections.Generic;

namespace DataDynamics.PageFX.Flash.Abc
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