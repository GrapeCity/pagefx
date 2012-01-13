using System.Diagnostics;
using System.Text;

namespace System.IO
{
    public class DebugWriter : TextWriter
    {
        public static DebugWriter Instance
        {
            get { return _instance; }
        }
        private static readonly DebugWriter _instance = new DebugWriter();

        public override Encoding Encoding
        {
            get { return Encoding.UTF8; }
        }

        public override void Write(string value)
        {
            Debug.Write(value);
        }

        public override void WriteLine(string value)
        {
            Debug.WriteLine(value);
        }
    }
}