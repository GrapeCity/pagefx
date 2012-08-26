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

			var val = "v".Id();
			var func = new JsFunction(null, val.Value);

			if (type.IsNullableInstance())
			{
				type = type.GetTypeArgument(0);
			}

			_host.CompileClass(type);
			_host.CompileFields(type, false);

			if (type.IsBoxableType())
			{
				var field = type.GetBoxValueField();
				var obj = "o".Id();

				func.Body.Add(type.New().Var(obj.Value));

				JsNode value = val;
				if (type == SystemTypes.Boolean)
				{
					value = val.Ternary(true, false);
				}

				func.Body.Add(obj.Set(field.JsName(), value));
				func.Body.Add(obj.Return());
			}
			else
			{
				//TODO: need copy value type?
				func.Body.Add(val.Return());
			}

			return context.Vars.Add(key, func).Id();
		}

		public object Unbox(MethodContext context, IType type)
		{
			var key = new InstructionKey(InstructionCode.Box, type);
			var var = context.Vars[key];
			if (var != null) return var.Id();

			var obj = "o".Id();
			var func = new JsFunction(null, obj.Value);

			if (type.IsBoxableType())
			{
				func.Body.Add("$unbox".Id().Call(obj).Return());
			}
			else
			{
				func.Body.Add("$copy".Id().Call(obj).Return());
			}

			return context.Vars.Add(key, func).Id();
		}
	}
}
