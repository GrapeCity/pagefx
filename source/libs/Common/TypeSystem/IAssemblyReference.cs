using System;
using System.Globalization;
using DataDynamics.PageFX.Common.CodeModel;

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

	/// <summary>
	/// Assembly flags
	/// </summary>
	[Flags]
	public enum AssemblyFlags
	{
		/// <summary>
		/// The assembly reference holds the full (unhashed) public key.
		/// </summary>
		PublicKey = 0x0001,

		/// <summary>
		/// The assembly is side by side compatible
		/// </summary>
		SideBySideCompatible = 0x0000,

		/// <summary>
		/// Reserved: both bits shall be zero
		/// </summary>
		Reserved = 0x0030,

		/// <summary>
		/// The implementation of this assembly used at runtime is not 
		/// expected to match the version seen at compile time. 
		/// (See the text following this table.)
		/// </summary>
		Retargetable = 0x0100,

		/// <summary>
		/// Reserved (a conforming implementation of the CLI may ignore this setting on read; 
		/// some implementations might use this bit to indicate that a 
		/// CIL-to-native-code compiler should not generate optimized code)
		/// </summary>
		DisableJITcompileOptimizer = 0x4000,

		/// <summary>
		/// Reserved  
		/// (a conforming implementation of the CLI may ignore this setting on read; 
		/// some implementations might use this bit to indicate that a 
		/// CIL-to-native-code compiler should generate CIL-to-native code map)
		/// </summary>
		EnableJITcompileTracking = 0x8000
	}

	public enum HashAlgorithmId
	{
		MD5 = 0x8003,
		None = 0,
		SHA1 = 0x8004
	}
}