namespace DataDynamics.PageFX.CodeModel
{
    public enum LoopType
    {
        Endless,
        PreTested,
        PostTested,
    }

    public interface ILoopStatement : IStatement
    {
        LoopType LoopType { get; set; }
        IExpression Condition { get; set; }
        IStatementCollection Body { get; }
    }

    public interface IForStatement : IStatement
    {
        IStatement Initializer { get; set; }
        IExpression Condition { get; set; }
        IStatement Increment { get; set; }
        IStatementCollection Body { get; }
    }
    
    public interface IForEachStatement : IStatement
    {
        IVariable Variable { get; set; }
        IExpression Expression { get; set; }
        IStatementCollection Body { get; }
    }
}