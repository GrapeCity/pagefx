using System;

namespace DataDynamics.PageFX.CodeModel
{
    public static class Arithmetic
    {
        private static IType Fix(IType type)
        {
            type = TypeService.UnwrapRef(type);
            if (type.IsEnum)
                return type.ValueType;
            return type;
        }

        public static bool IsNumeric(IType type)
        {
            if (type == null) return false;
            var st = type.SystemType;
            if (st == null) return false;
            return st.IsNumeric;
        }

        public static IType GetResultType(IType left, IType right, BinaryOperator op)
        {
            left = Fix(left);
            right = Fix(right);

            if (!IsNumeric(left))
                throw new ArgumentException("Type is not numeric type", "left");
            if (!IsNumeric(right))
                throw new ArgumentException("Type is not numeric type", "right");

            //decimal, double, float, long, ulong, int, uint

            //The shl and shr instructions return the same type as their first operand
            switch (op)
            {
                case BinaryOperator.LeftShift:
                case BinaryOperator.RightShift:
                    {
                        var l = left.SystemType;
                        if (l.LessThenInt32)
                            return SystemTypes.Int32;
                        return left;
                    }
            }

            {
                var l = left.SystemType;
                var r = right.SystemType;
                if (l.IsDecimal || r.IsDecimal)
                    return SystemTypes.Decimal;
                if (l.IsDouble || r.IsDouble)
                    return SystemTypes.Double;
                if (l.IsSingle || r.IsSingle)
                    return SystemTypes.Single;
                if (l.IsInt64 || l.IsUInt64)
                    return left;
                if (r.IsInt64 || r.IsUInt64)
                    return right;
                if (l.IsInt32 || l.IsUInt32)
                    return left;
                //if (l.IsInt32 || r.IsInt32)
                //    return SystemTypes.Int32;
                //if (l.IsUInt32 || r.IsUInt32)
                //    return SystemTypes.UInt32;
                return SystemTypes.Int32;
            }
        }

        public static IType GetResultType(IType type, UnaryOperator op)
        {
            type = Fix(type);

            if (!IsNumeric(type))
                throw new ArgumentException("Type is not numeric type", "type");

            //The not instruction is unary and returns the same type as the input.
            if (op == UnaryOperator.BitwiseNot)
                return type;

            var st = type.SystemType;

            if (op == UnaryOperator.Negate)
            {
                if (st.IsUnsigned)
                {
                    if (st.Size <= 4)
                        return SystemTypes.Int32;
                    return SystemTypes.Int64;
                }
            }

            if (st.IsIntegral && st.Bits <= 32)
                return SystemTypes.Int32;

            return type;
        }
    }
}