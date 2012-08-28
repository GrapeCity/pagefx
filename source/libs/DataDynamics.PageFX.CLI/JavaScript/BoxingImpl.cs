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

			//TODO: nullable

			var klass = _host.CompileClass(type);
			_host.CompileFields(type, false);

			if (type.IsBoxableType())
			{
				var name = string.Format("{0}.$box", type.JsFullName());

				if (!klass.BoxFunctionCompiled)
				{
					klass.BoxFunctionCompiled = true;

					var val = (JsNode)"v".Id();
					var func = new JsFunction(null, "v");

					if (type == SystemTypes.Boolean)
					{
						val = val.Ternary(true, false);
					}

					func.Body.Add(type.New(val).Return());

					klass.Add(new JsGeneratedMethod(name, func));
				}

				return name.Id();
			}

			return "$copy".Id();
		}

		public object Unbox(MethodContext context, IType type)
		{
			if (type.IsBoxableType())
			{
				return "$unbox".Id();
			}
			return "$copy".Id();
		}
	}
}
