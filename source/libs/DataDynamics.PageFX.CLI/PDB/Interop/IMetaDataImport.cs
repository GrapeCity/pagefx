using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using ProgramDebugDatabase;

namespace DataDynamics.PageFX.PDB.Interop
{
    using HRESULT = UInt32;

    using ULONG32 = UInt32;

    using MD_TOKEN = UInt32;                 //Generic token
    using MD_MODULE = UInt32;                // Module token (roughly, a scope)
    using MD_TYPE_REF = UInt32;              // TypeRef reference (this or other scope)
    using MD_TYPE_DEF = UInt32;              // using in this scope
    using MD_FIELD_DEF = UInt32;             // Field in this scope
    using MD_METHOD_DEF = UInt32;            // Method in this scope
    using MD_PARAM_DEF = UInt32;             // param token
    using MD_INTERFACE_IMPL = UInt32;        // interface implementation token
    using MD_MEMBER_REF = UInt32;            // MemberRef (this or other scope)
    using MD_CUSTOM_ATTRIBUTE = UInt32;      // attribute token
    using MD_PERMISSION = UInt32;            // DeclSecurity
    using MD_SIGNATURE = UInt32;             // Signature object
    using MD_EVENT = UInt32;                 // event token
    using MD_PROPERTY = UInt32;              // property token
    using MD_MODULE_REF = UInt32;            // Module reference (for the imported modules)

    // Assembly tokens.
    using MD_ASSEMBLY = UInt32;              // Assembly token.
    using MD_ASSEMBLY_REF = UInt32;          // AssemblyRef token.
    using MD_FILE = UInt32;                  // File token.
    using MD_EXPORTED_TYPE = UInt32;         // ExportedType token.
    using MD_MANIFEST_RESOURCE = UInt32;     // ManifestResource token.
    using MD_TYPE_SPEC = UInt32;             // TypeSpec object
    using MD_GENERIC_PARAM = UInt32;         // formal parameter to generic type or method
    using MD_METHOD_SPEC = UInt32;           // instantiation of a generic method
    using MD_GENERIC_PARAM_CONSTRAINT = UInt32;// constraint on a formal generic parameter

    // Application string.
    using MD_STRING = UInt32;                // User literal string token.
    using MD_CPTOKEN = UInt32;               // constantpool token

    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid(Guids.IID_IMetaDataImport)]
    public interface IMetaDataImport
    {
        [PreserveSig]
        UInt32 CloseEnum(
            IntPtr hEnum
            );

        [PreserveSig]
        UInt32 CountEnum(
            IntPtr hEnum,
            [MarshalAs(UnmanagedType.I4)] out Int32 count
            );

        [PreserveSig]
        UInt32 EnumCustomAttributes(
            [In, Out] ref IntPtr hEnum,
            [MarshalAs(UnmanagedType.U4)] UInt32 tk,
            [MarshalAs(UnmanagedType.U4)] UInt32 tkType,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] UInt32[] rCustomAttributes,
            [MarshalAs(UnmanagedType.I4)] Int32 cMax,
            [MarshalAs(UnmanagedType.I4)] out Int32 pcCustomAttributes
            );

        [PreserveSig]
        UInt32 EnumEvents(
            [In, Out] ref IntPtr hEnum,
            [MarshalAs(UnmanagedType.U4)] UInt32 td,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] UInt32 rEvents,
            [MarshalAs(UnmanagedType.I4)] Int32 cMax,
            [MarshalAs(UnmanagedType.I4)] out Int32 pcEvents
            );

        [PreserveSig]
        UInt32 EnumFields(
            [In, Out] ref IntPtr hEnum,
            [MarshalAs(UnmanagedType.U4)] UInt32 cl,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] UInt32 rFields,
            [MarshalAs(UnmanagedType.I4)] Int32 cMax,
            [MarshalAs(UnmanagedType.I4)] out Int32 pcTokens
            );

        [PreserveSig]
        UInt32 EnumFieldsWithName(
            [In, Out] ref IntPtr hEnum,
            [In, MarshalAs(UnmanagedType.U4)] UInt32 cl,
            [MarshalAs(UnmanagedType.LPWStr)] string szName,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] UInt32[] rFields,
            [MarshalAs(UnmanagedType.I4)] Int32 cMax,
            [MarshalAs(UnmanagedType.I4)] out Int32 pcTokens
            );

        [PreserveSig]
        UInt32 EnumInterfaceImpls(
            [In, Out] ref IntPtr hEnum,
            [In, MarshalAs(UnmanagedType.U4)] UInt32 td,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] UInt32[] rImpls,
            [In, MarshalAs(UnmanagedType.I4)] Int32 cMax,
            [MarshalAs(UnmanagedType.I4)] out Int32 pcTokens
            );

        [PreserveSig]
        UInt32 EnumMemberRefs(
            [In, Out] ref IntPtr hEnum,
            [In, MarshalAs(UnmanagedType.U4)] UInt32 tkParent,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] UInt32[] rMemberRefs,
            [In, MarshalAs(UnmanagedType.I4)] Int32 cMax,
            [MarshalAs(UnmanagedType.I4)] out Int32 pcTokens
            );

        [PreserveSig]
        UInt32 EnumMembers(
            [In, Out] ref IntPtr hEnum,
            [In, MarshalAs(UnmanagedType.U4)] UInt32 cl,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] UInt32[] rMembers,
            [In, MarshalAs(UnmanagedType.I4)] Int32 cMax,
            [MarshalAs(UnmanagedType.I4)] out Int32 pcTokens
            );

        [PreserveSig]
        UInt32 EnumMembersWithName(
            [In, Out] ref IntPtr hEnum,
            [In, MarshalAs(UnmanagedType.U4)] UInt32 cl,
            [MarshalAs(UnmanagedType.LPWStr)] string szName,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] UInt32[] rMembers,
            [In, MarshalAs(UnmanagedType.I4)] Int32 cMax,
            [MarshalAs(UnmanagedType.I4)] out Int32 pcTokens
            );

        [PreserveSig]
        UInt32 EnumMethodImpls(
            [In, Out] ref IntPtr hEnum,
            [In, MarshalAs(UnmanagedType.U4)] UInt32 td,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] UInt32[] rMethodBody,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] UInt32[] rMethodDecl,
            [In, MarshalAs(UnmanagedType.I4)] Int32 cMax,
            [MarshalAs(UnmanagedType.I4)] out Int32 pcTokens
            );

        [PreserveSig]
        UInt32 EnumMethodSemantics(
            [In, Out] ref IntPtr hEnum,
            [In, MarshalAs(UnmanagedType.U4)] UInt32 mb,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] UInt32[] rEventProp,
            [In, MarshalAs(UnmanagedType.I4)] Int32 cMax,
            [MarshalAs(UnmanagedType.I4)] out Int32 pcTokens
            );

        [PreserveSig]
        UInt32 EnumMethods(
            [In, Out] ref IntPtr hEnum,
            [In, MarshalAs(UnmanagedType.U4)] UInt32 cl,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] UInt32[] rMethods,
            [In, MarshalAs(UnmanagedType.I4)] Int32 cMax,
            [MarshalAs(UnmanagedType.I4)] out Int32 pcTokens
            );

        [PreserveSig]
        UInt32 EnumMethodsWithName(
            [In, Out] ref IntPtr hEnum,
            [In, MarshalAs(UnmanagedType.U4)] UInt32 cl,
            [MarshalAs(UnmanagedType.LPWStr)] string szName,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] UInt32[] rMethods,
            [In, MarshalAs(UnmanagedType.I4)] Int32 cMax,
            [MarshalAs(UnmanagedType.I4)] out Int32 pcTokens
            );

        [PreserveSig]
        UInt32 EnumModuleRefs(
            [In, Out] ref IntPtr hEnum,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] UInt32[] rModuleRefs,
            [In, MarshalAs(UnmanagedType.I4)] Int32 cMax,
            [MarshalAs(UnmanagedType.I4)] out Int32 pcModuleRefs
            );

        [PreserveSig]
        UInt32 EnumParams(
            [In, Out] ref IntPtr hEnum,
            [In, MarshalAs(UnmanagedType.U4)] UInt32 cl,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] UInt32[] rParams,
            [In, MarshalAs(UnmanagedType.I4)] Int32 cMax,
            [MarshalAs(UnmanagedType.I4)] out Int32 pcTokens
            );

        [PreserveSig]
        UInt32 EnumPermissionSets(
            [In, Out] ref IntPtr hEnum,
            [In, MarshalAs(UnmanagedType.U4)] UInt32 tk,
            [In, MarshalAs(UnmanagedType.U4)] UInt32 dwActions,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] UInt32[] rPermission,
            [In, MarshalAs(UnmanagedType.I4)] Int32 cMax,
            [MarshalAs(UnmanagedType.I4)] out Int32 pcTokens
            );

        [PreserveSig]
        UInt32 EnumProperties(
            [In, Out] ref IntPtr hEnum,
            [In, MarshalAs(UnmanagedType.U4)] UInt32 td,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] UInt32[] rProperies,
            [In, MarshalAs(UnmanagedType.I4)] Int32 cMax,
            [MarshalAs(UnmanagedType.I4)] out Int32 pcProperties
            );

        [PreserveSig]
        UInt32 EnumSignatures(
            [In, Out] ref IntPtr hEnum,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] UInt32[] rSignatures,
            [In, MarshalAs(UnmanagedType.I4)] Int32 cMax,
            [MarshalAs(UnmanagedType.I4)] out Int32 pcSignatures
            );

        [PreserveSig]
        UInt32 EnumTypeDefs(
            [In, Out] ref IntPtr hEnum,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] UInt32[] rTypeDefs,
            [In, MarshalAs(UnmanagedType.I4)] Int32 cMax,
            [MarshalAs(UnmanagedType.I4)] out Int32 pcTypeDefs
            );

        [PreserveSig]
        UInt32 EnumTypeRefs(
            [In, Out] ref IntPtr hEnum,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] UInt32[] rTypeRefs,
            [In, MarshalAs(UnmanagedType.I4)] Int32 cMax,
            [MarshalAs(UnmanagedType.I4)] out Int32 pcTypeDefs
            );

        [PreserveSig]
        UInt32 EnumTypeSpecs(
            [In, Out] ref IntPtr hEnum,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] UInt32[] rTypeSpecs,
            [In, MarshalAs(UnmanagedType.I4)] Int32 cMax,
            [MarshalAs(UnmanagedType.I4)] out Int32 pcTypeSpecs
            );

        [PreserveSig]
        UInt32 EnumUnresolvedMethods(
            [In, Out] ref IntPtr hEnum,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] UInt32[] rMethods,
            [In, MarshalAs(UnmanagedType.I4)] Int32 cMax,
            [MarshalAs(UnmanagedType.I4)] out Int32 pcTokens
            );

        [PreserveSig]
        UInt32 EnumUserStrings(
            [In, Out] ref IntPtr hEnum,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] UInt32[] rStrings,
            [In, MarshalAs(UnmanagedType.I4)] Int32 cMax,
            [MarshalAs(UnmanagedType.I4)] out Int32 pcStrings
            );

        [PreserveSig]
        UInt32 FindField(
            [In, MarshalAs(UnmanagedType.U4)] UInt32 td,
            [MarshalAs(UnmanagedType.LPWStr)] string szName,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] byte[] pvSigBlob,
            [In, MarshalAs(UnmanagedType.I4)] Int32 cbSigBlob,
            [MarshalAs(UnmanagedType.U4)] out UInt32 pmb
            );

        [PreserveSig]
        UInt32 FindMember(
            [In, MarshalAs(UnmanagedType.U4)] UInt32 td,
            [MarshalAs(UnmanagedType.LPWStr)] string szName,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] byte[] pvSigBlob,
            [In, MarshalAs(UnmanagedType.I4)] Int32 cbSigBlob,
            [MarshalAs(UnmanagedType.U4)] out UInt32 pmb
            );

        [PreserveSig]
        UInt32 FindMemberRef(
            [In, MarshalAs(UnmanagedType.U4)] UInt32 td,
            [MarshalAs(UnmanagedType.LPWStr)] string szName,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] byte[] pvSigBlob,
            [In, MarshalAs(UnmanagedType.I4)] Int32 cbSigBlob,
            [MarshalAs(UnmanagedType.U4)] out UInt32 pmr
            );

        [PreserveSig]
        UInt32 FindMethod(
            [In, MarshalAs(UnmanagedType.U4)] UInt32 td,
            [MarshalAs(UnmanagedType.LPWStr)] string szName,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] byte[] pvSigBlob,
            [In, MarshalAs(UnmanagedType.I4)] Int32 cbSigBlob,
            [MarshalAs(UnmanagedType.U4)] out UInt32 pmb
            );

        [PreserveSig]
        UInt32 FindTypeDefByName(
            [MarshalAs(UnmanagedType.LPWStr)] string szTypeDef,
            [In, MarshalAs(UnmanagedType.U4)] UInt32 tkEnclosingClass,
            [MarshalAs(UnmanagedType.U4)] out UInt32 ptd
            );

        [PreserveSig]
        UInt32 FindTypeRef(
            [In, MarshalAs(UnmanagedType.U4)] UInt32 tkResolutionScope,
            [MarshalAs(UnmanagedType.LPWStr)] string szName,
            [MarshalAs(UnmanagedType.U4)] out UInt32 ptr
            );

        [PreserveSig]
        UInt32 GetClassLayout(
            [In, MarshalAs(UnmanagedType.U4)] UInt32 td,
            [MarshalAs(UnmanagedType.U4)] out UInt32 pdwPackSize,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] long[] tFieldOffset,
            [In, MarshalAs(UnmanagedType.I4)] Int32 cMax,
            [MarshalAs(UnmanagedType.I4)] out Int32 pcFieldOffset,
            [MarshalAs(UnmanagedType.I4)] out UInt32 pulClassSize
            );

        [PreserveSig]
        UInt32 GetCustomAttributeByName(
            [In, MarshalAs(UnmanagedType.U4)] UInt32 tkObj,
            [MarshalAs(UnmanagedType.LPWStr)] string szName,
            out IntPtr ppData,
            [MarshalAs(UnmanagedType.I4)] out Int32 pcbData
            );

        [PreserveSig]
        UInt32 GetCustomAttributeProps(
            [In, MarshalAs(UnmanagedType.U4)] UInt32 cv,
            [MarshalAs(UnmanagedType.U4)] out UInt32 ptkObj,
            [MarshalAs(UnmanagedType.U4)] out UInt32 ptkType,
            out IntPtr ppBlob,
            [MarshalAs(UnmanagedType.I4)] out Int32 pcbSize
            );

        [PreserveSig]
        UInt32 GetEventProps(
            [In, MarshalAs(UnmanagedType.U4)] UInt32 ev,
            [MarshalAs(UnmanagedType.U4)] out UInt32 pClass,
            [MarshalAs(UnmanagedType.LPWStr)] StringBuilder szEvent,
            [In, MarshalAs(UnmanagedType.I4)] Int32 cchEvent,
            [MarshalAs(UnmanagedType.I4)] out Int32 pchEvent,
            [MarshalAs(UnmanagedType.U4)] out EventAttributes pdwEventFlags,
            [MarshalAs(UnmanagedType.U4)] out UInt32 ptkEventType,
            [MarshalAs(UnmanagedType.U4)] out UInt32 pmdAddOn,
            [MarshalAs(UnmanagedType.U4)] out UInt32 pmdRemoveOn,
            [MarshalAs(UnmanagedType.U4)] out UInt32 pmdFire,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 11)] UInt32[] rmdOtherMethod,
            [In, MarshalAs(UnmanagedType.I4)] Int32 cMax,
            [MarshalAs(UnmanagedType.I4)] out Int32 pcOtherMethod
            );

        [PreserveSig]
        UInt32 GetFieldMarshal(
            [In, MarshalAs(UnmanagedType.U4)] UInt32 tk,
            out IntPtr ppvNativeType,
            [MarshalAs(UnmanagedType.I4)] out Int32 pcbNativeType
            );

        [PreserveSig]
        UInt32 GetFieldProps(
            [In, MarshalAs(UnmanagedType.U4)] UInt32 md,
            [MarshalAs(UnmanagedType.U4)] out UInt32 pClass,
            [MarshalAs(UnmanagedType.LPWStr)] StringBuilder szField,
            [In, MarshalAs(UnmanagedType.I4)] Int32 cchField,
            [MarshalAs(UnmanagedType.I4)] out Int32 pchField,
            [MarshalAs(UnmanagedType.U4)] out EventAttributes pdwAttr,
            out IntPtr ppvSigBlob,
            [MarshalAs(UnmanagedType.I4)] out Int32 pcbSigBlob,
            [MarshalAs(UnmanagedType.U4)] out UInt32 pdwCPlusTypeFlag,
            out IntPtr ppValue,
            [MarshalAs(UnmanagedType.I4)] out Int32 pcchValue
            );

        [PreserveSig]
        UInt32 GetInterfaceImplProps(
            [In, MarshalAs(UnmanagedType.U4)] UInt32 iiImpl,
            [MarshalAs(UnmanagedType.U4)] out UInt32 pClass,
            [MarshalAs(UnmanagedType.U4)] out UInt32 ptkIface
            );

        [PreserveSig]
        UInt32 GetMemberProps(
            [In, MarshalAs(UnmanagedType.U4)] UInt32 mb,
            [MarshalAs(UnmanagedType.U4)] out UInt32 pClass,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] char[] szMember,
            [In, MarshalAs(UnmanagedType.I4)] Int32 cchMember,
            [MarshalAs(UnmanagedType.I4)] out Int32 pchMember,
            [MarshalAs(UnmanagedType.U4)] out UInt32 pdwAttr,
            out IntPtr ppvSigBlob,
            [MarshalAs(UnmanagedType.I4)] out Int32 pcbSigBlob,
            [MarshalAs(UnmanagedType.U4)] out UInt32 pulCodeRVA,
            [MarshalAs(UnmanagedType.U4)] out UInt32 pdwImplFlags,
            [MarshalAs(UnmanagedType.U4)] out UInt32 pdwCPlusTypeFlag,
            out IntPtr ppValue,
            [MarshalAs(UnmanagedType.I4)] out Int32 pcchValue
            );

        [PreserveSig]
        UInt32 GetMemberRefProps(
            [In, MarshalAs(UnmanagedType.U4)] UInt32 mr,
            [MarshalAs(UnmanagedType.U4)] out UInt32 ptk,
            [MarshalAs(UnmanagedType.LPWStr)] StringBuilder szMember,
            [In, MarshalAs(UnmanagedType.I4)] Int32 cchMember,
            [MarshalAs(UnmanagedType.I4)] out Int32 pchMember,
            out IntPtr ppvSigBlob,
            [MarshalAs(UnmanagedType.I4)] out Int32 pbSigBlob
            );

        [PreserveSig]
        UInt32 GetMethodProps(
            [In, MarshalAs(UnmanagedType.U4)] UInt32 mb,
            [MarshalAs(UnmanagedType.U4)] out UInt32 pClass,
            [MarshalAs(UnmanagedType.LPWStr)] StringBuilder szMethod,
            [In, MarshalAs(UnmanagedType.I4)] Int32 cchMethod,
            [MarshalAs(UnmanagedType.I4)] out Int32 pchMethod,
            [MarshalAs(UnmanagedType.U4)] out MethodAttributes pdwAttr,
            out IntPtr ppvSigBlob,
            [MarshalAs(UnmanagedType.I4)] out Int32 pcbSigBlob,
            [MarshalAs(UnmanagedType.U4)] out UInt32 pulCodeRVA,
            [MarshalAs(UnmanagedType.U4)] out UInt32 pdwImplFlags
            );

        [PreserveSig]
        UInt32 GetMethodSemantics(
            [In, MarshalAs(UnmanagedType.U4)] UInt32 mb,
            [In, MarshalAs(UnmanagedType.U4)] UInt32 tkEventProp,
            [MarshalAs(UnmanagedType.U4)] out COR_METHOD_SEMANTICS_ATTR pdwSemanticsFlags
            );

        [PreserveSig]
        UInt32 GetModuleFromScope(
            [MarshalAs(UnmanagedType.U4)] out UInt32 pmd
            );

        [PreserveSig]
        UInt32 GetModuleRefProps(
            [In, MarshalAs(UnmanagedType.U4)] UInt32 mur,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] char[] szName,
            [In, MarshalAs(UnmanagedType.I4)] Int32 cchName,
            [MarshalAs(UnmanagedType.I4)] out Int32 pchName
            );

        [PreserveSig]
        UInt32 GetNameFromToken(
            [In, MarshalAs(UnmanagedType.U4)] UInt32 tk,
            out IntPtr pszUtf8NamePtr
            );

        [PreserveSig]
        UInt32 GetNativeCallConvFromSig(
            IntPtr pvSig,
            [In, MarshalAs(UnmanagedType.I4)] Int32 cbSig,
            [MarshalAs(UnmanagedType.U4)] out CallingConvention pCallConv
            );

        [PreserveSig]
        UInt32 GetNestedClassProps(
            [In, MarshalAs(UnmanagedType.U4)] UInt32 tdNestedClass,
            [MarshalAs(UnmanagedType.U4)] out UInt32 ptdEnclosingClass
            );

        [PreserveSig]
        UInt32 GetParamForMethodIndex(
            [In, MarshalAs(UnmanagedType.U4)] UInt32 md,
            [In, MarshalAs(UnmanagedType.I4)] Int32 ulParamSeq,
            [MarshalAs(UnmanagedType.U4)] out UInt32 ppd
            );

        [PreserveSig]
        UInt32 GetParamProps(
            [In, MarshalAs(UnmanagedType.U4)] UInt32 tk,
            [MarshalAs(UnmanagedType.U4)] out  UInt32 pmd,
            [MarshalAs(UnmanagedType.U4)] out  UInt32 pulSequence,
            [MarshalAs(UnmanagedType.LPWStr)] StringBuilder szName,
            [In, MarshalAs(UnmanagedType.I4)] Int32 cchName,
            [MarshalAs(UnmanagedType.I4)] out Int32 pchName,
            [MarshalAs(UnmanagedType.U4)] out UInt32 pdwAttr,
            [MarshalAs(UnmanagedType.U4)] out UInt32 pdwCPlusTypeFlag,
            out IntPtr ppValue,
            [MarshalAs(UnmanagedType.I4)] out Int32 pcchValue
            );

        [PreserveSig]
        UInt32 GetPermissionSetProps(
            [In, MarshalAs(UnmanagedType.U4)] UInt32 pm,
            [MarshalAs(UnmanagedType.U4)] out SecurityAction pdwAction,
            out IntPtr ppvPermission,
            [MarshalAs(UnmanagedType.I4)] out Int32 pcbPermission
            );

        [PreserveSig]
        UInt32 GetPinvokeMap(
            [In, MarshalAs(UnmanagedType.U4)] UInt32 tk,
            [MarshalAs(UnmanagedType.U4)] out COR_PINVOKE_MAP pdwMappingFlags,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] char[] szImportName,
            [In, MarshalAs(UnmanagedType.I4)] Int32 cchImportName,
            [MarshalAs(UnmanagedType.I4)] out Int32 pchImportName,
            [MarshalAs(UnmanagedType.U4)] out UInt32 pmrImportDLL
            );

        [PreserveSig]
        UInt32 GetPropertyProps(
            [In, MarshalAs(UnmanagedType.U4)] UInt32 prop,
            [MarshalAs(UnmanagedType.U4)] out UInt32 pClass,
            [MarshalAs(UnmanagedType.LPWStr)] StringBuilder szProperty,
            [In, MarshalAs(UnmanagedType.I4)] Int32 cchProperty,
            [MarshalAs(UnmanagedType.I4)] out Int32 pchProperty,
            [MarshalAs(UnmanagedType.U4)] out PropertyAttributes pdwPropFlags,
            out IntPtr ppvSigBlob,
            [MarshalAs(UnmanagedType.I4)] out Int32 pcbSigBlob,
            [MarshalAs(UnmanagedType.U4)] out UInt32 pdwCPlusTypeFlag,
            out IntPtr ppDefaultValue,
            [MarshalAs(UnmanagedType.I4)] out Int32 pcchDefaultValue,
            [MarshalAs(UnmanagedType.U4)] out UInt32 pmdSetter,
            [MarshalAs(UnmanagedType.U4)] out UInt32 pmdGetter,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 14)] out UInt32[] rmdOtherMethod,
            [In, MarshalAs(UnmanagedType.I4)] Int32 cMax,
            [MarshalAs(UnmanagedType.I4)] out Int32 pcOtherMethod
            );

        [PreserveSig]
        UInt32 GetRVA(
            [In, MarshalAs(UnmanagedType.U4)] UInt32 tk,
            [MarshalAs(UnmanagedType.U4)] out UInt32 pulCodeRVA,
            [MarshalAs(UnmanagedType.U4)] out UInt32 pdwImplFlags
            );

        [PreserveSig]
        UInt32 GetScopeProps(
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] char[] szName,
            [In, MarshalAs(UnmanagedType.I4)] Int32 cchName,
            [MarshalAs(UnmanagedType.I4)] out Int32 pchName,
            [MarshalAs(UnmanagedType.Struct)] ref Guid mvid
            );

        [PreserveSig]
        UInt32 GetSigFromToken(
            [In, MarshalAs(UnmanagedType.U4)] UInt32 mdSig,
            out IntPtr ppvSig,
            [MarshalAs(UnmanagedType.I4)] out Int32 pcbSig
            );

        [PreserveSig]
        UInt32 GetTypeDefProps(
            [In, MarshalAs(UnmanagedType.U4)] UInt32 td,
            [MarshalAs(UnmanagedType.LPWStr)] StringBuilder szTypeDef,
            [In, MarshalAs(UnmanagedType.I4)] Int32 cchTypeDef,
            [MarshalAs(UnmanagedType.I4)] out Int32 pchTypeDef,
            [MarshalAs(UnmanagedType.U4)] out TypeAttributes pdwTypeDefFlags,
            [MarshalAs(UnmanagedType.U4)] out UInt32 ptkExtends
            );

        [PreserveSig]
        UInt32 GetTypeRefProps(
            [In, MarshalAs(UnmanagedType.U4)] UInt32 tr,
            [MarshalAs(UnmanagedType.U4)] out UInt32 ptkResolutionScope,
            [MarshalAs(UnmanagedType.LPWStr)] StringBuilder szName,
            [In, MarshalAs(UnmanagedType.I4)] Int32 cchName,
            [MarshalAs(UnmanagedType.I4)] out Int32 pchName
            );

        [PreserveSig]
        UInt32 GetTypeSpecFromToken(
            [In, MarshalAs(UnmanagedType.U4)] UInt32 typespec,
            out IntPtr ppvSig,
            [MarshalAs(UnmanagedType.I4)] out Int32 pcbSig
            );

        [PreserveSig]
        UInt32 GetUserString(
            [In, MarshalAs(UnmanagedType.U4)] UInt32 stk,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] char[] szString,
            [In, MarshalAs(UnmanagedType.I4)] Int32 cchString,
            [MarshalAs(UnmanagedType.I4)] out Int32 pchString
            );

        [PreserveSig]
        UInt32 IsGlobal(
            [In, MarshalAs(UnmanagedType.U4)] UInt32 pd,
            [MarshalAs(UnmanagedType.U4)] out UInt32 pbGlobal
            );

        [PreserveSig]
        UInt32 IsValidToken(
            [In, MarshalAs(UnmanagedType.U4)] UInt32 tk
            );

        [PreserveSig]
        UInt32 ResetEnum(
            [In] IntPtr hEnum,
            [MarshalAs(UnmanagedType.U4)] UInt32 ulPos
            );

        [PreserveSig]
        UInt32 ResolveTypeRef(
            [In, MarshalAs(UnmanagedType.U4)] UInt32 tr,
            [MarshalAs(UnmanagedType.Struct)] ref Guid riid,
            out IntPtr ppIScope,
            [MarshalAs(UnmanagedType.U4)] out UInt32 ptd
            );
    }
}
