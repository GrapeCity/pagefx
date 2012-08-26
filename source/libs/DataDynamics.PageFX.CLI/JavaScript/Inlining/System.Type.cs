namespace DataDynamics.PageFX.CLI.JavaScript.Inlining
{
	internal sealed class TypeInlines : InlineCodeProvider
	{
		[InlineImpl]
		public static void CreateInstanceDefault(JsBlock code)
		{
			code.Add("this".Id().Get("$new").Call().Return());
		}
	}
}