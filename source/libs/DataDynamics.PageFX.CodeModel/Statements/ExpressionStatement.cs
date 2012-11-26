using System.Collections.Generic;
using DataDynamics.PageFX.CodeModel.Expressions;

namespace DataDynamics.PageFX.CodeModel.Statements
{
    public sealed class ExpressionStatement : Statement, IExpressionStatement
    {
    	public ExpressionStatement(IExpression e)
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
            var s = obj as IExpressionStatement;
            if (s == null) return false;
            return Equals(s.Expression, Expression);
        }

        private static readonly int HashSalt = typeof(ExpressionStatement).GetHashCode();

        public override int GetHashCode()
        {
        	int h = HashSalt;
            if (Expression != null)
                h ^= Expression.GetHashCode();
            return h;
        }
    }
}