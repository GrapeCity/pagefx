using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.JavaScript
{
	internal sealed class FieldCompiler
	{
		private readonly JsCompiler _host;

		public FieldCompiler(JsCompiler host)
		{
			_host = host;
		}

		public object Compile(MethodContext context, IField field)
		{
			var var = context.Vars[field];
			if (var != null) return var;

			var obj = "o".Id();
			var val = "v".Id();
			var get = new JsFunction(null, obj.Value);
			var set = new JsFunction(null, obj.Value, val.Value);

			if (field.IsStatic)
			{
				var name = field.JsFullName();
				_host.InitClass(context, get, field);
				_host.InitClass(context, set, field);
				get.Body.Add(name.Id().Return());
				set.Body.Add(name.Id().Set(val));
			}
			else
			{
				var name = field.JsName();

				// for debug
				var value = obj.Get(name).Var("v");
				get.Body.Add(value);
				get.Body.Add(new JsText(string.Format("if (v === undefined) throw new ReferenceError('{0} is undefined');", field.FullName)));
				get.Body.Add(value.Name.Id().Return());

				//get.Body.Add(obj.Get(name).Return());

				set.Body.Add(obj.Set(name, val));
			}

			var info = new JsObject
				{
					{"get", get},
					{"set", set}
				};

			return context.Vars.Add(field, info);
		}
	}
}
