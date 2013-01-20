using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.FlashLand.Abc;

namespace DataDynamics.PageFX.FlashLand.Core.SpecialTypes
{
    internal sealed class InternalType : ITypeData, ISpecialType
    {
        public InternalType(IType type)
        {
            Type = type;
        }

	    public IType Type { get; private set; }

	    public AbcMultiname Name
	    {
			get { return null; }
	    }

	    public override string ToString()
        {
            return Type.ToString();
        }
    }
}