using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Ecma335.IL;

namespace DataDynamics.PageFX.Ecma335.JavaScript
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

	internal static class MethodContextExtensions
	{
		public static IType ResolveSystemType(this MethodContext context, SystemTypeCode typeCode)
		{
			return context.Method.DeclaringType.SystemType(typeCode);
		}
	}
}