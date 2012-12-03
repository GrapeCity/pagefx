namespace DataDynamics.PageFX.Ecma335.JavaScript.Inlining
{
	internal sealed class CharInlines : InlineCodeProvider
	{
		[InlineImpl(ArgCount = 0)]
		public static void ToString(JsBlock code)
		{
			code.Add("String.fromCharCode".Id().Call("this".Id().Get(SpecialFields.BoxValue)).Return());
		}
	}
}
