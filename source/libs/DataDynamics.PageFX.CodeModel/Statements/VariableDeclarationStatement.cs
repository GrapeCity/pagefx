using System.Collections.Generic;
using DataDynamics.PageFX.CodeModel.Expressions;

namespace DataDynamics.PageFX.CodeModel.Statements
{
    public sealed class VariableDeclarationStatement : Statement, IVariableDeclarationStatement, ITypeReferenceProvider
    {
    	public VariableDeclarationStatement(IVariable v)
        {
            Variable = v;
        }

    	public IVariable Variable { get; set; }

    	public override IEnumerable<ICodeNode> ChildNodes
        {
            get { return new ICodeNode[] {Variable}; }
        }

    	public override string ToString()
        {
            return ToString(null, null);
        }

        public override bool Equals(object obj)
        {
            if (obj == this) return true;
            var s = obj as IVariableDeclarationStatement;
            if (s == null) return false;
            if (!Equals(s.Variable, Variable)) return false;
            return true;
        }

        private static readonly int HashSalt = typeof(IVariableDeclarationStatement).GetHashCode();

        public override int GetHashCode()
        {
        	int h = HashSalt;
            if (Variable != null)
                h ^= Variable.GetHashCode();
            return h;
        }

    	public IEnumerable<IType> GetTypeReferences()
        {
            if (Variable != null)
                return new[] { Variable.Type };
            return null;
        }
    }
}