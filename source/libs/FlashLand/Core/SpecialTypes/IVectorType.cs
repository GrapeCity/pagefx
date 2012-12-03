using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.FlashLand.Abc;

namespace DataDynamics.PageFX.FlashLand.Core.SpecialTypes
{
    internal interface IVectorType : ISpecialType, IAbcFileSubject
    {
        IType Param { get; }

        AbcMultiname Name { get; }
    }
}