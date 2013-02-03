using System;
using System.Globalization;

namespace DataDynamics.PageFX.Common.TypeSystem
{
	/// <summary>
	/// Defines info about assembly.
	/// </summary>
	public interface IAssemblyReference
	{
		/// <summary>
		/// Gets assembly name.
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Gets assembly version.
		/// </summary>
		Version Version { get; }

		/// <summary>
		/// Gets assembly flags.
		/// </summary>
		AssemblyFlags Flags { get; }

		/// <summary>
		/// Gets the culture supported by the assembly.
		/// </summary>
		CultureInfo Culture { get; }

		/// <summary>
		/// Gets public key used to sign assembly.
		/// </summary>
		byte[] PublicKey { get; }

		/// <summary>
		/// Gets public key token used to sign the assembly.
		/// </summary>
		byte[] PublicKeyToken { get; }

		/// <summary>
		/// Gets hash value used to sign the assembly.
		/// </summary>
		byte[] HashValue { get; }

		/// <summary>
		/// Gets hash algorithm calculated when assembly was being signed.
		/// </summary>
		HashAlgorithmId HashAlgorithm { get; }

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