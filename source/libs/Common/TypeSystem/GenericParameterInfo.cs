using System;

namespace DataDynamics.PageFX.Common.TypeSystem
{
	public sealed class GenericParameterInfo
	{
		public readonly int Position;
		public readonly GenericParameterVariance Variance;
		public readonly GenericParameterSpecialConstraints SpecialConstraints;

		public GenericParameterInfo(int position,
		                            GenericParameterVariance variance,
		                            GenericParameterSpecialConstraints specialConstraints)
		{
			Position = position;
			Variance = variance;
			SpecialConstraints = specialConstraints;
		}
	}

	public enum GenericParameterVariance : byte
	{
		NonVariant,
		Covariant,
		Contravariant
	}

	[Flags]
	public enum GenericParameterSpecialConstraints : byte
	{
		None = 0,
		DefaultConstructor = 1,
		ReferenceType = 2,
		ValueType = 4,
	}
}