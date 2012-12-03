using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.FlashLand.Abc;

namespace DataDynamics.PageFX.FlashLand.Swc
{
    public interface ISwcLinker
    {
        IAssembly Assembly { get; }

        void LinkType(AbcInstance instance);

        object ResolveExternalReference(string id);
    }
}