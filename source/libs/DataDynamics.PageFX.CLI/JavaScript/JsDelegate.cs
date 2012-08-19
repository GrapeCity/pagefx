namespace DataDynamics.PageFX.CLI.JavaScript
{
	internal sealed class JsDelegate
	{
		public static void CreateInstanceImpl(JsClass klass)
		{
			var func = new JsFunction(null);
			func.Body.Add(klass.Type.New().Return());
			klass.Add(new JsGeneratedMethod(string.Format("{0}.prototype.CreateInstance", klass.Type.JsFullName()), func));
		}
	}
}