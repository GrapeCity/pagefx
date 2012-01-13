using System.Collections.Generic;

namespace DataDynamics.PageFX.CodeModel
{
    public class UnboxExpression : EnclosingExpression, IUnboxExpression, ITypeReferenceProvider
    {
        #region Constructors
        public UnboxExpression(IType type, IExpression e) : base(e)
        {
            _targetType = type;
        }
        #endregion

        #region IUnboxExpression Members
        public IType TargetType
        {
            get { return _targetType; }
            set { _targetType = value; }
        }
        private IType _targetType;
        #endregion

        #region IExpression Members
        public override IType ResultType
        {
            get { return _targetType; }
        }
        #endregion

        #region Object Override Members
        public override bool Equals(object obj)
        {
            if (obj == this) return true;
            var e = obj as IUnboxExpression;
            if (e == null) return false;
            if (e.TargetType != _targetType) return false;
            return base.Equals(obj);
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