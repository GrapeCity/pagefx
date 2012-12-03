using System.Collections.Generic;

namespace DataDynamics.PageFX.Ecma335.JavaScript
{
	internal sealed class JsFunction : JsNode
	{
		private readonly List<string> _parameters = new List<string>();
		private readonly JsBlock _body = new JsBlock();

		public JsFunction() : this(null)
		{
		}

		public JsFunction(string name, params string[] parameters)
		{
			Name = name;

			_parameters.AddRange(parameters);
		}

		public string Name { get; set; }

		public ICollection<string> Parameters
		{
			get { return _parameters; }
		}

		public JsBlock Body
		{
			get { return _body; }
		}

		public override void Write(JsWriter writer)
		{
			writer.Write("function");

			if (!string.IsNullOrEmpty(Name))
			{
				writer.Write(" ");
				writer.Write(Name);
			}

			writer.Write("(");
			bool sep = false;
			foreach (var parameter in _parameters)
			{
				if (sep) writer.Write(", ");
				writer.Write(parameter);
				sep = true;
			}
			writer.Write(") ");

			_body.Write(writer);
		}
	}
}