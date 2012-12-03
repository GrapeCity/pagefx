using DataDynamics.PageFX.Common.TypeSystem;

namespace DataDynamics.PageFX.Ecma335.JavaScript.Inlining
{
	internal sealed class RuntimeHelpersInlines : InlineCodeProvider
	{
		[InlineImpl]
		public static void InitializeArray(IMethod method, JsBlock code)
		{
			var args = method.JsArgs();
			code.Add("$initarr".Id().Call(args).AsStatement());
		}
	}
}
