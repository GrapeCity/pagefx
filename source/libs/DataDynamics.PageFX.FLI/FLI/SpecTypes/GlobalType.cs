using System;
using DataDynamics.PageFX.CodeModel;
using DataDynamics.PageFX.FLI.ABC;

namespace DataDynamics.PageFX.FLI
{
    /// <summary>
    /// Tag for types with global ABC functions.
    /// </summary>
    internal class GlobalType : ISpecialType
    {
        public GlobalType(IType type)
        {
            if (type == null)
                throw new ArgumentNullException("type");
            _type = type;
            type.Tag = this;
        }

        public IType Type
        {
            get { return _type; }
        }
        private readonly IType _type;

        public override string ToString()
        {
            return _type.ToString();
        }
    }
}