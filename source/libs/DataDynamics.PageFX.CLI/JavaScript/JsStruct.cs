using System;
using System.Linq;

namespace DataDynamics.PageFX.CLI.JavaScript
{
	internal sealed class JsStruct
	{
		public static void CopyImpl(JsClass klass)
		{
			var func = new JsFunction(null);

			var obj = "o".Id();

			func.Body.Add(klass.Type.New().Var(obj.Value));

			foreach (var field in klass.Type.Fields.Where(field => !field.IsStatic && !field.IsConstant))
			{
				var name = field.JsName();
				var value = "this".Id().Get(name);
				func.Body.Add(obj.Set(name, value));
			}

			func.Body.Add(obj.Return());

			klass.Add(new JsGeneratedMethod(String.Format("{0}.prototype.$copy", klass.Type.JsFullName()), func));
		}
	}
}