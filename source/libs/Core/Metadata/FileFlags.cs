using System;

namespace DataDynamics.PageFX.Core.Metadata
{
	/// <summary>
	/// Flags for files
	/// </summary>
	[Flags]
	internal enum FileFlags
	{
		/// <summary>
		/// This is not a resource file
		/// </summary>
		ContainsMetadata = 0x0000,

		/// <summary>
		/// This is a resource file or other non-metadata-containing file
		/// </summary>
		ContainsNoMetadata = 0x0001,
	}
}