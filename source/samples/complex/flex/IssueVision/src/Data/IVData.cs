namespace IssueVision
{
    public class IVData
    {
        public IVDataSet Issues
        {
            get { return _issues; }
            set { _issues = value; }
        }
        private IVDataSet _issues;

        public IVDataSet Lookup
        {
            get { return _lookup; }
            set { _lookup = value; }
        }
        private IVDataSet _lookup;

        public bool IsLoaded
        {
            get
            {
                return _issues != null && _lookup != null;
            }
        }
    }
}