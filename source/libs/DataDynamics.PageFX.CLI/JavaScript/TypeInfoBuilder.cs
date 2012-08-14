using System;
using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.JavaScript
{
	internal sealed class TypeInfoBuilder
	{
		private readonly JsProgram _program;

		public TypeInfoBuilder(JsProgram program)
		{
			_program = program;
		}

		public void Build()
		{
			foreach (var klass in _program.Classes.AsContinuous())
			{
				Build(klass);
			}
		}

		private void Build(JsClass klass)
		{
			var type = klass.Type;
			var fullName = type.FullName;

			// type init
			var init = new JsFunction(null);

			var prop = string.Format("$types['{0}']", "$$" + fullName.JsEscape());

			init.Body.Add(new JsText(string.Format("var t = {0};", prop)));
			init.Body.Add(new JsText(string.Format("if (t != undefined) return t;")));

			var t = "t".Id();
			init.Body.Add(t.Set(SystemTypes.Type.New()));
			init.Body.Add(new JsText(string.Format("{0} = t;", prop)));

			init.Body.Add(t.Set("ns", type.Namespace));
			init.Body.Add(t.Set("name", type.Name));
			//TODO: FullName

			var hierarchy = new JsObject(type.GetFullTypeHierarchy().Select(x => new KeyValuePair<string, object>(x.FullName, 1)));
			init.Body.Add(t.Set("$hierarchy", hierarchy));

			init.Body.Add(t.Return());

			_program.Add("$types".Id().Set(fullName, init));

			var getType = new JsFunction(null);
			getType.Body.Add("$types".Id().Get(fullName).Call().Return());

			klass.Add(new JsGeneratedMethod(fullName + ".prototype.GetType", getType));
		}
	}
}
