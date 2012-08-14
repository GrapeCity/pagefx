using System;
using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.CLI.JavaScript.Inlining;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.JavaScript
{
	internal sealed class InternalCallImpl
	{
		private static readonly Dictionary<string, InlineCodeProvider> Inlines =
			new Dictionary<string, InlineCodeProvider>
			    {
				    {"System.Object", new SystemObjectInlines()},
				    {"System.String", new StringInlines()},
				    {"System.Runtime.CompilerServices.RuntimeHelpers", new RuntimeHelpersInlines()},
			    };

		private readonly JsCompiler _host;

		public InternalCallImpl(JsCompiler host)
		{
			_host = host;
		}

		private JsClass CompileClass(IType type)
		{
			return _host.CompileClass(type);
		}

		public void Compile(IMethod method)
		{
			var info = method.GetInlineInfo();
			if (info != null)
			{
				CompileInlineMethod(method, info);
				return;
			}

			InlineCodeProvider provider;
			if (Inlines.TryGetValue(method.DeclaringType.FullName, out provider))
			{
				var jsMethod = method.Tag as JsGeneratedMethod;
				if (jsMethod != null) return;

				var klass = CompileClass(method.DeclaringType);
				var ctx = new MethodContext(klass, method);
				var impl = provider.GetImplementation(ctx);

				if (impl == null)
				{
					throw new NotImplementedException();
				}

				jsMethod = new JsGeneratedMethod(method.JsFullName(), impl);

				method.Tag = jsMethod;

				klass.Add(jsMethod);

				return;
			}

			//TODO:
			//throw new NotImplementedException();
		}

		private void CompileInlineMethod(IMethod method, InlineMethodInfo info)
		{
			var jsMethod = method.Tag as JsGeneratedMethod;
			if (jsMethod != null) return;

			var parameters = method.Parameters.Select(x => x.Name).ToArray();
			var func = new JsFunction(null, parameters);

			switch (info.Kind)
			{
				case InlineKind.Property:
					//TODO: support static properties
					var propertyName = info.Name;
					if (method.IsSetter)
					{
						func.Body.Add("this".Id().Set(propertyName, parameters[0].Id()));
					}
					else
					{
						func.Body.Add("this".Id().Get(propertyName).Return());
					}
					break;

				case InlineKind.Operator:
					var text = "return ";
					for (int i = 0; i < method.Parameters.Count; i++)
					{
						if (i > 0) text += info.Name;
						text += method.Parameters[i].Name;
					}
					text += ";";
					func.Body.Add(new JsText(text));
					break;

				default:
					func.Body.Add("this".Id().Call(info.Name, parameters.Select(x => x.Id())).Return());
					break;
			}

			var klass = CompileClass(method.DeclaringType);

			jsMethod = new JsGeneratedMethod(method.JsFullName(), func);

			method.Tag = jsMethod;

			klass.Add(jsMethod);
		}
	}
}
