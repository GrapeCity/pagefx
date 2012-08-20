using System.Linq;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.JavaScript.Inlining
{
	internal sealed class SystemObjectInlines : InlineCodeProvider
	{
		[InlineImpl]
		public static void ReferenceEquals(IMethod method, JsBlock code)
		{
			var args = method.JsArgs();
			code.Add(new JsBinaryOperator(args[0], args[1], "===").Return());
		}
	}
}