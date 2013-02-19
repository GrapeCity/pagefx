using System;
using DataDynamics.PageFX.Common.Collections;

namespace DataDynamics.PageFX.Common.TypeSystem
{
    public interface IArrayType : ICompoundType
    {
        int Rank { get; }

        IArrayDimensionCollection Dimensions { get; }
    }

    public interface IArrayDimension : IFormattable
    {
        int LowerBound { get; }
        int UpperBound { get; }
    }

    public interface IArrayDimensionCollection : IReadOnlyList<IArrayDimension>, IFormattable
    {
    }
}