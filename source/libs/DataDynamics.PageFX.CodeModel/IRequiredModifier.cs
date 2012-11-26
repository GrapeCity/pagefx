namespace DataDynamics.PageFX.CodeModel
{
    public interface IRequiredModifier : ICompoundType
    {
        IType Modifier { get; set; }
    }
}