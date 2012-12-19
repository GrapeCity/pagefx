using System;
using DataDynamics.PageFX.Common.Syntax;

namespace DataDynamics.PageFX.Common.TypeSystem
{
    public sealed class PropertyCollection : MultiMemberCollection<IProperty>, IPropertyCollection
    {
        public PropertyCollection(IType owner) : base(owner)
        {
        }

	    public object Data { get; set; }

    	public string ToString(string format, IFormatProvider formatProvider)
        {
            return SyntaxFormatter.Format(this, format, formatProvider);
        }

    	public override string ToString()
        {
            return ToString(null, null);
        }
    }
}