using DataDynamics.PageFX.FlashLand.IL;

namespace DataDynamics.PageFX.FlashLand.Core.Inlining
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