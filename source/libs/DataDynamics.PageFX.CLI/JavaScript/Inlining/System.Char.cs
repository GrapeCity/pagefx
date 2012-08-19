namespace DataDynamics.PageFX.CLI.JavaScript.Inlining
{
	internal sealed class CharInlines : InlineCodeProvider
	{
		[InlineImpl]
		public static void ToString(JsBlock code)
		{
			code.Add("String.fromCharCode".Id().Call("this".Id().Get(SpecialFields.BoxValue)).Return());
		}
	}
}
