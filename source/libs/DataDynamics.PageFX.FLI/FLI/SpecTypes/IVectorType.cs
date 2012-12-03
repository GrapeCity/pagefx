using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.FLI.ABC;

namespace DataDynamics.PageFX.FLI
{
    internal interface IVectorType : ISpecialType, IAbcFileSubject
    {
        IType Param { get; }

        AbcMultiname Name { get; }
    }
}