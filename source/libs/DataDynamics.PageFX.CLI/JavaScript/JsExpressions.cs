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
		private readonly JsNode _value;

		public JsReturn(JsNode value)
		{
			_value = value;
		}

		public override void Write(JsWriter writer)
		{
			if (_value == null)
			{
				writer.Write("return null;");
				return;
			}

			writer.Write("return ");
			_value.Write(writer);
			writer.Write(";");
		}
	}

	internal sealed class JsId : JsNode
	{
		private readonly string _value;

		public JsId(string value)
		{
			_value = value;
		}

		public override void Write(JsWriter writer)
		{
			writer.Write(_value);
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
		private readonly object _value;

		public JsConst(object value)
		{
			_value = value;
		}

		public override void Write(JsWriter writer)
		{
			writer.WriteValue(_value);
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
			writer.Write("[");
			_name.Write(writer);
			writer.Write("]");
		}
	}

	internal sealed class JsBinaryOperator : JsNode
	{
		private readonly JsNode _left;
		private readonly JsNode _right;
		private readonly BinaryOperator _op;

		public JsBinaryOperator(JsNode left, JsNode right, BinaryOperator op)
		{
			_left = left;
			_right = right;
			_op = op;
		}

		public override void Write(JsWriter writer)
		{
			_left.Write(writer);
			writer.Write(_op.EnumString("c#"));
			_right.Write(writer);
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

	internal static class JsExpressionExtensions
	{
		public static JsNode Const(this object value)
		{
			return new JsConst(value);
		}

		public static JsId Id(this string value)
		{
			return new JsId(value);
		}

		public static JsId Id(this JsVar var)
		{
			return new JsId(var.Name);
		}

		public static JsVar Var(this JsNode value, string name)
		{
			return new JsVar(name, value);
		}

		public static JsNode Call(this JsNode value, params object[] args)
		{
			return new JsCall(value, args);
		}

		public static JsNode AsStatement(this JsNode value)
		{
			return new JsStatement(value);
		}

		public static JsNode Return(this JsNode value)
		{
			return new JsReturn(value);
		}

		public static JsNode New(this IType type)
		{
			return new JsNewobj(type);
		}

		public static JsNode Set(this JsNode dest, JsNode value)
		{
			return new JsBinaryOperator(dest, value, BinaryOperator.Assign).AsStatement();
		}

		public static JsNode Get(this JsNode obj, string name)
		{
			return new JsPropertyRef(obj, name);
		}

		public static JsNode Get(this JsNode obj, JsNode name)
		{
			return new JsPropertyRef(obj, name);
		}

		public static JsNode Set(this JsNode obj, string name, JsNode value)
		{
			return Get(obj, name).Set(value);
		}

		public static JsNode Set(this JsNode obj, JsNode name, JsNode value)
		{
			return Get(obj, name).Set(value);
		}
	}
}