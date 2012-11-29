using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.Translation.Values
{
    class ArgPtr : IValue
    {
        public IParameter arg;

        public ArgPtr(IParameter p)
        {
            arg = p;
        }

        public int Index
        {
            get { return arg.Index - 1; }
        }

        public IType Type
        {
            get { return arg.Type.UnwrapRef(); }
        }

        public ValueKind Kind
        {
            get { return ValueKind.ArgPtr; }
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
            return string.Format("ArgPtr({0})", arg.Name);
        }
    }

    class MockArgPtr : IValue
    {
        public IParameter arg;
        
        public MockArgPtr(IParameter arg)
        {
            this.arg = arg;
        }

        public IType Type
        {
            get { return arg.Type.UnwrapRef(); }
        }

        public ValueKind Kind
        {
            get { return ValueKind.MockArgPtr; }
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
            return string.Format("MockArgPtr({0})", arg.Name);
        }
    }
}