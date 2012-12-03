using DataDynamics.PageFX.Common.TypeSystem;

namespace DataDynamics.PageFX.CLI.Translation.Values
{
    internal class ThisValue : IValue
    {
        public IType type;

        public ThisValue(IType type)
        {
            this.type = type;
        }

        public ThisValue(ITypeMember m)
        {
            type = m.DeclaringType;
        }

        public IType Type
        {
            get { return type; }
        }

        public ValueKind Kind
        {
            get { return ValueKind.This; }
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
            return string.Format("This({0})", type.FullName);
        }
    }
}