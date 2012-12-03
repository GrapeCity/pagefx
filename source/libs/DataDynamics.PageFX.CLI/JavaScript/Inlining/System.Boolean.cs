namespace DataDynamics.PageFX.Ecma335.JavaScript.Inlining
{
	internal sealed class BooleanInlines : InlineCodeProvider
	{
		[InlineImpl(ArgCount = 0)]
		public static void ToString(JsBlock code)
		{
			var value = "this".Id().Get(SpecialFields.BoxValue);
			code.Add(value.Ternary("True", "False").Return());
		}
	}
}
