namespace DataDynamics.PageFX.CodeModel
{
    public sealed class TypeOfExpression : Expression, ITypeOfExpression
    {
        #region Constructors
        public TypeOfExpression(IType type)
        {
            _type = type;
        }
        #endregion

        #region ITypeOfExpression Members
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
            get { return SystemTypes.Type; }
        }
        #endregion

        #region Object Override Members
        public override bool Equals(object obj)
        {
            if (this == obj) return true;
            var e = obj as ITypeOfExpression;
            if (e == null) return false;
            if (e.Type != _type) return false;
            return true;
        }

        private static readonly int _hs = typeof(ITypeOfExpression).GetHashCode();

        public override int GetHashCode()
        {
            int h = _hs;
            if (_type != null)
                h ^= _type.GetHashCode();
            return h;
        }
        #endregion
    }
}