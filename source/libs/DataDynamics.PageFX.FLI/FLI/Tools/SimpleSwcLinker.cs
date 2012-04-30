using DataDynamics.PageFX.CodeModel;
using DataDynamics.PageFX.FLI.ABC;
using DataDynamics.PageFX.FLI.SWC;

namespace DataDynamics.PageFX.FLI
{
    internal class SimpleSwcLinker : ISwcLinker
    {
        public SimpleSwcLinker(IAssembly assembly)
        {
            _assembly = assembly;
        }

        #region ISwcLinker Members
        public IAssembly Assembly
        {
            get { return _assembly; }
        }
        private readonly IAssembly _assembly;

        public void LinkType(AbcInstance instance)
        {
        }

        public object ResolveExternalReference(string id)
        {
            return AssemblyIndex.ResolveRef(_assembly, id);
        }
        #endregion
    }
}