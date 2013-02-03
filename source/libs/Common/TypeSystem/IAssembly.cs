using DataDynamics.PageFX.Common.CodeModel;
using DataDynamics.PageFX.Common.Collections;

namespace DataDynamics.PageFX.Common.TypeSystem
{
	/// <summary>
    /// Represents an assembly.
    /// </summary>
    public interface IAssembly : IAssemblyReference, ICustomAttributeProvider, ITypeContainer, ICodeNode
    {
		/// <summary>
        /// Gets or sets path to this assembly
        /// </summary>
        string Location { get; }

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

		/// <summary>
		/// Gets type factory.
		/// </summary>
		TypeFactory TypeFactory { get; }

		/// <summary>
		/// Gets exposed types.
		/// </summary>
		IReadOnlyList<IType> GetExposedTypes();
    }

    /// <summary>
    /// Represents collection of <see cref="IAssembly"/>s.
    /// </summary>
    public interface IAssemblyCollection : IReadOnlyList<IAssembly>
    {
	    void Add(IAssembly assembly);
    }
}

