namespace DataDynamics.PageFX.CodeModel.TypeSystem
{
    public interface ICompoundType : IType
    {
        IType ElementType { get; set; }
    }
}