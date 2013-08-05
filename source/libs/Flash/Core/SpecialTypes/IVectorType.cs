using DataDynamics.PageFX.Common.TypeSystem;

namespace DataDynamics.PageFX.Flash.Core.SpecialTypes
{
    internal interface IVectorType : ITypeData
    {
        IType Parameter { get; }
    }
}