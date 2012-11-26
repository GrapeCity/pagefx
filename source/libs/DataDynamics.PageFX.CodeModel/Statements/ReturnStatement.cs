using System.Collections.Generic;

namespace DataDynamics.PageFX.CodeModel
{
    public sealed class ReturnStatement : Statement, IReturnStatement
    {
    	public ReturnStatement(IExpression e)
        {
            Expression = e;
        }

    	public IExpression Expression { get; set; }

    	public override IEnumerable<ICodeNode> ChildNodes
        {
            get { return new ICodeNode[] {Expression}; }
        }

    	public override bool Equals(object obj)
        {
            if (obj == this) return true;
            var s = obj as IReturnStatement;
            if (s == null) return false;
            return Equals(s.Expression, Expression);
        }

        private static readonly int HashSalt = typeof(ReturnStatement).GetHashCode();

        public override int GetHashCode()
        {
        	int h = HashSalt;
            if (Expression != null)
                h ^= Expression.GetHashCode();
            return h;
        }
    }
}