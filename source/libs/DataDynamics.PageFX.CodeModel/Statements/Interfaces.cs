using System.Collections.Generic;
using DataDynamics.PageFX.CodeModel.Expressions;
using DataDynamics.PageFX.CodeModel.TypeSystem;

namespace DataDynamics.PageFX.CodeModel.Statements
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

	public interface ITryCatchStatement : IStatement
	{
		IStatementCollection Try { get; }
		IStatementCollection CatchClauses { get; }
		IStatementCollection Fault { get; }
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

	// TODO: consider to remove below interfaces with impls

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