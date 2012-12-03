using DataDynamics.PageFX.Common.TypeSystem;

namespace DataDynamics.PageFX.Ecma335.Translation.Values
{
    class Arg : IValue
    {
        readonly IParameter _arg;
        readonly IType _type;

        public Arg(IParameter p)
        {
            _arg = p;
            _type = p.Type.UnwrapRef();
        }

        public int Index
        {
            get { return _arg.Index - 1; }
        }

        public IType Type
        {
            get { return _type; }
        }

        public ValueKind Kind
        {
            get { return ValueKind.Arg; }
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
            return string.Format("Arg({0})", _arg.Name);
        }
    }
}