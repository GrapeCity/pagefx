using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.IL
{
    internal interface IMethodContext : IMetadataTokenResolver
    {
        IMethod CurrentMethod { get; }

        IVariableCollection ResolveLocalVariables(int sig, out bool hasGenericVars);

        IType ResolveType(int sig);

        void LinkDebugInfo(IMethodBody body);
    }
}