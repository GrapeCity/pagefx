using System;
using System.Collections.Generic;
using DataDynamics.PageFX.CodeModel.Expressions;

namespace DataDynamics.PageFX.CodeModel.Statements
{
    public class SwitchStatement : Statement, ISwitchStatement
    {
        public SwitchStatement()
        {
            _cases = new StatementCollection<ISwitchCase>(this);
        }

    	public IExpression Expression { get; set; }

    	public IStatementCollection<ISwitchCase> Cases
        {
            get { return _cases; }
        }
        private readonly StatementCollection<ISwitchCase> _cases;

    	public override IEnumerable<ICodeNode> ChildNodes
        {
            get { return new ICodeNode[] { Expression, _cases }; }
        }

    	public override string ToString()
        {
            return ToString(null, null);
        }

        public override bool Equals(object obj)
        {
            if (obj == this) return true;
            var s = obj as ISwitchStatement;
            if (s == null) return false;
            if (!Equals(s.Expression, Expression)) return false;
            if (!Equals(s.Cases, _cases)) return false;
            return true;
        }

        public override int GetHashCode()
        {
            return new object[]{Expression, _cases}.EvalHashCode();
        }
    }

    public class SwitchCase : Statement, ISwitchCase
    {
        public SwitchCase()
        {
            _body = new StatementCollection(this);
        }

    	public int From { get; set; }

    	public int To { get; set; }

    	public IStatementCollection Body
        {
            get { return _body; }
        }
        private readonly StatementCollection _body;

    	public override IEnumerable<ICodeNode> ChildNodes
        {
            get { return new ICodeNode[] {_body}; }
        }

    	public override bool Equals(object obj)
        {
            if (obj == this) return true;
            var c = obj as ISwitchCase;
            if (c == null) return false;
            if (c.From != From) return false;
            if (c.To != To) return false;
            if (!Equals(c.Body, _body)) return false;
            return true;
        }

        public override int GetHashCode()
        {
            int h = From ^ To;
            if (_body != null)
                h ^= _body.GetHashCode();
            return h;
        }
    }
}