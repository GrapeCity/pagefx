using System.IO;
using System.Text;

namespace DataDynamics.PageFX.CLI.JavaScript
{
	public sealed class JsWriter : CodeTextWriter
	{
		public JsWriter()
		{
		}

		public JsWriter(TextWriter output): base(output)
		{
		}
	}

	public abstract class JsNode
	{
		public abstract void Write(JsWriter writer);

		public void Write(FileInfo output)
		{
			using (var stream = output.Create())
			using (var writer = new StreamWriter(stream, Encoding.UTF8))
			{
				Write(new JsWriter(writer));
			}
		}

		public override string ToString()
		{
			var writer = new JsWriter();
			Write(writer);
			writer.Flush();
			return writer.ToString();
		}
	}
}