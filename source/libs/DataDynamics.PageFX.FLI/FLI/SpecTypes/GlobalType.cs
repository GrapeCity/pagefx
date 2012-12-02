using System;
using DataDynamics.PageFX.CodeModel;
using DataDynamics.PageFX.CodeModel.TypeSystem;

namespace DataDynamics.PageFX.FLI
{
    /// <summary>
    /// Tag for types with global ABC functions.
    /// </summary>
    internal sealed class GlobalType : ISpecialType
    {
        public GlobalType(IType type)
        {
            if (type == null)
                throw new ArgumentNullException("type");
            Type = type;
            type.Tag = this;
        }

    	public IType Type { get; private set; }

    	public override string ToString()
        {
            return Type.ToString();
        }
    }
}