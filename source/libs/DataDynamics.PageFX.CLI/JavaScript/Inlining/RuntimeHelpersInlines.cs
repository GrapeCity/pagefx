using DataDynamics.PageFX.CodeModel;
using DataDynamics.PageFX.CodeModel.TypeSystem;

namespace DataDynamics.PageFX.CLI.JavaScript.Inlining
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
