using System.Collections.Generic;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.Translation
{
	internal sealed class Code : List<IInstruction>
	{
		public readonly ICodeProvider Provider;

		public Code(ICodeProvider provider)
		{
			Provider = provider;
		}
	}
}
