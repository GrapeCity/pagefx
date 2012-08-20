using System;
using System.Linq;
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
		public static void WriteLine(MethodContext context, JsBlock code)
		{
			var method = context.Method;
			var args = method.JsArgs();
			if (args.Length == 0)
			{
				code.Add("console.log".Id().Call(""));
			}
			else if (args.Length == 1)
			{
				var p = method.Parameters[0].Type;
				if (p == SystemTypes.String)
				{
					code.Add("console.log".Id().Call(args[0]));
				}
				else
				{
					if (p == SystemTypes.Object)
					{
						context.Host.CompileMethod(SystemTypes.Object.Methods.Find("ToString").First());
					}

					var val = "$tostr".Id().Call("$unbox".Id().Call(args[0]));
					code.Add("console.log".Id().Call(val));
				}
			}
			else
			{
				throw new NotImplementedException();
			}
		}
	}
}
