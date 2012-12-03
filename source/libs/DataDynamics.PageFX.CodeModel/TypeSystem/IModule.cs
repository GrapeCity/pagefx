using System;
using System.Collections.Generic;
using DataDynamics.PageFX.Common.CodeModel;

namespace DataDynamics.PageFX.Common.TypeSystem
{
    /// <summary>
    /// Represents a module.
    /// </summary>
    public interface IModule : ICustomAttributeProvider, ITypeContainer, ICodeNode
    {
        /// <summary>
        /// Gets or sets assembly where the module is defined.
        /// </summary>
        IAssembly Assembly { get; set; }

        /// <summary>
        /// Gets or sets name of the module.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets version of the module.
        /// </summary>
        Guid Version { get; set; }

        string Location { get; set; }

        bool IsMain { get; set; }

        IAssemblyCollection References { get; }

        IManifestFileCollection Files { get; }

		IManifestResourceCollection Resources { get; set; }

        IUnmanagedResourceCollection UnmanagedResources { get; }

        object ResolveMetadataToken(IMethod method, int token);
    }

    public interface IModuleCollection : IList<IModule>, ICodeNode
    {
        IModule this[string name] { get; }
    }
}