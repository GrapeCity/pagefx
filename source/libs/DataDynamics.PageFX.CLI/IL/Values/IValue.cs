using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.IL
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

    internal class ValueUtils
    {
        public static bool IsPointer(ValueKind k)
        {
            return k >= ValueKind.ThisPtr && k <= ValueKind.ComputedPtr;
        }
    }
}