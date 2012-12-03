using DataDynamics.PageFX.Ecma335.IL;

namespace DataDynamics.PageFX.Ecma335.Translation
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
