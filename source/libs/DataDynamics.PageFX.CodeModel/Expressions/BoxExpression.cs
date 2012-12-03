using System.Collections.Generic;
using DataDynamics.PageFX.Common.TypeSystem;

namespace DataDynamics.PageFX.Common.Expressions
{
    public class BoxExpression : EnclosingExpression, IBoxExpression, ITypeReferenceProvider
    {
    	public BoxExpression(IType type, IExpression e) : base(e)
        {
            SourceType = type;
        }

    	public IType SourceType { get; set; }

    	public override IType ResultType
        {
            get { return SystemTypes.Object; }
        }

    	public override string ToString()
        {
            return ToString(null, null);
        }

        public override bool Equals(object obj)
        {
            if (obj == this) return true;
            var e = obj as IBoxExpression;
            if (e == null) return false;
            if (e.SourceType != SourceType) return false;
            return base.Equals(obj);
        }

    	public IEnumerable<IType> GetTypeReferences()
        {
            return new[] { SourceType, SystemTypes.Object };
        }
    }
}