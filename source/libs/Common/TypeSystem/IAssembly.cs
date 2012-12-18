using DataDynamics.PageFX.Common.Collections;

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

		/// <summary>
		/// Finds type by fullname.
		/// </summary>
		/// <param name="fullname"></param>
		/// <returns></returns>
        IType FindType(string fullname);

		/// <summary>
		/// Gets assembly loader.
		/// </summary>
		IAssemblyLoader Loader { get; }

		/// <summary>
		/// Gets system types.
		/// </summary>
		SystemTypes SystemTypes { get; }
    }

    /// <summary>
    /// Represents collection of <see cref="IAssembly"/>s.
    /// </summary>
    public interface IAssemblyCollection : IReadOnlyList<IAssembly>
    {
	    IAssembly ResolveAssembly(IAssemblyReference reference);

	    void Add(IAssembly assembly);
    }
}

