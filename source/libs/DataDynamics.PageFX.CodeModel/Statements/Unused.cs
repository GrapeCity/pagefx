namespace DataDynamics.PageFX.CodeModel
{
    public interface ILockStatement : IStatement
    {
        IStatementCollection Body { get; }

        IExpression Expression { get; set; }
    }
    
    public interface IFixedStatement : IStatement
    {
        IStatementCollection Body { get; }

        IExpression Expression { get; set; }

        IVariable Variable { get; set; }
    }
    
    public interface IAttachEventStatement : IStatement
    {
        IEventReferenceExpression Event { get; set; }

        IExpression Listener { get; set; }
    }

    public interface IRemoveEventStatement : IStatement
    {
        IEventReferenceExpression Event { get; set; }

        IExpression Listener { get; set; }
    }

    public interface IUsingStatement : IStatement
    {
        IExpression Variable { get; set; }
        IExpression Expression { get; set; }
        IStatementCollection Body { get; }
    }

    /// <summary>
    /// Memory block copy
    /// </summary>
    public interface IMemoryCopyStatement : IStatement
    {
        IExpression Destination { get; set; }
        IExpression Source { get; set; }
        IExpression Size { get; set; }
    }

    /// <summary>
    /// Initialization of memory block
    /// </summary>
    public interface IMemoryInitializeStatement : IStatement
    {
        IExpression Value { get; set; }
        IExpression Offset { get; set; }
        IExpression Size { get; set; }
    }
}