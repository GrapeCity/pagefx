using DataDynamics.PageFX.Flash.IL;

namespace DataDynamics.PageFX.Flash.Core.Inlining
{
	internal class FlashByteArrayInlines : InlineCodeProvider
	{
		[InlineImpl(".ctor")]
		public static void Ctor(AbcCode code)
		{
			code.Construct(0);
		}
	}
}
