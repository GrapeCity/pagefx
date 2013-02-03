using DataDynamics.PageFX.FlashLand.IL;

namespace DataDynamics.PageFX.FlashLand.Core.Inlining
{
	internal sealed class DelegateInlines : InlineCodeProvider
	{
		[InlineImpl]
		public static void AddEventListener(AbcCode code)
		{
			code.AddEventListener();
		}

		[InlineImpl]
		public static void RemoveEventListener(AbcCode code)
		{
			code.RemoveEventListener();
		}
	}
}