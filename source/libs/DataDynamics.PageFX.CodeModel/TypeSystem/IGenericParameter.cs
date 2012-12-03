using System.Collections.Generic;
using DataDynamics.PageFX.Common.CodeModel;

namespace DataDynamics.PageFX.Common.TypeSystem
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