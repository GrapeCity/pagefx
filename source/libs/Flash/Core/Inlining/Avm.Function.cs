using DataDynamics.PageFX.Flash.Core.CodeGeneration;
using DataDynamics.PageFX.Flash.IL;

namespace DataDynamics.PageFX.Flash.Core.Inlining
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