using System;

namespace DataDynamics.PageFX.CLI.GAC
{
    /// <summary>
    /// The ASM_CACHE_FLAGS enumeration contains the following values: 
    /// ASM_CACHE_ZAP - Enumerates the cache of precompiled assemblies by using Ngen.exe.
    /// ASM_CACHE_GAC - Enumerates the GAC.
    /// ASM_CACHE_DOWNLOAD - Enumerates the assemblies that have been downloaded on-demand or that have been shadow-copied.
    /// </summary>
    [Flags]
    public enum ASM_CACHE_FLAGS
    {
        ASM_CACHE_DOWNLOAD = 4,
        ASM_CACHE_GAC = 2,
        ASM_CACHE_ZAP = 1
    }

    [Flags]
    public enum ASM_CMP_FLAGS
    {
        ALL = 0xff,
        BUILD_NUMBER = 8,
        CULTURE = 0x40,
        CUSTOM = 0x80,
        DEFAULT = 0x100,
        MAJOR_VERSION = 2,
        MINOR_VERSION = 4,
        NAME = 1,
        PUBLIC_KEY_TOKEN = 0x20,
        REVISION_NUMBER = 0x10
    }

    /// <summary>
    /// <see cref="IAssemblyName.GetDisplayName" />
    /// </summary>
    [Flags]
    public enum ASM_DISPLAY_FLAGS
    {
        VERSION = 0x01,
        CULTURE = 0x02,
        PUBLIC_KEY_TOKEN = 0x04,
        PUBLIC_KEY = 0x08,
        CUSTOM = 0x10,
        PROCESSORARCHITECTURE = 0x20,
        LANGUAGEID = 0x40,
        RETARGETABLE = 0x80,
        ALL = VERSION
              | CULTURE
              | PUBLIC_KEY_TOKEN
              | PROCESSORARCHITECTURE
              | RETARGETABLE
    }

    /// <summary>
    /// The ASM_NAME enumeration property ID describes the valid names of the name-value pairs in an assembly name. 
    /// See the .NET Framework SDK for a description of these properties. 
    /// </summary>
    public enum ASM_NAME
    {
        PUBLIC_KEY,
        PUBLIC_KEY_TOKEN,
        HASH_VALUE,
        NAME,
        MAJOR_VERSION,
        MINOR_VERSION,
        BUILD_NUMBER,
        REVISION_NUMBER,
        CULTURE,
        PROCESSOR_ID_ARRAY,
        OSINFO_ARRAY,
        HASH_ALGID,
        ALIAS,
        CODEBASE_URL,
        CODEBASE_LASTMOD,
        NULL_PUBLIC_KEY,
        NULL_PUBLIC_KEY_TOKEN,
        CUSTOM,
        NULL_CUSTOM,
        MVID,
        MAX_PARAMS,
    }

    /// <summary>
    /// The CREATE_ASM_NAME_OBJ_FLAGS enumeration contains the following values: 
    /// CANOF_PARSE_DISPLAY_NAME - If this flag is specified, the szAssemblyName parameter is a full assembly name and is parsed to 
    /// the individual properties. If the flag is not specified, szAssemblyName is the "Name" portion of the assembly name.
    /// CANOF_SET_DEFAULT_VALUES - If this flag is specified, certain properties, such as processor architecture, are set to 
    /// their default values.
    /// <see cref="AssemblyCache.CreateAssemblyNameObject" />
    /// </summary>
    public enum CREATE_ASM_NAME_OBJ_FLAGS
    {
        CANOF_PARSE_DISPLAY_NAME = 1,
        CANOF_SET_DEFAULT_VALUES = 2
    }

    /// <summary>
    /// <see cref="IAssemblyCache.InstallAssembly" />
    /// </summary>
    public enum IASSEMBLYCACHE_INSTALL_FLAG
    {
        IASSEMBLYCACHE_INSTALL_FLAG_FORCE_REFRESH = 2,
        IASSEMBLYCACHE_INSTALL_FLAG_REFRESH = 1
    }

    /// <summary>
    /// <see cref="IAssemblyCache.UninstallAssembly" />
    /// </summary>
    public enum IASSEMBLYCACHE_UNINSTALL_DISPOSITION
    {
        IASSEMBLYCACHE_UNINSTALL_DISPOSITION_ALREADY_UNINSTALLED = 3,
        IASSEMBLYCACHE_UNINSTALL_DISPOSITION_DELETE_PENDING = 4,
        IASSEMBLYCACHE_UNINSTALL_DISPOSITION_HAS_INSTALL_REFERENCES = 5,
        IASSEMBLYCACHE_UNINSTALL_DISPOSITION_REFERENCE_NOT_FOUND = 6,
        IASSEMBLYCACHE_UNINSTALL_DISPOSITION_STILL_IN_USE = 2,
        IASSEMBLYCACHE_UNINSTALL_DISPOSITION_UNINSTALLED = 1
    }

    /// <summary>
    /// <see cref="IAssemblyCache.QueryAssemblyInfo" />
    /// </summary>
    public enum QUERYASMINFO_FLAG
    {
        QUERYASMINFO_FLAG_GETSIZE = 2,
        QUERYASMINFO_FLAG_VALIDATE = 1
    }

    [Flags]
    public enum AssemblyInfoFlag : uint
    {
        None = 0,
        Validate = 1,
        GetSize = 2
    }

    public enum CreateAssemblyNameObjectFlags
    {
        CANOF_DEFAULT = 0,
        CANOF_PARSE_DISPLAY_NAME = 1,
    }
}