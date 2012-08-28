using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.JavaScript.Inlining
{
	internal sealed class BitConverterInlines : InlineCodeProvider
	{
		private static void GetBytesImpl(MethodContext context, JsBlock code, string encodeFunc)
		{
			context.Host.CompileClass(SystemTypes.Array);
			context.Host.CompileClass(SystemTypes.Byte);
			context.Host.RegisterArrayType(SystemTypes.Byte);

			var arg = context.Method.JsArgs()[0];
			code.Add("$toSystemByteArray".Id().Call(encodeFunc.Id().Call(arg)).Return());
		}

		[InlineImpl("GetBytes", ArgCount = 1, ArgTypes = new[] { "Double" })]
		public static void GetBytesDouble(MethodContext context, JsBlock code)
		{
			GetBytesImpl(context, code, "$encodeDouble");
		}

		[InlineImpl("GetBytes", ArgCount = 1, ArgTypes = new[] { "Single" })]
		public static void GetBytesSingle(MethodContext context, JsBlock code)
		{
			GetBytesImpl(context, code, "$encodeSingle");
		}
	}
}
