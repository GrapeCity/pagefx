using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.JavaScript.Inlining
{
	internal sealed class AvmInlines : InlineCodeProvider
	{
		[InlineImpl]
		public static void get_IsFlashPlayer(JsBlock code)
		{
			code.Add(false.Return());
		}

		[InlineImpl]
		public static void Console_Write(IMethod method, JsBlock code)
		{
			var arg = method.JsArgs()[0];
			code.Add("console.log".Id().Call(arg));
		}

		[InlineImpl]
		public static void trace(IMethod method, JsBlock code)
		{
			Console_Write(method, code);
		}

		[InlineImpl]
		public static void ToString(IMethod method, JsBlock code)
		{
			var arg = method.JsArgs()[0];
			code.Add(arg.Get("toString").Call().Return());
		}

		[InlineImpl]
		public static void CopyArray(IMethod method, JsBlock code)
		{
			var arg = method.JsArgs()[0];
			code.Add(arg.Get("concat").Call().Return());
		}

		[InlineImpl]
		public static void get_m_value(IMethod method, JsBlock code)
		{
			var arg = method.JsArgs()[0];
			code.Add(arg.Get(SpecialFields.BoxValue).Return());
		}

		[InlineImpl]
		public static void set_m_value(IMethod method, JsBlock code)
		{
			var args = method.JsArgs();
			code.Add(args[0].Set(SpecialFields.BoxValue, args[1]));
		}
	}
}