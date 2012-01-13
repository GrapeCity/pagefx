using System;

namespace DataDynamics.PageFX.CodeModel
{
    public sealed class UnaryExpression : EnclosingExpression, IUnaryExpression
    {
        #region Constructors
        public UnaryExpression(IExpression e, UnaryOperator op) : base(e)
        {
            _op = op;
        }
        #endregion

        #region IUnaryExpression Members
        public UnaryOperator Operator
        {
            get { return _op; }
            set { _op = value; }
        }
        private UnaryOperator _op;
        #endregion

        #region IExpression Members
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
                return GetResultType(type, _op);
            }
        }
        #endregion

        #region Object Override Members
        public override string ToString()
        {
            return ToString(null, null);
        }

        public override bool Equals(object obj)
        {
            if (obj == this) return true;
            var e = obj as IUnaryExpression;
            if (e == null) return false;
            if (e.Operator != _op) return false;
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return _op.GetHashCode() ^ base.GetHashCode();
        }
        #endregion
    }
}