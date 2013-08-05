using System;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Flash.Abc;

namespace DataDynamics.PageFX.Flash.Core.SpecialTypes
{
    /// <summary>
    /// Tag for types with global ABC functions.
    /// </summary>
    internal sealed class GlobalFunctionsContainer : ITypeData, ISpecialType
    {
        public GlobalFunctionsContainer(IType type)
        {
            if (type == null)
                throw new ArgumentNullException("type");
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