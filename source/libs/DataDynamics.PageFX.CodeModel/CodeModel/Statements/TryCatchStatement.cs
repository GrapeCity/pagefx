using System;
using System.Collections.Generic;
using DataDynamics.PageFX.Common.CodeModel.Expressions;
using DataDynamics.PageFX.Common.TypeSystem;

namespace DataDynamics.PageFX.Common.CodeModel.Statements
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
            return new object[]{_try, _fault, _finally, _catch}.EvalHashCode();
        }
    }

    public class CatchClause : Statement, ICatchClause
    {
        public CatchClause()
        {
            _body = new StatementCollection(this);
        }

    	public IType ExceptionType { get; set; }

    	public IExpression Condition { get; set; }

    	public IVariable Variable { get; set; }

    	public IStatementCollection Body
        {
            get { return _body; }
        }
        private readonly StatementCollection _body;

    	public override IEnumerable<ICodeNode> ChildNodes
        {
            get { return new ICodeNode[] { Condition, Variable, _body }; }
        }

    	public override string ToString()
        {
            return ToString(null, null);
        }

        public override bool Equals(object obj)
        {
            if (obj == this) return true;
            var c = obj as ICatchClause;
            if (c == null) return false;
            if (c.ExceptionType != ExceptionType) return false;
            if (!Equals(c.Variable, Variable)) return false;
            if (!Equals(c.Condition, Condition)) return false;
            if (!Equals(c.Body, _body)) return false;
            return true;
        }

        public override int GetHashCode()
        {
            return new object[]{ExceptionType, Variable, Condition, _body}.EvalHashCode();
        }
    }
}