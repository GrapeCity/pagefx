namespace DataDynamics.PageFX.CLI.JavaScript.Inlining
{
	internal sealed class StringInlines : InlineCodeProvider
	{
		[InlineImpl(ArgCount = 2)]
		public static void Equals(JsBlock code)
		{
			//code.Add(InstructionCode.Equals);
			//code.FixBool();
		}

		[InlineImpl]
		public static void op_Implicit(JsBlock code)
		{
			// do nothing since System.String is implemented via native avm string
		}

		[InlineImpl]
		public static void op_Equality(JsBlock code)
		{
			//code.Add(InstructionCode.Equals);
			//code.FixBool();
		}

		[InlineImpl]
		public static void op_Inequality(JsBlock code)
		{
			//code.Add(InstructionCode.Equals);
			//code.Add(InstructionCode.Not);
			//code.FixBool();
		}

		[InlineImpl]
		public static void ReturnValue(JsBlock code)
		{
			//code.ReturnValue();
		}
	}
}
