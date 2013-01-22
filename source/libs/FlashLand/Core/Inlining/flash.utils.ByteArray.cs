using DataDynamics.PageFX.FlashLand.IL;

namespace DataDynamics.PageFX.FlashLand.Core.Inlining
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
