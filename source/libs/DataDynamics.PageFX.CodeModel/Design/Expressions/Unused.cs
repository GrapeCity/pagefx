namespace DataDynamics.PageFX.CodeModel
{
    public interface IMemberInitializerExpression : IExpression
    {
        ITypeMember Member { get; set; }

        IExpression Value { get; set; }
    }

    public interface INullCoalescingExpression : IExpression
    {
        IExpression Condition { get; set; }

        IExpression Expression { get; set; }
    }

    public interface ITypedReferenceCreateExpression : IExpression
    {
        IExpression Expression { get; set; }
    }

    public interface ITypeOfTypedReferenceExpression : IExpression
    {
        IExpression Expression { get; set; }
    }

    public interface IValueOfTypedReferenceExpression : IExpression
    {
        IExpression Expression { get; set; }

        IType TargetType { get; set; }
    }
}