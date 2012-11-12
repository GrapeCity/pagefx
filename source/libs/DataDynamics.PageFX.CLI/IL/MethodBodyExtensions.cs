using System.Collections.Generic;
using System.Linq;

namespace DataDynamics.PageFX.CLI.IL
{
	internal static class MethodBodyExtensions
	{
		public static IEnumerable<TryCatchBlock> GetAllProtectedBlocks(this IClrMethodBody body)
		{
			return (body.ProtectedBlocks ?? Enumerable.Empty<TryCatchBlock>())
				.SelectMany(x => x.GetSelfAndDescendants());
		}
	}
}