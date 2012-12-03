using System;
using DataDynamics.PageFX.Common.Expressions;
using DataDynamics.PageFX.Common.TypeSystem;

namespace DataDynamics.PageFX.Common
{
    public static class ExpressionService
    {
	    public static bool CanBrace(IExpression e)
        {
            if (e is IBinaryExpression) return true;
            if (e is IConditionExpression) return true;
            if (e is IExpressionCollection) return true;
            return false;
        }

        public static BranchOperator ToBranchOperator(BinaryOperator op)
        {
            switch (op)
            {
                case BinaryOperator.Equality:
                    return BranchOperator.Equality;

                case BinaryOperator.Inequality:
                    return BranchOperator.Inequality;

                case BinaryOperator.LessThan:
                    return BranchOperator.LessThan;

                case BinaryOperator.LessThanOrEqual:
                    return BranchOperator.LessThanOrEqual;

                case BinaryOperator.GreaterThan:
                    return BranchOperator.GreaterThan;

                case BinaryOperator.GreaterThanOrEqual:
                    return BranchOperator.GreaterThanOrEqual;
            }
            return BranchOperator.None;
        }

        public static BinaryOperator Negate(BinaryOperator op)
        {
            switch (op)
            {
                case BinaryOperator.Equality:
                    return BinaryOperator.Inequality;
                case BinaryOperator.Inequality:
                    return BinaryOperator.Equality;
                case BinaryOperator.BooleanOr:
                    return BinaryOperator.BooleanAnd;
                case BinaryOperator.BooleanAnd:
                    return BinaryOperator.BooleanOr;
                case BinaryOperator.LessThan:
                    return BinaryOperator.GreaterThanOrEqual;
                case BinaryOperator.LessThanOrEqual:
                    return BinaryOperator.GreaterThan;
                case BinaryOperator.GreaterThan:
                    return BinaryOperator.LessThanOrEqual;
                case BinaryOperator.GreaterThanOrEqual:
                    return BinaryOperator.LessThan;
                default:
                    throw new ArgumentOutOfRangeException("op");
            }
        }

        public static BranchOperator Negate(BranchOperator op)
        {
            switch (op)
            {
                case BranchOperator.True:
                    return BranchOperator.False;
                case BranchOperator.False:
                    return BranchOperator.True;
                case BranchOperator.Equality:
                    return BranchOperator.Inequality;
                case BranchOperator.Inequality:
                    return BranchOperator.Equality;
                case BranchOperator.LessThan:
                    return BranchOperator.GreaterThanOrEqual;
                case BranchOperator.LessThanOrEqual:
                    return BranchOperator.GreaterThan;
                case BranchOperator.GreaterThan:
                    return BranchOperator.LessThanOrEqual;
                case BranchOperator.GreaterThanOrEqual:
                    return BranchOperator.LessThan;
                default:
                    throw new ArgumentOutOfRangeException("op");
            }
        }

	    public static IExpression ToChar(IExpression e)
        {
            var constExp = e as IConstantExpression;
            if (constExp != null)
            {
                var v = constExp.Value;
                if (v == null)
                {
                    constExp.Value = '\0';
                    return constExp;
                }

                var c = v as IConvertible;
                if (c == null)
                    throw new InvalidCastException();

                v = c.ToChar(null);
                constExp.Value = v;
                return e;
            }

            var castExp = e as ICastExpression;
            if (castExp != null && castExp.Operator == CastOperator.Cast)
            {
                if (castExp.TargetType.Is(SystemTypeCode.UInt16))
                {
                    castExp.TargetType = SystemTypes.Char;
                    return e;
                }
            }

            return e;
        }

        public static IExpression FixConstant(IExpression e, IType type)
        {
            if (type.Is(SystemTypeCode.Boolean))
                return BooleanAlgebra.ToBool(e);

            if (type.Is(SystemTypeCode.Char))
                return ToChar(e);

            return e;
        }
    }
}