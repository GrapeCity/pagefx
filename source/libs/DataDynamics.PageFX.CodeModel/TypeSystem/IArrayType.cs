using System;
using System.Collections.Generic;

namespace DataDynamics.PageFX.CodeModel.TypeSystem
{
    public interface IArrayType : ICompoundType
    {
        int Rank { get; }

        IArrayDimensionCollection Dimensions { get; }
    }

    public interface IArrayDimension : IFormattable
    {
        int LowerBound { get; set; }
        int UpperBound { get; set; }
    }

    public interface IArrayDimensionCollection : IList<IArrayDimension>, IFormattable
    {
    }
}