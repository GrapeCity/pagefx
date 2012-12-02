using System;
using System.Collections.Generic;
using DataDynamics.PageFX.CodeModel.TypeSystem;

namespace DataDynamics.PageFX.CodeModel.Expressions
{
    public sealed class BinaryExpression : Expression, IBinaryExpression
    {
    	public BinaryExpression(IExpression left, IExpression right, BinaryOperator op)
        {
            Left = left;
            Right = right;
            Operator = op;
        }

    	public IExpression Left { get; set; }

    	public IExpression Right { get; set; }

    	public BinaryOperator Operator { get; set; }

    	public override IEnumerable<ICodeNode> ChildNodes
        {
            get { return new ICodeNode[] { Left, Right }; }
        }

    	public static IType GetResultType(IType left, IType right, BinaryOperator op)
        {
            switch (op)
            {
                case BinaryOperator.Addition:
                case BinaryOperator.Subtraction:
                case BinaryOperator.Multiply:
                case BinaryOperator.Division:
                case BinaryOperator.Modulus:
                case BinaryOperator.LeftShift:
                case BinaryOperator.RightShift:
                case BinaryOperator.BitwiseOr:
                case BinaryOperator.BitwiseAnd:
                case BinaryOperator.ExclusiveOr:
                    return Arithmetic.GetResultType(left, right, op);

                case BinaryOperator.Equality:
                case BinaryOperator.Inequality:
                case BinaryOperator.BooleanOr:
                case BinaryOperator.BooleanAnd:
                case BinaryOperator.LessThan:
                case BinaryOperator.LessThanOrEqual:
                case BinaryOperator.GreaterThan:
                case BinaryOperator.GreaterThanOrEqual:
                    return SystemTypes.Boolean;

                case BinaryOperator.Assign:
                    return left;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override IType ResultType
        {
            get
            {
                return GetResultType(Left.ResultType, Right.ResultType, Operator);
            }
        }

    	public override bool Equals(object obj)
        {
            if (obj == this) return true;
            var e = obj as IBinaryExpression;
            if (e == null) return false;
            if (e.Operator != Operator) return false;
            if (!Equals(e.Left, Left)) return false;
            if (!Equals(e.Right, Right)) return false;
            return true;
        }

        public override int GetHashCode()
        {
            return Operator.GetHashCode() ^ new[]{Left, Right}.EvalHashCode();
        }
    }
}