using System;

namespace DataDynamics.PageFX.Core.Metadata
{
	/// <summary>
	/// Method parameter metadata attribute
	/// </summary>
	[Flags]
	internal enum ParamAttributes
	{
		/// <summary>
		/// no flag is specified
		/// </summary>
		None = 0x0000,

		/// <summary>
		/// Param is [In]    
		/// </summary>
		In = 0x0001,

		/// <summary>
		/// Param is [Out]   
		/// </summary>
		Out = 0x0002,

		/// <summary>
		/// Param is [lcid]  
		/// </summary>
		Lcid = 0x0004,

		/// <summary>
		/// Param is [Retval]    
		/// </summary>
		Retval = 0x0008,

		/// <summary>
		/// Param is optional 
		/// </summary>
		Optional = 0x0010,

		/// <summary>
		/// Reserved flags for Runtime use only. 
		/// </summary>
		ReservedMask = 0xf000,

		/// <summary>
		/// Param has default value.
		/// </summary>
		HasDefault = 0x1000,

		/// <summary>
		/// Param has FieldMarshal.
		/// </summary>
		HasFieldMarshal = 0x2000,

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