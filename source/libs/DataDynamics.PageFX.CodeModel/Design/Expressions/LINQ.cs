using System.Collections.Generic;

namespace DataDynamics.PageFX.CodeModel
{
    public interface IQueryExpression : IExpression
    {
        IQueryBody Body { get; set; }

        IFromClause From { get; set; }
    }

    public interface IQueryBody : IExpression
    {
        IQueryClauseCollection Clauses { get; }

        IQueryContinuation Continuation { get; set; }

        IQueryOperation Operation { get; set; }
    }

    public interface IQueryClause : IExpression
    {
    }

    public interface IQueryClauseCollection : IList<IQueryClause>
    {
    }

    public interface IFromClause : IQueryClause
    {
        IExpression Expression { get; set; }

        IVariable Variable { get; set; }
    }

    public interface IJoinClause : IQueryClause
    {
        IExpression Equality { get; set; }

        IExpression In { get; set; }

        IVariable Into { get; set; }

        IExpression On { get; set; }

        IVariable Variable { get; set; }
    }

    public interface ILetClause : IQueryClause
    {
        IExpression Expression { get; set; }

        IVariable Variable { get; set; }
    }

    public interface IOrderClause : IQueryClause
    {
        OrderDirection Direction { get; set; }

        IExpression Expression { get; set; }
    }

    public interface IWhereClause : IQueryClause
    {
        IExpression Expression { get; set; }
    }

    public interface IQueryOperation : IExpression
    {
    }

    public interface ISelectOperation : IQueryOperation
    {
        IExpression Expression { get; set; }
    }

    public interface IGroupOperation : IQueryOperation
    {
        IExpression Item { get; set; }

        IExpression Key { get; set; }
    }

    public interface IQueryContinuation : IExpression
    {
        IQueryBody Body { get; set; }

        IVariable Variable { get; set; }
    }
}