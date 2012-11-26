namespace DataDynamics.PageFX.CodeModel
{
    public interface INewArrayExpression : IExpression
    {
        IType ElementType { get; set; }
        IExpressionCollection Dimensions { get; }
        IExpressionCollection Initializers { get; }
    }

    public interface IArrayIndexerExpression : IExpression
    {
        IExpression Array { get; set; }
        IExpressionCollection Index { get; }
    }

    public interface IArrayLengthExpression : IExpression
    {
        IExpression Array { get; set; }
    }
}