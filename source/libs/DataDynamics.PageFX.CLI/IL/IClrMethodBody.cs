using System.Collections.Generic;
using DataDynamics.PageFX.CLI.Translation;
using DataDynamics.PageFX.Common;

namespace DataDynamics.PageFX.CLI.IL
{
	internal interface IClrMethodBody : IMethodBody
	{
		bool HasProtectedBlocks { get; }

		IReadOnlyList<TryCatchBlock> ProtectedBlocks { get; }

		ILStream Code { get; }

		bool HasGenerics { get; }
		bool HasGenericVars { get; }
		bool HasGenericInstructions { get; }
		bool HasGenericExceptions { get; }

		int InstanceCount { get; set; }

		ControlFlowGraph ControlFlowGraph { get; set; }
	}
}