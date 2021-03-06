using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Core.IL;

namespace DataDynamics.PageFX.Core.Translation
{
	internal sealed class CallInstructionInfo
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