using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Flash.Abc;

namespace DataDynamics.PageFX.Flash.Core.SpecialTypes
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

	    public bool IsDefined(AbcFile abc)
	    {
		    return true;
	    }

	    public ITypeData Import(AbcFile abc)
	    {
		    return this;
	    }

	    public override string ToString()
        {
            return Type.ToString();
        }
    }
}