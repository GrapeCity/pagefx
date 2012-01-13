namespace DataDynamics.PageFX.CodeModel
{
    public interface ICompoundType : IType
    {
        IType ElementType { get; set; }
    }
}