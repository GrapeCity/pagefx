using DataDynamics.PageFX.Common.TypeSystem;

namespace DataDynamics.PageFX.FlashLand.Core
{
	internal static class InternalAssemblyExtensions
	{
		public static AssemblyCustomData CustomData(this IAssembly assembly)
		{
			return AssemblyCustomData.GetInstance(assembly);
		}
	}
}