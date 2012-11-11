using System;
using System.Linq;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.JavaScript
{
	internal sealed class JsGeneratedField : JsClassMember
	{
		private readonly string _name;
		private readonly object _value;

		public JsGeneratedField(string name, object value)
		{
			if (string.IsNullOrEmpty(name))
				throw new ArgumentNullException("name");
			
			_name = name;
			_value = value;
		}

		public override void Write(JsWriter writer)
		{
			writer.Write(_name);
			writer.Write(" = ");
			writer.WriteValue(_value);
			writer.Write(";");
		}

		public override bool IsStatic
		{
			get { return true; }
		}
	}

	internal sealed class JsField : JsClassMember
	{
		public JsField(IField field)
		{
			Field = field;
		}

		public override bool IsStatic
		{
			get { return Field.IsStatic; }
		}

		public IField Field { get; private set; }

		public string Name
		{
			get { return Field.JsName(); }
		}

		public override void Write(JsWriter writer)
		{
			var obj = Field.IsStatic ? Field.DeclaringType.JsFullName().Id() : "this".Id();
			obj.Get(Name).Write(writer);
			writer.Write(" = ");
			writer.WriteValue(Field.Type.InitialValue());
			writer.Write(";");
		}

		public static JsField Make(IField field)
		{
			var jsField = field.Tag as JsField;
			if (jsField == null)
			{
				jsField = new JsField(field);
				field.Tag = jsField;
			}
			return jsField;
		}
	}

	internal static class SpecialFields
	{
		public const string BoxValue = "$value";
	}

	internal static class JsFieldExtensions
	{
		public static string JsFullName(this IField field)
		{
			return field.DeclaringType.JsFullName().Id().Get(field.JsName()).ToString();
		}

		public static string JsName(this IField field)
		{
			if (field.IsBoxValue()) return SpecialFields.BoxValue;

			//TODO: ensure uniqueness
			return field.Name;
		}

		public static bool IsBoxValue(this IField field)
		{
			if (field.IsStatic || field.IsConstant) return false;
			return field.DeclaringType.IsBoxableType();
		}

		public static IField GetBoxValueField(this IType type)
		{
			return type.Fields.FirstOrDefault(x => x.IsBoxValue());
		}
	}
}
