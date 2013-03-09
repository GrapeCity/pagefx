using System;
using DataDynamics.PageFX.Common.CodeModel;
using DataDynamics.PageFX.Common.Collections;

namespace DataDynamics.PageFX.Common.TypeSystem
{
    public interface IGenericParameter : IType
    {
		long Id { get; }

        int Position { get; }

        GenericParameterVariance Variance { get; }

        GenericParameterSpecialConstraints SpecialConstraints { get; }
    }

    public interface IGenericParameterCollection : IReadOnlyList<IGenericParameter>, ICodeNode
    {
	    IGenericParameter Find(string name);

	    void Add(IGenericParameter parameter);
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