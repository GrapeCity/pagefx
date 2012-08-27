using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.JavaScript
{
	internal sealed class MethodContext
	{
		public readonly JsCompiler Host;
		public readonly JsClass Class;
		public readonly IMethod Method;
		public readonly JsPool<JsVar> Vars = new JsPool<JsVar>();
		
		public MethodContext(JsCompiler host, JsClass klass, IMethod method)
		{
			Host = host;
			Class = klass;
			Method = method;
		}

		public JsGlobalPool Pool
		{
			get { return Host.Pool; }
		}
	}
}