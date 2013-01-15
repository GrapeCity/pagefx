using System.Collections.Generic;
using DataDynamics.PageFX.Common.CodeModel;

namespace DataDynamics.PageFX.Core.Translation
{
	internal sealed class TranslatorResult
	{
		public IInstruction[] Begin;
		public IInstruction[] End;
		public IList<IInstruction> Output;
		public IList<Branch> Branches;
	}
}