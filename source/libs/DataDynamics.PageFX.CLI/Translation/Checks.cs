using System;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.Translation
{
	internal static class Checks
	{
		public static void ItShouldBeArray(this EvalItem item)
		{
			var type = item.Type;
			if (type.TypeKind != TypeKind.Array)
				throw new InvalidOperationException();
		}

		public static void ItShouldBeNonPointer(this EvalItem item)
		{
			if (item.IsPointer)
				throw new InvalidOperationException();
		}
	}
}
