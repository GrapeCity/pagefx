using System.Collections.Generic;

namespace DataDynamics.PageFX.CodeModel.TypeSystem
{
    public interface IGenericParameter : IType
    {
        int Position { get; set; }

        GenericParameterVariance Variance { get; set; }

        GenericParameterSpecialConstraints SpecialConstraints { get; set; }

        long ID { get; set; }
    }

    public interface IGenericParameterCollection : IReadOnlyList<IGenericParameter>, ICodeNode
    {
        IGenericParameter this[string name] { get; }
    }
}