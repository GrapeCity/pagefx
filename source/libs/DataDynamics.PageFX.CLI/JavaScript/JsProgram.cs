using System.Collections.Generic;
using System.Linq;

namespace DataDynamics.PageFX.CLI.JavaScript
{
	public sealed class JsProgram : JsNode
	{
		private readonly Dictionary<string, JsNamespace> _namespaces = new Dictionary<string, JsNamespace>();

		private readonly List<JsNode> _deps = new List<JsNode>();
		private readonly List<JsNode> _nodes = new List<JsNode>();
		private readonly ListEx<JsClass> _classes = new ListEx<JsClass>();
		
		internal IReadOnlyList<JsClass> Classes
		{
			get { return _classes; }
		}

		internal void DefineNamespace(string value)
		{
			if (string.IsNullOrEmpty(value)) return;

			AddNamespace(value);

			var i = value.IndexOf('.');
			while (i >= 0)
			{
				AddNamespace(value.Substring(0, i));
				i = value.IndexOf('.', i + 1);
			}
		}

		private void AddNamespace(string value)
		{
			if (_namespaces.ContainsKey(value)) return;

			_namespaces.Add(value, new JsNamespace(value));
		}

		internal void Require(string resource)
		{
			//TODO: use require("module")
			_deps.Add(new JsResource(resource));
		}

		internal void Add(JsNode node)
		{
			var klass = node as JsClass;
			if (klass != null)
			{
				_classes.Add(klass);
			}

			_nodes.Add(node);
		}

		public override void Write(JsWriter writer)
		{
			if (_deps.Count > 0)
			{
				writer.Write(_deps, "\n");
				writer.WriteLine();
			}

			if (_namespaces.Count > 0)
			{
				writer.Write(_namespaces.Values.OrderBy(x => x.Name), "\n");
				writer.WriteLine();
			}

			writer.Write(_nodes, "\n");
		}
	}
}