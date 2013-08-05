using System;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Flash.Swc;

namespace DataDynamics.PageFX.Flash.Core.Tools
{
    internal sealed class SimpleSwcLinker : ISwcLinker
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

	    public bool Run()
	    {
		    return false;
	    }
    }
}