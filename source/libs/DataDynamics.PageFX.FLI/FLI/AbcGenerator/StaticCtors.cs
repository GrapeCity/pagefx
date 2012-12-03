using System.Collections.Generic;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.FlashLand.Abc;
using DataDynamics.PageFX.FlashLand.IL;

namespace DataDynamics.PageFX.FLI
{
    partial class AbcGenerator
    {
        AbcTrait DefineStaticCtorFlag(AbcInstance instance)
        {
            if (instance.StaticCtorFlag == null)
            {
                var I = DefineRuntimeInstance();
                instance.StaticCtorFlag = I.DefineStaticSlot(
                    "~cctor_called." + instance.FullName, AvmTypeCode.Boolean);
            }
            return instance.StaticCtorFlag;
        }

        void GetStaticCtorFlag(AbcCode code, AbcInstance instance)
        {
            var f = DefineStaticCtorFlag(instance);
            code.GetStaticProperty(f);
        }

        void SetStaticCtorFlag(AbcCode code, AbcInstance instance, bool value)
        {
            var f = DefineStaticCtorFlag(instance);
            code.Getlex(f.Instance);
            code.PushNativeBool(value);
            code.SetProperty(f);
        }

        AbcMethod DefineStaticCtorCaller(AbcInstance instance)
        {
            var type = instance.Type;
            if (type == null) return null;
            var ctor = DefineStaticCtor(instance, type);
            if (ctor == null) return null;

            if (instance.StaticCtorCaller != null)
                return instance.StaticCtorCaller;

            var I = DefineRuntimeInstance();
            instance.StaticCtorCaller =
                I.DefineStaticMethod(
                    "call_cctor." + instance.FullName, AvmTypeCode.Void,
                    code =>
	                    {
		                    //NOTE: we must init class before setting cctor flag to true
		                    code.Getlex(ctor);

		                    GetStaticCtorFlag(code, instance);
		                    var br = code.IfFalse();
		                    code.ReturnVoid();

		                    br.BranchTarget = code.Label();
		                    SetStaticCtorFlag(code, instance, true);

		                    code.Call(ctor);
		                    code.ReturnVoid();
	                    });

            return instance.StaticCtorCaller;
        }

        void CallStaticCtor(AbcCode code, AbcInstance instance)
        {
            var m = DefineStaticCtorCaller(instance);
            if (m == null) return;
            code.Getlex(m);
            code.Call(m);
        }

        void CallStaticCtors(AbcCode code, IList<AbcInstance> list)
        {
            int n = list.Count;
            for (int i = 0; i < n; ++i)
                CallStaticCtor(code, list[i]);
        }

        internal void CallStaticCtor(AbcCode code, IType type)
        {
            var instance = type.Tag as AbcInstance;
            if (instance == null) return;
            CallStaticCtor(code, instance);
        }
    }
}