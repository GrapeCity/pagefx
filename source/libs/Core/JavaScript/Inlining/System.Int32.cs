namespace DataDynamics.PageFX.Core.JavaScript.Inlining
{
	internal sealed class Int32Inlines : InlineCodeProvider
	{
		[InlineImpl(ArgCount = 0)]
		public static void ToString(JsBlock code)
		{
			code.Add("this".Id().Get(SpecialFields.BoxValue).Get("toString").Call().Return());
		}
	}
}
