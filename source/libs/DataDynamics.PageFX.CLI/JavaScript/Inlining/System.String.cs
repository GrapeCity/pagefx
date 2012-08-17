using System.Linq;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.JavaScript.Inlining
{
	internal sealed class StringInlines : InlineCodeProvider
	{
		[InlineImpl(ArgCount = 2)]
		public static void Equals(IMethod method, JsBlock code)
		{
			var args = method.JsParameterIds();
			code.Add(args[0].Op(args[1], "==").Return());
		}

		[InlineImpl]
		public static void op_Implicit(IMethod method, JsBlock code)
		{
			var args = method.JsParameterIds();
			code.Add(args[0].Return());
		}

		[InlineImpl]
		public static void op_Equality(IMethod method, JsBlock code)
		{
			Equals(method, code);
		}

		[InlineImpl]
		public static void op_Inequality(IMethod method, JsBlock code)
		{
			var args = method.JsParameterIds();
			code.Add(args[0].Op(args[1], "!=").Return());
		}

		[InlineImpl]
		public static void ReturnValue(JsBlock code)
		{
			//TODO:
			//code.ReturnValue();
		}
	}
}
