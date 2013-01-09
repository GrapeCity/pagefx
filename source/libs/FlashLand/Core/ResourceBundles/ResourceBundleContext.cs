using DataDynamics.PageFX.FlashLand.Swc;

namespace DataDynamics.PageFX.FlashLand.Core.ResourceBundles
{
	public sealed class ResourceBundleContext
	{
		public SwcFile Swc { get; set; }

		public string Locale { get; set; }

		public int Line { get; set; }

		internal ResourceBundle ResourceBundle { get; set; }

		internal string ResolvedSource { get; set; }
	}
}