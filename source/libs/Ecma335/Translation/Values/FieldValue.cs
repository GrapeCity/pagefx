using DataDynamics.PageFX.Common.TypeSystem;

namespace DataDynamics.PageFX.Ecma335.Translation.Values
{
    class FieldValue : IValue
    {
        public IField field;

        public FieldValue(IField field)
        {
            this.field = field;
        }

        public IType Type
        {
            get { return field.Type; }
        }

        public ValueKind Kind
        {
            get { return ValueKind.Field; }
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
            return string.Format("Field({0})", field.FullName);
        }
    }
}