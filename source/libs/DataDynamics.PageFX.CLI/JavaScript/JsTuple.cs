namespace DataDynamics.PageFX.CLI.JavaScript
{
	internal sealed class JsTuple : JsNode
	{
		private readonly object[] _values;

		public JsTuple(params object[] values)
		{
			_values = values;
		}

		public override void Write(JsWriter writer)
		{
			writer.WriteValues(_values, ", ");
		}
	}
}