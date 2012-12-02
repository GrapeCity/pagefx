using DataDynamics.PageFX.CodeModel;
using DataDynamics.PageFX.CodeModel.TypeSystem;
using DataDynamics.PageFX.FLI.ABC;

namespace DataDynamics.PageFX.FLI
{
    internal interface IVectorType : ISpecialType, IAbcFileSubject
    {
        IType Param { get; }

        AbcMultiname Name { get; }
    }
}