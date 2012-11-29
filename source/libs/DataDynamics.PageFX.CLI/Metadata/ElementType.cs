namespace DataDynamics.PageFX.CLI.Metadata
{
	/// <summary>
    /// Element types used in signatures
    /// </summary>
    internal enum ElementType
    {
        /// <summary>
        /// Marks end of a list
        /// </summary>
        End = 0,

        /// <summary>
        /// This is a simple type (System.Void)
        /// </summary>
        Void = 1,

        /// <summary>
        /// This is a simple type  (System.Boolean)
        /// </summary>
        Boolean = 2,

        /// <summary>
        /// This is a simple type  (System.Char)
        /// </summary>
        Char = 3,

        /// <summary>
        /// This is a simple type  (System.SByte)
        /// </summary>
        Int8 = 4,

        /// <summary>
        /// This is a simple type  (System.Byte)
        /// </summary>
        UInt8 = 5,

        /// <summary>
        /// This is a simple type  (System.Int16)
        /// </summary>
        Int16 = 6,

        /// <summary>
        /// This is a simple type  (System.UInt16)
        /// </summary>
        UInt16 = 7,

        /// <summary>
        /// This is a simple type  (System.Int32)
        /// </summary>
        Int32 = 8,

        /// <summary>
        /// This is a simple type  (System.UInt32)
        /// </summary>
        UInt32 = 9,

        /// <summary>
        /// This is a simple type  (System.Int64)
        /// </summary>
        Int64 = 10,

        /// <summary>
        /// This is a simple type  (System.UInt64)
        /// </summary>
        UInt64 = 11,

        /// <summary>
        /// This is a simple type  (System.Single)
        /// </summary>
        Single = 12,

        /// <summary>
        /// This is a simple type  (System.Double)
        /// </summary>
        Double = 13,

        /// <summary>
        /// This is a simple type  (System.String)
        /// </summary>
        String = 14, //0x0E

        /// <summary>
        /// PTR Type
        /// </summary>
        Ptr = 15,

        /// <summary>
        /// BYREF Type
        /// </summary>
        ByRef = 16,

        /// <summary>
        /// VALUETYPE TypeDefOrRef
        /// </summary>
        ValueType = 17,

        /// <summary>
        /// CLASS TypeDefOrRef
        /// </summary>
        Class = 18,

        /// <summary>
        /// Generic parameter in a generic type definition, represented as number
        /// </summary>
        Var = 19,

        /// <summary>
        /// MDARRAY ArrayShape
        /// </summary>
        Array = 20,

        /// <summary>
        /// Generic type instantiation. Followed by type typearg-count type-1 ... type-n.
        /// </summary>
        GenericInstantiation = 21,

        /// <summary>
        /// This is a simple type (System.TypedReference)
        /// </summary>
        TypedReference = 22,

        /// <summary>
        /// System.IntPtr
        /// </summary>
        IntPtr = 24,

        /// <summary>
        /// System.UIntPtr
        /// </summary>
        UIntPtr = 25,

        /// <summary>
        /// FNPTR MethodSig
        /// </summary>
        MethodPtr = 27,

        /// <summary>
        /// Shortcut for System.Object
        /// </summary>
        Object = 28,

        /// <summary>
        /// Shortcut for single dimension zero lower bound array (SZARRAY Type)
        /// </summary>
        ArraySz = 29,

        /// <summary>
        /// Generic parameter in a generic method definition, represented as number
        /// </summary>
        MethodVar = 30,

        /// <summary>
        /// Required C modifier followed by TypeDefOrRef token
        /// </summary>
        RequiredModifier = 31,

        /// <summary>
        /// Optional C modifier followed by TypeDefOrRef token
        /// </summary>
        OptionalModifier = 32,

        /// <summary>
        /// INTERNAL TypeHandle
        /// </summary>
        Internal = 33,

        /// <summary>
        /// 
        /// </summary>
        ElementTypeModifier = 0x40,

        /// <summary>
        /// Sentinel for varargs
        /// </summary>
        Sentinel = 0x40 | 0x01,

        /// <summary>
        /// Denotes a local variable that points at a pinned object
        /// </summary>
        Pinned = 0x40 | 0x05,

        /// <summary>
        /// used only internally for R4 HFA types
        /// </summary>
        R4_HFA = 0x06 | 0x40,

        /// <summary>
        /// used only internally for R8 HFA types
        /// </summary>
        R8_HFA = 0x07 | 0x40,

        /// <summary>
        /// Indicates an argument of type System.Type.
        /// </summary>
        CustomArgsType = 0x50,

        /// <summary>
        /// Used in custom attributes to specify a boxed object
        /// </summary>
        CustomArgsBoxedObject = 0x51,

        /// <summary>
        /// Used in custom attributes to indicate a FIELD
        /// </summary>
        CustomArgsField = 0x53,

        /// <summary>
        /// Used in custom attributes to indicate a PROPERTY
        /// </summary>
        CustomArgsProperty = 0x54,

        /// <summary>
        /// Used in custom attributes to specify an enum
        /// </summary>
        CustomArgsEnum = 0x55
    }
}