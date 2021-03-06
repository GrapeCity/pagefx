using System.Collections.Generic;
using DataDynamics.PageFX.Common.CodeModel;
using DataDynamics.PageFX.Common.Collections;
using DataDynamics.PageFX.Core.Translation;

namespace DataDynamics.PageFX.Core.IL
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

		void SetSequencePoints(IEnumerable<SequencePoint> points);
	}
}