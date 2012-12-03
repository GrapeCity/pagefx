using System.Collections.Generic;
using DataDynamics.PageFX.Common.Services;
using DataDynamics.PageFX.Common.TypeSystem;

namespace DataDynamics.PageFX.Common.Expressions
{
    public sealed class CastExpression : EnclosingExpression, ICastExpression, ITypeReferenceProvider
    {
    	public CastExpression(IType targetType, IExpression e, CastOperator op) : base(e)
        {
            TargetType = targetType;
            SourceType = e.ResultType;
            Operator = op;
        }

    	public IType SourceType { get; private set; }

    	public IType TargetType { get; set; }

    	public CastOperator Operator { get; set; }

    	public override IType ResultType
        {
            get
            {
                if (Operator == CastOperator.Is)
                    return SystemTypes.Boolean;
                return TargetType;
            }
        }

    	public override bool Equals(object obj)
        {
            if (obj == this) return true;
            var e = obj as ICastExpression;
            if (e == null) return false;
            if (e.Operator != Operator) return false;
            if (e.TargetType != TargetType) return false;
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            int h = Operator.GetHashCode();
            if (TargetType != null)
                h ^= TargetType.GetHashCode();
            return h ^ base.GetHashCode();
        }

    	public IEnumerable<IType> GetTypeReferences()
        {
            return new[] { TargetType };
        }
    }
}