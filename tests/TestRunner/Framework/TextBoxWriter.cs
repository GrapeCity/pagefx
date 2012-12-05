using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace DataDynamics.PageFX.TestRunner.Framework
{
    /// <summary>
    /// A <see cref="System.IO.TextWriter"/> that writes to a text box
    /// </summary>
    public class TextBoxWriter : TextWriter
    {
        private delegate void AppendTextDelegate(string message);
        private readonly TextBoxBase textBox;

        /// <summary>
        /// creates a new instance of the class
        /// </summary>
        /// <param name="b">The <see cref="TextBoxBase"/> that will be written to</param>
        public TextBoxWriter(TextBoxBase b)
        {
            if (b == null)
                throw new ArgumentNullException("b");

            textBox = b;
        }

        /// <summary>
        /// Get's the encoding of the stream
        /// </summary>
        public override Encoding Encoding
        {
            get { return Encoding.UTF8; }
        }

        public override void Write(char value)
        {
            Write(value.ToString());
        }

        /// <summary>
        /// Write a string to the trace window
        /// </summary>
        /// <param name="message">The string to write</param>
        public override void Write(string message)
        {
            if (textBox.InvokeRequired)
                textBox.BeginInvoke(new AppendTextDelegate(textBox.AppendText), message);
            else
                textBox.AppendText(message);
        }

        /// <summary>
        /// Writes a string and a new line to the text box
        /// </summary>
        /// <param name="message">The string to write</param>
        public override void WriteLine(string message)
        {
            Write(string.Format("{0}\n", message));
        }
    }
}