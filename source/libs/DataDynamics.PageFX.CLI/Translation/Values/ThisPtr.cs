using DataDynamics.PageFX.Common.TypeSystem;

namespace DataDynamics.PageFX.Ecma335.Translation.Values
{
    class ThisPtr : IValue
    {
        public IType type;

        public ThisPtr(IType type)
        {
            this.type = type;
        }

        public ThisPtr(ITypeMember m)
        {
            type = m.DeclaringType;
        }

        #region IValue Members
        public IType Type
        {
            get { return type; }
        }

        public ValueKind Kind
        {
            get { return ValueKind.ThisPtr; }
        }

        public bool IsPointer
        {
            get { return true; }
        }

        public bool IsMockPointer
        {
            get { return false; }
        }
        #endregion

        public override string ToString()
        {
            return string.Format("ThisPtr({0})", type.FullName);
        }
    }

    class MockThisPtr : IValue
    {
        public IType type;

        public MockThisPtr(IType type)
        {
            this.type = type;
        }

        public IType Type
        {
            get { return type; }
        }

        public ValueKind Kind
        {
            get { return ValueKind.MockThisPtr; }
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
            return string.Format("MockThisPtr({0})", type.FullName);
        }
    }
}