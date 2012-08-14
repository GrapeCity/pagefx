namespace DataDynamics.PageFX.CLI.JavaScript
{
	internal sealed class JsNamespace : JsNode
	{
		public JsNamespace(string value)
		{
			Name = value;
		}

		public string Name { get; private set; }

		public override void Write(JsWriter writer)
		{
			//TODO: if not defined
			writer.WriteLine("{0} = {{}};", Name);
		}
	}
}