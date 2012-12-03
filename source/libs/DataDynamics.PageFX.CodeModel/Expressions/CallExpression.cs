using System;
using System.Collections.Generic;
using DataDynamics.PageFX.Common.TypeSystem;

namespace DataDynamics.PageFX.Common.Expressions
{
    public class CallExpression : Expression, ICallExpression
    {
    	public CallExpression()
        {    
        }

        public CallExpression(IMethodReferenceExpression method)
        {
            Method = method;
        }

    	public IMethodReferenceExpression Method { get; set; }

    	public IExpressionCollection Arguments
        {
            get { return _args; }
        }
        private readonly ExpressionCollection _args = new ExpressionCollection();

    	public override IEnumerable<ICodeNode> ChildNodes
        {
            get { return new ICodeNode[] { Method, _args }; }
        }

    	public override IType ResultType
        {
            get { return Method.ResultType; }
        }

    	public override bool Equals(object obj)
        {
            if (obj == this) return true;
            var e = obj as ICallExpression;
            if (e == null) return false;
            if (!Equals(e.Method, Method)) return false;
            if (!Equals(e.Arguments, _args)) return false;
            return true;
        }

        public override int GetHashCode()
        {
            return new object []{Method, _args}.EvalHashCode();
        }
    }
}