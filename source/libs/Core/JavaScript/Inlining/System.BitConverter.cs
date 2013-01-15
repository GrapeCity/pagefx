using DataDynamics.PageFX.Common.TypeSystem;

namespace DataDynamics.PageFX.Core.JavaScript.Inlining
{
	internal sealed class BitConverterInlines : InlineCodeProvider
	{
		private static void GetBytesImpl(MethodContext context, JsBlock code, string encodeFunc)
		{
			var arrayType = context.ResolveSystemType(SystemTypeCode.Array);
			var elementType = context.ResolveSystemType(SystemTypeCode.UInt8);

			context.Host.CompileClass(arrayType);
			context.Host.CompileClass(elementType);
			context.Host.RegisterArrayType(elementType);

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
