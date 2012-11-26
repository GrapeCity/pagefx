namespace DataDynamics.PageFX.CodeModel
{
    public interface IOptionalModifier : ICompoundType
    {
        IType Modifier { get; set; }
    }
}