using DataDynamics.PageFX.FLI.IL;

namespace DataDynamics.PageFX.FLI.Inlining
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