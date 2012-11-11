using System;
using System.Collections;
using System.Collections.Generic;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.JavaScript
{
	using Property = KeyValuePair<object, object>;

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

		public void Add(object name, object value)
		{
			if (name == null) throw new ArgumentNullException("name");
			_properties.Add(new Property(name, value));
		}

		public override void Write(JsWriter writer)
		{
			writer.Write("{");
			bool sep = false;
			foreach (var pair in _properties)
			{
				if (sep) writer.Write(",");
				var key = pair.Key;

				var name = key as string;
				if (name != null)
				{
					if (name.IsJsid()) writer.Write("{0}:", name);
					else writer.Write("'{0}':", name.JsEscape());
				}
				else
				{
					writer.WriteValue(key);
					writer.Write(":");
				}
				
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
