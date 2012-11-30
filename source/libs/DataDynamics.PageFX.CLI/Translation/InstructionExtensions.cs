using DataDynamics.PageFX.CLI.IL;

namespace DataDynamics.PageFX.CLI.Translation
{
	internal static class InstructionExtensions
	{
		public static bool IsByRef(this Instruction instruction)
		{
			var p = instruction.Parameter;
			return p != null && p.IsByRef;
		}
	}
}
