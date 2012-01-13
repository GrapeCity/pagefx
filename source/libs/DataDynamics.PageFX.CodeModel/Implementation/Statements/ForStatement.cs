using System;
using System.Collections.Generic;

namespace DataDynamics.PageFX.CodeModel
{
    public class ForStatement : Statement, IForStatement
    {
        public ForStatement()
        {
            _body = new StatementCollection(this);
        }

        #region IForStatement Members
        public IStatement Initializer
        {
            get { return _initializer; }
            set { _initializer = value; }
        }
        private IStatement _initializer;

        public IExpression Condition
        {
            get { return _condition; }
            set { _condition = value; }
        }
        private IExpression _condition;

        public IStatement Increment
        {
            get { return _increment; }
            set { _increment = value; }
        }
        private IStatement _increment;
        
        public IStatementCollection Body
        {
            get { return _body; }
        }
        private readonly StatementCollection _body;
        #endregion

        #region ICodeNode Members
        public override IEnumerable<ICodeNode> ChildNodes
        {
            get { return CMHelper.Enumerate(_initializer, _condition, _increment, _body); }
        }
        #endregion

        #region Object Override Members
        public override bool Equals(object obj)
        {
            if (obj == this) return true;
            var s = obj as IForStatement;
            if (s == null) return false;
            if (!Equals(s.Initializer, _initializer)) return false;
            if (!Equals(s.Condition, _condition)) return false;
            if (!Equals(s.Increment, _increment)) return false;
            if (!Equals(s.Body, _body)) return false;
            return true;
        }

        public override int GetHashCode()
        {
            return Object2.GetHashCode(_initializer, _condition, _increment, _body);
        }
        #endregion
    }
}