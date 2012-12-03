using System;
using System.Collections.Generic;
using DataDynamics.PageFX.Common.CodeModel.Expressions;

namespace DataDynamics.PageFX.Common.CodeModel.Statements
{
    public sealed class LockStatement : Statement, ILockStatement
    {
        public LockStatement()
        {
            _body = new StatementCollection(this);
        }

    	public IExpression Expression { get; set; }

    	public IStatementCollection Body
        {
            get { return _body; }
        }
        private readonly StatementCollection _body;

    	public override IEnumerable<ICodeNode> ChildNodes
        {
            get { return new ICodeNode[] { Expression, _body }; }
        }

    	public override bool Equals(object obj)
        {
            if (obj == this) return true;
            var s = obj as ILockStatement;
            if (s == null) return false;
            if (!Equals(s.Expression, Expression)) return false;
            if (!Equals(s.Body, _body)) return false;
            return true;
        }

        public override int GetHashCode()
        {
            return new object[]{Expression, _body}.EvalHashCode();
        }
    }
}