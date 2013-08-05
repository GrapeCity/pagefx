using DataDynamics.PageFX.Flash.IL;

namespace DataDynamics.PageFX.Flash.Core.Inlining
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