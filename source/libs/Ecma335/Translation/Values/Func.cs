using System.Text;
using DataDynamics.PageFX.Common.TypeSystem;

namespace DataDynamics.PageFX.Ecma335.Translation.Values
{
    internal sealed class Func : IValue
    {
        public IValue Obj;
        public IMethod Method;
	    private readonly IType _type;

	    public Func(IMethod f, IType type)
        {
	        Method = f;
	        _type = type;
        }

	    public Func(IValue obj, IMethod f)
        {
            Obj = obj;
            Method = f;
        }

	    public IType Type
        {
			get { return _type; }
        }

        public ValueKind Kind
        {
            get { return ValueKind.Function; }
        }

        public bool IsPointer
        {
            get { return false; }
        }

        public bool IsMockPointer
        {
            get { return false; }
        }

	    public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("Func: ");
            if (Obj != null)
            {
                sb.Append(Obj + ".");
            }
            sb.Append(Method);
            return sb.ToString();
        }
    }
}