using System;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.FlashLand.Swc;

namespace DataDynamics.PageFX.FlashLand.Core.Tools
{
    internal class SimpleSwcLinker : ISwcLinker
    {
        public SimpleSwcLinker(IAssembly assembly)
        {
            Assembly = assembly;
        }

	    public IAssembly Assembly { get; private set; }

	    public object ResolveExternalReference(string id)
        {
            return AssemblyIndex.ResolveRef(Assembly, id);
        }

	    public event EventHandler<TypeEventArgs> TypeLinked;
    }
}