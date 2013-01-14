using System.Collections;
using System.Collections.Generic;
using DataDynamics.PageFX.Common.Collections;

namespace DataDynamics.PageFX.Ecma335.JavaScript
{
	internal sealed class JsBlock : JsNode, IReadOnlyList<JsNode>
	{
		private readonly List<JsNode> _kids = new List<JsNode>();

		public int Count
		{
			get { return _kids.Count; }
		}

		public JsNode this[int index]
		{
			get { return _kids[index]; }
		}

		public void Add(JsNode child)
		{
			_kids.Add(child);
		}

		public override void Write(JsWriter writer)
		{
			writer.WriteBlock(_kids, "\n");
		}

		public IEnumerator<JsNode> GetEnumerator()
		{
			return _kids.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}