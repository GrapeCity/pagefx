using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Flash.IL;

namespace DataDynamics.PageFX.Flash.Core.Inlining
{
	internal sealed class AvmClassInlines : InlineCodeProvider
	{
		[InlineImpl("Find", ArgTypes = new[] { "String", "String" })]
		public static void Find_String_String(AbcCode code)
		{
			code.FixRuntimeQName();

			Find_Namespace_String(code);
		}

		[InlineImpl("Find", ArgTypes = new[] { "Namespace", "String" })]
		public static void Find_Namespace_String(AbcCode code)
		{
			//NOTE: VerifyError: Error #1078: Illegal opcode/multiname combination: 96<[]::[]>.
			//code.Getlex(code.abc.RuntimeQName);

			var m = code.Generator.RuntimeImpl.FindClass();
			code.Call(m);
		}

		[InlineImpl]
		public static void CreateInstance(IMethod method, AbcCode code)
		{
			code.Construct(method.Parameters.Count);
		}

		[InlineImpl]
		public static void op_Implicit(AbcCode code)
		{
			code.GetProperty("Class");
		}
	}
}