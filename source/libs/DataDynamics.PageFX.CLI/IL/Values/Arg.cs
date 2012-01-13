using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.IL
{
    class Arg : IValue
    {
        readonly IParameter _arg;
        readonly IType _type;

        public Arg(IParameter p)
        {
            _arg = p;
            _type = TypeService.UnwrapRef(p.Type);
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