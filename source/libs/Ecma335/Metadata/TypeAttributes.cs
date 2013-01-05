using System;

namespace DataDynamics.PageFX.Ecma335.Metadata
{
	/// <summary>
	/// Type record metadata attributes
	/// </summary>
	[Flags]
	internal enum TypeAttributes
	{
		#region Visibility attributes
		/// <summary>
		/// Type visibility mask
		/// </summary>
		VisibilityMask = 0x00000007,

		/// <summary>
		/// Class is not public scope.
		/// </summary>
		NotPublic = 0x00000000,

		/// <summary>
		/// Class is public scope.
		/// </summary>
		Public = 0x00000001,

		/// <summary>
		/// Class is nested with public visibility.
		/// </summary>
		NestedPublic = 0x00000002,

		/// <summary>
		/// Class is nested with private visibility.
		/// </summary>
		NestedPrivate = 0x00000003,

		/// <summary>
		/// Class is nested with family visibility.
		/// </summary>
		NestedFamily = 0x00000004,

		/// <summary>
		/// Class is nested with assembly visibility.
		/// </summary>
		NestedAssembly = 0x00000005,

		/// <summary>
		/// Class is nested with family and assembly visibility.
		/// </summary>
		NestedFamANDAssem = 0x00000006,

		/// <summary>
		/// Class is nested with family or assembly visibility.
		/// </summary>
		NestedFamORAssem = 0x00000007,
		#endregion

		#region Class layout attributes
		/// <summary>
		/// Use this mask to retrieve class layout informaiton
		/// </summary>
		LayoutMask = 0x00000018,

		/// <summary>
		/// Class fields are auto-laid out
		/// </summary>
		AutoLayout = 0x00000000,

		/// <summary>
		/// Class fields are laid out sequentially
		/// </summary>
		SequentialLayout = 0x00000008,

		/// <summary>
		/// Layout is supplied explicitly
		/// </summary>
		ExplicitLayout = 0x00000010,
		#endregion

		#region Class semantics attributes
		/// <summary>
		/// Use this mask to distinguish a type declaration as a Class, ValueType or Interface
		/// </summary>
		ClassSemanticsMask = 0x00000020,

		/// <summary>
		/// Type is a class.
		/// </summary>
		Class = 0x00000000,

		/// <summary>
		/// Type is an interface.
		/// </summary>
		Interface = 0x00000020,

		//Special semantics in addition to class semantics
		/// <summary>
		/// Class is abstract
		/// </summary>
		Abstract = 0x00000080,

		/// <summary>
		/// Class is concrete and may not be extended
		/// </summary>
		Sealed = 0x00000100,

		/// <summary>
		/// Class name is special.  Name describes how.
		/// </summary>
		SpecialName = 0x00000400,
		#endregion

		#region Implementation attributes
		/// <summary>
		/// Class / interface is imported
		/// </summary>
		Import = 0x00001000,

		/// <summary>
		/// The class is Serializable.
		/// </summary>
		Serializable = 0x00002000,
		#endregion

		#region String formatting Attributes
		/// <summary>
		/// Use StringFormatMask to retrieve string information for native interop
		/// </summary>
		StringFormatMask = 0x00030000,

		/// <summary>
		/// LPTSTR is interpreted as ANSI in this class
		/// </summary>
		AnsiClass = 0x00000000,

		/// <summary>
		/// LPTSTR is interpreted as UNICODE
		/// </summary>
		UnicodeClass = 0x00010000,

		/// <summary>
		/// LPTSTR is interpreted automatically
		/// </summary>
		AutoClass = 0x00020000,

		/// <summary>
		/// A non-standard encoding specified by CustomStringFormatMask
		/// </summary>
		CustomFormatClass = 0x00030000,

		/// <summary>
		/// Use this mask to retrieve non-standard encoding information for native interop. 
		/// The meaning of the values of these 2 bits is unspecified.
		/// </summary>
		CustomStringFormatMask = 0x00C00000,
		#endregion

		#region Class Initialization Attributes
		/// <summary>
		/// Initialize the class any time before first static field access.
		/// </summary>
		BeforeFieldInit = 0x00100000,
		#endregion

		#region Additional Flags
		/// <summary>
		/// Flags reserved for runtime use.
		/// </summary>
		ReservedMask = 0x00040800,

		/// <summary>
		/// Runtime should check name encoding.
		/// </summary>
		RTSpecialName = 0x00000800,

		/// <summary>
		/// Class has security associate with it.
		/// </summary>
		HasSecurity = 0x00040000,
		#endregion
	}
}