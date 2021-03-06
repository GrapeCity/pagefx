using DataDynamics.PageFX.Common.TypeSystem;

namespace DataDynamics.PageFX.Common.CodeModel.Expressions
{
    public sealed class SizeOfExpression : Expression, ISizeOfExpression
    {
    	public SizeOfExpression(IType type)
        {
            Type = type;
        }

    	public IType Type { get; set; }

    	public override IType ResultType
        {
            get { return Type.SystemType(SystemTypeCode.Int32); }
        }

    	public override bool Equals(object obj)
        {
            if (obj == this) return true;
            var e = obj as ISizeOfExpression;
            if (e == null) return false;
            if (e.Type != Type) return false;
            return true;
        }

        private static readonly int HashSalt = typeof(ISizeOfExpression).GetHashCode();

        public override int GetHashCode()
        {
        	int h = HashSalt;
            if (Type != null)
                h ^= Type.GetHashCode();
            return h;
        }
    }
}