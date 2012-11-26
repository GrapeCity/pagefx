using System;
using System.Collections.Generic;
using System.Globalization;

namespace DataDynamics.PageFX.CodeModel
{
    /// <summary>
    /// Contains info about assembly name.
    /// </summary>
    public interface IAssemblyReference : ICodeNode
    {
        /// <summary>
        /// Gets or sets assembly name.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets assembly version.
        /// </summary>
        Version Version { get; set; }

        /// <summary>
        /// Gets or sets assembly flags.
        /// </summary>
        AssemblyFlags Flags { get; set; }

        /// <summary>
        /// Gets or sets the culture supported by the assembly.
        /// </summary>
        CultureInfo Culture { get; set; }

        /// <summary>
        /// Gets or sets public key used to sign assembly.
        /// </summary>
        byte[] PublicKey { get; set; }

        /// <summary>
        /// Gets or sets public key token used to sign the assembly.
        /// </summary>
        byte[] PublicKeyToken { get; set; }

        /// <summary>
        /// Gets or sets hash value used to sign the assembly.
        /// </summary>
        byte[] HashValue { get; set; }

		/// <summary>
		/// Gets or sets hash algorithm calculated when assembly was being signed.
		/// </summary>
		HashAlgorithmId HashAlgorithm { get; set; }

        /// <summary>
        /// Gets the full name of the assembly.
        /// </summary>
        string FullName { get; }
    }

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
        /// Gets or sets auxiliary marker that can be used for some needs.
        /// </summary>
        int Marker { get; set; }

        /// <summary>
        /// Gets or sets entry point in this assembly.
        /// </summary>
        IMethod EntryPoint { get; set; }

		/// <summary>
		/// Gets assembly modules.
		/// </summary>
        IModuleCollection Modules { get; }

		/// <summary>
		/// Gets main module.
		/// </summary>
        IModule MainModule { get; }

        IType FindType(string fullname);
    }

    /// <summary>
    /// Represents collection of <see cref="IAssembly"/>s.
    /// </summary>
    public interface IAssemblyCollection : IList<IAssembly>
    {
        IAssembly this[IAssemblyReference r] { get; }
    }
}

