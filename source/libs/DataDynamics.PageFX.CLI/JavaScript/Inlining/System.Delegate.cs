using System;
using System.Linq;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.JavaScript.Inlining
{
	internal sealed class DelegateInlines : InlineCodeProvider
	{
		public static readonly InlineCodeProvider Instance = new DelegateInlines();

		[InlineImpl("*", Attrs = MethodAttrs.Constructor)]
		public static void Ctor(IMethod method, JsBlock code)
		{
			if (method.Parameters.Count != 2)
				throw new InvalidOperationException();

			var args = method.Parameters.Select(x => x.Name.Id()).ToArray();
			code.Add("this".Id().Set("m_target", args[0]));
			code.Add("this".Id().Set("m_function", args[1]));
		}

		[InlineImpl]
		public static void Invoke(IMethod method, JsBlock code)
		{
			var args = new JsArray(method.Parameters.Select(x => (object)x.Name.Id()));
			code.Add("$invokeDelegate".Id().Call("this".Id(), args, method.IsVoid() ? 0 : 1));
		}
	}
}
