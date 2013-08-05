using DataDynamics.PageFX.Common.TypeSystem;

namespace DataDynamics.PageFX.Flash.Core
{
	internal static class InternalAssemblyExtensions
	{
		public static AssemblyCustomData CustomData(this IAssembly assembly)
		{
			return AssemblyCustomData.GetInstance(assembly);
		}
	}
}