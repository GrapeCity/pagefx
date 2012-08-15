using System;
using System.Collections.Generic;
using System.Diagnostics;
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
		private readonly HashList<IType, IType> _types = new HashList<IType, IType>(x => x);

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

			foreach (var vcall in _virtualCalls.AsContinuous())
			{
				CompileImpls(vcall);
			}

			// build types
			CompileClass(SystemTypes.Type);

			new TypeInfoBuilder(_program).Build();

			//TODO: pass args to main from node.js args

			var ctx = new MethodContext(CompileClass(entryPoint.DeclaringType), entryPoint);
			var main = new JsFunction(null);
			InitClass(ctx, main, entryPoint);
			main.Body.Add(method.FullName.Id().Call().AsStatement());

			_program.Add(main.Call().AsStatement());
			
			return _program;
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
			if (method.IsGetType()) return;

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

		internal JsMethod CompileMethod(IMethod method)
		{
			if (method.IsGetType()) return null;

			var jsMethod = method.Tag as JsMethod;
			if (jsMethod != null) return jsMethod;

			var klass = CompileClass(method.DeclaringType);

			jsMethod = new JsMethod(method);

			method.Tag = jsMethod;

			jsMethod.Function = CompileFunction(klass, method);

			klass.Add(jsMethod);

			return jsMethod;
		}

		private static void Analyze(IMethod method)
		{
			var translator = new ILTranslator();
			translator.Translate(method, method.Body, new NopCodeProvider());
		}

		private JsFunction CompileFunction(JsClass klass, IMethod method)
		{
			Analyze(method);

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
			switch (i.Code)
			{
				case InstructionCode.Call:
				case InstructionCode.Callvirt:
					return OpCall(context, i.Method);

				case InstructionCode.Ldftn:
				case InstructionCode.Ldvirtftn:
					return OpLdftn(i.Method);
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
			if (var != null) return var.Id();

			var field = member as IField;
			if (field != null)
			{
				if (field.IsArrayInitializer())
				{
					var value = field.Value;
					var blob = value as byte[];
					if (blob == null)
					{
						switch (Type.GetTypeCode(value.GetType()))
						{
							case TypeCode.Boolean:
								blob = new [] {(bool)value ? (byte)1 : (byte)0};
								break;
							case TypeCode.Char:
								blob = BitConverter.GetBytes((char)value);
								break;
							case TypeCode.SByte:
								blob = new[] { (byte)(sbyte)value };
								break;
							case TypeCode.Byte:
								blob = new[] { (byte)value };
								break;
							case TypeCode.Int16:
								blob = BitConverter.GetBytes((Int16)value);
								break;
							case TypeCode.UInt16:
								blob = BitConverter.GetBytes((UInt16)value);
								break;
							case TypeCode.Int32:
								blob = BitConverter.GetBytes((Int32)value);
								break;
							case TypeCode.UInt32:
								blob = BitConverter.GetBytes((UInt32)value);
								break;
							case TypeCode.Int64:
								blob = BitConverter.GetBytes((Int64)value);
								break;
							case TypeCode.UInt64:
								blob = BitConverter.GetBytes((UInt64)value);
								break;
							default:
								throw new ArgumentOutOfRangeException();
						}
					}

					var arr = new JsArray(blob.Select(x => (object)x));
					return context.Vars.Add(key, arr).Id();
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

		private object OpLdftn(IMethod method)
		{
			CompileCallMethod(method);

			return method.JsFullName();
		}

		private JsNode OpNewarr(MethodContext context, IType elemType)
		{
			var key = new InstructionKey(InstructionCode.Newarr, elemType);
			var var = context.Vars[key];
			if (var != null) return var.Id();

			var type = TypeFactory.MakeArray(elemType);
			if (!_types.Contains(type))
				_types.Add(type);

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

			return context.Vars.Add(key, info).Id();
		}

		private static int GetArrayElementTypeCode(IType elemType)
		{
			return (int)SystemTypes.GetTypeCode(elemType);
		}

		private JsNode OpNewobj(MethodContext context, IMethod method)
		{
			var key = new InstructionKey(InstructionCode.Newobj, method);
			var var = context.Vars[key];
			if (var != null) return var.Id();

			var func = new JsFunction(null, "a");

			CompileCallMethod(method);
			
			//TODO: internal calls (delegates, md-arrays)

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

		private JsNode OpInitobj(MethodContext context, IType type)
		{
			var key = new InstructionKey(InstructionCode.Initobj, type);
			var var = context.Vars[key];
			if (var != null) return var.Id();

			CompileClass(type);

			var func = new JsFunction(null);
			InitClass(context, func, type);
			func.Body.Add(type.New().Return());

			return context.Vars.Add(key, func).Id();
		}

		private JsNode OpCall(MethodContext context, IMethod method)
		{
			var var = context.Vars[method];
			if (var != null) return var.Id();

			var func = new JsFunction(null, "o", "a");

			//TODO: remove temp code, now we redirect Console.WriteLine to console.log
			if (method.DeclaringType.FullName == "System.Console")
			{
				switch (method.Name)
				{
					case "WriteLine":
						func.Body.Add("console.log".Id().Call("a".Id().Get(0)));
						break;
					default:
						throw new NotImplementedException();
				}

				return CreateCallInfo(context, method, func);
			}

			CompileCallMethod(method);

			if (method.IsInternalCall)
			{
				new InternalCallImpl(this).Compile(method);
			}

			InitClass(context, func, method);

			JsNode call;
			if (method.IsStatic)
			{
				call = method.JsFullName().Id().Get("apply").Call("o".Id(), "a".Id());
			}
			else
			{
				call = "o".Id().Get(method.JsName()).Get("apply").Call("o".Id(), "a".Id());
			}
			
			func.Body.Add(method.IsVoid() ? call.AsStatement() : call.Return());

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

		private void CompileCallMethod(IMethod method)
		{
			if (method.Body is IClrMethodBody)
			{
				CompileMethod(method);
			}

			if (method.IsAbstract || method.IsVirtual)
			{
				if (!_virtualCalls.Contains(method))
					_virtualCalls.Add(method);
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
			if (type.IsExcluded())
			{
				return null;
			}

			var klass = type.Tag as JsClass;
			if (klass != null) return klass;

			var baseClass = type.BaseType != null ? CompileClass(type.BaseType) : null;

			if (string.IsNullOrEmpty(type.Namespace))
				_program.DefineNamespace("$global");
			else
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

			_program.Add(klass);

			JsClass.DefineCopyMethod(klass);

			return klass;
		}

		internal void InitClass(MethodContext context, JsFunction func, ITypeMember member)
		{
			if (!(member is IType))
			{
				CompileFields(member.DeclaringType, member.IsStatic);
			}

			var type = member as IType;
			type = type ?? member.DeclaringType;

			if (type == SystemTypes.Type || type == SystemTypes.Array)
				return;

			new ClassInitImpl(this).InitClass(context, func, member);
		}

		internal void CompileFields(IType type, bool isStatic)
		{
			if (type is ICompoundType) return;

			if (type == SystemTypes.Type || type == SystemTypes.Array)
				return;

			foreach (var field in GetFields(type, isStatic))
			{
				CompileField(field);
			}
		}

		private void CompileField(IField field)
		{
			if (field.Tag != null) return;

			//TODO: do not compile primitive types
			var klass = CompileType(field.DeclaringType) as JsClass;
			if (klass != null)
			{
				klass.Add(JsField.Make(field));
			}
		}

		private static IEnumerable<IField> GetFields(IType type, bool isStatic)
		{
			if (isStatic)
			{
				return type.Fields.Where(x => x.IsStatic && !x.IsConstant && !x.IsArrayInitializer());
			}
			return type.Fields.Where(f => !f.IsStatic && !f.IsConstant && !f.IsArrayInitializer());
		}
	}
}