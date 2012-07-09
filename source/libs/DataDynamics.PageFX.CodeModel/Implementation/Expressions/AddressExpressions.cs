namespace DataDynamics.PageFX.CodeModel
{
    public sealed class AddressOfExpression : EnclosingExpression, IAddressOfExpression
    {
    	public AddressOfExpression(IExpression e) : base(e)
        {
        }

    	public override IType ResultType
        {
            get
            {
                return TypeFactory.MakePointerType(Expression.ResultType);
            }
        }
    }

    public sealed class AddressDereferenceExpression : EnclosingExpression, IAddressDereferenceExpression
    {
    	public AddressDereferenceExpression(IType type, IExpression e) : base(e)
        {
            Type = type;
        }

    	public IType Type { get; set; }

    	public override IType ResultType
        {
            get
            {
                return Type;
                //return _expression.ResultType;
            }
        }

    	public override bool Equals(object obj)
        {
            if (obj == this) return true;
            var e = obj as IAddressDereferenceExpression;
            if (e == null) return false;
            if (e.Type != Type) return false;
            return base.Equals(obj);
        }

        private static readonly int _hs = "*".GetHashCode();

        public override int GetHashCode()
        {
            return base.GetHashCode() ^ _hs;
        }
    }

    public sealed class AddressOutExpression : EnclosingExpression, IAddressOutExpression
    {
    	public AddressOutExpression(IExpression e)
            : base(e)
        {
        }

    	public override IType ResultType
        {
            get { return Expression.ResultType; }
        }
    }

    public sealed class AddressRefExpression : EnclosingExpression, IAddressRefExpression
    {
    	public AddressRefExpression(IExpression e)
            : base(e)
        {
        }

    	public override IType ResultType
        {
            get
            {
                return TypeFactory.MakeReferenceType(Expression.ResultType);
            }
        }
    }
}