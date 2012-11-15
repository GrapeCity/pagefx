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

		public void BoxUnboxed(MethodContext context, IType type, JsFunction func, JsNode obj)
		{
			var f = Box(context, type);
			func.Body.Add(obj.Set("$cbox".Id().Call(obj, f)));
		}

		public JsNode Box(MethodContext context, IType type)
		{
			var klass = CompileClass(type);

			if (type.IsBoxableType() || type.IsNullableInstance())
			{
				var name = CompileBox(type, klass);

				return name.Id();
			}

			return "$copy".Id();
		}

		public JsNode Unbox(MethodContext context, IType type)
		{
			var klass = CompileClass(type);

			//TODO: InvalidCastException

			if (type.IsNullableInstance())
			{
				return CompileUnbox(context, type, klass).Id();
			}

			if (type.IsBoxableType())
			{
				return "$unbox".Id();
			}

			return "$copy".Id();
		}

		private JsClass CompileClass(IType type)
		{
			var klass = _host.CompileClass(type);
			_host.CompileFields(type, false);

			if (type.IsNullableInstance())
			{
				JsStruct.CompileCopy(klass);
			}

			return klass;
		}

		private string CompileBox(IType type, JsClass klass)
		{
			var name = string.Format("{0}.$box", type.JsFullName());

			if (klass.BoxCompiled) return name;

			klass.BoxCompiled = true;

			var val = (JsNode)"v".Id();
			var func = new JsFunction(null, "v");

			if (type.IsNullableInstance())
			{
				func.Body.Add(new JsText(string.Format("if (!v.has_value) return null;")));
				func.Body.Add("$copy".Id().Call(val).Return());

				type = type.GetTypeArgument(0);
				CompileClass(type);
			}
			else
			{
				if (type == SystemTypes.Boolean)
				{
					val = val.Op("!!");
				}

				func.Body.Add(type.New(val).Return());
			}

			klass.Add(new JsGeneratedMethod(name, func));

			return name;
		}

		private string CompileUnbox(MethodContext context, IType type, JsClass klass)
		{
			var name = string.Format("{0}.$unbox", type.JsFullName());

			if (klass.UnboxCompiled) return name;

			klass.UnboxCompiled = true;

			var valueType = type.GetTypeArgument(0);
			var unbox = Unbox(context, valueType);

			var val = (JsNode)"v".Id();
			var func = new JsFunction(null, "v");

			func.Body.Add(val.Set(unbox.Call(val)));
			func.Body.Add(type.New(val).Return());

			klass.Add(new JsGeneratedMethod(name, func));

			return name;
		}
	}
}
