using System;
using System.Collections.Generic;

namespace DataDynamics.PageFX.CodeModel
{
    public class SwitchStatement : Statement, ISwitchStatement
    {
        public SwitchStatement()
        {
            _cases = new StatementCollection<ISwitchCase>(this);
        }

        #region ISwitchStatement Members
        public IExpression Expression
        {
            get { return _expression; }
            set { _expression = value; }
        }
        private IExpression _expression;

        public IStatementCollection<ISwitchCase> Cases
        {
            get { return _cases; }
        }
        private readonly StatementCollection<ISwitchCase> _cases;
        #endregion

        #region ICodeNode Members
        public override IEnumerable<ICodeNode> ChildNodes
        {
            get { return CMHelper.Enumerate(_expression, _cases); }
        }
        #endregion

        #region Object Override Members
        public override string ToString()
        {
            return ToString(null, null);
        }

        public override bool Equals(object obj)
        {
            if (obj == this) return true;
            var s = obj as ISwitchStatement;
            if (s == null) return false;
            if (!Equals(s.Expression, _expression)) return false;
            if (!Equals(s.Cases, _cases)) return false;
            return true;
        }

        public override int GetHashCode()
        {
            return Object2.GetHashCode(_expression, _cases);
        }
        #endregion
    }

    public class SwitchCase : Statement, ISwitchCase
    {
        public SwitchCase()
        {
            _body = new StatementCollection(this);
        }

        #region ISwitchCase Members
        public int From
        {
            get { return _from; }
            set { _from = value; }
        }
        private int _from;

        public int To
        {
            get { return _to; }
            set { _to = value; }
        }
        private int _to;

        public IStatementCollection Body
        {
            get { return _body; }
        }
        private readonly StatementCollection _body;
        #endregion

        #region ICodeNode Members
        public override IEnumerable<ICodeNode> ChildNodes
        {
            get { return CMHelper.Enumerate(_body); }
        }
        #endregion

        #region Object Override Members
        public override bool Equals(object obj)
        {
            if (obj == this) return true;
            var c = obj as ISwitchCase;
            if (c == null) return false;
            if (c.From != _from) return false;
            if (c.To != _to) return false;
            if (!Equals(c.Body, _body)) return false;
            return true;
        }

        public override int GetHashCode()
        {
            int h = _from ^ _to;
            if (_body != null)
                h ^= _body.GetHashCode();
            return h;
        }
        #endregion
    }
}