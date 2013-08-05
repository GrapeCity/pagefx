using DataDynamics.PageFX.Flash.Swc;

namespace DataDynamics.PageFX.Flash.Core.ResourceBundles
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