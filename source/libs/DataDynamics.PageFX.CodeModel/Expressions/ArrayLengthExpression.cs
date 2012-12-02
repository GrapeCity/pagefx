using DataDynamics.PageFX.CodeModel.TypeSystem;

namespace DataDynamics.PageFX.CodeModel.Expressions
{
    public sealed class ArrayLengthExpression : Expression, IArrayLengthExpression
    {
    	public ArrayLengthExpression(IExpression array)
        {
            Array = array;
        }

    	public IExpression Array { get; set; }

    	public override IType ResultType
        {
            get { return SystemTypes.Int32; }
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