namespace DataDynamics.PageFX.CodeModel
{
    /// <summary>
    /// *exp
    /// </summary>
    public interface IAddressDereferenceExpression : IEnclosingExpression
    {
        IType Type { get; set; }
    }

    /// <summary>
    /// &exp
    /// </summary>
    public interface IAddressOfExpression : IEnclosingExpression
    {
    }

    /// <summary>
    /// out exp
    /// </summary>
    public interface IAddressOutExpression : IEnclosingExpression
    {
    }

    /// <summary>
    /// ref exp
    /// </summary>
    public interface IAddressRefExpression : IEnclosingExpression
    {
    }
}