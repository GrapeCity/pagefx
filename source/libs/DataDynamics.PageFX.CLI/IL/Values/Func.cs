using System.Text;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.IL
{
    class Func : IValue
    {
        public IValue obj;
        public IMethod method;

        public Func(IMethod f)
        {
            method = f;
        }

        public Func(IValue obj, IMethod f)
        {
            this.obj = obj;
            method = f;
        }

        #region IValue Members
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
        #endregion

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("Func: ");
            if (obj != null)
            {
                sb.Append(obj + ".");
            }
            sb.Append(method.ToString());
            return sb.ToString();
        }
    }
}