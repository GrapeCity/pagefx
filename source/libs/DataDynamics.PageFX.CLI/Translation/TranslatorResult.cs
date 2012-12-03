using System.Collections.Generic;
using DataDynamics.PageFX.Common;

namespace DataDynamics.PageFX.CLI.Translation
{
	internal sealed class TranslatorResult
	{
		public IInstruction[] Begin;
		public IInstruction[] End;
		public IList<IInstruction> Output;
		public IList<Branch> Branches;
	}
}