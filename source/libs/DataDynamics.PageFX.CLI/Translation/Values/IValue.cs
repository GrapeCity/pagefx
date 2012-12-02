using DataDynamics.PageFX.CodeModel;
using DataDynamics.PageFX.CodeModel.TypeSystem;

namespace DataDynamics.PageFX.CLI.Translation.Values
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