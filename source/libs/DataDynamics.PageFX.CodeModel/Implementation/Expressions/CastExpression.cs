using System.Collections.Generic;

namespace DataDynamics.PageFX.CodeModel
{
    public sealed class CastExpression : EnclosingExpression, ICastExpression, ITypeReferenceProvider
    {
        #region Constructors
        public CastExpression(IType targetType, IExpression e, CastOperator op) : base(e)
        {
            _targetType = targetType;
            _sourceType = e.ResultType;
            _op = op;
        }
        #endregion

        #region ICastExpression Members
        public IType SourceType
        {
            get { return _sourceType; }
        }
        private readonly IType _sourceType;

        public IType TargetType
        {
            get { return _targetType; }
            set { _targetType = value; }
        }
        private IType _targetType;

        public CastOperator Operator
        {
            get { return _op; }
            set { _op = value; }
        }
        private CastOperator _op;
        #endregion

        #region IExpression Members
        public override IType ResultType
        {
            get
            {
                if (_op == CastOperator.Is)
                    return SystemTypes.Boolean;
                return _targetType;
            }
        }
        #endregion

        #region Object Override Members
        public override bool Equals(object obj)
        {
            if (obj == this) return true;
            var e = obj as ICastExpression;
            if (e == null) return false;
            if (e.Operator != _op) return false;
            if (e.TargetType != _targetType) return false;
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            int h = _op.GetHashCode();
            if (_targetType != null)
                h ^= _targetType.GetHashCode();
            return h ^ base.GetHashCode();
        }
        #endregion

        #region ITypeReferenceProvider Members
        public IEnumerable<IType> GetTypeReferences()
        {
            return new[] { _targetType };
        }
        #endregion
    }
}