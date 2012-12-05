using System;

namespace DataDynamics.PageFX.TestRunner.Framework
{
    public class TestDriverSettings
    {
        public bool ExportCSharpFile = true;
    	public bool UpdateReport;
    	public bool IsClrEmulation;

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

		public bool IsJavaScript
		{
			get { return string.Compare(OutputFormat, "js", true) == 0; }
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
            get { return CancelCallback != null && CancelCallback(); }
        }

        public Func<bool> CancelCallback;

        public TestReport Report
        {
            get { return _report ?? (_report = new TestReport()); }
        }
        private TestReport _report;
    }
}