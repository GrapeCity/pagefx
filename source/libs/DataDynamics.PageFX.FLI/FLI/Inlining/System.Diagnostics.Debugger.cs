using DataDynamics.PageFX.FLI.IL;

namespace DataDynamics.PageFX.FLI.Inlining
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