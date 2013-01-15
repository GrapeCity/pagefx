using System;

namespace DataDynamics.PageFX.Core.Metadata
{
	/// <summary>
	/// Property record metadata attributes
	/// </summary>
	[Flags]
	internal enum PropertyAttributes
	{
		/// <summary>
		/// No special attributes
		/// </summary>
		None = 0x0000,

		/// <summary>
		/// property is special.  Name describes how.
		/// </summary>
		SpecialName = 0x0200,

		/// <summary>
		/// Reserved flags for Runtime use only. 
		/// </summary>
		ReservedMask = 0xf400,

		/// <summary>
		/// Runtime(metadata internal APIs) should check name encoding.
		/// </summary>
		RTSpecialName = 0x0400,

		/// <summary>
		/// Property has default 
		/// </summary>
		HasDefault = 0x1000,

		/// <summary>
		/// reserved bit
		/// </summary>
		Reserved2 = 0x2000,

		/// <summary>
		/// reserved bit 
		/// </summary>
		Reserved3 = 0x4000,

		/// <summary>
		/// reserved bit 
		/// </summary>
		Reserved4 = 0x8000
	}
}