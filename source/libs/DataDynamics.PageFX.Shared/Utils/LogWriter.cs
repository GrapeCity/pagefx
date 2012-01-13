using System.IO;

namespace DataDynamics
{
    public class LogWriter
    {
        private readonly TextWriter _writer;

        public LogWriter(TextWriter writer)
        {
            _writer = writer;
        }

        public void Info(string msg)
        {
            _writer.WriteLine("INFO: {0}", msg);
        }

        public void Info(string format, params object[] args)
        {
            string msg = string.Format(format, args);
            _writer.WriteLine("INFO: {0}", msg);
        }

        public void Error(string msg)
        {
            _writer.WriteLine("ERROR: {0}", msg);
        }

        public void Error(string format, params object[] args)
        {
            string msg = string.Format(format, args);
            _writer.WriteLine("ERROR: {0}", msg);
        }

        public void Warning(string msg)
        {
            _writer.WriteLine("WARNING: {0}", msg);
        }

        public void Warning(string format, params object[] args)
        {
            string msg = string.Format(format, args);
            _writer.WriteLine("WARNING: {0}", msg);
        }
    }
}