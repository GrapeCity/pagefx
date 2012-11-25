using System;
using System.Globalization;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.IL
{
    internal sealed class ConstValue : IValue
    {
        readonly IType _type;
        public readonly object Value;

        public ConstValue(object value)
        {
            Value = value;

			// TODO: pass type to ctor
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
            string v = Value != null ? Convert.ToString(Value, CultureInfo.InvariantCulture) : "null";
            return string.Format("Const({0})", v);
        }
    }
}