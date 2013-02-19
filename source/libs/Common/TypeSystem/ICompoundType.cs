namespace DataDynamics.PageFX.Common.TypeSystem
{
    public interface ICompoundType : IType
    {
		/// <summary>
		/// Gets element type.
		/// </summary>
        IType ElementType { get; }
    }
}