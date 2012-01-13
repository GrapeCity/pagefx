using System;
using System.Collections.Generic;

namespace DataDynamics.PageFX.CodeModel
{
    public class NewDelegateExpression : Expression, INewDelegateExpression
    {
        #region Constructors
        public NewDelegateExpression(IType type, IMethodReferenceExpression method)
        {
            _delegateType = type;
            _method = method;
        }
        #endregion

        #region IDelegateCreateExpression Members
        public IType DelegateType
        {
            get { return _delegateType; }
            set { _delegateType = value; }
        }
        private IType _delegateType;

        public IMethodReferenceExpression Method
        {
            get { return _method; }
            set { _method = value; }
        }
        private IMethodReferenceExpression _method;
        #endregion

        #region Override Members
        public override IType ResultType
        {
            get { return _delegateType; }
        }

        public override IEnumerable<ICodeNode> ChildNodes
        {
            get { return CMHelper.Enumerate(_method); }
        }

        public override bool Equals(object obj)
        {
            if (obj == this) return true;
            var e = obj as INewDelegateExpression;
            if (e == null) return false;
            if (e.DelegateType != _delegateType) return false;
            if (!Equals(e.Method, _method)) return false;
            return true;
        }

        public override int GetHashCode()
        {
            return Object2.GetHashCode(_delegateType, _method);
        }
        #endregion
    }

    public class DelegateInvokeExpression : Expression, IDelegateInvokeExpression
    {
        #region IDelegateInvokeExpression Members
        public IExpression Target
        {
            get { return _target; }
            set { _target = value; }
        }
        private IExpression _target;

        public IMethod Method
        {
            get { return _method; }
            set { _method = value; }
        }
        private IMethod _method;

        public IExpressionCollection Arguments
        {
            get { return _args; }
        }
        private readonly ExpressionCollection _args = new ExpressionCollection();
        #endregion

        #region Override Members
        public override IType ResultType
        {
            get
            {
                if (_method != null)
                    return _method.Type;
                return null;
            }
        }

        public override IEnumerable<ICodeNode> ChildNodes
        {
            get { return CMHelper.Enumerate(_target, _args); }
        }

        public override bool Equals(object obj)
        {
            if (obj == this) return true;
            var e = obj as IDelegateInvokeExpression;
            if (e == null) return false;
            if (!Equals(e.Target, _target)) return false;
            if (!Equals(e.Arguments, _args)) return false;
            return true;
        }

        private static readonly int _hs = typeof(IDelegateInvokeExpression).GetHashCode();

        public override int GetHashCode()
        {
            return Object2.GetHashCode(_target, _args) ^ _hs;
        }
        #endregion
    }
}