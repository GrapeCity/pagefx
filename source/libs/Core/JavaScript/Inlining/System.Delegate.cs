using System;
using DataDynamics.PageFX.Common.TypeSystem;

namespace DataDynamics.PageFX.Core.JavaScript.Inlining
{
	internal sealed class DelegateInlines : InlineCodeProvider
	{
		public static readonly InlineCodeProvider Instance = new DelegateInlines();

		[InlineImpl("*", Attrs = MethodAttrs.Constructor)]
		public static void Ctor(IMethod method, JsBlock code)
		{
			if (method.Parameters.Count != 2)
				throw new InvalidOperationException();

			//TODO: get fields from System.Delegate and System.Multicast delegate types
			var args = method.JsArgs();
			code.Add("this".Id().Set("m_target", args[0]));
			code.Add("this".Id().Set("m_function", args[1]));
			code.Add("this".Id().Set("m_prev", null));
			code.Add("this".Id().Set("m_next", null));
		}

		[InlineImpl]
		public static void Invoke(IMethod method, JsBlock code)
		{
			var args = new JsArray(method.JsArgs());
			var call = "$invokeDelegate".Id().Call("this".Id(), args, method.IsVoid() ? 0 : 1);
			code.Add(method.IsVoid() ? call.AsStatement() : call.Return());
		}
	}
}
