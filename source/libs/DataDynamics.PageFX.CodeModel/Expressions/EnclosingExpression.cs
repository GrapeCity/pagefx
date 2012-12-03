using System.Collections.Generic;
using DataDynamics.PageFX.Common.CodeModel;

namespace DataDynamics.PageFX.Common.Expressions
{
    public abstract class EnclosingExpression : Expression, IEnclosingExpression
    {
    	protected EnclosingExpression()
        {
        }

    	protected EnclosingExpression(IExpression e)
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
            var e = obj as IEnclosingExpression;
            if (e == null) return false;
            return Equals(e.Expression, Expression);
        }

    	private static readonly int HashSalt = typeof(EnclosingExpression).GetHashCode();

        public override int GetHashCode()
        {
            int h = HashSalt;
            if (Expression != null)
                h ^= Expression.GetHashCode();
            return h;
        }
    }
}