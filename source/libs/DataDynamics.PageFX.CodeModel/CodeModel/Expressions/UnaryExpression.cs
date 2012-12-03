using System;
using DataDynamics.PageFX.Common.Services;
using DataDynamics.PageFX.Common.TypeSystem;

namespace DataDynamics.PageFX.Common.CodeModel.Expressions
{
    public sealed class UnaryExpression : EnclosingExpression, IUnaryExpression
    {
    	public UnaryExpression(IExpression e, UnaryOperator op) : base(e)
        {
            Operator = op;
        }

    	public UnaryOperator Operator { get; set; }

    	public static IType GetResultType(IType type, UnaryOperator op)
        {
            switch (op)
            {
                case UnaryOperator.Negate:
                case UnaryOperator.BitwiseNot:
                    return Arithmetic.GetResultType(type, op);

                case UnaryOperator.BooleanNot:
                    return SystemTypes.Boolean;

                case UnaryOperator.PreIncrement:
                case UnaryOperator.PostIncrement:
                    return Arithmetic.GetResultType(type, type, BinaryOperator.Addition);

                case UnaryOperator.PreDecrement:
                case UnaryOperator.PostDecrement:
                    return Arithmetic.GetResultType(type, type, BinaryOperator.Subtraction);

                default:
                    throw new ArgumentOutOfRangeException("op");
            }
        }

        public override IType ResultType
        {
            get
            {
                var type = Expression.ResultType;
                return GetResultType(type, Operator);
            }
        }

    	public override string ToString()
        {
            return ToString(null, null);
        }

        public override bool Equals(object obj)
        {
            if (obj == this) return true;
            var e = obj as IUnaryExpression;
            if (e == null) return false;
            if (e.Operator != Operator) return false;
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return Operator.GetHashCode() ^ base.GetHashCode();
        }
    }
}