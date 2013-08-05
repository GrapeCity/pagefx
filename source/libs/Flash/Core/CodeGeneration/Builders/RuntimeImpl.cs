using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Flash.Abc;
using DataDynamics.PageFX.Flash.Avm;
using DataDynamics.PageFX.Flash.IL;

namespace DataDynamics.PageFX.Flash.Core.CodeGeneration.Builders
{
    //contains runtime types, methods
	internal sealed class RuntimeImpl
	{
		private readonly AbcGenerator _generator;
		private AbcInstance _runtimeInstance;
		private AbcInstance _valueHolder;
		private AbcInstance _qnameHolder;

		public RuntimeImpl(AbcGenerator generator)
		{
			_generator = generator;
		}

		private AbcFile Abc
		{
			get { return _generator.Abc; }
		}

		public AbcInstance Instance
		{
			get { return _runtimeInstance ?? (_runtimeInstance = BuildInstance(_generator.AppAssembly.Corlib())); }
		}

		private AbcInstance BuildInstance(IAssembly assembly)
		{
			var name = Abc.DefineName(QName.PfxPackage(assembly.Name + "$runtime"));
			var instance = Abc.Instances[name];
			if (instance != null) return instance;
			instance = Abc.DefineEmptyInstance(name, true);
			Abc.DefineScript(instance);
			return instance;
		}

		public AbcMethod IsFlashPlayer()
		{
			var instance = _generator.Corlib.Environment.Instance;

			return instance.DefineMethod(
				Sig.get("isFlash", Abc.BuiltinTypes.RealBoolean).@static(),
				code =>
					{
						var isFlash = instance.DefineStaticSlot("__isFlash", AvmTypeCode.Boolean);
						var isFlashInitialized =
							instance.DefineStaticSlot("__isFlashInitialized", AvmTypeCode.Boolean);

						code.GetLocal(0);
						code.GetProperty(isFlashInitialized);

						var ifTrue = code.IfTrue();

						code.GetLocal(0);
						code.GetLocal(0);
						var m = IsFlashPlayerInternal();
						code.Call(m);
						code.SetProperty(isFlash);

						ifTrue.BranchTarget = code.Label();

						code.GetLocal(0);
						code.GetProperty(isFlash);
						code.FixBool();
						code.ReturnValue();
					});
		}

		private AbcMethod IsFlashPlayerInternal()
		{
			var instance = _generator.Corlib.Environment.Instance;

			return instance.DefineMethod(
				Sig.get("isFlashInternal", Abc.BuiltinTypes.Boolean).@static(),
				code =>
					{
						code.GetPlayerType();
						code.PushString("AVMPlus");
						code.Add(InstructionCode.Equals);
						code.Add(InstructionCode.Not);
						code.ReturnValue();
					});
		}

		public AbcMethod Exit()
		{
			var instance = _generator.Corlib.Environment.Instance;
			return instance.DefineMethod(
				Sig.@static("exit_impl", AvmTypeCode.Void, AvmTypeCode.Int32, "exitCode"),
				code =>
					{
						var isFlash = IsFlashPlayer();
						code.Getlex(isFlash);
						code.Call(isFlash);
						var ifNotFlash = code.IfFalse();

						var ns = Abc.DefinePackage("avmplus");
						var mn = Abc.DefineQName(ns, "System");
						code.Getlex(mn);
						mn = Abc.DefineQName(ns, "exit");
						code.GetLocal(1); //exitCode
						code.Call(mn, 1);
						code.ReturnVoid();

						ifNotFlash.BranchTarget = code.Label();

						ns = Abc.DefinePackage("flash.System");
						mn = Abc.DefineQName(ns, "System");
						code.Getlex(mn);
						mn = Abc.DefineQName(ns, "exit");
						code.GetLocal(1); //exitCode
						code.Add(InstructionCode.Coerce_u); //???
						code.Call(mn, 1);
						code.ReturnVoid();
					});
		}

		private AbcInstance BuildRecord(object name, params object[] args)
		{
			var mn = Abc.DefineName(name);

			var instance = Abc.DefineEmptyInstance(mn, true);

			int slotID = 1;
			for (int i = 0; i < args.Length; i += 2)
			{
				var value = instance.CreateStaticSlot(args[i], args[i + 1]);
				value.SlotId = slotID;
				++slotID;
			}

			return instance;
		}

		public AbcInstance ValueHolder
		{
			get
			{
				return _valueHolder ?? (_valueHolder =
				                        BuildRecord(QName.PfxPackage("value_holder"),
				                                    "value", AvmTypeCode.Object));
			}
		}

		public AbcInstance QNameHolder
		{
			get
			{
				return _qnameHolder ?? (_qnameHolder =
				                        BuildRecord(QName.PfxPackage("qname_holder"),
				                                    "ns", AvmTypeCode.Namespace,
				                                    "name", AvmTypeCode.String));
			}
		}

		public AbcMethod FindClass()
		{
			var instance = _generator.Corlib.SystemType.Instance;
			return instance.DefineMethod(
				Sig.@static(QName.PfxPackage("FindClass"), AvmTypeCode.Class,
				            AvmTypeCode.Namespace, "ns",
				            AvmTypeCode.String, "name"),
				code =>
					{
						const int ns = 1;
						const int name = 2;

						code.GetLocal(ns);
						code.GetLocal(name);
						code.FindPropertyStrict(code.Abc.RuntimeQName);

						code.GetLocal(ns);
						code.GetLocal(name);
						code.GetRuntimeProperty();

						code.CoerceClass();

						code.ReturnValue();
					});
		}

		//returns true if passed object is null
		public AbcMethod IsNullImpl()
		{
			var instance = _generator.Corlib.SystemType.Instance;

			return instance.DefineMethod(
				Sig.@static("IsNull", AvmTypeCode.Boolean, AvmTypeCode.Object, "value"),
				code =>
					{
						const int value = 1;

						code.GetLocal(value);
						var notNull = code.IfNotNull();
						code.PushNativeBool(true);
						code.ReturnValue();

						notNull.BranchTarget = code.Label();
						code.Try();

						code.GetLocal(value);
						code.Nullable_HasValue(true);
						code.Add(InstructionCode.Not);
						code.ReturnValue();

						code.BeginCatch();
						code.Pop();
						code.PushNativeBool(false);
						code.ReturnValue();
						code.EndCatch(true);
					});
		}

		public AbcMethod IsNullableImpl()
		{
			var instance = _generator.Corlib.SystemType.Instance;

			return instance.DefineMethod(
				Sig.@static("IsNullable", AvmTypeCode.Boolean, AvmTypeCode.Object, "value"),
				code =>
					{
						const int value = 1;

						code.GetLocal(value);
						var notNull = code.IfNotNull();
						code.PushNativeBool(true);
						code.ReturnValue();
						notNull.BranchTarget = code.Label();

						code.GetLocal(value);
						code.Nullable_HasValue(true);
						code.Add(InstructionCode.Not);
						code.ReturnValue();
					});
		}
	}
}