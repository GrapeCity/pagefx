namespace DataDynamics.PageFX.Common.TypeSystem
{
    public interface ICompoundType : IType
    {
        IType ElementType { get; set; }
    }
}