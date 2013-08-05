using DataDynamics.PageFX.Flash.IL;

namespace DataDynamics.PageFX.Flash.Core.Inlining
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