using System;

namespace DataDynamics.PageFX.FLI.ABC
{
    #region enum AbcMethodFlags
    [Flags]
    public enum AbcMethodFlags
    {
        None = 0,

        /// <summary>
        /// Suggests to the run-time that an “arguments” object (as specified by the
        /// ActionScript 3.0 Language Reference) be created. Must not be used
        /// together with NeedRest. See Chapter 3.
        /// </summary>
        NeedArguments = 0x01,

        /// <summary>
        /// Must be set if this method uses the newactivation opcode.
        /// </summary>
        NeedActivation = 0x02,

        /// <summary>
        /// This flag creates an ActionScript 3.0 rest arguments array. Must not be
        /// used with NeedArguments. See Chapter 3.
        /// </summary>
        NeedRest = 0x04,

        /// <summary>
        /// Must be set if this method has optional parameters and the options
        /// field is present in this method_info structure.
        /// </summary>
        HasOptional = 0x08,

        /// <summary>
        /// 
        /// </summary>
        IgnoreRest = 0x10,

        /// <summary>
        /// Specifies whether method is native (implementation provided by AVM+)
        /// </summary>
        Native = 0x20,

        /// <summary>
        /// Must be set if this method uses the dxns or dxnslate opcodes.
        /// </summary>
        SetDxns = 0x40,

        /// <summary>
        /// Must be set when the param_names field is present in this method_info structure.
        /// </summary>
        HasParamNames = 0x80,
    }
    #endregion

    #region enum AbcConstKind
    /// <summary>
    /// Enumerates constant types supported by AVM+
    /// </summary>
    public enum AbcConstKind : byte
    {
        /// <summary>
        /// undefined
        /// </summary>
        Undefined = 0x00,

        /// <summary>
        /// UTF8 encoded string
        /// </summary>
        String = 0x01,

        /// <summary>
        /// Decimal constant
        /// </summary>
        Decimal = 0x02, // the number 2 was unused in previous version

        /// <summary>
        /// signed integer
        /// </summary>
        Int = 0x03,

        /// <summary>
        /// unsigned integer
        /// </summary>
        UInt = 0x04,

        /// <summary>
        /// non-shared namespace
        /// </summary>
        PrivateNamespace = 0x05,

        /// <summary>
        /// double
        /// </summary>
        Double = 0x06,

        /// <summary>
        /// 
        /// </summary>
        QName = 0x07,

        PublicNamespace = 0x08,

        Multiname = 0x09, // o.name, ct nsset, ct name

        False = 0x0A,

        True = 0x0B,

        Null = 0x0C,

        QNameA = 0x0D, // o.@ns::name, ct ns, ct attr-name
        MultinameA = 0x0E, // o.@name, ct attr-name
        RTQName = 0x0F, // o.ns::name, rt ns, ct name
        RTQNameA = 0x10, // o.@ns::name, rt ns, ct attr-name
        RTQNameL = 0x11, // o.ns::[name], rt ns, rt name
        RTQNameLA = 0x12, // o.@ns::[name], rt ns, rt attr-name

        NamespaceSet = 0x15,
        PackageNamespace = 0x16,
        InternalNamespace = 0x17,
        ProtectedNamespace = 0x18,
        ExplicitNamespace = 0x19,
        StaticProtectedNamespace = 0x1A,
        MultinameL = 0x1B,
        MultinameLA = 0x1C,
        TypeName = 0x1D,    // used for parametrized types 
    }
    #endregion

    #region enum AbcMethodSemantics
    [Flags]
    public enum AbcMethodSemantics
    {
        Default = 0,
        Static = 1,
        Virtual = 2,
        Override = 4,
        VirtualOverride = Virtual | Override,
    }
    #endregion

    [Flags]
    public enum AbcTraitOwner
    {
        None,
        Instance = 1,
        Script = 2,
        MethodBody = 4,
        All = Instance | Script | MethodBody,
    }
}