﻿using System;
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
		private readonly HashList<IType, IType> _constructedTypes = new HashList<IType, IType>(x => x);

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

			new TypeInfoBuilder(this, _program).Build();

			//TODO: pass args to main from node.js args

			var ctx = new MethodContext(this, CompileClass(entryPoint.DeclaringType), entryPoint);
			var main = new JsFunction(null);
			InitClass(ctx, main, entryPoint);
			main.Body.Add(method.FullName.Id().Call().AsStatement());

			_program.Add(main.Call().AsStatement());
			
			return _program;
		}

		internal IList<IType> ConstructedTypes
		{
			get { return _constructedTypes; }
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

					CompileCallMethod(impl);
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
			var value = i.Value;
			if (value is long)
			{
				var hi = (int)((long)value >> 32);
				var lo = (uint)((long)value & 0xffffffff);
				CompileClass(SystemTypes.Int64);
				return new JsNewobj(SystemTypes.Int64, hi, lo);
			}
			if (value is ulong)
			{
				var hi = (uint)((ulong)value >> 32);
				var lo = (uint)((ulong)value & 0xffffffff);
				CompileClass(SystemTypes.UInt64);
				return new JsNewobj(SystemTypes.UInt64, hi, lo);
			}

			switch (i.Code)
			{
				case InstructionCode.Conv_I1:
				case InstructionCode.Conv_Ovf_I1:
				case InstructionCode.Ldelem_I1:
				case InstructionCode.Stelem_I1:
				case InstructionCode.Ldind_I1:
				case InstructionCode.Stind_I1:
				case InstructionCode.Conv_I2:
				case InstructionCode.Conv_Ovf_I2:
				case InstructionCode.Ldelem_I2:
				case InstructionCode.Stelem_I2:
				case InstructionCode.Ldind_I2:
				case InstructionCode.Stind_I2:
				case InstructionCode.Conv_I4:
				case InstructionCode.Conv_Ovf_I4:
				case InstructionCode.Ldelem_I4:
				case InstructionCode.Stelem_I4:
				case InstructionCode.Ldind_I4:
				case InstructionCode.Stind_I4:
				case InstructionCode.Conv_I8:
				case InstructionCode.Conv_Ovf_I8:
				case InstructionCode.Ldelem_I8:
				case InstructionCode.Stelem_I8:
				case InstructionCode.Ldind_I8:
				case InstructionCode.Stind_I8:
				case InstructionCode.Conv_R4:
				case InstructionCode.Conv_R_Un:
				case InstructionCode.Ldelem_R4:
				case InstructionCode.Stelem_R4:
				case InstructionCode.Ldind_R4:
				case InstructionCode.Stind_R4:
				case InstructionCode.Conv_R8:
				case InstructionCode.Ldelem_R8:
				case InstructionCode.Stelem_R8:
				case InstructionCode.Ldind_R8:
				case InstructionCode.Stind_R8:
				case InstructionCode.Conv_U1:
				case InstructionCode.Conv_Ovf_U1:
				case InstructionCode.Ldelem_U1:
				case InstructionCode.Ldind_U1:
				case InstructionCode.Conv_U2:
				case InstructionCode.Conv_Ovf_U2:
				case InstructionCode.Ldelem_U2:
				case InstructionCode.Ldind_U2:
				case InstructionCode.Conv_U4:
				case InstructionCode.Conv_Ovf_U4:
				case InstructionCode.Ldelem_U4:
				case InstructionCode.Ldind_U4:
				case InstructionCode.Conv_U8:
				case InstructionCode.Conv_Ovf_U8:
					return OpWithTypeCode(i);

				case InstructionCode.Ldstr:
					// string should be compiled to be ready for Object method calls.
					CompileClass(SystemTypes.String);
					return value;

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

				#region conditional branches

				case InstructionCode.Beq:
				case InstructionCode.Beq_S:
				case InstructionCode.Bne_Un:
				case InstructionCode.Bne_Un_S:
					return Br(i, BinaryOperator.Equality);
				case InstructionCode.Blt:
				case InstructionCode.Blt_S:
				case InstructionCode.Blt_Un:
				case InstructionCode.Blt_Un_S:
					return Br(i, BinaryOperator.LessThan);
				case InstructionCode.Ble:
				case InstructionCode.Ble_S:
				case InstructionCode.Ble_Un:
				case InstructionCode.Ble_Un_S:
					return Br(i, BinaryOperator.LessThanOrEqual);
				case InstructionCode.Bgt:
				case InstructionCode.Bgt_S:
				case InstructionCode.Bgt_Un:
				case InstructionCode.Bgt_Un_S:
					return Br(i, BinaryOperator.GreaterThan);
				case InstructionCode.Bge:
				case InstructionCode.Bge_S:
				case InstructionCode.Bge_Un:
				case InstructionCode.Bge_Un_S:
					return Br(i, BinaryOperator.GreaterThanOrEqual);

				#endregion
			}

			return value;
		}

		private object Br(Instruction i, BinaryOperator op)
		{
			CompileOp(i, op);

			var x = (int)i.InputTypes[0].GetTypeCode();
			var y = (int)i.InputTypes[1].GetTypeCode();
			var t = (x << 8) | y;

			return new JsTuple(i.Value, t);
		}

		private object OpWithTypeCode(Instruction i)
		{
			var type = i.InputTypes[0];

			if (type is ICompoundType)
			{
				Debugger.Break();
			}

			if (type.IsInt64())
			{
				CompileClass(type);
			}
			return (int)type.GetTypeCode();
		}
		
		private object Op(Instruction i, UnaryOperator op, bool checkOverflow)
		{
			var t = (int)i.OutputType.GetTypeCode();

			CompileOp(i.InputTypes[0], op);

			return t;
		}

		private object Op(Instruction i, BinaryOperator op, bool unsigned, bool checkOverflow)
		{
			CompileOp(i, op);

			var x = (int)i.InputTypes[0].GetTypeCode();
			var y = (int)i.InputTypes[1].GetTypeCode();
			var t = (x << 8) | y;

			return t;
		}

		private void CompileOp(Instruction i, BinaryOperator op)
		{
			CompileOp(i.InputTypes[0], op);
			CompileOp(i.InputTypes[1], op);

			if (i.OutputType != null)
			{
				CompileOp(i.OutputType, op);
			}
		}

		private readonly OperatorResolver _operators = new OperatorResolver();

		private static bool HasOperators(IType type)
		{
			switch (type.GetTypeCode())
			{
				case TypeCode.Int64:
				case TypeCode.UInt64:
				case TypeCode.Decimal:
					return true;
				default:
					return false;
			}
		}

		private void CompileOp(IType type, UnaryOperator op)
		{
			// exclude natively supported types
			if (!HasOperators(type)) return;

			var method = _operators.Find(op, type);
			if (method == null)
			{
				return;
			}

			if (method.Tag != null) return;

			var jsMethod = CompileMethod(method);

			var func = new JsFunction(null);
			func.Body.Add(jsMethod.FullName.Id().Call("this".Id()).Return());

			ExtendPrototype(type, op.JsName(), func);
		}

		private void CompileOp(IType type, BinaryOperator op)
		{
			// exclude natively supported types
			if (!HasOperators(type)) return;

			var method = _operators.Find(op, type, type);
			if (method == null)
			{
				return;
			}

			if (method.Tag != null) return;

			var jsMethod = CompileMethod(method);

			var val = "v".Id();
			var func = new JsFunction(null, val.Value);
			func.Body.Add(jsMethod.FullName.Id().Call("this".Id(), val).Return());

			ExtendPrototype(type, op.JsName(), func);
		}

		internal void ExtendPrototype(IType type, string name, JsFunction func)
		{
			var klass = CompileClass(type);

			var fullName = type.JsFullName() + ".prototype." + name;

			klass.Add(new JsGeneratedMethod(fullName, func));
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

			var info = context.Vars[key];
			if (info != null) return info;

			CompileCallMethod(method);

			var func = CreateCallFunc(context, method, i.CallInfo);

			return context.Vars.Add(key, func);
		}

		private JsNode OpNewarr(MethodContext context, IType elemType)
		{
			var key = new InstructionKey(InstructionCode.Newarr, elemType);
			var var = context.Vars[key];
			if (var != null) return var.Id();

			var type = RegisterArrayType(elemType);
			
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

			var = context.Vars.Add(key, info);

			CompileClass(SystemTypes.Array);
			CompileType(elemType);

			return var.Id();
		}

		internal IType RegisterArrayType(IType elementType)
		{
			var type = TypeFactory.MakeArray(elementType);
			if (!_constructedTypes.Contains(type))
				_constructedTypes.Add(type);
			return type;
		}

		private static int GetArrayElementTypeCode(IType elemType)
		{
			return (int)elemType.GetTypeCode();
		}

		private JsNode OpNewobj(MethodContext context, IMethod method)
		{
			var key = new InstructionKey(InstructionCode.Newobj, method);
			var data = context.Vars[key];
			if (data != null) return data.Id();

			var args = "a".Id();

			var func = new JsFunction(null, args.Value);

			var info = new JsObject
				{
					{"n", method.Parameters.Count},
					{"f", func},
				};

			data = context.Vars.Add(key, info);

			CompileCallMethod(method);

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

			return data.Id();
		}

		private JsNode OpInitobj(MethodContext context, IType type)
		{
			var key = new InstructionKey(InstructionCode.Initobj, type);
			var info = context.Vars[key];
			if (info != null) return info.Id();

			var func = new JsFunction(null);

			info = context.Vars.Add(key, func);

			CompileClass(type);
			
			InitClass(context, func, type);

			func.Body.Add(type.New().Return());

			return info.Id();
		}

		private object OpCall(MethodContext context, Instruction i)
		{
			var method = i.Method;
			var callInfo = context.Vars[method];
			if (callInfo != null) return callInfo.Id();

			CompileCallMethod(method);

			var func = CreateCallFunc(context, method, i.CallInfo);

			callInfo = context.Vars[method];
			if (callInfo != null) return callInfo.Id();

			var info = new JsObject
				{
					{"n", method.Parameters.Count},
					{"s", method.IsStatic},
					{"r", !method.IsVoid()},
					{"f", func},
				};

			callInfo = context.Vars.Add(method, info);

			return callInfo.Id();
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

			//to detect stackoverflows
			//func.Body.Add("console.log".Id().Call(method.JsFullName()).AsStatement());

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
			{
				if (!method.IsStatic && !method.IsConstructor && method.DeclaringType.IsBoxableType())
				{
					new BoxingImpl(this).BoxUnboxed(context, method.DeclaringType, func, obj);
				}

				call = method.Apply(obj, args);
			}

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

			klass = new JsClass(type, baseType == SystemTypes.ValueType || type.IsString() ? null : baseClass);

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
			if (type == SystemTypes.Type || type == SystemTypes.Array)
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

			if (isStatic) klass.StaticFieldsCompiled = true;
			else klass.InstanceFieldsCompiled = true;

			foreach (var field in type.GetFields(isStatic))
			{
				CompileField(klass, field);
			}
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