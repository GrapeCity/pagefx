namespace DataDynamics.PageFX.Ecma335.JavaScript.Inlining
{
	internal sealed class ArrayInlines : InlineCodeProvider
	{
		[InlineImpl(ArgCount = 0)]
		public static void get_Length(JsBlock code)
		{
			//TODO: use field instead of "m_value" const
			code.Add("this".Id().Get("m_value").Get("length").Return());
		}
	}
}
