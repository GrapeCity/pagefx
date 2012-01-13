using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.IL
{
    class ConstValue : IValue
    {
        readonly IType _type;
        public readonly object value;

        public ConstValue(object value)
        {
            this.value = value;
            if (value != null)
                _type = SystemTypes.GetType(value);
        }

        public IType Type
        {
            get { return _type; }
        }

        public ValueKind Kind
        {
            get { return ValueKind.Const; }
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
            string v = value != null ? value.ToString() : "null";
            return string.Format("Const({0})", v);
        }
    }
}