using System;
using System.Collections;
using System.Collections.Generic;

namespace DataDynamics.PageFX.CLI.JavaScript
{
	using Property = KeyValuePair<string, object>;

	internal sealed class JsObject : JsNode, IEnumerable<Property>
	{
		private readonly List<Property> _properties = new List<Property>();

		public JsObject()
		{
		}

		public JsObject(IEnumerable<Property> properties)
		{
			if (properties == null)
				throw new ArgumentNullException("properties");

			_properties.AddRange(properties);
		}

		public JsObject(params Property[] properties)
		{
			_properties.AddRange(properties);
		}

		public void Add(string name, object value)
		{
			_properties.Add(new Property(name, value));
		}

		public override void Write(JsWriter writer)
		{
			writer.Write("{");
			bool sep = false;
			foreach (var pair in _properties)
			{
				if (sep) writer.Write(",");
				//TODO: check valid id, otherwise write key as string
				writer.Write("{0}:", pair.Key);
				writer.WriteValue(pair.Value);
				sep = true;
			}
			writer.Write("}");
		}

		public IEnumerator<Property> GetEnumerator()
		{
			return _properties.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
