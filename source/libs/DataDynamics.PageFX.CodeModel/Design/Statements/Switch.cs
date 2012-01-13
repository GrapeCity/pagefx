namespace DataDynamics.PageFX.CodeModel
{
    public interface ISwitchStatement : IStatement
    {
        IExpression Expression { get; set; }
        IStatementCollection<ISwitchCase> Cases { get; }
    }

    public interface ISwitchCase : IStatement
    {
        int From { get; set; }
        int To { get; set; }
        IStatementCollection Body { get; }
    }
}