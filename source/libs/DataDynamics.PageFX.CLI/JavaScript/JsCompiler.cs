using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using DataDynamics.PageFX.CLI.IL;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.JavaScript
{
	/// <summary>
	/// Compiles assembly into javascript.
	/// </summary>
	public sealed class JsCompiler
	{
		private readonly IAssembly _assembly;
		private JsProgram _program;
		private readonly HashList<IType, IType> _arrayTypes = new HashList<IType, IType>(x => x);

		internal readonly CorlibTypeCache CorlibTypes = new CorlibTypeCache();

		/// <summary>
		/// Initializes a new instance of the <see cref="JsCompiler"/> class.
		/// </summary>
		/// <param name="assembly">The assembly to be compiled.</param>
		public JsCompiler(IAssembly assembly)
		{
			if (assembly == null)
				throw new ArgumentNullException("assembly");

			if (assembly.EntryPoint == null)
				throw new NotSupportedException("Class library is not supported yet!");

			_assembly = assembly;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="JsCompiler"/> class.
		/// </summary>
		/// <param name="assemblyFile">The file of the assembly to be compiled.</param>
		public JsCompiler(FileInfo assemblyFile)
		{
			if (assemblyFile == null)
				throw new ArgumentNullException("assemblyFile");

			_assembly = CommonLanguageInfrastructure.Deserialize(assemblyFile.FullName, null);
		}

		public void Compile(FileInfo output)
		{
			var program = Compile();

			program.Write(output);
		}

		public JsProgram Compile()
		{
			if (_program != null) return _program;

			_program = new JsProgram();

			_program.Require("core.js");

			var entryPoint = _assembly.EntryPoint;
			var method = CompileMethod(entryPoint);

			// build types
			CompileClass(SystemTypes.Type);

			new TypeInfoBuilder(_program).Build();

			//TODO: pass args to main from node.js args

			var ctx = new MethodContext(this, CompileClass(entryPoint.DeclaringType), entryPoint);
			var main = new JsFunction(null);
			InitClass(ctx, main, entryPoint);
			main.Body.Add(method.FullName.Id().Call().AsStatement());

			_program.Add(main.Call().AsStatement());
			
			return _program;
		}

		private void CompileImpls(IMethod method)
		{
			var declaringType = method.DeclaringType;
			if (declaringType.IsGenericArrayInterface())
			{
				new ArrayInterfaceImpl(this).Compile(method);
			}

			var iface = declaringType.Tag as JsInterface;
			if (iface != null)
			{
				// ToList is required since during compilation new implementations could be added
				foreach (var implClass in iface.Implementations.ToList())
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

			var klass = declaringType.Tag as JsClass;
			if (klass != null)
			{
				CompileOverrides(klass, method);
			}
		}

		private void CompileOverrides(JsClass klass, IMethod method)
		{
			// GetType is implemented by TypeInfoBuilder
			if (method.IsGetType()) return;

			foreach (var subclass in klass.Subclasses.ToList())
			{
				var o = subclass.Type.FindOverrideMethod(method);
				if (o != null)
				{
					CompileMethod(o);
				}
				else if (subclass.Type.TypeKind == TypeKind.Struct && method.IsEquals())
				{
					JsStruct.DefaultEqualsImpl(this, subclass);
				}

				CompileOverrides(subclass, method);
			}
		}

		internal JsMethod CompileMethod(IMethod method)
		{
			if (method == null)
				throw new ArgumentNullException("method");

			if (Exclude(method)) return null;

			var jsMethod = method.Tag as JsMethod;
			if (jsMethod != null) return jsMethod;

			var klass = CompileClass(method.DeclaringType);

			jsMethod = new JsMethod(method);

			method.Tag = jsMethod;

			jsMethod.Function = CompileFunction(klass, method);

			klass.Add(jsMethod);

			CompileImplementedMethods(klass, method);

			return jsMethod;
		}

		private static void CompileImplementedMethods(JsClass klass, IMethod method)
		{
			if (method.IsExplicitImplementation) return;

			var impls = method.ImplementedMethods;
			if (impls == null) return;
			if (impls.Length <= 0) return;

			var type = method.DeclaringType.JsFullName(method);
			var name = method.JsName();

			foreach (var i in impls)
			{
				var func = new JsFunction(null, i.JsParams());

				var call = "this".Id().Get(name).Call(i.JsArgs());
				func.Body.Add(method.IsVoid() ? call.AsStatement() : call.Return());

				klass.Add(new JsGeneratedMethod(string.Format("{0}.prototype.{1}", type, i.JsName()), func));
			}
		}

		private static bool Exclude(IMethod method)
		{
			if (method.IsGetType())
				return true;

			if (method.IsToString() && method.DeclaringType.IsString())
				return true;

			return false;
		}

		private JsFunction CompileFunction(JsClass klass, IMethod method)
		{
			var func = CompileInlineFunction(method);
			if (func != null) return func;

			var body = method.Body as IClrMethodBody;
			if (body == null)
				throw new NotSupportedException("The method format is not supported");

			var translator = new ILTranslator();
			var codeProvider = new NopCodeProvider(this, klass, method);
			translator.Translate(method, method.Body, codeProvider);

			return codeProvider.Function;
		}

		internal JsFunction CompileJsil(JsClass klass, IMethod method, IClrMethodBody body)
		{
			var context = new MethodContext(this, klass, method);

			var parameters = method.JsParams();
			var func = new JsFunction(null, parameters);

			//TODO: cache info and code as separate class property
			var info = new JsObject
				{
					{"IsVoid", method.IsVoid()},
				};

			var args = new JsArray((method.IsStatic ? new string[0] : new[] {"this"}).Concat(parameters).Select(x => (object)new JsId(x)));
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

		private JsFunction CompileInlineFunction(IMethod method)
		{
			return new InternalCallImpl(this).CompileInlineFunction(method);
		}

		private object CompileInstruction(MethodContext context, Instruction i)
		{
			switch (i.Code)
			{
				case InstructionCode.Ldstr:
					// string should be compiled to be ready for Object method calls.
					CompileClass(SystemTypes.String);
					return i.Value;

				case InstructionCode.Call:
				case InstructionCode.Callvirt:
					return OpCall(context, i);

				case InstructionCode.Ldftn:
				case InstructionCode.Ldvirtftn:
					return OpLdftn(context, i);
				case InstructionCode.Calli:
					throw new NotImplementedException();

				case InstructionCode.Newobj:
					return OpNewobj(context, i.Method);
				case InstructionCode.Initobj:
					return OpInitobj(context, i.Type);
				case InstructionCode.Newarr:
					return OpNewarr(context, i.Type);

				case InstructionCode.Ldfld:
				case InstructionCode.Ldsfld:
				case InstructionCode.Ldflda:
				case InstructionCode.Ldsflda:
				case InstructionCode.Stfld:
				case InstructionCode.Stsfld:
					CompileFields(i.Field.DeclaringType, i.Field.IsStatic);
					return new FieldCompiler(this).Compile(context, i.Field);

				case InstructionCode.Box:
					return new BoxingImpl(this).Box(context, i.Type);
				case InstructionCode.Unbox:
				case InstructionCode.Unbox_Any:
					return new BoxingImpl(this).Unbox(context, i.Type);

				case InstructionCode.Ldtoken:
					return OpLdtoken(context, i.Member);

				case InstructionCode.Isinst:
				case InstructionCode.Castclass:
				case InstructionCode.Ldobj:
				case InstructionCode.Stobj:
					CompileType(i.Type);
					return i.Type.FullName;

				case InstructionCode.Ldelema:
					return null;

				#region binary, unary arithmetic operations
				//arithmetic operations
				// a + b
				case InstructionCode.Add:
					return Op(i, BinaryOperator.Addition, false, false);
				case InstructionCode.Add_Ovf:
					return Op(i, BinaryOperator.Addition, false, true);
				case InstructionCode.Add_Ovf_Un:
					return Op(i, BinaryOperator.Addition, true, true);

				// a - b
				case InstructionCode.Sub:
					return Op(i, BinaryOperator.Subtraction, false, false);
				case InstructionCode.Sub_Ovf:
					return Op(i, BinaryOperator.Subtraction, false, true);
				case InstructionCode.Sub_Ovf_Un:
					return Op(i, BinaryOperator.Subtraction, true, true);

				// a * b
				case InstructionCode.Mul:
					return Op(i, BinaryOperator.Multiply, false, false);
				case InstructionCode.Mul_Ovf:
					return Op(i, BinaryOperator.Multiply, false, true);
				case InstructionCode.Mul_Ovf_Un:
					return Op(i, BinaryOperator.Multiply, true, true);

				// a / b
				case InstructionCode.Div:
					return Op(i, BinaryOperator.Division, false, false);
				case InstructionCode.Div_Un:
					return Op(i, BinaryOperator.Division, true, false);

				// a % b
				case InstructionCode.Rem:
					return Op(i, BinaryOperator.Modulus, false, false);
				case InstructionCode.Rem_Un:
					return Op(i, BinaryOperator.Modulus, true, false);

				//bitwise operations
				// a & b
				case InstructionCode.And:
					return Op(i, BinaryOperator.BitwiseAnd, false, false);
				// a | b
				case InstructionCode.Or:
					return Op(i, BinaryOperator.BitwiseOr, false, false);
				// a ^ b
				case InstructionCode.Xor:
					return Op(i, BinaryOperator.ExclusiveOr, false, false);
				// a << b
				case InstructionCode.Shl:
					return Op(i, BinaryOperator.LeftShift, false, false);
				// a >> b
				case InstructionCode.Shr:
					return Op(i, BinaryOperator.RightShift, false, false);
				case InstructionCode.Shr_Un:
					return Op(i, BinaryOperator.RightShift, true, false);

				//unary operations
				case InstructionCode.Neg:
					return Op(i, UnaryOperator.Negate, false);
				case InstructionCode.Not:
					return Op(i, UnaryOperator.BitwiseNot, false);

				//relation operations
				// a == b
				case InstructionCode.Ceq:
					return Op(i, BinaryOperator.Equality, false, false);
				// a > b
				case InstructionCode.Cgt:
					return Op(i, BinaryOperator.GreaterThan, false, false);
				case InstructionCode.Cgt_Un:
					return Op(i, BinaryOperator.GreaterThan, true, false);
				// a < b
				case InstructionCode.Clt:
					return Op(i, BinaryOperator.LessThan, false, false);
				case InstructionCode.Clt_Un:
					return Op(i, BinaryOperator.LessThan, true, false);
				#endregion
			}

			return i.Value;
		}

		private static object Op(Instruction i, UnaryOperator op, bool checkOverflow)
		{
			return (int)SystemTypes.GetTypeCode(i.OutputType);
		}

		private static object Op(Instruction i, BinaryOperator op, bool unsigned, bool checkOverflow)
		{
			return (int)SystemTypes.GetTypeCode(i.OutputType);
		}

		private object OpLdtoken(MethodContext context, ITypeMember member)
		{
			var key = new InstructionKey(InstructionCode.Ldtoken, member);
			var var = context.Vars[key];
			if (var != null) return var;

			var field = member as IField;
			if (field != null)
			{
				if (field.IsArrayInitializer())
				{
					var blob = field.GetBlob();
					var arr = new JsArray(blob.Select(x => (object)x));
					return context.Vars.Add(key, arr);
				}

				throw new NotImplementedException();
			}

			var type = member as IType;
			if (type != null)
			{
				CompileType(type);
				return type.FullName;
			}

			throw new NotImplementedException();
		}

		private object OpLdftn(MethodContext context, Instruction i)
		{
			var method = i.Method;
			var key = new InstructionKey(i.Code, method);
			var var = context.Vars[key];
			if (var != null) return var;

			CompileCallMethod(method);

			var func = CreateCallFunc(context, method, i.CallInfo);
			
			return context.Vars.Add(key, func);
		}

		private JsNode OpNewarr(MethodContext context, IType elemType)
		{
			var key = new InstructionKey(InstructionCode.Newarr, elemType);
			var var = context.Vars[key];
			if (var != null) return var;

			var type = TypeFactory.MakeArray(elemType);
			if (!_arrayTypes.Contains(type))
				_arrayTypes.Add(type);

			CompileClass(SystemTypes.Array);

			var elemInit = new JsFunction(null);
			elemInit.Body.Add(elemType.InitialValue().Return());

			var info = new JsObject
				{
					{"init", elemInit},
					{"type", type.FullName},
					{"box", new BoxingImpl(this).Box(context, elemType)},
					{"unbox", new BoxingImpl(this).Unbox(context, elemType)},
					{"etc", GetArrayElementTypeCode(elemType)},
				};

			return context.Vars.Add(key, info);
		}

		private static int GetArrayElementTypeCode(IType elemType)
		{
			return (int)SystemTypes.GetTypeCode(elemType);
		}

		private JsNode OpNewobj(MethodContext context, IMethod method)
		{
			var key = new InstructionKey(InstructionCode.Newobj, method);
			var var = context.Vars[key];
			if (var != null) return var;

			CompileCallMethod(method);

			var args = "a".Id();

			var func = new JsFunction(null, args.Value);

			InitClass(context, func, method);

			if (method.IsConstructor && method.DeclaringType.IsString())
			{
				func.Body.Add(method.Apply(null, args).Return());
			}
			else
			{
				var obj = "o".Id();
				func.Body.Add(method.DeclaringType.New().Var(obj.Value));
				func.Body.Add(method.Apply(obj, args).AsStatement());
				func.Body.Add(obj.Return());
			}

			var info = new JsObject
				{
					{"n", method.Parameters.Count},
					{"f", func},
				};

			return context.Vars.Add(key, info);
		}

		private JsNode OpInitobj(MethodContext context, IType type)
		{
			var key = new InstructionKey(InstructionCode.Initobj, type);
			var var = context.Vars[key];
			if (var != null) return var;

			CompileClass(type);

			var func = new JsFunction(null);
			InitClass(context, func, type);
			func.Body.Add(type.New().Return());

			return context.Vars.Add(key, func);
		}

		private object OpCall(MethodContext context, Instruction i)
		{
			var method = i.Method;
			var var = context.Vars[method];
			if (var != null) return var;

			CompileCallMethod(method);

			var func = CreateCallFunc(context, method, i.CallInfo);

			var info = new JsObject
				{
					{"n", method.Parameters.Count},
					{"s", method.IsStatic},
					{"r", !method.IsVoid()},
					{"f", func},
				};

			return context.Vars.Add(method, info);
		}

		private JsFunction CreateCallFunc(MethodContext context, IMethod method, CallInfo info)
		{
			var obj = "o".Id();
			var args = "a".Id();
			var func = new JsFunction(null, obj.Value, args.Value);

			InitClass(context, func, method);

			JsNode call = null;

			//TODO: inplace inline calls
			if (method.DeclaringType == SystemTypes.Boolean && method.IsToString())
			{
				call = obj.Ternary("True", "False");
			}

			if (call == null && info != null)
			{
				if (info.ReceiverType != null && (info.Flags & CallFlags.Basecall) != 0)
				{
					call = method.JsFullName(info.ReceiverType).Id().Apply(obj, args);
				}
				else if (IsSuperCall(context, method, info.Flags))
				{
					var baseType = context.Method.DeclaringType.BaseType;
					call = method.JsFullName(baseType).Id().Apply(obj, args);
					//TODO: remove $base if it is not needed
					//call = obj.Get("$base").Get(method.JsName()).Apply(obj, args);
				}
			}
			
			if (call == null)
				call = method.Apply(obj, args);

			func.Body.Add(method.IsVoid() ? call.AsStatement() : call.Return());

			return func;
		}

		private static bool IsSuperCall(MethodContext context, IMethod method, CallFlags flags)
		{
			bool thiscall = (flags & CallFlags.Thiscall) != 0;
			bool virtcall = (flags & CallFlags.Virtcall) != 0;
			return thiscall && !virtcall && IsSuperCall(context, method);
		}

		private static bool IsSuperCall(MethodContext context, IMethod method)
		{
			if (method.IsStatic || method.IsConstructor || method.IsAbstract) return false;
			return context.Method.IsBaseMethod(method);
		}

		internal void CompileCallMethod(IMethod method)
		{
			if (method.Tag != null) return;

			if (method.Body is IClrMethodBody)
			{
				CompileMethod(method);
			}

			if (method.IsInternalCall || method.CodeType == MethodCodeType.Runtime)
			{
				new InternalCallImpl(this).Compile(method);
			}

			if (method.Tag == null)
			{
				// mark abstract methods to detect that it is called in the program
				method.Tag = this;
			}

			if (method.IsAbstract || method.IsVirtual)
			{
				CompileImpls(method);
			}
		}

		internal JsNode CompileType(IType type)
		{
			if (type.IsInterface)
			{
				return JsInterface.Make(type);
			}

			if (type.IsArray)
			{
				CompileType(type.GetElementType());
				return CompileClass(SystemTypes.Array);
			}

			return CompileClass(type);
		}

		internal JsClass CompileClass(IType type)
		{
			if (type == null || type.IsExcluded())
			{
				return null;
			}

			var klass = type.Tag as JsClass;
			if (klass != null) return klass;

			var baseType = type.BaseType;
			var baseClass = CompileClass(baseType == SystemTypes.ValueType ? SystemTypes.Object : baseType);

			if (string.IsNullOrEmpty(type.Namespace))
				_program.DefineNamespace("$global");
			else
				_program.DefineNamespace(type.Namespace);

			klass = new JsClass(type, baseType == SystemTypes.ValueType || baseType.IsString() ? null : baseClass);

			type.Tag = klass;

			if (baseClass != null)
			{
				baseClass.Subclasses.Add(klass);
			}

			foreach (var iface in type.Interfaces)
			{
				JsInterface.Make(iface).Implementations.Add(klass);
			}

			_program.Add(klass);

			switch (type.TypeKind)
			{
				case TypeKind.Struct:
					JsStruct.CopyImpl(klass);
					break;
				case TypeKind.Delegate:
					JsDelegate.CreateInstanceImpl(klass);
					break;
			}

			CompileImpls(klass, type);

			return klass;
		}

		private void CompileImpls(JsClass klass, IType type)
		{
			var isValueType = type.TypeKind == TypeKind.Struct;
			bool equalsDefined = false;

			foreach (var method in type.Methods)
			{
				if (isValueType)
				{
					if (method.IsEquals())
					{
						equalsDefined = true;
					}
				}

				if (IsCompilableImpl(method))
				{
					CompileMethod(method);
				}
			}

			if (isValueType)
			{
				if (!equalsDefined && JsStruct.GetObjectEqualsMethod().Tag != null)
				{
					JsStruct.DefaultEqualsImpl(this, klass);
				}
			}
		}

		private static bool IsCompilableImpl(IMethod method)
		{
			if (method.IsStatic || method.IsAbstract || method.IsConstructor)
				return false;

			if (method.ImplementedMethods != null
				&& method.ImplementedMethods.Any(x => x.Tag != null))
			{
				return true;
			}

			if (method.IsOverride)
			{
				var baseMethod = method.BaseMethod;
				if (baseMethod != null && baseMethod.Tag != null)
					return true;
			}

			return false;
		}

		internal void InitClass(MethodContext context, JsFunction func, ITypeMember member)
		{
			var type = member is IType ? (IType)member : member.DeclaringType;

			if (ExcludeInitClass(type)) return;

			if (member is IType)
			{
				CompileFields(type, true);
				CompileFields(type, false);
			}
			else
			{
				CompileFields(type, member.IsStatic);
			}
			
			new ClassInitImpl(this).InitClass(context, func, member);
		}

		private static bool ExcludeInitClass(IType type)
		{
			if (type.IsInterface || type is ICompoundType)
				return true;
			if (type == SystemTypes.Type || type == SystemTypes.Array || type.FullName == "System.Console")
				return true;
			return false;
		}

		internal void CompileFields(IType type, bool isStatic)
		{
			if (type == null) throw new ArgumentNullException("type");

			if (type is ICompoundType || type.IsInterface) return;

			if (!CompileFieldsFor(type)) return;

			var klass = CompileType(type) as JsClass;
			if (klass == null) return;

			if (isStatic ? klass.StaticFieldsCompiled : klass.InstanceFieldsCompiled) return;

			foreach (var field in type.GetFields(isStatic))
			{
				CompileField(klass, field);
			}

			if (isStatic) klass.StaticFieldsCompiled = true;
			else klass.InstanceFieldsCompiled = true;
		}

		private static bool CompileFieldsFor(IType type)
		{
			if (type.SystemType != null)
			{
				switch (type.SystemType.Code)
				{
					case SystemTypeCode.Void:
					case SystemTypeCode.Array:
					case SystemTypeCode.Type:
					case SystemTypeCode.Delegate:
					case SystemTypeCode.MulticastDelegate:
						return false;
				}
			}

			return true;
		}

		private void CompileField(JsClass klass, IField field)
		{
			if (field.Tag != null) return;

			//TODO: do not compile primitive types
			klass.Add(JsField.Make(field));

			if (field.Type.TypeKind == TypeKind.Struct)
			{
				CompileType(field.Type);
			}
		}
	}
}