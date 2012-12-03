using DataDynamics.PageFX.FlashLand.IL;

namespace DataDynamics.PageFX.FlashLand.Core.Inlining
{
	internal sealed class DebuggerInlines : InlineCodeProvider
	{
		[InlineImpl]
		public static void Break(AbcCode code)
		{
			code.DebuggerBreak();
		}
	}
}