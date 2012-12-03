using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.Common.Extensions;
using DataDynamics.PageFX.Common.TypeSystem;

namespace DataDynamics.PageFX.Ecma335.JavaScript
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

			var func = new JsFunction(null);

			if (type.Is(SystemTypeCode.Array))
			{
				func.Body.Add("$types".Id().Get("this".Id().Get("m_type")).Call().Return());
			}
			else
			{
				func.Body.Add("$types".Id().Get(type.FullName).Call().Return());
			}

			klass.ExtendPrototype(func, "GetType");
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

			var description = Describe(type);
			foreach (var property in description)
			{
				init.Body.Add(t.Set(property.Key, property.Value));
			}

			init.Body.Add(t.Return());

			_program.Add("$types".Id().Set(type.FullName, init));
		}

		private static IEnumerable<KeyValuePair<string,object>> Describe(IType type)
		{
			var hierarchy = new JsObject(type.GetFullTypeHierarchy().Select(x => new KeyValuePair<object, object>(x.FullName, 1)));

			var newFunc = new JsFunction();
			newFunc.Body.Add(type.New().Return());

			var list = new PropertyList
				{
					{"ns", type.Namespace ?? ""},
					{"name", type.Name},
					{"kind", type.GetCorlibKind()},
					{"$typecode", type.JsTypeCode()},
					{"$hierarchy", hierarchy},
					{"$new", newFunc},
				};
			
			var compoundType = type as ICompoundType;
			if (compoundType != null)
			{
				list.Add("$elemType", compoundType.ElementType.FullName);
			}

			if (type.ValueType != null)
			{
				list.Add("$valueType", type.ValueType.FullName);
			}

			return list;
		}

		private sealed class PropertyList : List<KeyValuePair<string,object>>
		{
			public void Add(string key, object value)
			{
				Add(new KeyValuePair<string, object>(key, value));
			}
		}
	}
}
