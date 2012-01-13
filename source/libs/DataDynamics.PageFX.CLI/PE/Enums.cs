using System;

namespace DataDynamics.PE
{
    #region enum PESignature
    public enum PESignature : ushort
    {
        UNKNOWN = 0,

        /// <summary>
        ///  "MZ"
        /// </summary>
        /// <remarks>
        ///  IMAGE_DOS_SIGNATURE
        /// </remarks>
        DOS = 0x5A4D,


        /// <summary>
        /// "NE"
        /// </summary>
        /// <remarks>
        ///  IMAGE_OS2_SIGNATURE
        /// </remarks>
        OS2 = 0x454E,


        /// <summary>
        ///  "LE"
        /// </summary>
        /// <remarks>
        ///  IMAGE_OS2_SIGNATURE_LE
        /// </remarks>
        OS2_LE = 0x454C,


        /// <summary>
        ///  "LE"
        /// </summary>
        /// <remarks>
        ///  IMAGE_VXD_SIGNATURE
        /// </remarks>
        VXD = OS2_LE,


        /// <summary>
        ///  "PE", the complete signature is "PE\0\0" (that is, NT followed by NT2).
        /// </summary>
        /// <remarks>
        ///  IMAGE_NT_SIGNATURE
        /// </remarks>
        NT = 0x4550,
        NT2 = 0
    }
    #endregion

    #region enum PEMachineId
    public enum PEMachineId : ushort
    {
        /// <summary>
        /// </summary>
        /// <remarks>
        /// IMAGE_FILE_MACHINE_UNKNOWN
        /// </remarks>
        UNKNOWN = 0,

        /// <summary>
        /// Intel 386.
        /// </summary>
        /// <remarks>
        /// IMAGE_FILE_MACHINE_I386
        /// </remarks>
        I386 = 0x014c,

        /// <summary>
        /// Intel 486.
        /// </summary>
        /// <remarks>
        /// </remarks>
        I486 = 0x014d,

        /// <summary>
        /// Intel Pentium.
        /// </summary>
        /// <remarks>
        /// </remarks>
        PENTIUM = 0x014e,

        /// <summary>
        /// MIPS 3K big-endian
        /// </summary>
        /// <remarks>
        /// IMAGE_FILE_MACHINE_R3000
        /// </remarks>
        R3000_BE = 0x0160,

        /// <summary>
        /// MIPS 3K little-endian, 0x160 big-endian
        /// </summary>
        /// <remarks>
        /// IMAGE_FILE_MACHINE_R3000
        /// </remarks>
        R3000 = 0x0162,

        /// <summary>
        /// MIPS 4K little-endian
        /// </summary>
        /// <remarks>
        /// IMAGE_FILE_MACHINE_R4000
        /// </remarks>
        R4000 = 0x0166,

        /// <summary>
        /// MIPS little-endian
        /// </summary>
        /// <remarks>
        /// IMAGE_FILE_MACHINE_R10000
        /// </remarks>
        R10000 = 0x0168,

        /// <summary>
        /// MIPS little-endian WCE v2
        /// </summary>
        /// <remarks>
        /// IMAGE_FILE_MACHINE_WCEMIPSV2
        /// </remarks>
        WCEMIPSV2 = 0x0169,

        /// <summary>
        /// Alpha_AXP
        /// </summary>
        /// <remarks>
        /// IMAGE_FILE_MACHINE_ALPHA
        /// </remarks>
        ALPHA = 0x0184,

        /// <summary>
        /// SH3 little-endian
        /// </summary>
        /// <remarks>
        /// IMAGE_FILE_MACHINE_SH3
        /// </remarks>
        SH3 = 0x01a2,

        /// <summary>
        /// </summary>
        /// <remarks>
        /// IMAGE_FILE_MACHINE_SH3DSP
        /// </remarks>
        SH3DSP = 0x01a3,

        /// <summary>
        /// SH3E little-endian
        /// </summary>
        /// <remarks>
        /// IMAGE_FILE_MACHINE_SH3E
        /// </remarks>
        SH3E = 0x01a4,

        /// <summary>
        /// SH4 little-endian
        /// </summary>
        /// <remarks>
        /// IMAGE_FILE_MACHINE_SH4
        /// </remarks>
        SH4 = 0x01a6,

        /// <summary>
        /// SH5
        /// </summary>
        /// <remarks>
        /// IMAGE_FILE_MACHINE_SH5
        /// </remarks>
        SH5 = 0x01a8,

        /// <summary>
        /// ARM Little-Endian
        /// </summary>
        /// <remarks>
        /// IMAGE_FILE_MACHINE_ARM
        /// </remarks>
        ARM = 0x01c0,

        /// <summary>
        ///  ARM 10 Thumb family CPU.
        /// </summary>
        /// <remarks>
        /// IMAGE_FILE_MACHINE_THUMB
        /// http://www.arm.com/armtech/ARM10_Thumb?OpenDocument&ExpandSection=2
        /// </remarks>
        THUMB = 0x01c2,

        /// <summary>
        /// </summary>
        /// <remarks>
        /// IMAGE_FILE_MACHINE_AM33
        /// </remarks>
        AM33 = 0x01d3,

        /// <summary>
        /// IBM PowerPC Little-Endian
        /// </summary>
        /// <remarks>
        /// IMAGE_FILE_MACHINE_POWERPC
        /// </remarks>
        POWERPC = 0x01F0,

        /// <summary>
        /// </summary>
        /// <remarks>
        /// IMAGE_FILE_MACHINE_POWERPCFP
        /// </remarks>
        POWERPCFP = 0x01f1,

        /// <summary>
        /// Intel 64
        /// </summary>
        /// <remarks>
        /// IMAGE_FILE_MACHINE_IA64
        /// </remarks>
        IA64 = 0x0200,

        /// <summary>
        /// MIPS
        /// </summary>
        /// <remarks>
        /// IMAGE_FILE_MACHINE_MIPS16
        /// </remarks>
        MIPS16 = 0x0266,

        /// <summary>
        /// ALPHA64
        /// </summary>
        /// <remarks>
        /// IMAGE_FILE_MACHINE_ALPHA64
        /// </remarks>
        ALPHA64 = 0x0284,

        /// <summary>
        /// MIPS
        /// </summary>
        /// <remarks>
        /// IMAGE_FILE_MACHINE_MIPSFPU
        /// </remarks>
        MIPSFPU = 0x0366,

        /// <summary>
        /// MIPS
        /// </summary>
        /// <remarks>
        /// IMAGE_FILE_MACHINE_MIPSFPU16
        /// </remarks>
        MIPSFPU16 = 0x0466,

        /// <summary>
        /// </summary>
        /// <remarks>
        /// IMAGE_FILE_MACHINE_AXP64
        /// </remarks>
        AXP64 = ALPHA64,

        /// <summary>
        /// Infineon
        /// </summary>
        /// <remarks>
        ///  IMAGE_FILE_MACHINE_TRICORE
        ///  http://www.infineon.com/tricore
        /// </remarks>
        TRICORE = 0x0520,

        /// <summary>
        /// Common Executable Format (Windows CE).
        /// </summary>
        /// <remarks>
        /// IMAGE_FILE_MACHINE_CEF
        /// </remarks>
        CEF = 0x0CEF,

        /// <summary>
        /// EFI Byte Code
        /// </summary>
        EBC = 0x0EBC,

        /// <summary>
        /// AMD64 (K8)
        /// </summary>
        /// <remarks>
        /// IMAGE_FILE_MACHINE_AMD64
        /// </remarks>
        AMD64 = 0x8664,

        /// <summary>
        /// M32R little-endian
        /// </summary>
        /// <remarks>
        /// IMAGE_FILE_MACHINE_M32R
        /// </remarks>
        M32R = 0x9104,

        /// <summary>
        /// </summary>
        /// <remarks>
        /// IMAGE_FILE_MACHINE_CEE
        /// </remarks>
        CEE = 0xC0EE,
    }
    #endregion

    #region enum PECharacteristics
    [Flags]
    public enum PECharacteristics : ushort
    {
        /// <summary>
        /// Relocation info stripped from file.
        /// </summary>
        /// <remarks>
        ///  IMAGE_FILE_RELOCS_STRIPPED
        /// </remarks>
        RELOCS_STRIPPED = 0x0001,


        /// <summary>
        /// File is executable
        /// (i.e. file is neither object file nor library file,
        /// so there are no unresolved externel references).
        /// </summary>
        /// <remarks>
        ///  IMAGE_FILE_EXECUTABLE_IMAGE
        /// </remarks>
        EXECUTABLE_IMAGE = 0x0002,


        /// <summary>
        /// Line nunbers stripped from file.
        /// </summary>
        /// <remarks>
        ///  IMAGE_FILE_LINE_NUMS_STRIPPED
        /// </remarks>
        LINE_NUMS_STRIPPED = 0x0004,


        /// <summary>
        /// Local symbols stripped from file.
        /// </summary>
        /// <remarks>
        ///  IMAGE_FILE_LOCAL_SYMS_STRIPPED
        /// </remarks>
        LOCAL_SYMS_STRIPPED = 0x0008,


        /// <summary>
        /// Agressively trim working set
        /// </summary>
        /// <remarks>
        ///  IMAGE_FILE_AGGRESIVE_WS_TRIM
        /// </remarks>
        AGGRESIVE_WS_TRIM = 0x0010,


        /// <summary>
        /// App can handle >2gb addresses
        /// </summary>
        /// <remarks>
        ///  IMAGE_FILE_LARGE_ADDRESS_AWARE
        /// </remarks>
        LARGE_ADDRESS_AWARE = 0x0020,


        /// <summary>
        /// Bytes of machine word are reversed.
        /// </summary>
        /// <remarks>
        ///  IMAGE_FILE_BYTES_REVERSED_LO
        /// </remarks>
        BYTES_REVERSED_LO = 0x0080,


        /// <summary>
        /// 32 bit word machine.
        /// </summary>
        /// <remarks>
        /// IMAGE_FILE_32BIT_MACHINE
        /// </remarks>
        MACHINE_32BIT = 0x0100,


        /// <summary>
        /// Debugging info stripped from file in .DBG file
        /// </summary>
        /// <remarks>
        ///  IMAGE_FILE_DEBUG_STRIPPED
        /// </remarks>
        DEBUG_STRIPPED = 0x0200,


        /// <summary>
        /// If Image is on removable media, copy and run from the swap file.
        /// </summary>
        /// <remarks>
        ///  IMAGE_FILE_REMOVABLE_RUN_FROM_SWAP
        /// </remarks>
        REMOVABLE_RUN_FROM_SWAP = 0x0400,


        /// <summary>
        /// If Image is on Net, copy and run from the swap file.
        /// </summary>
        /// <remarks>
        ///  IMAGE_FILE_NET_RUN_FROM_SWAP
        /// </remarks>
        NET_RUN_FROM_SWAP = 0x0800,


        /// <summary>
        /// This flag is used to indicate that the file
        /// is a system sile, such as device driver.
        /// </summary>
        /// <remarks>
        ///  IMAGE_FILE_SYSTEM
        /// </remarks>
        SYSTEM = 0x1000,


        /// <summary>
        /// This flag indicates that the file
        /// is a dynamic library (DLL).
        /// </summary>
        /// <remarks>
        ///  IMAGE_FILE_DLL
        /// </remarks>
        DLL = 0x2000,


        /// <summary>
        /// File should only be run on a uni-processor (UP) machine.
        /// </summary>
        /// <remarks>
        ///  IMAGE_FILE_UP_SYSTEM_ONLY
        /// </remarks>
        UP_SYSTEM_ONLY = 0x4000,


        /// <summary>
        /// Bytes of machine word are reversed.
        /// </summary>
        /// <remarks>
        ///  IMAGE_FILE_BYTES_REVERSED_HI
        /// </remarks>
        BYTES_REVERSED_HI = 0x8000,


        /// <summary>
        /// Default flags that must be set in CIL-only image.
        /// </summary>
        /// <remarks>
        /// See Partition II, 24.2.2.1
        /// </remarks>
        CIL_DEFAULT = LINE_NUMS_STRIPPED |
                      LOCAL_SYMS_STRIPPED |
                      DEBUG_STRIPPED
    }
    #endregion

    #region enum PESectionCharacteristics
    [Flags]
    public enum PESectionCharacteristics : uint
    {
        /// <summary>
        /// Reserved.
        /// </summary>
        IMAGE_SCN_TYPE_REG = 0x00000000,

        /// <summary>
        /// Reserved.
        /// </summary>
        IMAGE_SCN_TYPE_DSECT = 0x00000001,

        /// <summary>
        /// Reserved.
        /// </summary>
        IMAGE_SCN_TYPE_NOLOAD = 0x00000002,

        /// <summary>
        /// Reserved.
        /// </summary>
        IMAGE_SCN_TYPE_GROUP = 0x00000004,

        /// <summary>
        /// Reserved.
        /// </summary>
        IMAGE_SCN_TYPE_NO_PAD = 0x00000008,

        /// <summary>
        /// Reserved.
        /// </summary>
        IMAGE_SCN_TYPE_COPY = 0x00000010,

        /// <summary>
        /// Section contains code.
        /// </summary>
        IMAGE_SCN_CNT_CODE = 0x00000020,

        /// <summary>
        /// Section contains initialized data.
        /// </summary>
        IMAGE_SCN_CNT_INITIALIZED_DATA = 0x00000040,

        /// <summary>
        /// Section contains uninitialized data.
        /// </summary>
        IMAGE_SCN_CNT_UNINITIALIZED_DATA = 0x00000080,

        /// <summary>
        /// Reserved.
        /// </summary>
        IMAGE_SCN_LNK_OTHER = 0x00000100,

        /// <summary>
        /// Section contains comments or some other type of information.
        /// </summary>
        IMAGE_SCN_LNK_INFO = 0x00000200,

        /// <summary>
        /// Reserved.
        /// </summary>
        IMAGE_SCN_TYPE_OVER = 0x00000400,

        /// <summary>
        /// Section contents will not become part of image.
        /// </summary>
        IMAGE_SCN_LNK_REMOVE = 0x00000800,

        /// <summary>
        /// Section contents comdat.
        /// </summary>
        IMAGE_SCN_LNK_COMDAT = 0x00001000,


        /// <summary>
        /// Reset speculative exceptions handling bits in the TLB entries for this section.
        /// </summary>
        /// <remarks>
        /// IMAGE_SCN_MEM_PROTECTED - Obsolete.
        /// </remarks>
        IMAGE_SCN_NO_DEFER_SPEC_EXC = 0x00004000,

        /// <summary>
        /// Section content can be accessed relative to GP (MIPS).
        /// </summary>
        IMAGE_SCN_GPREL = 0x00008000,

        /// <summary>
        /// </summary>
        IMAGE_SCN_MEM_FARDATA = 0x00008000,

        /// <summary>
        /// Obsolete.
        /// </summary>
        IMAGE_SCN_MEM_PURGEABLE = 0x00020000,

        /// <summary>
        /// Obsolete.
        /// </summary>
        IMAGE_SCN_MEM_16BIT = 0x00020000,

        /// <summary>
        /// Obsolete.
        /// </summary>
        IMAGE_SCN_MEM_LOCKED = 0x00040000,

        /// <summary>
        /// Obsolete.
        /// </summary>
        IMAGE_SCN_MEM_PRELOAD = 0x00080000,

        /// <summary>
        /// </summary>
        /// <remarks>
        /// IMAGE_SCN_MEM_SYSHEAP  - Obsolete    0x00010000
        /// </remarks>
        IMAGE_SCN_ALIGN_1BYTES = 0x00100000,

        IMAGE_SCN_ALIGN_2BYTES = 0x00200000,
        IMAGE_SCN_ALIGN_4BYTES = 0x00300000,
        IMAGE_SCN_ALIGN_8BYTES = 0x00400000,

        // default alignment
        IMAGE_SCN_ALIGN_16BYTES = 0x00500000,

        IMAGE_SCN_ALIGN_32BYTES = 0x00600000,
        IMAGE_SCN_ALIGN_64BYTES = 0x00700000,
        IMAGE_SCN_ALIGN_128BYTES = 0x00800000,
        IMAGE_SCN_ALIGN_256BYTES = 0x00900000,
        IMAGE_SCN_ALIGN_512BYTES = 0x00A00000,
        IMAGE_SCN_ALIGN_1024BYTES = 0x00B00000,
        IMAGE_SCN_ALIGN_2048BYTES = 0x00C00000,
        IMAGE_SCN_ALIGN_4096BYTES = 0x00D00000,
        IMAGE_SCN_ALIGN_8192BYTES = 0x00E00000,

        IMAGE_SCN_ALIGN_MASK = 0x00F00000,

        /// <summary>
        /// Section contains extended relocations.
        /// </summary>
        IMAGE_SCN_LNK_NRELOC_OVFL = 0x01000000,

        /// <summary>
        /// Section can be discarded.
        /// </summary>
        IMAGE_SCN_MEM_DISCARDABLE = 0x02000000,

        /// <summary>
        /// Section is not cachable.
        /// </summary>
        IMAGE_SCN_MEM_NOT_CACHED = 0x04000000,

        /// <summary>
        /// Section is not pageable.
        /// </summary>
        IMAGE_SCN_MEM_NOT_PAGED = 0x08000000,

        /// <summary>
        /// Section is shareable.
        /// </summary>
        IMAGE_SCN_MEM_SHARED = 0x10000000,

        /// <summary>
        /// Section is executable.
        /// </summary>
        IMAGE_SCN_MEM_EXECUTE = 0x20000000,

        /// <summary>
        /// Section is readable.
        /// </summary>
        IMAGE_SCN_MEM_READ = 0x40000000,

        /// <summary>
        /// Section is writeable.
        /// </summary>
        IMAGE_SCN_MEM_WRITE = 0x80000000,


        /// <summary>
        /// TLS index is scaled.
        /// </summary>
        IMAGE_SCN_SCALE_INDEX = 0x00000001,
    }
    #endregion

    #region enum PEDebugType
    /// <summary>
    /// Summary description for DebugType.
    /// </summary>
    public enum PEDebugType : uint
    {
        Unknown = 0,
        COFF = 1,
        Codeview = 2,
        FPO = 3,
        Misc = 4,
        Exception = 5,
        Fixup = 6,
        OMAPtoSRC = 7,
        OMAPfromSRC = 8,
        Borland = 9,
        Reserved10 = 10,
        CLSID = 11
    }
    #endregion
}