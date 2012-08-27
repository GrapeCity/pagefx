#define JSC_FIELD_DEBUG

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
			var fieldInfo = context.Vars[field];
			if (fieldInfo != null) return fieldInfo.Id();

			var obj = "o".Id();
			var val = "v".Id();
			var get = new JsFunction(null, obj.Value);
			var set = new JsFunction(null, obj.Value, val.Value);

			var info = new JsObject
				{
					{"get", get},
					{"set", set}
				};

			fieldInfo = context.Vars.Add(field, info);

			if (field.IsStatic)
			{
				_host.InitClass(context, get, field);
				_host.InitClass(context, set, field);

				var name = field.JsFullName();

#if JSC_FIELD_DEBUG
				var value = name.Id().Var("v");
				get.Body.Add(value);
				get.Body.Add(new JsText(string.Format("if (v === undefined) throw new ReferenceError('{0} is undefined');", field.FullName)));
				get.Body.Add(value.Name.Id().Return());
#else
				get.Body.Add(name.Id().Return());
#endif

				set.Body.Add(name.Id().Set(val));
			}
			else
			{
				var name = field.JsName();

#if JSC_FIELD_DEBUG
				var value = obj.Get(name).Var("v");
				get.Body.Add(value);
				get.Body.Add(new JsText(string.Format("if (v === undefined) throw new ReferenceError('{0} is undefined');", field.FullName)));
				get.Body.Add(value.Name.Id().Return());
#else
				get.Body.Add(obj.Get(name).Return());
#endif

				set.Body.Add(obj.Set(name, val));
			}

			return fieldInfo.Id();
		}
	}
}
