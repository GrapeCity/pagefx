using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.IL
{
    class Elem : IValue
    {
        public IType elemType;
        
        public Elem(IType elemType)
        {
            this.elemType = elemType;
        }

        public IType Type
        {
            get { return elemType; }
        }

        public ValueKind Kind
        {
            get { return ValueKind.Elem; }
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
            return string.Format("Elem({0})", elemType.FullName);
        }
    }
}