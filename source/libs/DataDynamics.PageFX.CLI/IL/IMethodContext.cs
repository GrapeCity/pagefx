using DataDynamics.PageFX.Common.CodeModel;
using DataDynamics.PageFX.Common.TypeSystem;

namespace DataDynamics.PageFX.Ecma335.IL
{
    internal interface IMethodContext : IMetadataTokenResolver
    {
        IVariableCollection ResolveLocalVariables(IMethod method, int sig, out bool hasGenericVars);

        IType ResolveType(IMethod method, int sig);

        void LinkDebugInfo(IMethodBody body);
    }
}