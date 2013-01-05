using System;

namespace DataDynamics.PageFX.Common.TypeSystem
{
	/// <summary>
	/// Method implementation flags
	/// </summary>
	[Flags]
	public enum MethodImplAttributes
	{
		/// <summary>
		/// Flags about code type.   
		/// </summary>
		CodeTypeMask = 0x0003,

		/// <summary>
		/// Specifies that the method implementation is in Common Intermediate Language (CIL).
		/// </summary>
		IL = 0x0000,

		/// <summary>
		/// Specifies that the method is implemented in native code.
		/// </summary>
		Native = 0x0001,

		/// <summary>
		/// Specifies that the method implementation is in optimized intermediate language (OPTIL).
		/// </summary>
		OPTIL = 0x0002,

		/// <summary>
		/// Specifies that the method implementation is provided by the runtime.
		/// </summary>
		Runtime = 0x0003,

		/// <summary>
		/// Flags specifying whether the code is managed or unmanaged.
		/// </summary>
		ManagedMask = 0x0004,

		/// <summary>
		/// Specifies that the method is implemented in unmanaged code.
		/// </summary>
		Unmanaged = 0x0004,

		/// <summary>
		/// Method impl is managed.
		/// </summary>
		Managed = 0x0000,

		/// <summary>
		/// Specifies that the method is declared, but its implementation is provided elsewhere.
		/// </summary>
		ForwardRef = 0x0010,

		/// <summary>
		/// Specifies that the method signature is exported exactly as declared.
		/// </summary>
		PreserveSig = 0x0080,

		/// <summary>
		/// Specifies an internal call. An internal call is a call to a method implemented within the common language runtime itself.
		/// </summary>
		InternalCall = 0x1000,

		/// <summary>
		/// Specifies that the method can be executed by only one thread at a time.
		/// Static methods lock on the type, while instance methods lock on the instance.
		/// Only one thread can execute in any of the instance functions and only one thread 
		/// can execute in any of a class's static functions.
		/// </summary>
		Synchronized = 0x0020,

		/// <summary>
		/// Specifies that the method can not be inlined.
		/// </summary>
		NoInlining = 0x0008,
	}
}