namespace DataDynamics.PageFX.CodeModel
{
    public sealed class SizeOfExpression : Expression, ISizeOfExpression
    {
        #region Constructors
        public SizeOfExpression(IType type)
        {
            _type = type;
        }
        #endregion

        #region ISizeOfExpression Members
        public IType Type
        {
            get { return _type; }
            set { _type = value; }
        }
        private IType _type;
        #endregion

        #region IExpression Members
        public override IType ResultType
        {
            get { return SystemTypes.Int32; }
        }
        #endregion

        #region Object Override Members
        public override bool Equals(object obj)
        {
            if (obj == this) return true;
            var e = obj as ISizeOfExpression;
            if (e == null) return false;
            if (e.Type != _type) return false;
            return true;
        }

        private static readonly int _hs = typeof(ISizeOfExpression).GetHashCode();

        public override int GetHashCode()
        {
            if (_type != null)
                return _type.GetHashCode() ^ _hs;
            return base.GetHashCode();
        }
        #endregion
    }
}