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
			var info = context.Vars[key];
			if (info != null) return info.Id();

			var val = "v".Id();
			var func = new JsFunction(null, val.Value);

			info = context.Vars.Add(key, func);

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

			return info.Id();
		}

		public object Unbox(MethodContext context, IType type)
		{
			var key = new InstructionKey(InstructionCode.Unbox, type);
			var info = context.Vars[key];
			if (info != null) return info.Id();

			var obj = "o".Id();
			var func = new JsFunction(null, obj.Value);

			info = context.Vars.Add(key, func);

			if (type.IsBoxableType())
			{
				func.Body.Add("$unbox".Id().Call(obj).Return());
			}
			else
			{
				func.Body.Add("$copy".Id().Call(obj).Return());
			}

			return info.Id();
		}
	}
}
