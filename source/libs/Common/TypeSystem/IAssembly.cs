using System;
using System.Collections.Generic;

namespace DataDynamics.PageFX.Common.TypeSystem
{
	/// <summary>
    /// Represents an assembly.
    /// </summary>
    public interface IAssembly : IAssemblyReference, ICustomAttributeProvider, ITypeContainer
    {
		/// <summary>
		/// Specifies whether this assembly is corlib.
		/// </summary>
        bool IsCorlib { get; set; }

        /// <summary>
        /// Gets or sets path to this assembly
        /// </summary>
        string Location { get; set; }

        /// <summary>
        /// Gets or sets type of this assembly
        /// </summary>
        AssemblyType Type { get; set; }

        /// <summary>
        /// Gets the entry point in this assembly.
        /// </summary>
        IMethod EntryPoint { get; }

		/// <summary>
		/// Gets assembly modules.
		/// </summary>
        IModuleCollection Modules { get; }

		/// <summary>
		/// Gets main module.
		/// </summary>
        IModule MainModule { get; }

        IType FindType(string fullname);

		IAssemblyLoader Loader { get; }
    }

    /// <summary>
    /// Represents collection of <see cref="IAssembly"/>s.
    /// </summary>
    public interface IAssemblyCollection : IList<IAssembly>
    {
        IAssembly this[IAssemblyReference r] { get; }
    }
}

