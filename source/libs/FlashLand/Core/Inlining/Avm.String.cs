using DataDynamics.PageFX.FlashLand.IL;

namespace DataDynamics.PageFX.FlashLand.Core.Inlining
{
	internal sealed class AvmStringInlines : InlineCodeProvider
	{
		[InlineImpl]
		public static void op_Implicit(AbcCode code)
		{
			// do nothing since System.String is implemented via native avm string
		}
	}
}