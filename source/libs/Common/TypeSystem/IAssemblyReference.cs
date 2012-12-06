using System;
using System.Globalization;
using DataDynamics.PageFX.Common.CodeModel;
using DataDynamics.PageFX.Common.Metadata;

namespace DataDynamics.PageFX.Common.TypeSystem
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
}