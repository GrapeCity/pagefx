using System;

namespace DataDynamics.PageFX.Ecma335.Metadata
{
	/// <summary>
	/// Event record metadata attributes
	/// </summary>
	[Flags]
	internal enum EventAttributes
	{
		/// <summary>
		/// No additional info
		/// </summary>
		None = 0x0000,

		/// <summary>
		/// event is special.  Name describes how.
		/// </summary>
		SpecialName = 0x0200,

		/// <summary>
		/// Reserved flags for Runtime use only (left here for compatibility)
		/// </summary>
		ReservedMask = 0x0400,

		/// <summary>
		/// Runtime(metadata internal APIs) should check name encoding.
		/// </summary>
		RTSpecialName = 0x0400,
	}
}