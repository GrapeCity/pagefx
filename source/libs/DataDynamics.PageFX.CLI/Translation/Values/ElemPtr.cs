using DataDynamics.PageFX.Common.TypeSystem;

namespace DataDynamics.PageFX.CLI.Translation.Values
{
    class ElemPtr : IValue
    {
        public IType arrType;
        public IType elemType;

        public ElemPtr(IType arrType, IType elemType)
        {
            this.arrType = arrType;
            this.elemType = elemType;
        }

        #region IValue Members
        public IType Type
        {
            get { return elemType; }
        }

        public ValueKind Kind
        {
            get { return ValueKind.ElemPtr; }
        }

        public bool IsPointer
        {
            get { return true; }
        }

        public bool IsMockPointer
        {
            get { return false; }
        }
        #endregion

        public override string ToString()
        {
            return string.Format("ElemPtr({0})", arrType.FullName);
        }
    }

    class MockElemPtr : IValue, IDupSource
    {
        public int arr; //temp register for array
        public int index; //temp register for index

        public IType arrType;
        public IType elemType;

        //registers of array element from which this ptr was created
        public MockElemPtr dup_source;
        
        public MockElemPtr(IType arrType, IType elemType, int arr, int index)
        {
            this.arrType = arrType;
            this.elemType = elemType;
            this.arr = arr;
            this.index = index;
        }

        public IValue DupSource
        {
            get { return dup_source; }
        }

        public IType Type
        {
            get { return elemType; }
        }

        public ValueKind Kind
        {
            get { return ValueKind.MockElemPtr; }
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
            return string.Format("MockElemPtr({0}[{1}], {2})", arr, index, arrType.FullName);
        }
    }
}