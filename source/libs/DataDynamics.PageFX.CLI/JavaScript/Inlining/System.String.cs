using System;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.JavaScript.Inlining
{
	internal sealed class StringInlines : InlineCodeProvider
	{
		[InlineImpl("*", Attrs = MethodAttrs.Instance | MethodAttrs.Constructor)]
		public static void Ctor(MethodContext context, JsBlock code)
		{
			var method = context.Method;
			var args = method.JsArgs();

			var build = method.DeclaringType.Methods.Find("Build", method.Parameters.Count);
			if (build == null)
				throw new InvalidOperationException("Unable to find String.Build.");

			context.Host.CompileMethod(build);

			code.Add(build.JsFullName(method.DeclaringType).Id().Call(args).Return());
		}

		[InlineImpl(ArgCount = 2)]
		public static void Equals(IMethod method, JsBlock code)
		{
			var args = method.JsArgs();
			code.Add(args[0].Op(args[1], "==").Return());
		}

		[InlineImpl]
		public static void op_Implicit(IMethod method, JsBlock code)
		{
			var args = method.JsArgs();
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
			var args = method.JsArgs();
			code.Add(args[0].Op(args[1], "!=").Return());
		}

		[InlineImpl]
		public static void ReturnValue(JsBlock code)
		{
			throw new NotImplementedException();
		}
	}
}
