using System;
using System.Globalization;
using DataDynamics.PageFX.Common.TypeSystem;

namespace DataDynamics.PageFX.Ecma335.Translation.Values
{
    internal sealed class ConstValue : IValue
    {
        private readonly IType _type;
        public readonly object Value;

        public ConstValue(object value, IType type)
        {
            Value = value;
	        _type = type;
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