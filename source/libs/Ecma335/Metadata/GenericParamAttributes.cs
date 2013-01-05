using System;

namespace DataDynamics.PageFX.Ecma335.Metadata
{
	/// <summary>
	/// Attributes for Generic Parameters
	/// </summary>
	[Flags]
	internal enum GenericParamAttributes
	{
		/// <summary>
		/// The generic parameter is non-variant
		/// </summary>
		None = 0,

		/// <summary>
		/// The generic parameter is covariant
		/// </summary>
		Covariant = 1,

		/// <summary>
		/// The generic parameter is contravariant
		/// </summary>
		Contravariant = 2,

		/// <summary>
		/// Mask for variance type of flags
		/// </summary>
		VarianceMask = 3,

		/// <summary>
		/// Parameters must be referencied type
		/// </summary>
		ReferenceTypeConstraint = 4,

		/// <summary>
		/// Parameter must be not nullable
		/// </summary>
		NotNullableValueTypeConstraint = 8,

		/// <summary>
		/// Parameter must have default constructors.
		/// </summary>
		DefaultConstructorConstraint = 0x10,

		/// <summary>
		/// Mask for special part
		/// </summary>
		SpecialConstraintMask = 0x1c,
	}
}