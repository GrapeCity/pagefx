using System;
using System.IO;
using System.Text;

namespace DataDynamics.PageFX.Common.Utilities
{
    public class CodeTextWriter : TextWriter
    {
		private readonly TextWriter _writer;
	    private string _indentChars = "\t";
		private string _indent = "";
	    private bool _doindent;

        public CodeTextWriter(TextWriter baseWriter)
        {
            if (baseWriter == null)
                throw new ArgumentNullException("baseWriter");

            _writer = baseWriter;
        }

        public CodeTextWriter() : this(new StringWriter())
        {
        }

	    public string IndentChars
	    {
			get { return _indentChars; }
			set { _indentChars = string.IsNullOrEmpty(value) ? "\t" : value; }
	    }

        public override Encoding Encoding
        {
            get { return _writer.Encoding; }
        }

		/// <summary>
		/// Increases the current indent level by one.
		/// </summary>
		public void Indent()
        {
			_indent += _indentChars;
	        _doindent = true;
        }

		/// <summary>
		/// Decreases the current indent level by one.
		/// </summary>
        public void Unindent()
        {
	        _indent = _indent.Length - _indentChars.Length > 0
		                  ? _indent.Substring(0, _indent.Length - _indentChars.Length)
		                  : "";
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

	    public override string ToString()
        {
            var sw = _writer as StringWriter;
            if (sw != null)
                return sw.ToString();
            return base.ToString();
        }
    }
}