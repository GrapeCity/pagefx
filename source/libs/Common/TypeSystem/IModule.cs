using System;
using DataDynamics.PageFX.Common.CodeModel;
using DataDynamics.PageFX.Common.Collections;

namespace DataDynamics.PageFX.Common.TypeSystem
{
    /// <summary>
    /// Represents a module.
    /// </summary>
    public interface IModule : ICustomAttributeProvider, ITypeContainer, ICodeNode
    {
        /// <summary>
        /// Gets assembly where the module is defined.
        /// </summary>
        IAssembly Assembly { get; }

        /// <summary>
        /// Gets name of the module.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets version of the module.
        /// </summary>
        Guid Version { get; }

        bool IsMain { get; }

        IAssemblyCollection References { get; }

        IManifestFileCollection Files { get; }

		IManifestResourceCollection Resources { get; }

	    object ResolveMetadataToken(IMethod method, int token);
    }

    public interface IModuleCollection : IReadOnlyList<IModule>, ICodeNode
    {
        IModule this[string name] { get; }

	    void Add(IModule module);
    }
}