using System.Collections.Generic;

namespace DataDynamics.PageFX.CodeModel
{
    public class UnboxExpression : EnclosingExpression, IUnboxExpression, ITypeReferenceProvider
    {
    	public UnboxExpression(IType type, IExpression e) : base(e)
        {
            TargetType = type;
        }

    	public IType TargetType { get; set; }

    	public override IType ResultType
        {
            get { return TargetType; }
        }

    	public override bool Equals(object obj)
        {
            if (obj == this) return true;
            var e = obj as IUnboxExpression;
            if (e == null) return false;
            if (e.TargetType != TargetType) return false;
            return base.Equals(obj);
        }

    	public IEnumerable<IType> GetTypeReferences()
        {
            return new[] { TargetType };
        }
    }
}