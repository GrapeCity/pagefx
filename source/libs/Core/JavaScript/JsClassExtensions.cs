namespace DataDynamics.PageFX.Core.JavaScript
{
	internal static class JsClassExtensions
	{
		public static void ExtendPrototype(this JsClass klass, JsFunction func, string name)
		{
			var type = klass.Type;

			var typeName = type.IsString() ? "String" : type.JsFullName();
			var fullName = typeName + ".prototype." + name;

			klass.Add(new JsGeneratedMethod(fullName, func));
		}
	}
}
