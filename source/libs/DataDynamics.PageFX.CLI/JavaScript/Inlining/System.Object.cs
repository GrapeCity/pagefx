namespace DataDynamics.PageFX.CLI.JavaScript.Inlining
{
	internal sealed class SystemObjectInlines : InlineCodeProvider
	{
		[InlineImpl]
		public static void ReferenceEquals(JsBlock code)
		{
			//code.Add(InstructionCode.Strictequals);
			//code.FixBool();
		}
	}
}