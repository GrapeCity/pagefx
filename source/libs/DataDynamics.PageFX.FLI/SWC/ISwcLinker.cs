using DataDynamics.PageFX.CodeModel;
using DataDynamics.PageFX.CodeModel.TypeSystem;
using DataDynamics.PageFX.FLI.ABC;

namespace DataDynamics.PageFX.FLI.SWC
{
    public interface ISwcLinker
    {
        IAssembly Assembly { get; }

        void LinkType(AbcInstance instance);

        object ResolveExternalReference(string id);
    }
}