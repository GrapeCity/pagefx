using DataDynamics.PageFX.Common.TypeSystem;

namespace DataDynamics.PageFX.FlashLand.Core.SpecialTypes
{
    internal class InternalType : ISpecialType
    {
        public InternalType(IType type)
        {
            Type = type;
        }

	    public IType Type { get; private set; }

	    public override string ToString()
        {
            return Type.ToString();
        }
    }
}