using DataDynamics.PageFX.Common.TypeSystem;

namespace DataDynamics.PageFX.Ecma335.Translation.Values
{
    interface IValue
    {
        IType Type { get; }

        ValueKind Kind { get; }

        bool IsPointer { get; }

        bool IsMockPointer { get; }
    }

    interface IDupSource
    {
        IValue DupSource { get; }
    }

    enum ValueKind
    {
        Const,
        Var,
        This,
        Arg,
        Field,
        Elem,

        /// <summary>
        /// Metadata token
        /// </summary>
        Token,

        Function,
        Computed,

        ThisPtr,
        ArgPtr,
        VarPtr,
        FieldPtr,
        ElemPtr,

        MockThisPtr,
        MockArgPtr,
        MockVarPtr,
        MockFieldPtr,
        MockElemPtr,

        ComputedPtr,
    }

    internal static class ValueKindEnum
    {
        public static bool IsPointer(this ValueKind value)
        {
            return value >= ValueKind.ThisPtr && value <= ValueKind.ComputedPtr;
        }
    }
}