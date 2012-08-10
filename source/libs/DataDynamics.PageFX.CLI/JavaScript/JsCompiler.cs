using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DataDynamics.PageFX.CLI.IL;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.JavaScript
{
	public sealed class JsCompiler
	{
		private readonly IAssembly _assembly;
		private JsProgram _program;
		private readonly HashList<IMethod, IMethod> _virtualCalls = new HashList<IMethod, IMethod>(x => x);

		public JsCompiler(IAssembly assembly)
		{
			if (assembly == null)
				throw new ArgumentNullException("assembly");

			if (assembly.EntryPoint == null)
				throw new NotSupportedException("Class library is not supported yet!");

			_assembly = assembly;
		}

		public JsCompiler(FileInfo assemblyFile)
		{
			if (assemblyFile == null)
				throw new ArgumentNullException("assemblyFile");

			_assembly = CommonLanguageInfrastructure.Deserialize(assemblyFile.FullName, null);
		}

		public JsProgram Compile()
		{
			if (_program != null) return _program;

			_program = new JsProgram();

			_program.Require("core.js");

			var method = CompileMethod(_assembly.EntryPoint);

			foreach (var vcall in _virtualCalls.AsContinuous())
			{
				CompileImpls(vcall);
			}
			
			//TODO: pass args to main from node.js args
			_program.Add(method.FullName.Id().Call().AsStatement());
			
			return _program;
		}

		public void Compile(FileInfo output)
		{
			var program = Compile();

			program.Write(output);
		}

		private void CompileImpls(IMethod method)
		{
			var iface = method.DeclaringType.Tag as JsInterface;
			if (iface != null)
			{
				foreach (var implClass in iface.Implementations.AsContinuous())
				{
					var implType = implClass.Type;
					if (implType.IsInterface) continue;

					var impl = implType.FindImplementation(method);

					if (impl == null)
					{
						throw new InvalidOperationException(
							string.Format("Unable to find implementation for method {0} in type {1}",
							              method.FullName, implType.FullName));
					}

					impl = impl.ResolveGenericInstance(implType, method);

					CompileMethod(impl);
				}
				return;
			}

			var klass = method.DeclaringType.Tag as JsClass;
			if (klass != null)
			{
				CompileOverrides(klass, method);
			}
		}

		private void CompileOverrides(JsClass klass, IMethod method)
		{
			foreach (var subclass in klass.Subclasses.AsContinuous())
			{
				var o = subclass.Type.FindOverrideMethod(method);
				if (o != null)
				{
					CompileMethod(o);
				}

				CompileOverrides(subclass, method);
			}
		}

		private JsMethod CompileMethod(IMethod method)
		{
			var jsMethod = method.Tag as JsMethod;
			if (jsMethod != null) return jsMethod;

			var klass = CompileClass(method.DeclaringType);

			jsMethod = new JsMethod(method);

			method.Tag = jsMethod;

			jsMethod.Function = CompileFunction(klass, method);

			klass.Add(jsMethod);

			return jsMethod;
		}

		private sealed class MethodContext
		{
			public readonly JsClass Class;
			public readonly IMethod Method;
			public readonly JsPool<JsVar> Vars = new JsPool<JsVar>();

			public MethodContext(JsClass klass, IMethod method)
			{
				Class = klass;
				Method = method;
			}
		}

		private JsFunction CompileFunction(JsClass klass, IMethod method)
		{
			var body = method.Body as IClrMethodBody;
			if (body == null)
				throw new NotSupportedException("The method format is not supported");

			var context = new MethodContext(klass, method);

			var parameters = method.Parameters.Select(x => x.Name).ToArray();
			var func = new JsFunction(null, parameters);

			//TODO: cache info and code as separate class property
			var info = new JsObject
				{
                    {"IsVoid", method.IsVoid()},
				};

			//TODO: string instance calls as static calls

			var args = new JsArray((method.IsStatic ? new string[0] : new []{"this"}).Concat(parameters).Select(x => (object)new JsId(x)));
			var vars = new JsArray(method.Body.LocalVariables.Select(x => x.Type.InitialValue()));
			var code = new JsArray(body.Code.Select<Instruction, object>(i => new JsInstruction(i, CompileInstruction(context, i))), "\n");

			func.Body.Add(args.Var("args"));
			func.Body.Add(vars.Var("vars"));
			func.Body.Add(info.Var("info"));

			foreach (var var in context.Vars)
			{
				func.Body.Add(var);
			}

			func.Body.Add(code.Var("code"));
			func.Body.Add(new JsText("var ctx = new $context(info, args, vars);"));
			func.Body.Add(new JsText("return ctx.exec(code);"));

			return func;
		}

		private object CompileInstruction(MethodContext context, Instruction i)
		{
			var method = i.Method;
			if (method != null)
			{
				//cases: static or instance call, new object, new md-array
				switch (i.Code)
				{
					case InstructionCode.Newobj:
						return CompileNewobj(context, method);
					case InstructionCode.Ldftn:
					case InstructionCode.Ldvirtftn:
						throw new NotImplementedException();
					default:
						return CompileCall(context, method);
				}
			}

			var field = i.Field;
			if (field != null)
			{
				return CompileFieldAccessor(context, field);
			}

			var type = i.Type;
			if (type != null)
			{
				switch (i.Code)
				{
					case InstructionCode.Initobj:
						return CompileInitobj(context, type);
					default:
						throw new NotImplementedException();
				}
			}

			return i.Value;
		}

		private JsNode CompileFieldAccessor(MethodContext context, IField field)
		{
			var var = context.Vars[field];
			if (var != null) return var.Id();
			
			string name = field.JsName();
			
			var get = new JsFunction(null, "o");
			var set = new JsFunction(null, "o", "v");

			if (field.IsStatic)
			{
				name = field.DeclaringType.FullName + "." + name;
				InitClass(context, get, field);
				InitClass(context, set, field);
				get.Body.Add(name.Id().Return());
				set.Body.Add(name.Id().Set("v".Id()));
			}
			else
			{
				get.Body.Add("o".Id().Get(name).Return());
				set.Body.Add("o".Id().Set(name, "v".Id()));
			}

			var = context.Vars.Add(field, new JsObject {{"get", get}, {"set", set}});

			return var.Id();
		}

		private sealed class Key
		{
			private readonly ITypeMember _member;
			private readonly InstructionCode _code;

			public Key(ITypeMember member, InstructionCode code)
			{
				_member = member;
				_code = code;
			}

			public override bool Equals(object obj)
			{
				var k = obj as Key;
				if (k == null) return false;
				return k._member == _member && k._code == _code;
			}

			public override int GetHashCode()
			{
				return _member.GetHashCode() ^ (int)_code ^ typeof(Key).GetHashCode();
			}
		}

		private JsNode CompileNewobj(MethodContext context, IMethod method)
		{
			var key = new Key(method, InstructionCode.Newobj);
			var var = context.Vars[key];
			if (var != null) return var.Id();

			var func = new JsFunction(null, "a");

			if (method.Body is IClrMethodBody)
			{
				CompileMethod(method);
			}
			else // internal call
			{
				throw new NotImplementedException();
			}

			InitClass(context, func, method);

			func.Body.Add(method.DeclaringType.New().Var("o"));
			func.Body.Add((method.JsFullName() + ".apply").Id().Call("o".Id(), "a".Id()).AsStatement());
			func.Body.Add("o".Id().Return());

			var info = new JsObject
				{
					{"n", method.Parameters.Count},
					{"f", func},
				};

			var = context.Vars.Add(key, info);

			return var.Id();
		}

		private JsNode CompileInitobj(MethodContext context, IType type)
		{
			var key = new Key(type, InstructionCode.Initobj);
			var var = context.Vars[key];
			if (var != null) return var.Id();

			CompileClass(type);

			var func = new JsFunction(null);
			InitClass(context, func, type);
			func.Body.Add(type.New().Return());

			return context.Vars.Add(key, func).Id();
		}

		private JsNode CompileCall(MethodContext context, IMethod method)
		{
			var var = context.Vars[method];
			if (var != null) return var.Id();

			var func = new JsFunction(null, "o");

			//TODO: remove temp code, now we redirect Console.WriteLine to console.log
			if (method.DeclaringType.FullName == "System.Console")
			{
				switch (method.Name)
				{
					case "WriteLine":
						func.Body.Add(new JsReturn(new JsId("console.log")));
						break;
					default:
						throw new NotImplementedException();
				}

				return CreateCallInfo(context, method, func);
			}

			if (method.Body is IClrMethodBody)
			{
				CompileMethod(method);
			}

			if (method.IsAbstract || method.IsVirtual)
			{
				if (!_virtualCalls.Contains(method))
					_virtualCalls.Add(method);
			}
			else //TODO: native/internal calls
			{
			}

			InitClass(context, func, method);

			func.Body.Add(method.JsFullName().Id().Return());

			return CreateCallInfo(context, method, func);
		}

		private static JsNode CreateCallInfo(MethodContext context, IMethod method, JsFunction func)
		{
			var info = new JsObject
				{
					{"n", method.Parameters.Count},
					{"s", method.IsStatic},
					{"r", !method.IsVoid()},
					{"f", func},
				};

			return context.Vars.Add(method, info).Id();
		}

		private JsClass CompileClass(IType type)
		{
			var klass = type.Tag as JsClass;
			if (klass != null) return klass;

			var baseClass = type.BaseType != null ? CompileClass(type.BaseType) : null;

			if (!string.IsNullOrEmpty(type.Namespace))
				_program.DefineNamespace(type.Namespace);

			klass = new JsClass(type, baseClass);

			type.Tag = klass;

			if (baseClass != null)
			{
				baseClass.Subclasses.Add(klass);
			}

			foreach (var iface in type.Interfaces)
			{
				JsInterface.Make(iface).Implementations.Add(klass);
			}

			foreach (var field in type.Fields)
			{
				klass.Add(JsField.Make(field));
			}

			_program.Add(klass);

			DefineCopyMethod(klass);

			return klass;
		}

		private void InitClass(MethodContext context, JsFunction func, ITypeMember member)
		{
			var method = member as IMethod;
			if (method != null && (method.IsStatic || method.IsConstructor))
			{
				CallClassInit(context, func, member.DeclaringType);
				return;
			}

			var type = member as IType;
			if (type != null && !type.IsInterface)
			{
				CallClassInit(context, func, type);
				return;
			}

			var field = member as IField;
			if (field != null && field.IsStatic)
			{
				CallClassInit(context, func, field.DeclaringType);
				return;
			}
		}

		private void CallClassInit(MethodContext context, JsFunction func, IType type)
		{
			if (type == null) return;
			
			CallClassInit(context, func, type.BaseType);

			var ctor = type.GetStaticCtor();
			if (ctor == null || ctor == context.Method) return;

			CompileMethod(ctor);

			var klass = CompileClass(type);

			var flag = string.Format("{0}.$cinit_done", type.FullName);

			var cinit = new JsFunction(null);
			cinit.Body.Add(new JsText(string.Format("if ({0} != undefined) return;", flag)));
			cinit.Body.Add(new JsText(string.Format("{0} = 1;", flag)));
			cinit.Body.Add(ctor.JsFullName().Id().Call().AsStatement());

			var cinitName = string.Format("{0}.$cinit", type.FullName);

			klass.Add(new JsStaticGeneratedMethod(cinitName){Function = cinit});

			func.Body.Add(cinitName.Id().Call().AsStatement());
		}

		private void DefineCopyMethod(JsClass klass)
		{
			if (klass.Type.TypeKind != TypeKind.Struct) return;

			var func = new JsFunction(null);
		}
	}
}