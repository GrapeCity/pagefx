namespace DataDynamics.PageFX.CodeModel
{
    public interface ITryCatchStatement : IStatement
    {
        IStatementCollection Try { get; }
        IStatementCollection CatchClauses { get; }
        IStatementCollection Fault { get;  }
        IStatementCollection Finally { get; }
    }

    public interface ICatchClause : IStatement
    {
        IType ExceptionType { get; set; }
        IExpression Condition { get; set; }
        IVariable Variable { get; set; }
        IStatementCollection Body { get; }
    }

    public interface IThrowExceptionStatement : IStatement
    {
        IExpression Expression { get; set; }
    }
}