using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.JavaScript.Inlining
{
	//TODO: remove this temp inlines, inline only internal methods used by Console impl

	internal sealed class ConsoleInlines : InlineCodeProvider
	{
		[InlineImpl("*", Attrs = MethodAttrs.Constructor)]
		public static void Ctor(IMethod method, JsBlock code)
		{
		}

		[InlineImpl]
		public static void WriteLine(IMethod method, JsBlock code)
		{
			var args = method.JsParameterIds();
			if (args.Length == 0)
			{
				code.Add("console.log".Id().Call("\n"));
			}
			else
			{
				code.Add("$unbox".Id().Call(args[0]).Var("t"));
				code.Add(new JsText(string.Format("if (t != undefined) {0} = t;", args[0].Value)));
				code.Add("console.log".Id().Call(args[0]));
			}
		}
	}
}
