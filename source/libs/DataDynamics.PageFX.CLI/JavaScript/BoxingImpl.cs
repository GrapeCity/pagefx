using DataDynamics.PageFX.CLI.IL;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.JavaScript
{
	internal sealed class BoxingImpl
	{
		private readonly JsCompiler _host;

		public BoxingImpl(JsCompiler host)
		{
			_host = host;
		}

		public object Box(MethodContext context, IType type)
		{
			var key = new InstructionKey(InstructionCode.Box, type);
			var var = context.Vars[key];
			if (var != null) return var.Id();

			var func = new JsFunction(null, "v");

			if (type.IsNullableInstance())
			{
				type = type.GetTypeArgument(0);
			}

			_host.CompileClass(type);
			_host.CompileFields(type, false);

			func.Body.Add(type.New().Var("o"));
			var obj = "o".Id();
			//TODO: find boxing field and gets its JsName
			func.Body.Add(obj.Set("m_value", "v".Id()));
			func.Body.Add(obj.Return());

			return context.Vars.Add(key, func).Id();
		}

		public object Unbox(MethodContext context, IType type)
		{
			var key = new InstructionKey(InstructionCode.Box, type);
			var var = context.Vars[key];
			if (var != null) return var.Id();

			var func = new JsFunction(null, "o");

			//TODO: find boxing field and gets its JsName
			func.Body.Add("o".Id().Get("m_value"));

			return context.Vars.Add(key, func).Id();
		}
	}
}
