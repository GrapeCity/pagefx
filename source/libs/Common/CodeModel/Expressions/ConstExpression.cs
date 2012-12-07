using DataDynamics.PageFX.Common.TypeSystem;

namespace DataDynamics.PageFX.Common.CodeModel.Expressions
{
    public sealed class ConstExpression : Expression, IConstantExpression
    {
	    private readonly IType _resultType;

	    public ConstExpression(object value, IType resultType)
    	{
    		_resultType = resultType;
    		Value = value;
    	}

	    public object Value { get; set; }

    	public override IType ResultType
        {
            get { return _resultType; }
        }

    	public override bool Equals(object obj)
        {
            var e = obj as IConstantExpression;
            if (e == null) return false;
            return Equals(e.Value, Value);
        }

        private static readonly int HashSalt = typeof(IConstantExpression).GetHashCode();

		public override int GetHashCode()
		{
			int h = HashSalt;
			if (Value != null)
				h ^= Value.GetHashCode();
			return h;
		}
    }
}