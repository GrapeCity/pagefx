using DataDynamics.PageFX.CodeModel;
using DataDynamics.PageFX.FLI.ABC;

namespace DataDynamics.PageFX.FLI
{
	internal static class MethodExtensions
	{
		public static bool IsInitializer(this IMethod method)
		{
			if (method == null) return false;
			if (!method.IsConstructor) return false;
			var m = method.Tag as AbcMethod;
			if (m == null) return false;
			return m.IsInitializer;
		}

		public static bool IsFlashEventMethod(this IMethod method)
		{
			if (method == null) return false;
			var e = method.Association as IEvent;
			if (e == null) return false;
			return Attrs.Has(e, "EventAttribute");
		}
	}
}