using DataDynamics.PageFX.CodeModel;
using DataDynamics.PageFX.CodeModel.TypeSystem;

namespace DataDynamics.PageFX.CLI.Translation.Values
{
    class VarPtr : IValue
    {
        public IVariable var;

        public VarPtr(IVariable var)
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
            get { return ValueKind.VarPtr; }
        }

        public bool IsPointer
        {
            get { return true; }
        }

        public bool IsMockPointer
        {
            get { return false; }
        }

        public override string ToString()
        {
            return string.Format("VarPtr({0}, {1})", var.Index, var.Type.FullName);
        }
    }

    class MockVarPtr : IValue
    {
        public IVariable var;

        public MockVarPtr(IVariable var)
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
            get { return ValueKind.MockVarPtr; }
        }

        public bool IsPointer
        {
            get { return true; }
        }

        public bool IsMockPointer
        {
            get { return true; }
        }

        public override string ToString()
        {
            return string.Format("MockVarPtr({0}, {1})", var.Index, var.Type.FullName);
        }
    }
}