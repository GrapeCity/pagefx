using DataDynamics.PageFX.FlashLand.Core.CodeGeneration;
using DataDynamics.PageFX.FlashLand.IL;

namespace DataDynamics.PageFX.FlashLand.Core.Inlining
{
	internal sealed class AvmFunctionInlines : InlineCodeProvider
	{
		// implicit cast operator from System.Delegate to Function
		[InlineImpl]
		public static void op_Implicit(AbcCode code)
		{
			code.GetField(FieldId.Delegate_Function);
		}
	}
}