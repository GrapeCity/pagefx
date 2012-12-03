using System.Collections.Generic;
using System.Linq;

namespace DataDynamics.PageFX.Ecma335.JavaScript
{
	internal sealed class JsGlobalPool : JsNode
	{
		private const string PoolName = "$C";

		private readonly List<JsNode> _list = new List<JsNode>();
		private readonly Dictionary<object, KeyValuePair<int,JsNode>> _cache = new Dictionary<object, KeyValuePair<int,JsNode>>();
		
		public JsNode this[object key]
		{
			get
			{
				KeyValuePair<int, JsNode> value;
				if (_cache.TryGetValue(key, out value))
					return ItemRef(value.Key);
				return null;
			}
		}

		public JsNode Add(object key, JsNode value)
		{
			var pair = new KeyValuePair<int, JsNode>(_list.Count, value);
			_cache.Add(key, pair);
			_list.Add(value);
			return ItemRef(pair.Key);
		}

		private static JsNode ItemRef(int index)
		{
			return PoolName.Id().Get(index);
		}

		public override void Write(JsWriter writer)
		{
			writer.Write("{0} = ", PoolName);
			new JsArray(_list.Cast<object>()).Write(writer);
			writer.WriteLine(";");
		}
	}
}