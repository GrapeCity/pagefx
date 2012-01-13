using System.Collections.Generic;

namespace DataDynamics.PageFX.CodeModel
{
    public class BoxExpression : EnclosingExpression, IBoxExpression, ITypeReferenceProvider
    {
        #region Constructors
        public BoxExpression(IType type, IExpression e) : base(e)
        {
            _sourceType = type;
        }
        #endregion

        #region IBoxExpression Members
        public IType SourceType
        {
            get { return _sourceType; }
            set { _sourceType = value; }
        }
        private IType _sourceType;
        #endregion

        #region IExpression Members
        public override IType ResultType
        {
            get { return SystemTypes.Object; }
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
            var e = obj as IBoxExpression;
            if (e == null) return false;
            if (e.SourceType != _sourceType) return false;
            return base.Equals(obj);
        }
        #endregion

        #region ITypeReferenceProvider Members
        public IEnumerable<IType> GetTypeReferences()
        {
            return new[] { _sourceType, SystemTypes.Object };
        }
        #endregion
    }
}