using System;
using System.IO;

namespace DataDynamics.PageFX.CLI.JavaScript
{
	internal sealed class JsResource : JsNode
	{
		private readonly string _text;

		public JsResource(string resourceName)
		{
			var stream = typeof(JsCompiler).Assembly.GetResourceStream(resourceName);
			if (stream == null)
				throw new ResourceNotFoundException(resourceName);
			_text = stream.ReadText();
		}

		public override void Write(JsWriter writer)
		{
			writer.WriteLine(_text);
		}
	}

	internal sealed class ResourceNotFoundException : Exception
	{
		private readonly string _resourceName;

		public ResourceNotFoundException(string resourceName)
			: base(string.Format("Embedded resource '{0}' was not found.", resourceName))
		{
			_resourceName = resourceName;
		}

		public string ResourceName
		{
			get { return _resourceName; }
		}
	}
}