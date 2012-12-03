using DataDynamics.PageFX.CLI.IL;
using DataDynamics.PageFX.Common.TypeSystem;

namespace DataDynamics.PageFX.CLI.Translation
{
	internal class CallInstructionInfo
	{
		public readonly IMethod Method;
		public readonly Instruction Instruction;
		public bool SwapAfter;

		public bool IsNewobj
		{
			get { return Instruction.Code == InstructionCode.Newobj; }
		}

		public CallInstructionInfo(IMethod method, Instruction call)
		{
			Method = method;
			Instruction = call;
		}
	}
}