using System;
using DataDynamics.PageFX.Common.TypeSystem;

namespace DataDynamics.PageFX.FlashLand.Core.SpecialTypes
{
    /// <summary>
    /// Tag for types with global ABC functions.
    /// </summary>
    internal sealed class GlobalFunctionsContainer : ISpecialType
    {
        public GlobalFunctionsContainer(IType type)
        {
            if (type == null)
                throw new ArgumentNullException("type");
            Type = type;
        }

    	public IType Type { get; private set; }

    	public override string ToString()
        {
            return Type.ToString();
        }
    }
}