namespace DataDynamics.PageFX.CodeModel
{
    public interface INewDelegateExpression : IExpression
    {
        IType DelegateType { get; set; }
        IMethodReferenceExpression Method { get; set; }
    }

    public interface IDelegateInvokeExpression : IExpression
    {
        IExpression Target { get; set; }
        IMethod Method { get; set; }
        IExpressionCollection Arguments { get; }
    }

    public interface IAnonymousMethodExpression : IExpression
    {
        IStatementCollection Body { get; }

        IType DelegateType { get; set; }

        IParameterCollection Parameters { get; }

        IType ReturnType { get; set; }
    }

    public interface ILambdaExpression : IExpression
    {
        IVariableCollection Parameters { get; }
        IExpression Body { get; set; }
    }
}