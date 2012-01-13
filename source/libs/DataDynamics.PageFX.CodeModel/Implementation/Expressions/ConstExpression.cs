using System;

namespace DataDynamics.PageFX.CodeModel
{
    public sealed class ConstExpression : Expression, IConstantExpression
    {
        #region Constructors
        public ConstExpression(object value)
        {
            _value = value;
        }
        #endregion

        #region IConstantExpression Members
        public object Value
        {
            get { return _value; }
            set { _value = value; }
        }
        private object _value;
        #endregion

        #region IExpression Members
        public override IType ResultType
        {
            get
            {
                if (_value == null)
                    return SystemTypes.Object;
                var code = Type.GetTypeCode(_value.GetType());
                switch (code)
                {
                    case TypeCode.Object:
                        return SystemTypes.Object;

                    case TypeCode.Boolean:
                        return SystemTypes.Boolean;

                    case TypeCode.Char:
                        return SystemTypes.Char;

                    case TypeCode.SByte:
                        return SystemTypes.SByte;

                    case TypeCode.Byte:
                        return SystemTypes.Byte;

                    case TypeCode.Int16:
                        return SystemTypes.Int16;

                    case TypeCode.UInt16:
                        return SystemTypes.UInt16;

                    case TypeCode.Int32:
                        return SystemTypes.Int32;

                    case TypeCode.UInt32:
                        return SystemTypes.UInt32;

                    case TypeCode.Int64:
                        return SystemTypes.Int64;

                    case TypeCode.UInt64:
                        return SystemTypes.UInt64;

                    case TypeCode.Single:
                        return SystemTypes.Single;

                    case TypeCode.Double:
                        return SystemTypes.Double;

                    case TypeCode.Decimal:
                        return SystemTypes.Decimal;

                    case TypeCode.String:
                        return SystemTypes.String;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
        #endregion

        #region Object Override Members
        public override bool Equals(object obj)
        {
            var e = obj as IConstantExpression;
            if (e == null) return false;
            if (!Equals(e.Value, _value)) return false;
            return true;
        }

        private static readonly int _hs = typeof(IConstantExpression).GetHashCode();

        public override int GetHashCode()
        {
            if (_value != null)
                return _value.GetHashCode() ^ _hs;
            return base.GetHashCode();
        }
        #endregion
    }
}