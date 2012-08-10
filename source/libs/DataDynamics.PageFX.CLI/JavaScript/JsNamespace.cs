namespace DataDynamics.PageFX.CLI.JavaScript
{
	internal sealed class JsNamespace : JsNode
	{
		private readonly string _value;

		public JsNamespace(string value)
		{
			_value = value;
		}

		public override void Write(JsWriter writer)
		{
			//TODO: if not defined
			writer.WriteLine("{0} = {{}};", _value);
		}
	}
}