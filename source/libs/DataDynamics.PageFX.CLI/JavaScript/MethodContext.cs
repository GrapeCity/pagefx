using DataDynamics.PageFX.CLI.IL;
using DataDynamics.PageFX.Common.TypeSystem;

namespace DataDynamics.PageFX.CLI.JavaScript
{
	internal sealed class MethodContext
	{
		public readonly JsCompiler Host;
		public readonly JsClass Class;
		public readonly IMethod Method;
		public readonly JsPool<JsVar> Vars = new JsPool<JsVar>();
		public readonly TryCatchBlock[] ProtectedBlocks;
		
		public MethodContext(JsCompiler host, JsClass klass, IMethod method, TryCatchBlock[] blocks)
		{
			ProtectedBlocks = blocks;
			Host = host;
			Class = klass;
			Method = method;
		}
	}
}