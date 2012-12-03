using System.Collections.Generic;
using DataDynamics.PageFX.Common.Expressions;

namespace DataDynamics.PageFX.Common.Statements
{
    public sealed class ThrowExceptionStatement : Statement, IThrowExceptionStatement
    {
    	public ThrowExceptionStatement()
        {
        }

        public ThrowExceptionStatement(IExpression e)
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
            var s = obj as IThrowExceptionStatement;
            if (s == null) return false;
            return Equals(s.Expression, Expression);
        }

        private static readonly int HashSalt = typeof(IThrowExceptionStatement).GetHashCode();

        public override int GetHashCode()
        {
        	int h = HashSalt;
            if (Expression != null)
                h ^= Expression.GetHashCode();
            return h;
        }
    }
}