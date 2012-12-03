using DataDynamics.PageFX.Common;
using DataDynamics.PageFX.Common.Services;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.FlashLand.Abc;
using DataDynamics.PageFX.FlashLand.IL;

namespace DataDynamics.PageFX.FLI
{
    //contains runtime types, methods
    internal partial class AbcGenerator
    {
        #region DefineRuntimeInstance
        public AbcInstance DefineRuntimeInstance(IAssembly assembly)
        {
            var name = DefinePfxName(assembly.Name + "$runtime", false);
            var instance = _abc.Instances[name];
            if (instance != null) return instance;
            instance = _abc.DefineEmptyInstance(name, true);
            _abc.DefineScript(instance);
            return instance;
        }

        public AbcInstance DefineRuntimeInstance(IType type)
        {
            return DefineRuntimeInstance(type.Assembly);
        }

        public AbcInstance DefineRuntimeInstance()
        {
            return DefineRuntimeInstance(Corlib.Assembly);
        }
        #endregion

        #region DefineIsFlashPlayerInternal
        private AbcMethod DefineIsFlashPlayerInternal()
        {
            var instance = GetInstance(CorlibTypeId.Environment);

            return instance.DefineStaticGetter(
                "isFlashInternal",
                _abc.BuiltinTypes.Boolean,
                code =>
	                {
		                code.GetPlayerType();
		                code.PushString("AVMPlus");
		                code.Add(InstructionCode.Equals);
		                code.Add(InstructionCode.Not);
		                code.ReturnValue();
	                }
	            );
        }
        #endregion

        #region DefineIsFlashPlayer
        public AbcMethod DefineIsFlashPlayer()
        {
            var instance = GetInstance(CorlibTypeId.Environment);

            return instance.DefineStaticGetter(
                "isFlash",
                _abc.BuiltinTypes.RealBoolean,
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
		                var m = DefineIsFlashPlayerInternal();
		                code.Call(m);
		                code.SetProperty(isFlash);

		                ifTrue.BranchTarget = code.Label();

		                code.GetLocal(0);
		                code.GetProperty(isFlash);
		                code.FixBool();
		                code.ReturnValue();
	                }
	            );
        }
        #endregion

        #region DefineExitMethod
        public AbcMethod DefineExitMethod()
        {
            var instance = GetInstance(CorlibTypeId.Environment);
            return instance.DefineStaticMethod(
                "exit_impl", AvmTypeCode.Void,
                code =>
	                {
		                var isFlash = DefineIsFlashPlayer();
		                code.Getlex(isFlash);
		                code.Call(isFlash);
		                var ifNotFlash = code.IfFalse();

		                var ns = _abc.DefinePackage("avmplus");
		                var mn = _abc.DefineQName(ns, "System");
		                code.Getlex(mn);
		                mn = _abc.DefineQName(ns, "exit");
		                code.GetLocal(1); //exitCode
		                code.Call(mn, 1);
		                code.ReturnVoid();

		                ifNotFlash.BranchTarget = code.Label();

		                ns = _abc.DefinePackage("flash.System");
		                mn = _abc.DefineQName(ns, "System");
		                code.Getlex(mn);
		                mn = _abc.DefineQName(ns, "exit");
		                code.GetLocal(1); //exitCode
		                code.Add(InstructionCode.Coerce_u); //???
		                code.Call(mn, 1);
		                code.ReturnVoid();
	                },
                AvmTypeCode.Int32, "exitCode");
        }
        #endregion

        #region DefineRecord
        public AbcInstance DefineRecord(object name, params object[] args)
        {
            var mn = _abc.DefineName(name);

            var instance = _abc.DefineEmptyInstance(mn, true);

            int slotID = 1;
            for (int i = 0; i < args.Length; i += 2)
            {
                var value = instance.CreateStaticSlot(args[i], args[i + 1]);
                value.SlotID = slotID;
                ++slotID;
            }

            return instance;
        }
        #endregion

        #region DefineValueHolder
        public AbcInstance DefineValueHolder()
        {
            if (_holderValue != null) return _holderValue;

            var name = DefinePfxName("value_holder");
            var instance = DefineRecord(name, "value", AvmTypeCode.Object);
            _holderValue = instance;

            return instance;
        }

        private AbcInstance _holderValue;
        #endregion

        #region DefineQNameHolder
        public AbcInstance DefineQNameHolder()
        {
            if (_holderQName != null) return _holderQName;

            var name = DefinePfxName("qname_holder");
            var instance = DefineRecord(name,
                "ns", AvmTypeCode.Namespace,
                "name", AvmTypeCode.String);
            _holderQName = instance;

            return instance;
        }

        private AbcInstance _holderQName;
        #endregion

        #region DefineFindClass
        public AbcMethod DefineFindClass()
        {
            if (_methodFindClass != null)
                return _methodFindClass;

            var instance = GetTypeInstance();
            _methodFindClass = instance.DefineStaticMethod(
                new PfxQName("FindClass"),
                AvmTypeCode.Class,
                code =>
	                {
		                const int ns = 1;
		                const int name = 2;

		                code.GetLocal(ns);
		                code.GetLocal(name);
		                code.FindPropertyStrict(code.abc.RuntimeQName);

		                code.GetLocal(ns);
		                code.GetLocal(name);
		                code.GetRuntimeProperty();

		                code.CoerceClass();

		                code.ReturnValue();
	                },
                AvmTypeCode.Namespace, "ns",
                AvmTypeCode.String, "name");

            return _methodFindClass;
        }
        AbcMethod _methodFindClass;
        #endregion

        #region DefineIsNullMethod
        //returns true if passed object is null
        public AbcMethod DefineIsNullMethod()
        {
            var instance = GetTypeInstance();

            return instance.DefineStaticMethod(
                "IsNull",
                AvmTypeCode.Boolean,
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
	                },
                AvmTypeCode.Object, "value");
        }

        public AbcMethod DefineIsNullableMethod()
        {
            var instance = GetTypeInstance();

            return instance.DefineStaticMethod(
                "IsNullable",
                AvmTypeCode.Boolean,
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
	                },
                AvmTypeCode.Object, "value");
        }
        #endregion
    }
}