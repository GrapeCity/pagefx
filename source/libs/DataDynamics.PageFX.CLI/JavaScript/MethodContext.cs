using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.JavaScript
{
	internal sealed class MethodContext
	{
		public readonly JsClass Class;
		public readonly IMethod Method;
		public readonly JsPool<JsVar> Vars = new JsPool<JsVar>();

		public MethodContext(JsClass klass, IMethod method)
		{
			Class = klass;
			Method = method;
		}
	}
}