using System;
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
				code.Add("console.log".Id().Call(""));
			}
			else if (args.Length == 1)
			{
				var val = "$unbox".Id().Call(args[0]);
				code.Add("console.log".Id().Call(val));
			}
			else
			{
				throw new NotImplementedException();
			}
		}
	}
}
