using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Core.IL;

namespace DataDynamics.PageFX.Core.Translation
{
	internal static class InstructionExtensions
	{
		public static bool IsByRef(this Instruction instruction)
		{
			var p = instruction.Parameter;
			return p != null && p.IsByRef();
		}
	}
}
