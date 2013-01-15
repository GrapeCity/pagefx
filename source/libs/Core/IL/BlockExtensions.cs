using System.Collections.Generic;
using System.Linq;

namespace DataDynamics.PageFX.Core.IL
{
	internal static class BlockExtensions
	{
		public static IEnumerable<TryCatchBlock> GetSelfAndDescendants(this TryCatchBlock block)
		{
			yield return block;

			foreach (var d in GetDescendants(block))
			{
				yield return d;
			}
		}

		public static IEnumerable<TryCatchBlock> GetDescendants(this TryCatchBlock block)
		{
			foreach (var kid in block.Kids.OfType<TryCatchBlock>())
			{
				yield return kid;

				foreach (var d in GetDescendants(kid))
				{
					yield return d;
				}
			}
		}
	}
}