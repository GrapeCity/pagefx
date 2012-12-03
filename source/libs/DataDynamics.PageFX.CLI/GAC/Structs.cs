using System;
using System.Runtime.InteropServices;

namespace DataDynamics.PageFX.Ecma335.GAC
{
    /// <summary>
    /// The ASSEMBLY_INFO structure represents information about an assembly in the assembly cache. 
    /// The fields of the structure are defined as follows: 
    /// cbAssemblyInfo - Size of the structure in bytes. Permits additions to the structure in future version of the .NET Framework.
    /// dwAssemblyFlags - Indicates one or more of the ASSEMBLYINFO_FLAG_* bits.
    /// uliAssemblySizeInKB - The size of the files that make up the assembly in kilobytes (KB).
    /// pszCurrentAssemblyPathBuf - A pointer to a string buffer that holds the current path of the directory that contains the 
    /// files that make up the assembly. The path must end with a zero.
    /// cchBuf - Size of the buffer that the pszCurrentAssemblyPathBug field points to.
    /// dwAssemblyFlags can have one of the following values: 
    /// ASSEMBLYINFO_FLAG__INSTALLED - Indicates that the assembly is actually installed. Always set in current version of the 
    /// .NET Framework.
    /// ASSEMBLYINFO_FLAG__PAYLOADRESIDENT - Never set in the current version of the .NET Framework.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet=CharSet.Unicode)]
    public struct ASSEMBLY_INFO
    {
        public uint cbAssemblyInfo;
        public uint dwAssemblyFlags;
        public ulong uliAssemblySizeInKB;
        public string pszCurrentAssemblyPathBuf;
        public uint cchBuf;
    }

    /// <summary>
    /// The FUSION_INSTALL_REFERENCE structure represents a reference that is made when an application has installed an 
    /// assembly in the GAC. 
    /// The fields of the structure are defined as follows: 
    /// cbSize - The size of the structure in bytes.
    /// dwFlags - Reserved, must be zero.
    /// guidScheme - The entity that adds the reference.
    /// szIdentifier - A unique string that identifies the application that installed the assembly.
    /// szNonCannonicalData - A string that is only understood by the entity that adds the reference. 
    /// The GAC only stores this string.
    /// Possible values for the guidScheme field can be one of the following: 
    /// FUSION_REFCOUNT_MSI_GUID - The assembly is referenced by an application that has been installed by using 
    /// Windows Installer. The szIdentifier field is set to MSI, and szNonCannonicalData is set to Windows Installer. 
    /// This scheme must only be used by Windows Installer itself.
    /// FUSION_REFCOUNT_UNINSTALL_SUBKEY_GUID - The assembly is referenced by an application that appears in Add/Remove 
    /// Programs. The szIdentifier field is the token that is used to register the application with Add/Remove programs.
    /// FUSION_REFCOUNT_FILEPATH_GUID - The assembly is referenced by an application that is represented by a file in 
    /// the file system. The szIdentifier field is the path to this file.
    /// FUSION_REFCOUNT_OPAQUE_STRING_GUID - The assembly is referenced by an application that is only represented 
    /// by an opaque string. The szIdentifier is this opaque string. The GAC does not perform existence checking 
    /// for opaque references when you remove this.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct FUSION_INSTALL_REFERENCE
    {
        public uint cbSize;
        public uint dwFlags;
        public Guid guidScheme;
        public string szIdentifier;
        public string szNonCannonicalData;
    }
}