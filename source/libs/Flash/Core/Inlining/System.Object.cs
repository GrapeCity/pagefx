using DataDynamics.PageFX.Flash.IL;

namespace DataDynamics.PageFX.Flash.Core.Inlining
{
	internal sealed class SystemObjectInlines : InlineCodeProvider
	{
		[InlineImpl]
		public static void ReferenceEquals(AbcCode code)
		{
			code.Add(InstructionCode.Strictequals);
			code.FixBool();
		}
	}
}