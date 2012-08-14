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

		public JsNode Compile(MethodContext context, IField field)
		{
			var var = context.Vars[field];
			if (var != null) return var.Id();

			var get = new JsFunction(null, "o");
			var set = new JsFunction(null, "o", "v");

			if (field.IsStatic)
			{
				var name = field.JsFullName();
				_host.InitClass(context, get, field);
				_host.InitClass(context, set, field);
				get.Body.Add(name.Id().Return());
				set.Body.Add(name.Id().Set("v".Id()));
			}
			else
			{
				var name = field.JsName();
				get.Body.Add("o".Id().Get(name).Return());
				set.Body.Add("o".Id().Set(name, "v".Id()));
			}

			var info = new JsObject
				{
					{"get", get},
					{"set", set}
				};

			return context.Vars.Add(field, info).Id();
		}
	}
}
