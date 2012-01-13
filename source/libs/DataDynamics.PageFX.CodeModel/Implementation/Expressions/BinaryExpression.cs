using System;
using System.Collections.Generic;

namespace DataDynamics.PageFX.CodeModel
{
    public sealed class BinaryExpression : Expression, IBinaryExpression
    {
        #region Constructors
        public BinaryExpression(IExpression left, IExpression right, BinaryOperator op)
        {
            _left = left;
            _right = right;
            _op = op;
        }
        #endregion

        #region IBinaryExpression Members
        public IExpression Left
        {
            get { return _left; }
            set { _left = value; }
        }
        private IExpression _left;

        public IExpression Right
        {
            get { return _right; }
            set { _right = value; }
        }
        private IExpression _right;

        public BinaryOperator Operator
        {
            get { return _op; }
            set { _op = value; }
        }
        private BinaryOperator _op;
        #endregion

        #region ICodeNode Members
        public override IEnumerable<ICodeNode> ChildNodes
        {
            get { return CMHelper.Enumerate(_left, _right); }
        }
        #endregion

        #region IExpression Members
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
                return GetResultType(_left.ResultType, _right.ResultType, _op);
            }
        }
        #endregion

        #region Object Override Members
        public override bool Equals(object obj)
        {
            if (obj == this) return true;
            var e = obj as IBinaryExpression;
            if (e == null) return false;
            if (e.Operator != _op) return false;
            if (!Equals(e.Left, _left)) return false;
            if (!Equals(e.Right, _right)) return false;
            return true;
        }

        public override int GetHashCode()
        {
            return _op.GetHashCode() ^ Object2.GetHashCode(_left, _right);
        }
        #endregion
    }
}