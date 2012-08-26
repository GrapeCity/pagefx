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

		[InlineImpl]
		public static void MemberwiseClone(JsBlock code)
		{
			code.Add("$clone".Id().Call("this".Id()).Return());
		}
	}
}