using System;
using System.Collections.Generic;
using DataDynamics.PageFX.CodeModel;
using DataDynamics.PageFX.CodeModel.Syntax;

namespace DataDynamics.PageFX.CLI.JavaScript
{
	internal sealed class JsText : JsNode
	{
		private readonly string _text;

		public JsText(string text)
		{
			_text = text;
		}

		public override void Write(JsWriter writer)
		{
			writer.Write(_text);
		}
	}

	internal sealed class JsCall : JsNode
	{
		private readonly JsNode _value;
		private readonly IEnumerable<object> _args;

		public JsCall(JsNode value, IEnumerable<object> args)
		{
			if (value == null)
				throw new ArgumentNullException("value");

			_value = value;
			_args = args;
		}

		public JsCall(string value, IEnumerable<object> args)
			: this(new JsId(value), args)
		{
		}

		public override void Write(JsWriter writer)
		{
			_value.Write(writer);
			writer.Write("(");
			writer.WriteValues(_args, ", ");
			writer.Write(")");
		}
	}

	internal sealed class JsStatement : JsNode
	{
		private readonly JsNode _expression;

		public JsStatement(JsNode expression)
		{
			_expression = expression;
		}

		public override void Write(JsWriter writer)
		{
			_expression.Write(writer);
			writer.Write(";");
		}
	}

	internal sealed class JsReturn : JsNode
	{
		private readonly object _value;

		public JsReturn(object value)
		{
			_value = value;
		}

		public override void Write(JsWriter writer)
		{
			writer.Write("return ");
			writer.WriteValue(_value);
			writer.Write(";");
		}
	}

	internal sealed class JsId : JsNode
	{
		public JsId(string value)
		{
			Value = value;
		}

		public string Value { get; private set; }

		public override void Write(JsWriter writer)
		{
			writer.Write(Value);
		}
	}

	internal interface IJsNameValuePair
	{
		string Name { get; set; }

		JsNode Value { get; set; }
	}

	internal sealed class JsVar : JsNode, IJsNameValuePair
	{
		public JsVar()
		{
		}

		public JsVar(string name, JsNode value)
		{
			Name = name;
			Value = value;
		}

		public string Name { get; set; }

		public JsNode Value { get; set; }

		public override void Write(JsWriter writer)
		{
			writer.Write("var {0} = ", Name);
			Value.Write(writer);
			writer.Write(";");
		}
	}

	internal sealed class JsProperty : JsNode, IJsNameValuePair
	{
		public JsProperty()
		{
		}

		public JsProperty(string name, JsNode value)
		{
			Name = name;
			Value = value;
		}

		public string Name { get; set; }

		public JsNode Value { get; set; }

		public override void Write(JsWriter writer)
		{
			writer.Write("{0}: ", Name);
			Value.Write(writer);
		}
	}

	internal sealed class JsConst : JsNode
	{
		public JsConst(object value)
		{
			Value = value;
		}

		public object Value { get; private set; }

		public override void Write(JsWriter writer)
		{
			writer.WriteValue(Value);
		}
	}

	internal sealed class JsPropertyRef : JsNode
	{
		private readonly JsNode _obj;
		private readonly JsNode _name;

		public JsPropertyRef(JsNode obj, JsNode name)
		{
			_obj = obj;
			_name = name;
		}

		public JsPropertyRef(JsNode obj, string name)
			: this(obj, new JsConst(name))
		{
		}

		public override void Write(JsWriter writer)
		{
			_obj.Write(writer);

			var c = _name as JsConst;
			if (c != null && c.Value is String && ((String)c.Value).IsValidId())
			{
				writer.Write(".");
				writer.Write((String)c.Value);
			}
			else
			{
				writer.Write("[");
				_name.Write(writer);
				writer.Write("]");
			}
		}
	}

	internal sealed class JsBinaryOperator : JsNode
	{
		private readonly JsNode _left;
		private readonly object _value;
		private readonly string _op;

		public JsBinaryOperator(JsNode left, object value, BinaryOperator op)
		{
			_left = left;
			_value = value;
			_op = op.EnumString("c#");
		}

		public JsBinaryOperator(JsNode left, object value, string op)
		{
			_left = left;
			_value = value;
			_op = op;
		}

		public override void Write(JsWriter writer)
		{
			_left.Write(writer);
			writer.Write(" ");
			writer.Write(_op);
			writer.Write(" ");
			writer.WriteValue(_value);
		}
	}

	internal sealed class JsNew : JsNode
	{
		private readonly JsNode _value;

		public JsNew(JsNode value)
		{
			_value = value;
		}

		public override void Write(JsWriter writer)
		{
			writer.Write("new ");
			_value.Write(writer);
		}
	}

	internal sealed class JsNewobj : JsNode
	{
		private readonly IType _type;

		public JsNewobj(IType type)
		{
			_type = type;
		}

		public override void Write(JsWriter writer)
		{
			writer.Write("new {0}()", _type.FullName);
		}
	}
}