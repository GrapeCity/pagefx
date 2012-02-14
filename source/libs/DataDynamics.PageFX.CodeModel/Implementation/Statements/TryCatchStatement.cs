using System;
using System.Collections.Generic;

namespace DataDynamics.PageFX.CodeModel
{
    public class TryCatchStatement : Statement, ITryCatchStatement
    {
        public TryCatchStatement()
        {
            _try = new StatementCollection(this);
            _catch = new StatementCollection(this);
            _fault = new StatementCollection(this);
            _finally = new StatementCollection(this);
        }

        #region ITryCatchStatement Members
        public IStatementCollection Try
        {
            get { return _try; }
        }
        private readonly StatementCollection _try;

        public IStatementCollection CatchClauses
        {
            get { return _catch; }
        }
        private readonly StatementCollection _catch;

        public IStatementCollection Fault
        {
            get { return _fault; }
        }
        private readonly StatementCollection _fault;
        
        public IStatementCollection Finally
        {
            get { return _finally; }
        }
        private readonly StatementCollection _finally;
        #endregion

        #region ICodeNode Members
        public override IEnumerable<ICodeNode> ChildNodes
        {
            get
            {
                yield return _try;
                foreach (ICatchClause clause in _catch)
                {
                    yield return clause;
                }
                yield return _fault;
                yield return _finally;
            }
        }
        #endregion

        #region Object Override Members
        public override bool Equals(object obj)
        {
            if (obj == this) return true;
            var s = obj as ITryCatchStatement;
            if (s == null) return false;
            if (!Equals(s.Try, _try)) return false;
            if (!Equals(s.Fault, _fault)) return false;
            if (!Equals(s.Finally, _finally)) return false;
            if (!Equals(s.CatchClauses, _catch)) return false;
            return true;
        }

        public override int GetHashCode()
        {
            return Object2.GetHashCode(_try, _fault, _finally, _catch);
        }
        #endregion
    }

    public class CatchClause : Statement, ICatchClause
    {
        public CatchClause()
        {
            _body = new StatementCollection(this);
        }

        #region ICatchClause Members
        public IType ExceptionType
        {
            get { return _exceptionType; }
            set { _exceptionType = value; }
        }
        private IType _exceptionType;

        public IExpression Condition
        {
            get { return _condition; }
            set { _condition = value; }
        }
        private IExpression _condition;

        public IVariable Variable
        {
            get { return _var; }
            set { _var = value; }
        }
        private IVariable _var;
        
        public IStatementCollection Body
        {
            get { return _body; }
        }
        private readonly StatementCollection _body;
        #endregion

        #region ICodeNode Members
        public override IEnumerable<ICodeNode> ChildNodes
        {
            get { return new ICodeNode[] { _condition, _var, _body }; }
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
            var c = obj as ICatchClause;
            if (c == null) return false;
            if (c.ExceptionType != _exceptionType) return false;
            if (!Equals(c.Variable, _var)) return false;
            if (!Equals(c.Condition, _condition)) return false;
            if (!Equals(c.Body, _body)) return false;
            return true;
        }

        public override int GetHashCode()
        {
            return Object2.GetHashCode(_exceptionType, _var, _condition, _body);
        }
        #endregion
    }
}