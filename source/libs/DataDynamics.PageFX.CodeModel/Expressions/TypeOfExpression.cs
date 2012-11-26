namespace DataDynamics.PageFX.CodeModel.Expressions
{
    public sealed class TypeOfExpression : Expression, ITypeOfExpression
    {
    	public TypeOfExpression(IType type)
        {
            Type = type;
        }

    	public IType Type { get; set; }

    	public override IType ResultType
        {
            get { return SystemTypes.Type; }
        }

    	public override bool Equals(object obj)
        {
            if (this == obj) return true;
            var e = obj as ITypeOfExpression;
            if (e == null) return false;
            if (e.Type != Type) return false;
            return true;
        }

        private static readonly int HashSalt = typeof(ITypeOfExpression).GetHashCode();

        public override int GetHashCode()
        {
            int h = HashSalt;
            if (Type != null)
                h ^= Type.GetHashCode();
            return h;
        }
    }
}