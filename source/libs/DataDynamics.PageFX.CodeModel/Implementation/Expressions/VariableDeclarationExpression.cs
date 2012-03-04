using System.Collections.Generic;

namespace DataDynamics.PageFX.CodeModel
{
    public sealed class VariableDeclarationExpression : Expression, IVariableDeclarationExpression, ITypeReferenceProvider
    {
    	public IVariable Variable { get; set; }

    	public override IEnumerable<ICodeNode> ChildNodes
        {
            get { return new ICodeNode[] {Variable}; }
        }

    	public override IType ResultType
        {
            get { return Variable != null ? Variable.Type : null; }
        }

    	public override bool Equals(object obj)
        {
            if (obj == this) return true;
            var e = obj as IVariableDeclarationExpression;
            if (e == null) return false;
            return e.Variable == Variable;
        }

        private static readonly int HashSalt = typeof(IVariableDeclarationExpression).GetHashCode();

        public override int GetHashCode()
        {
        	int h = HashSalt;
            if (Variable != null)
                h ^= Variable.GetHashCode();
            return h;
        }

    	public IEnumerable<IType> GetTypeReferences()
    	{
    		return Variable != null ? new[] { Variable.Type } : new IType[0];
    	}
    }
}