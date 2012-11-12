using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.JavaScript.Inlining
{
	internal sealed class SystemExceptionInlines : InlineCodeProvider
	{
		[InlineImpl]
		public static void GetInternalStackTrace(IMethod method, JsBlock code)
		{
			code.Add("".Return());
		}

		[InlineImpl]
		public static void get_InternalMessage(IMethod method, JsBlock code)
		{
			code.Add("this.$message".Id().Return());
		}

		[InlineImpl]
		public static void set_InternalMessage(IMethod method, JsBlock code)
		{
			var value = method.JsArgs()[0];
			code.Add("this.$message".Id().Set(value));
		}
	}
}