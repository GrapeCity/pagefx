using DataDynamics.PageFX.FlashLand.IL;

namespace DataDynamics.PageFX.FlashLand.Core.Inlining
{
	internal sealed class StringInlines : InlineCodeProvider
	{
		[InlineImpl(ArgCount = 2)]
		public static void Equals(AbcCode code)
		{
			code.Add(InstructionCode.Equals);
			code.FixBool();
		}

		[InlineImpl]
		public static void op_Implicit(AbcCode code)
		{
			// do nothing since System.String is implemented via native avm string
		}

		[InlineImpl]
		public static void op_Equality(AbcCode code)
		{
			code.Add(InstructionCode.Equals);
			code.FixBool();
		}

		[InlineImpl]
		public static void op_Inequality(AbcCode code)
		{
			code.Add(InstructionCode.Equals);
			code.Add(InstructionCode.Not);
			code.FixBool();
		}

		[InlineImpl]
		public static void ReturnValue(AbcCode code)
		{
			code.ReturnValue();
		}
	}
}
