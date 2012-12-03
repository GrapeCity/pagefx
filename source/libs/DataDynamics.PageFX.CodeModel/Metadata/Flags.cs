using System;

namespace DataDynamics.PageFX.Common.Metadata
{
    #region enum AssemblyFlags
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
        /// Reserved  
        /// (a conforming implementation of the CLI may ignore this setting on read; 
        /// some implementations might use this bit to indicate that a 
        /// CIL-to-native-code compiler should generate CIL-to-native code map)
        /// </summary>
        EnableJITcompileTracking = 0x8000,

        /// <summary>
        /// Reserved (a conforming implementation of the CLI may ignore this setting on read; 
        /// some implementations might use this bit to indicate that a 
        /// CIL-to-native-code compiler should not generate optimized code)
        /// </summary>
        DisableJITcompileOptimizer = 0x4000
    }
    #endregion

    #region enum EventAttributes
    /// <summary>
    /// Event record metadata attributes
    /// </summary>
    [Flags]
    public enum EventAttributes
    {
        /// <summary>
        /// No additional info
        /// </summary>
        None = 0x0000,

        /// <summary>
        /// event is special.  Name describes how.
        /// </summary>
        SpecialName = 0x0200,

        /// <summary>
        /// Reserved flags for Runtime use only (left here for compatibility)
        /// </summary>
        ReservedMask = 0x0400,

        /// <summary>
        /// Runtime(metadata internal APIs) should check name encoding.
        /// </summary>
        RTSpecialName = 0x0400,
    }
    #endregion

    #region enum FieldAttributes
    /// <summary>
    /// Filed record metadata attributes
    /// </summary>
    [Flags]
    public enum FieldAttributes
    {
        /// <summary>
        /// member access mask - Use this mask to retrieve accessibility information.
        /// </summary>
        FieldAccessMask = 0x0007,

        /// <summary>
        /// Member not referenceable.
        /// </summary>
        PrivateScope = 0x0000, //name in specification is CompilerControlled

        /// <summary>
        /// Accessible only by the parent type.
        /// </summary>
        Private = 0x0001,

        /// <summary>
        /// Accessible by sub-types only in this Assembly.
        /// </summary>
        FamANDAssem = 0x0002,

        /// <summary>
        /// Accessibly by anyone in the Assembly.
        /// </summary>
        Assembly = 0x0003,

        /// <summary>
        /// Accessible only by type and sub-types.    
        /// </summary>
        Family = 0x0004,

        /// <summary>
        /// Accessibly by sub-types anywhere, plus anyone in assembly.
        /// </summary>
        FamORAssem = 0x0005,

        /// <summary>
        /// Accessibly by anyone who has visibility to this scope.    
        /// </summary>
        Public = 0x0006,

        /// <summary>
        /// Defined on type, else per instance.
        /// </summary>
        Static = 0x0010,

        /// <summary>
        /// Field may only be initialized, not written to after init.
        /// </summary>
        InitOnly = 0x0020,

        /// <summary>
        /// Value is compile time constant.
        /// </summary>
        Literal = 0x0040,

        /// <summary>
        /// Field does not have to be serialized when type is remoted.
        /// </summary>
        NotSerialized = 0x0080,

        /// <summary>
        /// field is special.  Name describes how.
        /// </summary>
        SpecialName = 0x0200,

        /// <summary>
        /// Implementation is forwarded through pinvoke.
        /// </summary>
        PinvokeImpl = 0x2000,

        /// <summary>
        /// Reserved flags for runtime use only. 
        /// </summary>
        ReservedMask = 0x9500,

        /// <summary>
        /// Runtime(metadata internal APIs) should check name encoding.
        /// </summary>
        RTSpecialName = 0x0400,

        /// <summary>
        /// Field has marshalling information.
        /// </summary>
        HasFieldMarshal = 0x1000,

        /// <summary>
        /// Field has default.
        /// </summary>
        HasDefault = 0x8000,

        /// <summary>
        /// Field has RVA.
        /// </summary>
        HasFieldRVA = 0x0100,
    }
    #endregion

    #region enum GenericParamAttributes
    /// <summary>
    /// Attributes for Generic Parameters
    /// </summary>
    [Flags]
    public enum GenericParamAttributes
    {
        /// <summary>
        /// The generic parameter is non-variant
        /// </summary>
        None = 0,

        /// <summary>
        /// The generic parameter is covariant
        /// </summary>
        Covariant = 1,

        /// <summary>
        /// The generic parameter is contravariant
        /// </summary>
        Contravariant = 2,

        /// <summary>
        /// Mask for variance type of flags
        /// </summary>
        VarianceMask = 3,

        /// <summary>
        /// Parameters must be referencied type
        /// </summary>
        ReferenceTypeConstraint = 4,

        /// <summary>
        /// Parameter must be not nullable
        /// </summary>
        NotNullableValueTypeConstraint = 8,

        /// <summary>
        /// Parameter must have default constructors.
        /// </summary>
        DefaultConstructorConstraint = 0x10,

        /// <summary>
        /// Mask for special part
        /// </summary>
        SpecialConstraintMask = 0x1c,
    }
    #endregion

    #region enum ImplementationAttributes
    /// <summary>
    /// PinvokeMap attr bits, used by DefinePinvokeMap.
    /// </summary>
    [Flags]
    public enum ImplementationAttributes : ushort
    {
        /// <summary>
        /// Pinvoke is to use the member name as specified.
        /// </summary>
        NoMangle = 0x0001,

        /// <summary>
        /// Use this mask to retrieve the CharSet information.
        /// </summary>
        CharSetMask = 0x0006,

        /// <summary>
        /// Not specified
        /// </summary>
        CharSetNotSpec = 0x0000,

        /// <summary>
        /// Single byte  - ANSI chars
        /// </summary>
        CharSetAnsi = 0x0002,

        /// <summary>
        /// Unicode - UTF 16 chars
        /// </summary>
        CharSetUnicode = 0x0004,

        /// <summary>
        /// Platform depended
        /// </summary>
        CharSetAuto = 0x0006,

        /// <summary>
        /// 
        /// </summary>
        BestFitUseAssem = 0x0000,

        /// <summary>
        /// 
        /// </summary>
        BestFitEnabled = 0x0010,

        /// <summary>
        /// 
        /// </summary>
        BestFitDisabled = 0x0020,

        /// <summary>
        /// 
        /// </summary>
        BestFitMask = 0x0030,

        /// <summary>
        /// 
        /// </summary>
        ThrowOnUnmappableCharUseAssem = 0x0000,

        /// <summary>
        /// 
        /// </summary>
        ThrowOnUnmappableCharEnabled = 0x1000,

        /// <summary>
        /// 
        /// </summary>
        ThrowOnUnmappableCharDisabled = 0x2000,

        /// <summary>
        /// 
        /// </summary>
        ThrowOnUnmappableCharMask = 0x3000,

        /// <summary>
        /// Information about target function. Not relevant for fields.
        /// </summary>
        SupportsLastError = 0x0040,

        /// <summary>
        /// None of the calling convention flags is relevant for fields. 
        /// </summary>
        CallConvMask = 0x0700,

        /// <summary>
        /// Pinvoke will use native callconv appropriate to target windows platform.
        /// </summary>
        CallConvWinapi = 0x0100,

        /// <summary>
        /// Cdecl calling conventions
        /// </summary>
        CallConvCdecl = 0x0200,

        /// <summary>
        /// Stdcall  calling conventions
        /// </summary>
        CallConvStdcall = 0x0300,

        /// <summary>
        /// In M9, pinvoke will raise exception.
        /// </summary>
        CallConvThiscall = 0x0400,

        /// <summary>
        /// _fastcall calling conventions
        /// </summary>
        CallConvFastcall = 0x0500,
    }
    #endregion

    #region enum MethodAttributes
    /// <summary>
    /// Method recoed metadata attributes
    /// </summary>
    [Flags]
    public enum MethodAttributes
    {
        /// <summary>
        /// member access mask - Use this mask to retrieve accessibility information.
        /// </summary>
        MemberAccessMask = 0x0007,

        /// <summary>
        /// Indicates that the member cannot be referenced.
        /// </summary>
        PrivateScope = 0x0000,

        /// <summary>
        /// Indicates that the method is accessible only to the current class.  
        /// </summary>
        Private = 0x0001,

        /// <summary>
        /// Indicates that the method is accessible to members of this type and its derived types that are in this assembly only.
        /// </summary>
        FamANDAssem = 0x0002,

        /// <summary>
        /// Indicates that the method is accessible to any class of this assembly.
        /// </summary>
        Assembly = 0x0003,

        /// <summary>
        /// Indicates that the method is accessible only to members of this class and its derived classes.    
        /// </summary>
        Family = 0x0004,

        /// <summary>
        /// Indicates that the method is accessible to derived classes anywhere, as well as to any class in the assembly.
        /// </summary>
        FamORAssem = 0x0005,

        /// <summary>
        /// Indicates that the method is accessible to any object for which this object is in scope.   
        /// </summary>
        Public = 0x0006,

        /// <summary>
        /// Indicates that the method is defined on the type; otherwise, it is defined per instance.
        /// </summary>
        Static = 0x0010,

        /// <summary>
        /// Indicates that the method cannot be overridden.
        /// </summary>
        Final = 0x0020,

        /// <summary>
        /// Indicates that the method is virtual.
        /// </summary>
        Virtual = 0x0040,

        /// <summary>
        /// Indicates that the method hides by name and signature; otherwise, by name only.
        /// </summary>
        HideBySig = 0x0080,

        /// <summary>
        /// vtable layout mask - Use this mask to retrieve vtable attributes.
        /// </summary>
        VtableLayoutMask = 0x0100,

        /// <summary>
        /// Indicates that the method will reuse an existing slot in the vtable. This is the default behavior.
        /// </summary>
        ReuseSlot = 0x0000,

        /// <summary>
        /// Indicates that the method always gets a new slot in the vtable.
        /// </summary>
        NewSlot = 0x0100,

        /// <summary>
        /// Indicates that the class does not provide an implementation of this method.
        /// </summary>
        Abstract = 0x0400,

        /// <summary>
        /// Indicates that the method is special. The name describes how this method is special.
        /// </summary>
        SpecialName = 0x0800,

        /// <summary>
        /// Indicates that the method implementation is forwarded through PInvoke (Platform Invocation Services).
        /// </summary>
        PinvokeImpl = 0x2000,

        /// <summary>
        /// Indicates that the managed method is exported by thunk to unmanaged code. 
        /// </summary>
        UnmanagedExport = 0x0008,

        /// <summary>
        /// Indicates that the common language runtime checks the name encoding.
        /// </summary>
        RTSpecialName = 0x1000,

        /// <summary>
        /// Reserved flags for runtime use only. 
        /// </summary>
        ReservedMask = 0xd000,

        /// <summary>
        /// Indicates that the method has security associated with it. Reserved flag for runtime use only.
        /// </summary>
        HasSecurity = 0x4000,

        /// <summary>
        /// Indicates that the method calls another method containing security code. Reserved flag for runtime use only.
        /// </summary>
        RequireSecObject = 0x8000,
    }
    #endregion

    #region enum MethodImplAttributes
    /// <summary>
    /// Method implementation flags
    /// </summary>
    [Flags]
    public enum MethodImplAttributes
    {
        /// <summary>
        /// Flags about code type.   
        /// </summary>
        CodeTypeMask = 0x0003,

        /// <summary>
        /// Specifies that the method implementation is in Common Intermediate Language (CIL).
        /// </summary>
        IL = 0x0000,

        /// <summary>
        /// Specifies that the method is implemented in native code.
        /// </summary>
        Native = 0x0001,

        /// <summary>
        /// Specifies that the method implementation is in optimized intermediate language (OPTIL).
        /// </summary>
        OPTIL = 0x0002,

        /// <summary>
        /// Specifies that the method implementation is provided by the runtime.
        /// </summary>
        Runtime = 0x0003,

        /// <summary>
        /// Flags specifying whether the code is managed or unmanaged.
        /// </summary>
        ManagedMask = 0x0004,

        /// <summary>
        /// Specifies that the method is implemented in unmanaged code.
        /// </summary>
        Unmanaged = 0x0004,

        /// <summary>
        /// Method impl is managed.
        /// </summary>
        Managed = 0x0000,

        /// <summary>
        /// Specifies that the method is declared, but its implementation is provided elsewhere.
        /// </summary>
        ForwardRef = 0x0010,

        /// <summary>
        /// Specifies that the method signature is exported exactly as declared.
        /// </summary>
        PreserveSig = 0x0080,

        /// <summary>
        /// Specifies an internal call. An internal call is a call to a method implemented within the common language runtime itself.
        /// </summary>
        InternalCall = 0x1000,

        /// <summary>
        /// Specifies that the method can be executed by only one thread at a time.
        /// Static methods lock on the type, while instance methods lock on the instance.
        /// Only one thread can execute in any of the instance functions and only one thread 
        /// can execute in any of a class's static functions.
        /// </summary>
        Synchronized = 0x0020,

        /// <summary>
        /// Specifies that the method can not be inlined.
        /// </summary>
        NoInlining = 0x0008,
    }
    #endregion

    #region enum MethodSemanticsAttributes
    /// <summary>
    /// Flags for MethodSemantics [MethodSemanticsAttributes]
    /// </summary>
    [Flags]
    public enum MethodSemanticsAttributes : short
    {
        None = 0,

        /// <summary>
        /// Setter for property  
        /// </summary>
        Setter = 0x0001,

        /// <summary>
        /// Getter for property  
        /// </summary>
        Getter = 0x0002,

        /// <summary>
        /// Other method for property or event   
        /// </summary>
        Other = 0x0004,

        /// <summary>
        /// AddOn method for event   
        /// </summary>
        AddOn = 0x0008,

        /// <summary>
        /// RemoveOn method for event    
        /// </summary>
        RemoveOn = 0x0010,

        /// <summary>
        /// Fire method for event   
        /// </summary>
        Fire = 0x0020,
    }
    #endregion

    #region enum ParamAttributes
    /// <summary>
    /// Method parameter metadata attribute
    /// </summary>
    [Flags]
    public enum ParamAttributes
    {
        /// <summary>
        /// no flag is specified
        /// </summary>
        None = 0x0000,

        /// <summary>
        /// Param is [In]    
        /// </summary>
        In = 0x0001,

        /// <summary>
        /// Param is [Out]   
        /// </summary>
        Out = 0x0002,

        /// <summary>
        /// Param is [lcid]  
        /// </summary>
        Lcid = 0x0004,

        /// <summary>
        /// Param is [Retval]    
        /// </summary>
        Retval = 0x0008,

        /// <summary>
        /// Param is optional 
        /// </summary>
        Optional = 0x0010,

        /// <summary>
        /// Reserved flags for Runtime use only. 
        /// </summary>
        ReservedMask = 0xf000,

        /// <summary>
        /// Param has default value.
        /// </summary>
        HasDefault = 0x1000,

        /// <summary>
        /// Param has FieldMarshal.
        /// </summary>
        HasFieldMarshal = 0x2000,

        /// <summary>
        /// reserved bit
        /// </summary>
        Reserved3 = 0x4000,

        /// <summary>
        /// reserved bit 
        /// </summary>
        Reserved4 = 0x8000
    }
    #endregion

    #region enum PropertyAttributes
    /// <summary>
    /// Property record metadata attributes
    /// </summary>
    [Flags]
    public enum PropertyAttributes
    {
        /// <summary>
        /// No special attributes
        /// </summary>
        None = 0x0000,

        /// <summary>
        /// property is special.  Name describes how.
        /// </summary>
        SpecialName = 0x0200,

        /// <summary>
        /// Reserved flags for Runtime use only. 
        /// </summary>
        ReservedMask = 0xf400,

        /// <summary>
        /// Runtime(metadata internal APIs) should check name encoding.
        /// </summary>
        RTSpecialName = 0x0400,

        /// <summary>
        /// Property has default 
        /// </summary>
        HasDefault = 0x1000,

        /// <summary>
        /// reserved bit
        /// </summary>
        Reserved2 = 0x2000,

        /// <summary>
        /// reserved bit 
        /// </summary>
        Reserved3 = 0x4000,

        /// <summary>
        /// reserved bit 
        /// </summary>
        Reserved4 = 0x8000
    }
    #endregion

    #region enum TypeAttributes
    /// <summary>
    /// Type record metadata attributes
    /// </summary>
    [Flags]
    public enum TypeAttributes
    {
        #region Visibility attributes
        /// <summary>
        /// Type visibility mask
        /// </summary>
        VisibilityMask = 0x00000007,

        /// <summary>
        /// Class is not public scope.
        /// </summary>
        NotPublic = 0x00000000,

        /// <summary>
        /// Class is public scope.
        /// </summary>
        Public = 0x00000001,

        /// <summary>
        /// Class is nested with public visibility.
        /// </summary>
        NestedPublic = 0x00000002,

        /// <summary>
        /// Class is nested with private visibility.
        /// </summary>
        NestedPrivate = 0x00000003,

        /// <summary>
        /// Class is nested with family visibility.
        /// </summary>
        NestedFamily = 0x00000004,

        /// <summary>
        /// Class is nested with assembly visibility.
        /// </summary>
        NestedAssembly = 0x00000005,

        /// <summary>
        /// Class is nested with family and assembly visibility.
        /// </summary>
        NestedFamANDAssem = 0x00000006,

        /// <summary>
        /// Class is nested with family or assembly visibility.
        /// </summary>
        NestedFamORAssem = 0x00000007,
        #endregion

        #region Class layout attributes
        /// <summary>
        /// Use this mask to retrieve class layout informaiton
        /// </summary>
        LayoutMask = 0x00000018,

        /// <summary>
        /// Class fields are auto-laid out
        /// </summary>
        AutoLayout = 0x00000000,

        /// <summary>
        /// Class fields are laid out sequentially
        /// </summary>
        SequentialLayout = 0x00000008,

        /// <summary>
        /// Layout is supplied explicitly
        /// </summary>
        ExplicitLayout = 0x00000010,
        #endregion

        #region Class semantics attributes
        /// <summary>
        /// Use this mask to distinguish a type declaration as a Class, ValueType or Interface
        /// </summary>
        ClassSemanticsMask = 0x00000020,

        /// <summary>
        /// Type is a class.
        /// </summary>
        Class = 0x00000000,

        /// <summary>
        /// Type is an interface.
        /// </summary>
        Interface = 0x00000020,

        //Special semantics in addition to class semantics
        /// <summary>
        /// Class is abstract
        /// </summary>
        Abstract = 0x00000080,

        /// <summary>
        /// Class is concrete and may not be extended
        /// </summary>
        Sealed = 0x00000100,

        /// <summary>
        /// Class name is special.  Name describes how.
        /// </summary>
        SpecialName = 0x00000400,
        #endregion

        #region Implementation attributes
        /// <summary>
        /// Class / interface is imported
        /// </summary>
        Import = 0x00001000,

        /// <summary>
        /// The class is Serializable.
        /// </summary>
        Serializable = 0x00002000,
        #endregion

        #region String formatting Attributes
        /// <summary>
        /// Use StringFormatMask to retrieve string information for native interop
        /// </summary>
        StringFormatMask = 0x00030000,

        /// <summary>
        /// LPTSTR is interpreted as ANSI in this class
        /// </summary>
        AnsiClass = 0x00000000,

        /// <summary>
        /// LPTSTR is interpreted as UNICODE
        /// </summary>
        UnicodeClass = 0x00010000,

        /// <summary>
        /// LPTSTR is interpreted automatically
        /// </summary>
        AutoClass = 0x00020000,

        /// <summary>
        /// A non-standard encoding specified by CustomStringFormatMask
        /// </summary>
        CustomFormatClass = 0x00030000,

        /// <summary>
        /// Use this mask to retrieve non-standard encoding information for native interop. 
        /// The meaning of the values of these 2 bits is unspecified.
        /// </summary>
        CustomStringFormatMask = 0x00C00000,
        #endregion

        #region Class Initialization Attributes
        /// <summary>
        /// Initialize the class any time before first static field access.
        /// </summary>
        BeforeFieldInit = 0x00100000,
        #endregion

        #region Additional Flags
        /// <summary>
        /// Flags reserved for runtime use.
        /// </summary>
        ReservedMask = 0x00040800,

        /// <summary>
        /// Runtime should check name encoding.
        /// </summary>
        RTSpecialName = 0x00000800,

        /// <summary>
        /// Class has security associate with it.
        /// </summary>
        HasSecurity = 0x00040000,
        #endregion
    }
    #endregion

    #region enum FileFlags
    /// <summary>
    /// Flags for files
    /// </summary>
    [Flags]
    public enum FileFlags
    {
        /// <summary>
        /// This is not a resource file
        /// </summary>
        ContainsMetadata = 0x0000,

        /// <summary>
        /// This is a resource file or other non-metadata-containing file
        /// </summary>
        ContainsNoMetadata = 0x0001,
    }
    #endregion

    #region enum PInvokeAttributes
    /// <summary>
    /// Flags for ImplMap [PInvokeAttributes]
    /// </summary>
    [Flags]
    public enum PInvokeAttributes
    {
        /// <summary>
        /// PInvoke is to use the member name as specified
        /// </summary>
        NoMangle = 0x0001,

        /// <summary>
        /// This is a resource file or other non-metadata-containing file.
        /// </summary>
        CharSetMask = 0x0006,
        CharSetNotSpec = 0x0000,
        CharSetAnsi = 0x0002,
        CharSetUnicode = 0x0004,
        CharSetAuto = 0x0006,

        /// <summary>
        /// Information about target function. Not relevant for fields
        /// </summary>
        SupportsLastError = 0x0040,

        CallConvMask = 0x0700,
        CallConvWinapi = 0x0100,
        CallConvCdecl = 0x0200,
        CallConvStdcall = 0x0300,
        CallConvThiscall = 0x0400,
        CallConvFastcall = 0x0500
    }
    #endregion

    #region enum ManifestResourceAttributes
    /// <summary>
    /// Flags for ManifestResource
    /// </summary>
    [Flags]
    public enum ManifestResourceAttributes
    {
        VisibilityMask = 0x0007,

        /// <summary>
        /// The Resource is exported from the Assembly
        /// </summary>
        Public = 0x0001,

        /// <summary>
        /// The Resource is private to the Assembly
        /// </summary>
        Private = 0x0002,
    }
    #endregion
}