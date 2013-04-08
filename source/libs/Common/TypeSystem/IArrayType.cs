namespace DataDynamics.PageFX.Common.TypeSystem
{
    public interface IArrayType : IType
    {
        int Rank { get; }

        ArrayDimensionCollection Dimensions { get; }
    }
}