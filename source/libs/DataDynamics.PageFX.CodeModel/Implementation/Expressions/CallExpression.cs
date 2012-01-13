using System;
using System.Collections.Generic;

namespace DataDynamics.PageFX.CodeModel
{
    public class CallExpression : Expression, ICallExpression
    {
        #region Constructors
        public CallExpression()
        {    
        }

        public CallExpression(IMethodReferenceExpression method)
        {
            _method = method;
        }
        #endregion

        #region ICallExpression Members
        public IMethodReferenceExpression Method
        {
            get { return _method; }
            set { _method = value; }
        }
        private IMethodReferenceExpression _method;

        public IExpressionCollection Arguments
        {
            get { return _args; }
        }
        private readonly ExpressionCollection _args = new ExpressionCollection();
        #endregion

        #region ICodeNode Members
        public override IEnumerable<ICodeNode> ChildNodes
        {
            get { return CMHelper.Enumerate(_method, _args); }
        }
        #endregion

        #region IExpression Members
        public override IType ResultType
        {
            get { return _method.ResultType; }
        }
        #endregion

        #region Object Override Members
        public override bool Equals(object obj)
        {
            if (obj == this) return true;
            var e = obj as ICallExpression;
            if (e == null) return false;
            if (!Equals(e.Method, _method)) return false;
            if (!Equals(e.Arguments, _args)) return false;
            return true;
        }

        public override int GetHashCode()
        {
            return Object2.GetHashCode(_method, _args);
        }
        #endregion
    }
}