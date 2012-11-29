using System.Text;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.Translation.Values
{
    internal sealed class Func : IValue
    {
        public IValue Obj;
        public IMethod Method;

        public Func(IMethod f)
        {
            Method = f;
        }

        public Func(IValue obj, IMethod f)
        {
            Obj = obj;
            Method = f;
        }

	    public IType Type
        {
            get { return SystemTypes.IntPtr; }
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