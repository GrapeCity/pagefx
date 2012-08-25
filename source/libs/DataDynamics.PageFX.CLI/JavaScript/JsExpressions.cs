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
			bool br = _value is JsFunction;
			if (br) writer.Write("(");
			_value.Write(writer);
			if (br) writer.Write(")");
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

	internal sealed class JsPropertyRef : JsNode
	{
		private readonly JsNode _obj;
		private readonly object _name;

		public JsPropertyRef(JsNode obj, object name)
		{
			_obj = obj;
			_name = name;
		}

		public override void Write(JsWriter writer)
		{
			_obj.Write(writer);

			var s = _name as String;
			if (s != null && s.IsJsid())
			{
				writer.Write(".");
				writer.Write(s);
			}
			else
			{
				writer.Write("[");
				writer.WriteValue(_name);
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

	internal sealed class JsNewobj : JsNode
	{
		private readonly IType _type;

		public JsNewobj(IType type)
		{
			_type = type;
		}

		public override void Write(JsWriter writer)
		{
			writer.Write("new {0}()", _type.JsFullName());
		}
	}

	internal sealed class JsAnd : JsNode
	{
		public JsAnd(params object[] args)
		{
			Args = args == null ? new List<object>() : new List<object>(args);
		}

		public IList<object> Args { get; private set; }

		public override void Write(JsWriter writer)
		{
			writer.WriteValues(Args, " && ");
		}
	}

	internal sealed class JsOr : JsNode
	{
		public JsOr(params object[] args)
		{
			Args = args == null ? new List<object>() : new List<object>(args);
		}

		public IList<object> Args { get; private set; }

		public override void Write(JsWriter writer)
		{
			writer.WriteValues(Args, " || ");
		}
	}
}