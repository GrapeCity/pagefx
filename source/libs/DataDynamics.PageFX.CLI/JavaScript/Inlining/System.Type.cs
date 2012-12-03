namespace DataDynamics.PageFX.Ecma335.JavaScript.Inlining
{
	internal sealed class TypeInlines : InlineCodeProvider
	{
		[InlineImpl]
		public static void CreateInstanceDefault(JsBlock code)
		{
			code.Add("this".Id().Get("$new").Call().Return());
		}

		[InlineImpl]
		public static void get_ElementType(JsBlock code)
		{
			code.Add(new JsText("if (this.$elemType === undefined) return null;"));
			code.Add(new JsText("return $types[this.$elemType]();"));
		}

		[InlineImpl]
		public static void get_UnderlyingSystemType(JsBlock code)
		{
			code.Add(new JsText("if (this.$valueType === undefined) return null;"));
			code.Add(new JsText("return $types[this.$valueType]();"));
		}

		[InlineImpl]
		public static void get_FullName(JsBlock code)
		{
			code.Add(new JsText("return this.ns ? this.ns + '.' + this.name : this.name;"));
		}
	}
}