using DataDynamics.PageFX.FLI.IL;

namespace DataDynamics.PageFX.FLI.Inlining
{
	internal sealed class ExceptionInlines : InlineCodeProvider
	{
		[InlineImpl]
		public static void avm_getStackTrace(AbcCode code)
		{
			code.GetErrorStackTrace();
		}

		[InlineImpl]
		public static void avm_getMessage(AbcCode code)
		{
			code.GetErrorMessage();
		}

		[InlineImpl]
		public static void avm_setMessage(AbcCode code)
		{
			code.SetProperty("message");
		}
	}
}