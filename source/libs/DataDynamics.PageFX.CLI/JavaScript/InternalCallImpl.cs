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
					{"System.Boolean", new BooleanInlines()},
					{"System.Int32", new Int32Inlines()},
				    {"System.Char", new CharInlines()},
					{"System.Array", new ArrayInlines()},
					{"System.Type", new TypeInlines()},
					{"Avm.Array", new AvmArrayInlines()},
					{"avm", new AvmInlines()},
				    {"System.Console", new ConsoleInlines()},
					{"System.ConsoleWriter", new ConsoleWriterInlines()},
				    {"System.BitConverter", new BitConverterInlines()},
				    {"System.Runtime.CompilerServices.RuntimeHelpers", new RuntimeHelpersInlines()},
			    };

		private static readonly Dictionary<string, bool> ExcludedMethods =
			new Dictionary<string, bool>
				{
					{"Avm.String.toLowerCase", true}
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

		public JsFunction CompileInlineFunction(IMethod method)
		{
			if (Excluded(method)) return null;

			var info = method.GetInlineInfo();
			if (info != null)
			{
				return CompileInlineFunction(method, info);
			}

			var type = method.DeclaringType;
			if (type.TypeKind == TypeKind.Delegate)
			{
				//TODO: 
				return null;
			}

			InlineCodeProvider provider;
			if (Inlines.TryGetValue(type.FullName, out provider))
			{
				return CompileInlineFunction(method, provider);
			}

			return null;
		}

		public void Compile(IMethod method)
		{
			if (Excluded(method)) return;

			var info = method.GetInlineInfo();
			if (info != null)
			{
				CompileInlineMethod(method, info);
				return;
			}

			var type = method.DeclaringType;
            if (type.TypeKind == TypeKind.Delegate)
            {
	            Inline(DelegateInlines.Instance, method);
				return;
            }

			InlineCodeProvider provider;
			if (Inlines.TryGetValue(type.FullName, out provider))
			{
				Inline(provider, method);
				return;
			}

			//TODO:
			//throw new NotImplementedException();
		}

		private static bool Excluded(IMethod method)
		{
			string key = method.DeclaringType.FullName + "." + method.Name;
			return ExcludedMethods.ContainsKey(key);
		}

		private void Inline(InlineCodeProvider provider, IMethod method)
		{
			var jsMethod = method.Tag as JsGeneratedMethod;
			if (jsMethod != null) return;
			
			var impl = CompileInlineFunction(method, provider);
			if (impl == null)
			{
				throw new NotImplementedException();
			}

			jsMethod = new JsGeneratedMethod(method.JsFullName(), impl);
			method.Tag = jsMethod;

			var klass = CompileClass(method.DeclaringType);
			klass.Add(jsMethod);
		}

		private JsFunction CompileInlineFunction(IMethod method, InlineCodeProvider provider)
		{
			var klass = CompileClass(method.DeclaringType);
			var ctx = new MethodContext(_host, klass, method);
			return provider.GetImplementation(ctx);
		}

		private void CompileInlineMethod(IMethod method, InlineMethodInfo info)
		{
			var jsMethod = method.Tag as JsGeneratedMethod;
			if (jsMethod != null) return;

			var func = CompileInlineFunction(method, info);
			var klass = CompileClass(method.DeclaringType);

			jsMethod = new JsGeneratedMethod(method.JsFullName(), func);
			method.Tag = jsMethod;

			klass.Add(jsMethod);
		}

		private static JsFunction CompileInlineFunction(IMethod method, InlineMethodInfo info)
		{
			var parameters = method.JsParams();
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
						text += parameters[i];
					}
					text += ";";
					func.Body.Add(new JsText(text));
					break;

				default:
					var type = method.DeclaringType;
					//TODO: use ABC, pfx meta attributes
					var typeName = type.Namespace == "Avm" ? type.Name : type.JsFullName(method);
					var target = method.IsStatic ? typeName.Id() : "this".Id();
					func.Body.Add(target.Get(info.Name).Call(parameters.Select(x => x.Id()).ToArray()).Return());
					break;
			}

			return func;
		}
	}
}
