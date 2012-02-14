using System;
using System.Collections.Generic;

namespace DataDynamics.PageFX.CodeModel
{
    public sealed class LockStatement : Statement, ILockStatement
    {
        public LockStatement()
        {
            _body = new StatementCollection(this);
        }

        #region ILockStatement Members
        public IExpression Expression
        {
            get { return _expression; }
            set { _expression = value; }
        }
        private IExpression _expression;

        public IStatementCollection Body
        {
            get { return _body; }
        }
        private readonly StatementCollection _body;
        #endregion

        #region ICodeNode Members
        public override IEnumerable<ICodeNode> ChildNodes
        {
            get { return new ICodeNode[] { _expression, _body }; }
        }
        #endregion

        #region Object Override Members
        public override bool Equals(object obj)
        {
            if (obj == this) return true;
            var s = obj as ILockStatement;
            if (s == null) return false;
            if (!Equals(s.Expression, _expression)) return false;
            if (!Equals(s.Body, _body)) return false;
            return true;
        }

        public override int GetHashCode()
        {
            return Object2.GetHashCode(_expression, _body);
        }
        #endregion
    }
}