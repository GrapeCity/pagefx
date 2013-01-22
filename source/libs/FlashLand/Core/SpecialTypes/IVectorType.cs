using DataDynamics.PageFX.Common.TypeSystem;

namespace DataDynamics.PageFX.FlashLand.Core.SpecialTypes
{
    internal interface IVectorType : ITypeData
    {
        IType Parameter { get; }
    }
}