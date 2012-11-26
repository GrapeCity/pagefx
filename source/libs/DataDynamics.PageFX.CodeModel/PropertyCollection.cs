using System;
using DataDynamics.PageFX.CodeModel.Syntax;

namespace DataDynamics.PageFX.CodeModel
{
    public sealed class PropertyCollection : MultiMemberCollection<IProperty>, IPropertyCollection
    {
        public PropertyCollection(IType owner) : base(owner)
        {
        }

    	public CodeNodeType NodeType
        {
            get { return CodeNodeType.Properties; }
        }

        public object Tag { get; set; }

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