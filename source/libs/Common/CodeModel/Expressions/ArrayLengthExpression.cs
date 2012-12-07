using DataDynamics.PageFX.Common.TypeSystem;

namespace DataDynamics.PageFX.Common.CodeModel.Expressions
{
    public sealed class ArrayLengthExpression : Expression, IArrayLengthExpression
    {
	    private readonly IType _resultType;

	    public ArrayLengthExpression(IExpression array, IType resultType)
    	{
    		_resultType = resultType;
    		Array = array;
    	}

	    public IExpression Array { get; set; }

    	public override IType ResultType
        {
            get { return _resultType; }
        }

    	public override bool Equals(object obj)
        {
            if (obj == this) return true;
            var e = obj as IArrayLengthExpression;
            if (e == null) return false;
            return Equals(e.Array, Array);
        }

        private static readonly int HashSalt = typeof(ArrayLengthExpression).GetHashCode();

        public override int GetHashCode()
        {
        	int h = HashSalt;
            if (Array != null)
                h ^= Array.GetHashCode();
            return h;
        }
    }
}