using System;

namespace DataDynamics.PageFX.CodeModel
{
    public sealed class ConstExpression : Expression, IConstantExpression
    {
    	public ConstExpression(object value)
        {
            Value = value;
        }

    	public object Value { get; set; }

    	public override IType ResultType
        {
            get
            {
                if (Value == null)
                    return SystemTypes.Object;

                var code = Type.GetTypeCode(Value.GetType());
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

    	public override bool Equals(object obj)
        {
            var e = obj as IConstantExpression;
            if (e == null) return false;
            return Equals(e.Value, Value);
        }

        private static readonly int HashSalt = typeof(IConstantExpression).GetHashCode();

		public override int GetHashCode()
		{
			int h = HashSalt;
			if (Value != null)
				h ^= Value.GetHashCode();
			return h;
		}
    }
}