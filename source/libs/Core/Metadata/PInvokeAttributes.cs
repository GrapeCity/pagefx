using System;

namespace DataDynamics.PageFX.Core.Metadata
{
	/// <summary>
	/// Flags for ImplMap [PInvokeAttributes]
	/// </summary>
	[Flags]
	internal enum PInvokeAttributes
	{
		/// <summary>
		/// PInvoke is to use the member name as specified
		/// </summary>
		NoMangle = 0x0001,

		/// <summary>
		/// This is a resource file or other non-metadata-containing file.
		/// </summary>
		CharSetMask = 0x0006,
		CharSetNotSpec = 0x0000,
		CharSetAnsi = 0x0002,
		CharSetUnicode = 0x0004,
		CharSetAuto = 0x0006,

		/// <summary>
		/// Information about target function. Not relevant for fields
		/// </summary>
		SupportsLastError = 0x0040,

		CallConvMask = 0x0700,
		CallConvWinapi = 0x0100,
		CallConvCdecl = 0x0200,
		CallConvStdcall = 0x0300,
		CallConvThiscall = 0x0400,
		CallConvFastcall = 0x0500
	}
}