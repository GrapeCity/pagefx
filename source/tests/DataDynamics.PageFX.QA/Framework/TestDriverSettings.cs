namespace DataDynamics.PageFX
{
    public class TestDriverSettings
    {
        public bool ExportCSharpFile = true;
        public bool SetDecompiledCode = true;
        public bool UpdateReport;

        //abc, swf, swc
        public string OutputFormat
        {
            get
            {
                if (string.IsNullOrEmpty(_outputFormat))
                    return "abc";
                return _outputFormat;
            }
            set
            {
                _outputFormat = value;
            }
        }
        string _outputFormat;

        public string OutputExtension
        {
            get
            {
                return OutputFormat.ToLower();
            }
        }

        public bool IsABC
        {
            get { return string.Compare(OutputFormat, "abc", true) == 0; }
        }

        public bool IsSWF
        {
            get { return string.Compare(OutputFormat, "swf", true) == 0; }
        }

        public bool IsSWC
        {
            get { return string.Compare(OutputFormat, "swc", true) == 0; }
        }
        
        public bool IsCancel
        {
            get
            {
                if (CancelCallback != null)
                    return CancelCallback();
                return false;
            }
        }

        public CancelCallback CancelCallback;

        public TestReport Report
        {
            get
            {
                if (_report == null)
                    _report = new TestReport();
                return _report;
            }
        }
        private TestReport _report;
    }
}