using System;
using System.Collections.Generic;
using DataDynamics.PageFX.Common.Expressions;

namespace DataDynamics.PageFX.Common.Statements
{
    public class ForStatement : Statement, IForStatement
    {
        public ForStatement()
        {
            _body = new StatementCollection(this);
        }

    	public IStatement Initializer { get; set; }

    	public IExpression Condition { get; set; }

    	public IStatement Increment { get; set; }

    	public IStatementCollection Body
        {
            get { return _body; }
        }
        private readonly StatementCollection _body;

    	public override IEnumerable<ICodeNode> ChildNodes
        {
            get { return new ICodeNode[] { Initializer, Condition, Increment, _body }; }
        }

    	public override bool Equals(object obj)
        {
            if (obj == this) return true;
            var s = obj as IForStatement;
            if (s == null) return false;
            if (!Equals(s.Initializer, Initializer)) return false;
            if (!Equals(s.Condition, Condition)) return false;
            if (!Equals(s.Increment, Increment)) return false;
            if (!Equals(s.Body, _body)) return false;
            return true;
        }

        public override int GetHashCode()
        {
            return new object[]{Initializer, Condition, Increment, _body}.EvalHashCode();
        }
    }
}