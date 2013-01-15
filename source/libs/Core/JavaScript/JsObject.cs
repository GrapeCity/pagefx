using System;
using System.Collections;
using System.Collections.Generic;
using DataDynamics.PageFX.Common.JavaScript;

namespace DataDynamics.PageFX.Core.JavaScript
{
	internal sealed class JsObject : JsNode, IEnumerable<KeyValuePair<object, object>>
	{
		private readonly List<KeyValuePair<object, object>> _properties = new List<KeyValuePair<object, object>>();

		public JsObject()
		{
		}

		public JsObject(bool newLine)
		{
			NewLine = newLine;
		}

		public bool NewLine { get; set; }

		public JsObject(IEnumerable<KeyValuePair<object, object>> properties)
		{
			if (properties == null)
				throw new ArgumentNullException("properties");

			_properties.AddRange(properties);
		}

		public JsObject(params KeyValuePair<object, object>[] properties)
		{
			_properties.AddRange(properties);
		}

		public void Add(object name, object value)
		{
			if (name == null) throw new ArgumentNullException("name");
			_properties.Add(new KeyValuePair<object, object>(name, value));
		}

		public override void Write(JsWriter writer)
		{
			writer.Write("{");

			if (NewLine)
			{
				writer.WriteLine();
				writer.IncreaseIndent();
			}

			bool sep = false;
			foreach (var pair in _properties)
			{
				if (sep)
				{
					writer.Write(",");
					if (NewLine) writer.WriteLine();
				}
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

			if (NewLine)
			{
				writer.WriteLine();
				writer.DecreaseIndent();
			}

			writer.Write("}");
		}

		public IEnumerator<KeyValuePair<object, object>> GetEnumerator()
		{
			return _properties.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
