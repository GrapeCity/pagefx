using System;
using DataDynamics.PageFX.Common.TypeSystem;

namespace DataDynamics.PageFX.Flash.Swc
{
    public interface ISwcLinker
    {
        IAssembly Assembly { get; }

	    object ResolveExternalReference(string id);

	    event EventHandler<TypeEventArgs> TypeLinked;

	    bool Run();
    }
}