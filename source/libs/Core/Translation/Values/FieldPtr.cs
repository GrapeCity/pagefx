using DataDynamics.PageFX.Common.TypeSystem;

namespace DataDynamics.PageFX.Core.Translation.Values
{
    class FieldPtr : IValue
    {
        public IField field;

        public FieldPtr(IField field)
        {
            this.field = field;
        }

        public IType Type
        {
            get { return field.Type; }
        }

        public ValueKind Kind
        {
            get { return ValueKind.FieldPtr; }
        }

        public bool IsPointer
        {
            get { return true; }
        }

        public bool IsMockPointer
        {
            get { return false; }
        }
        
        public override string ToString()
        {
            return string.Format("FieldPtr({0})", field.FullName);
        }
    }

    class MockFieldPtr : IValue, IDupSource
    {
        public int obj = -1; //temp register where object is stored, only for non-static fields
        public IField field;

        public MockFieldPtr(IField field)
        {
            this.field = field;
        }

        public MockFieldPtr(IField field, int obj)
        {
            this.field = field;
            this.obj = obj;
        }

        public bool IsStatic
        {
            get { return field.IsStatic; }
        }

        public bool IsInstance
        {
            get { return !field.IsStatic; }
        }

        public IType Type
        {
            get { return field.Type; }
        }

        public ValueKind Kind
        {
            get { return ValueKind.MockFieldPtr; }
        }

        public bool IsPointer
        {
            get { return true; }
        }

        public bool IsMockPointer
        {
            get { return true; }
        }

        public override string ToString()
        {
            if (field.IsStatic)
                return string.Format("MockFieldPtr({0})", field.FullName);
            return string.Format("MockFieldPtr({0}.{1})", obj, field.FullName);
        }

        public MockFieldPtr dup_source;

        public IValue DupSource
        {
            get { return dup_source; }
        }
    }
}