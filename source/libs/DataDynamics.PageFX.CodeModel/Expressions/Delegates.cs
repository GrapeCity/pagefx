using System;
using System.Collections.Generic;
using DataDynamics.PageFX.CodeModel.TypeSystem;

namespace DataDynamics.PageFX.CodeModel.Expressions
{
    public class NewDelegateExpression : Expression, INewDelegateExpression
    {
    	public NewDelegateExpression(IType type, IMethodReferenceExpression method)
        {
            DelegateType = type;
            Method = method;
        }

    	public IType DelegateType { get; set; }

    	public IMethodReferenceExpression Method { get; set; }

    	public override IType ResultType
        {
            get { return DelegateType; }
        }

        public override IEnumerable<ICodeNode> ChildNodes
        {
            get { return new ICodeNode[] {Method}; }
        }

        public override bool Equals(object obj)
        {
            if (obj == this) return true;
            var e = obj as INewDelegateExpression;
            if (e == null) return false;
            if (e.DelegateType != DelegateType) return false;
            if (!Equals(e.Method, Method)) return false;
            return true;
        }

        public override int GetHashCode()
        {
            return new object[]{DelegateType, Method}.EvalHashCode();
        }
    }

    public class DelegateInvokeExpression : Expression, IDelegateInvokeExpression
    {
    	public IExpression Target { get; set; }

    	public IMethod Method { get; set; }

    	public IExpressionCollection Arguments
        {
            get { return _args; }
        }
        private readonly ExpressionCollection _args = new ExpressionCollection();

    	public override IType ResultType
        {
            get
            {
                if (Method != null)
                    return Method.Type;
                return null;
            }
        }

        public override IEnumerable<ICodeNode> ChildNodes
        {
            get { return new ICodeNode[] { Target, _args }; }
        }

        public override bool Equals(object obj)
        {
            if (obj == this) return true;
            var e = obj as IDelegateInvokeExpression;
            if (e == null) return false;
            if (!Equals(e.Target, Target)) return false;
            if (!Equals(e.Arguments, _args)) return false;
            return true;
        }

        private static readonly int _hs = typeof(IDelegateInvokeExpression).GetHashCode();

        public override int GetHashCode()
        {
            return new object[]{Target, _args}.EvalHashCode() ^ _hs;
        }
    }
}