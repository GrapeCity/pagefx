using System;

namespace DataDynamics.PageFX.Core.Metadata
{
	/// <summary>
	/// Flags for ManifestResource
	/// </summary>
	[Flags]
	internal enum ManifestResourceAttributes
	{
		VisibilityMask = 0x0007,

		/// <summary>
		/// The Resource is exported from the Assembly
		/// </summary>
		Public = 0x0001,

		/// <summary>
		/// The Resource is private to the Assembly
		/// </summary>
		Private = 0x0002,
	}
}