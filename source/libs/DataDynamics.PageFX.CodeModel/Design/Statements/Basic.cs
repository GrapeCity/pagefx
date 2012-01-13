using System.Collections.Generic;

namespace DataDynamics.PageFX.CodeModel
{
    /// <summary>
    /// Represents single statement within method body code
    /// </summary>
    public interface IStatement : ICodeNode
    {
        /// <summary>
        /// Gets or sets parent statement
        /// </summary>
        IStatement ParentStatement { get; set; }
    }

    /// <summary>
    /// Represents collection (block) of <see cref="IStatement"/>s.
    /// </summary>
    public interface IStatementCollection : IList<IStatement>, IStatement
    {
        void AddRange(IEnumerable<IStatement> collection);
    }

    public interface IStatementCollection<T> : IList<T>, IStatement where T : IStatement
    {
    }

    public interface ILabeledStatement : IStatement
    {
        string Name { get; set; }
        IStatement Statement { get; set; }
    }

    /// <summary>
    /// Represents statement with some expression
    /// </summary>
    public interface IExpressionStatement : IStatement
    {
        /// <summary>
        /// Gets or sets the statement expression
        /// </summary>
        IExpression Expression { get; set; }
    }

    /// <summary>
    /// Represents comment statement.
    /// </summary>
    public interface ICommentStatement : IStatement
    {
        /// <summary>
        /// Gets or sets comment text.
        /// </summary>
        string Comment { get; set; }
    }

    public interface IReturnStatement : IStatement
    {
        IExpression Expression { get; set; }
    }

    /// <summary>
    /// Represents goto statement.
    /// </summary>
    public interface IGotoStatement : IStatement
    {
        ILabeledStatement Label { get; set; }
    }

    public interface IBreakStatement : IGotoStatement
    {
    }

    public interface IContinueStatement : IGotoStatement
    {
    }

    /// <summary>
    /// Debugger break statement
    /// </summary>
    public interface IDebuggerBreakStatement : IStatement
    {
        
    }

    public interface IVariableDeclarationStatement : IStatement
    {
        IVariable Variable { get; set; }
    }

    /// <summary>
    /// Represents if-else statement
    /// </summary>
    public interface IIfStatement : IStatement
    {
        /// <summary>
        /// Gets or sets expression for condition
        /// </summary>
        IExpression Condition { get; set; }

        /// <summary>
        /// Gets or sets statements for true block.
        /// </summary>
        IStatementCollection Then { get; set; }

        /// <summary>
        /// Gets or sets statements for false block.
        /// </summary>
        IStatementCollection Else { get; set; }
    }
}