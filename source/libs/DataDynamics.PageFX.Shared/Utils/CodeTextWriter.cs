using System;
using System.IO;
using System.Text;

namespace DataDynamics
{
    public class CodeTextWriter : TextWriter
    {
        readonly Encoding _encoding = Encoding.UTF8;
        readonly TextWriter _writer;
        Indent _indent = new Indent();

        public CodeTextWriter(TextWriter baseWriter, Encoding encoding)
        {
            if (baseWriter == null)
                throw new ArgumentNullException("baseWriter");
            _writer = baseWriter;
            if (encoding != null)
                _encoding = encoding;
        }

        public CodeTextWriter(TextWriter baseWriter)
            : this(baseWriter, null)
        {
        }

        public CodeTextWriter()
            : this(new StringWriter())
        {
        }

        public override Encoding Encoding
        {
            get { return _encoding; }
        }

        public void IncreaseIndent()
        {
            ++_indent;
        }

        public void DecreaseIndent()
        {
            --_indent;
        }

        public override void WriteLine(string value)
        {
            if (!string.IsNullOrEmpty(value))
                value = _indent + value;
            _writer.WriteLine(value);
        }

        public void EndBlock()
        {
            DecreaseIndent();
            WriteLine("}");
        }

        public override string ToString()
        {
            var sw = _writer as StringWriter;
            if (sw != null)
                return sw.ToString();
            return base.ToString();
        }
    }
}