using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.JavaScript
{
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
			var obj = Field.IsStatic ? Field.DeclaringType.FullName.Id() : "this".Id();
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

	internal static class JsFieldExtensions
	{
		public static string JsFullName(this IField field)
		{
			return field.DeclaringType.FullName + "." + field.JsName();
		}

		public static string JsName(this IField field)
		{
			//TODO: ensure uniqueness
			return field.Name;
		}
	}
}
