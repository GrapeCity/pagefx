namespace DataDynamics.PageFX.Core.JavaScript
{
	internal sealed class JsDelegate
	{
		public static void CreateInstanceImpl(JsClass klass)
		{
			var func = new JsFunction(null);
			func.Body.Add(klass.Type.New().Return());

			klass.ExtendPrototype(func, "CreateInstance");
		}
	}
}