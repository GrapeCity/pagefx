using DataDynamics.PageFX.Common.CodeModel;
using DataDynamics.PageFX.Common.TypeSystem;

namespace DataDynamics.PageFX.Ecma335.Translation.Values
{
    internal class Var : IValue
    {
        public IVariable var;

        public Var(IVariable var)
        {
            this.var = var;
        }

        public int Index
        {
            get { return var.Index; }
        }

        public IType Type
        {
            get { return var.Type; }
        }

        public ValueKind Kind
        {
            get { return ValueKind.Var; }
        }

        public bool IsPointer
        {
            get { return false; }
        }

        public bool IsMockPointer
        {
            get { return false; }
        }

        public override string ToString()
        {
            return string.Format("Var({0}, {1})", var.Index, var.Type.FullName);
        }
    }
}