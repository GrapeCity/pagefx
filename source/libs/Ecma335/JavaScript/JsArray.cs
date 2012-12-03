using System;
using System.Collections.Generic;
using System.Linq;

namespace DataDynamics.PageFX.Ecma335.JavaScript
{
	internal sealed class JsArray : JsNode
	{
		private readonly object[] _values;
		private readonly string _separator;

		public static readonly JsArray Empty = new JsArray(new object[0]);

		public JsArray(IEnumerable<object> values) : this(values, " ")
		{			
		}

		public JsArray(IEnumerable<object> values, string separator)
		{
			if (values == null)
				throw new ArgumentNullException("values");

			_values = values.ToArray();
			_separator = separator;
		}

		public override void Write(JsWriter writer)
		{
			writer.Write("[");
			writer.WriteValues(_values, "," + _separator);
			writer.Write("]");
		}
	}
}
