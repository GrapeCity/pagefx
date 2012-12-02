using DataDynamics.PageFX.CodeModel;
using DataDynamics.PageFX.CodeModel.TypeSystem;

namespace DataDynamics.PageFX.CLI.Translation.Values
{
    class ComputedValue : IValue
    {
        public IType type;

        public ComputedValue(IType type)
        {
            this.type = type;
        }

        public IType Type
        {
            get { return type; }
        }

        public ValueKind Kind
        {
            get { return ValueKind.Computed; }
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
            return string.Format("ComputedValue({0})", type.FullName);
        }
    }
}