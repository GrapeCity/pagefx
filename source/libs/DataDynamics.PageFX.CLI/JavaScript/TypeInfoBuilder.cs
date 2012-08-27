using System;
using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.JavaScript
{
	internal sealed class TypeInfoBuilder
	{
		private readonly JsCompiler _host;
		private readonly JsProgram _program;

		public TypeInfoBuilder(JsCompiler host, JsProgram program)
		{
			_host = host;
			_program = program;
		}

		public void Build()
		{
			foreach (var klass in _program.Classes.AsContinuous())
			{
				Build(klass);
			}

			foreach (var type in _host.ConstructedTypes.AsContinuous())
			{
				Register(type);
			}
		}

		private void Build(JsClass klass)
		{
			var type = klass.Type;

			// ignore Avm.String we use System.String
			if (type.IsAvmString()) return;

			Register(type);

			var getType = new JsFunction(null);

			if (type == SystemTypes.Array)
			{
				getType.Body.Add("$types".Id().Get("this".Id().Get("m_type")).Call().Return());
			}
			else
			{
				getType.Body.Add("$types".Id().Get(type.FullName).Call().Return());
			}

			klass.Add(new JsGeneratedMethod((type.IsString() ? "String" : type.JsFullName()) + ".prototype.GetType", getType));
		}

		private void Register(IType type)
		{
			var init = new JsFunction();

			var prop = string.Format("$types['{0}']", "$$" + type.FullName.JsEscape());

			init.Body.Add(new JsText(string.Format("var t = {0};", prop)));
			init.Body.Add(new JsText(string.Format("if (t != undefined) return t;")));

			var t = "t".Id();
			init.Body.Add(t.Set(SystemTypes.Type.New()));
			init.Body.Add(new JsText(string.Format("{0} = t;", prop)));

			init.Body.Add(t.Set("ns", type.Namespace ?? ""));
			init.Body.Add(t.Set("name", type.Name));
			init.Body.Add(t.Set("kind", type.GetCorlibKind()));

			var hierarchy = new JsObject(type.GetFullTypeHierarchy().Select(x => new KeyValuePair<string, object>(x.FullName, 1)));
			init.Body.Add(t.Set("$hierarchy", hierarchy));

			var newFunc = new JsFunction();
			newFunc.Body.Add(type.New().Return());
			init.Body.Add(t.Set("$new", newFunc));

			var arrayType = type as IArrayType;
			if (arrayType != null)
			{
				init.Body.Add(t.Set("$elemType", arrayType.ElementType.FullName));
			}

			init.Body.Add(t.Return());

			_program.Add("$types".Id().Set(type.FullName, init));
		}
	}
}
