namespace DataDynamics.PageFX.CodeModel
{
    public sealed class AddressOfExpression : EnclosingExpression, IAddressOfExpression
    {
        #region Constructors
        public AddressOfExpression(IExpression e) : base(e)
        {
        }
        #endregion

        #region IExpression Members
        public override IType ResultType
        {
            get
            {
                return TypeFactory.MakePointerType(Expression.ResultType);
            }
        }
        #endregion
    }

    public sealed class AddressDereferenceExpression : EnclosingExpression, IAddressDereferenceExpression
    {
        #region Constructors
        public AddressDereferenceExpression(IType type, IExpression e) : base(e)
        {
            _type = type;
        }
        #endregion

        #region IAddressDereferenceExpression Members
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
            get
            {
                return _type;
                //return _expression.ResultType;
            }
        }
        #endregion

        #region Object Override Members
        public override bool Equals(object obj)
        {
            if (obj == this) return true;
            var e = obj as IAddressDereferenceExpression;
            if (e == null) return false;
            if (e.Type != _type) return false;
            return base.Equals(obj);
        }

        private static readonly int _hs = "*".GetHashCode();

        public override int GetHashCode()
        {
            return base.GetHashCode() ^ _hs;
        }
        #endregion
    }

    public sealed class AddressOutExpression : EnclosingExpression, IAddressOutExpression
    {
        #region Constructors
        public AddressOutExpression(IExpression e)
            : base(e)
        {
        }
        #endregion

        #region IExpression Members
        public override IType ResultType
        {
            get { return Expression.ResultType; }
        }
        #endregion
    }

    public sealed class AddressRefExpression : EnclosingExpression, IAddressRefExpression
    {
        #region Constructors
        public AddressRefExpression(IExpression e)
            : base(e)
        {
        }
        #endregion

        #region IExpression Members
        public override IType ResultType
        {
            get
            {
                return TypeFactory.MakeReferenceType(Expression.ResultType);
            }
        }
        #endregion
    }
}