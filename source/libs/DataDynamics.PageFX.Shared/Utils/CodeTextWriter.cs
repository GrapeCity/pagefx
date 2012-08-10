using System;
using System.IO;
using System.Text;

namespace DataDynamics
{
    public class CodeTextWriter : TextWriter
    {
        private readonly Encoding _encoding = Encoding.UTF8;
		private readonly TextWriter _writer;
		private Indent _indent = new Indent();
	    private bool _doindent;

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
	        _doindent = true;
        }

        public void DecreaseIndent()
        {
			--_indent;
			_doindent = true;
        }

	    public override void Flush()
	    {
		    _writer.Flush();
	    }

	    public override void Write(char value)
	    {
			if (_doindent && _indent.Length > 0)
			{
				var str = _indent + value;
				_doindent = false;
				_writer.Write(str);
			}
			else
			{
				_writer.Write(value);
			}

		    if (value == '\n')
				_doindent = true;
	    }

	    public override void Write(string value)
	    {
			if (string.IsNullOrEmpty(value)) return;

			if (_doindent && _indent.Length > 0)
			{
				value = _indent + value;
				_doindent = false;
			}

			_writer.Write(value);

			if (value.EndsWith("\n"))
				_doindent = true;
	    }

	    public override void WriteLine()
	    {
		    _doindent = false;
			_writer.WriteLine();
			_doindent = true;
	    }

	    public override void WriteLine(string value)
        {
			if (string.IsNullOrEmpty(value)) return;

			if (_doindent && _indent.Length > 0)
			{
				value = _indent + value;
				_doindent = false;
			}

		    _writer.WriteLine(value);

		    _doindent = true;
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