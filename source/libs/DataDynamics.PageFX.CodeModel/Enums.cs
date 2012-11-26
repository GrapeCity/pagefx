using System;
using DataDynamics.PageFX.CodeModel.Syntax;

namespace DataDynamics.PageFX.CodeModel
{
    #region enum HashAlgorithmId
    public enum HashAlgorithmId
    {
        MD5 = 0x8003,
        None = 0,
        SHA1 = 0x8004
    }
    #endregion

    #region enum AssemblyType
    public enum AssemblyType
    {
        None,
        Console,
        Application,
        Library
    }
    #endregion

    #region enum ArgumentKind
    public enum ArgumentKind : byte
    {
        Fixed = 0,
        Field = 0x53,
        Property = 0x54,
    }
    #endregion

    #region enum CodeNodeType
    public enum CodeNodeType
    {
        Assembly,
        AssemblyReference,
        Modules,
        Module,
        Namespaces,
        Namespace,
        Attribute,
        Attributes,
        Type,
        Types,
        GenericParameter,
        GenericParameters,
        TypeMember,
        TypeMembers,
        Field,
        Fields,
        Property,
        Properties,
        Event,
        Events,
        Constructor,
        Constructors,
        Method,
        Methods,
        Parameter,
        Parameters,
        Argument,
        Arguments,
        Variable,
        Variables,
        Statement,
        Statements,
        Expression
    }
    #endregion

    #region enum TypeKind
    public enum TypeKind : byte
    {
        [CSharp("class")]
        [VB("Class")]
        [ActionScript("class")]
        Class,

        [CSharp("interface")]
        [VB("Interface")]
        [ActionScript("interface")]
        Interface,

        [CSharp("struct")]
        [VB("Struct")]
        [ActionScript("class")]
        Struct,

        [CSharp("enum")]
        [VB("Enumeration")]
        [ActionScript("class")]
        Enum,

        [CSharp("delegate")]
        Delegate,

        GenericParameter,

        Array,
        Reference,
        Pointer,

        [CSharp("struct")]
        [VB("Struct")]
        Primitive,
    }
    #endregion

    #region enum MemberType
    public enum MemberType
    {
        Field,
        Method,
        Constructor,
        Property,
        Event,
        Type,
    }
    #endregion

    #region enum ExceptionHandlerType
    public enum ExceptionHandlerType
    {
        Finally,
        Catch,
        Filter,
        Fault
    }
    #endregion

    #region enum GenericParameterVariance
    public enum GenericParameterVariance : byte 
    {
        NonVariant,
        Covariant,
        Contravariant
    }
    #endregion

    #region enum GenericParameterSpecialConstraints
    [Flags]
    public enum GenericParameterSpecialConstraints : byte 
    {
        None = 0,
        DefaultConstructor = 1,
        ReferenceType = 2,
        ValueType = 4,
    }
    #endregion

    #region enum MethodCallingConvention
    public enum MethodCallingConvention : byte 
    {
        Default,
        C,
        StandardCall,
        ThisCall,
        FastCall,
        VariableArguments
    }
    #endregion

    #region enum OrderDirection
    public enum OrderDirection : byte 
    {
        Ascending,
        Descending
    }
    #endregion

    #region enum Visibility
    public enum Visibility : byte 
    {
        [CSharp("private")]
        [ActionScript("private")]
        PrivateScope,

        [CSharp("private")]
        [VB("Private")]
        [ActionScript("private")]
        NestedPrivate,

        [CSharp("protected")]
        [ActionScript("protected")]
        NestedProtected,

        [CSharp("protected internal")]
        [ActionScript("protected")]
        NestedProtectedInternal,

        [CSharp("internal")]
        [ActionScript("internal")]
        NestedInternal,

        [CSharp("public")]
        [ActionScript("public")]
        NestedPublic,

        [CSharp("private")]
        [ActionScript("private")]
        Private,

        [CSharp("protected")]
        [ActionScript("protected")]
        Protected,

        [CSharp("protected internal")]
        [ActionScript("protected")]
        ProtectedInternal,

        [CSharp("internal")]
        [ActionScript("internal")]
        Internal,

        [CSharp("public")]
        [ActionScript("public")]
        Public,
    }
    #endregion

    #region enum Modifiers
    [Flags]
    public enum Modifiers
    {
        None = 0,
        Const = 0x01,
        ReadOnly = 0x02,
        Volatile = 0x04,
        Static = 0x08,
        Extern = 0x10,
        Virtual = 0x20,
        Abstract = 0x40,
        Sealed = 0x80,
        HideBySig = 0x100,
        New = 0x200,
        Unsafe = 0x400,
        HasThis = 0x800,
        ExplicitThis = 0x1000,
        HasDefault = 0x2000,
        BeforeFieldInit = 0x4000,
        SpecialName = 0x8000,
        RuntimeSpecialName = 0x10000,
        EntryPoint = 0x20000,
        CompilerGenerated = 0x40000,
        ExplicitImplementation = 0x80000,
        PInvoke = 0x100000,
    }
    #endregion

    #region enum ClassSemantics
    public enum ClassSemantics
    {
        None,

        [CSharp("static")]
        [ActionScript("abstract final")]
        Static,

        [CSharp("sealed")]
        [ActionScript("final")]
        Sealed,
    
        [CSharp("abstract")]
        [ActionScript("abstract")]
        Abstract,
    }
    #endregion

    #region enum CastOperator
    public enum CastOperator
    {
        Cast,
        Is,
        As
    }
    #endregion

    #region enum BinaryOperator
    public enum BinaryOperator
    {
        [CSharp("+")]
        Addition,

        [CSharp("-")]
        Subtraction,

        [CSharp("*")]
        Multiply,

        [CSharp("/")]
        Division,
        
        [CSharp("%")]
        Modulus,

        [CSharp("<<")]
        LeftShift,

        [CSharp(">>")]
        RightShift,

        [CSharp("==")]
        Equality,

        [CSharp("!=")]
        Inequality,

        [CSharp("|")]
        BitwiseOr,

        [CSharp("&")]
        BitwiseAnd,

        [CSharp("^")]
        ExclusiveOr,

        [CSharp("||")]
        BooleanOr,

        [CSharp("&&")]
        BooleanAnd,

        [CSharp("<")]
        LessThan,

        [CSharp("<=")]
        LessThanOrEqual,

        [CSharp(">")]
        GreaterThan,

        [CSharp(">=")]
        GreaterThanOrEqual,

        [CSharp("=")]
        Assign,
    }
    #endregion

    #region enum UnaryOperator
    public enum UnaryOperator
    {
        [CSharp("-")]
        Negate,

        [CSharp("!")]
        BooleanNot,

        [CSharp("~")]
        BitwiseNot,

        [CSharp("++")]
        PreIncrement,

        [CSharp("++")]
        PostIncrement,

        [CSharp("--")]
        PreDecrement,

        [CSharp("--")]
        PostDecrement
    }
    #endregion

    public enum BranchOperator
    {
        None,
        True,
        False,
        Null,
        NotNull,
        Equality,
        Inequality,
        LessThan,
        LessThanOrEqual,
        GreaterThan,
        GreaterThanOrEqual,
    }
}