using System.Collections.Generic;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.FlashLand.Abc;
using DataDynamics.PageFX.FlashLand.IL;

namespace DataDynamics.PageFX.FlashLand.Core.CodeGeneration.Builders
{
    internal sealed class StaticCtorsImpl
    {
	    private readonly AbcGenerator _generator;

	    public StaticCtorsImpl(AbcGenerator generator)
		{
			_generator = generator;
		}

		#region cctor IsCalled flag

		private AbcTrait CalledFlag(AbcInstance instance)
        {
            if (instance.StaticCtorFlag == null)
            {
                var I = _generator.RuntimeImpl.Instance;
                instance.StaticCtorFlag = I.DefineStaticSlot(
                    "~cctor_called." + instance.FullName, AvmTypeCode.Boolean);
            }
            return instance.StaticCtorFlag;
        }

        private void GetCalledFlag(AbcCode code, AbcInstance instance)
        {
            var f = CalledFlag(instance);
            code.GetStaticProperty(f);
        }

		private void SetCalledFlag(AbcCode code, AbcInstance instance, bool value)
        {
            var f = CalledFlag(instance);
            code.Getlex(f.Instance);
            code.PushNativeBool(value);
            code.SetProperty(f);
        }

		#endregion

		private AbcMethod BuildCctorCaller(AbcInstance instance)
        {
            var type = instance.Type;
            if (type == null) return null;
            var ctor = DefineStaticCtor(instance, type);
            if (ctor == null) return null;

            if (instance.StaticCtorCaller != null)
                return instance.StaticCtorCaller;

            var I = _generator.RuntimeImpl.Instance;
	        instance.StaticCtorCaller =
		        I.DefineMethod(
			        Sig.@static("call_cctor." + instance.FullName, AvmTypeCode.Void),
			        code =>
				        {
					        //NOTE: we must init class before setting cctor flag to true
					        code.Getlex(ctor);

					        GetCalledFlag(code, instance);
					        var br = code.IfFalse();
					        code.ReturnVoid();

					        br.BranchTarget = code.Label();
					        SetCalledFlag(code, instance, true);

					        code.Call(ctor);
					        code.ReturnVoid();
				        });

            return instance.StaticCtorCaller;
        }

	    public void Call(AbcCode code, AbcInstance instance)
	    {
		    var m = BuildCctorCaller(instance);
		    if (m == null) return;
		    code.Getlex(m);
		    code.Call(m);
	    }

	    public void Call(AbcCode code, IType type)
        {
            var instance = type.AbcInstance();
            if (instance == null) return;
            Call(code, instance);
        }

		public void CallRange(AbcCode code, IEnumerable<AbcInstance> list)
		{
			foreach (var instance in list)
				Call(code, instance);
		}

	    public AbcMethod DefineStaticCtor(AbcInstance instance)
		{
			if (instance == null) return null;
			var type = instance.Type;
			if (type == null) return null;
			return DefineStaticCtor(instance, type);
		}

		private AbcMethod DefineStaticCtor(AbcInstance instance, IType type)
		{
			if (type == null) return null;
			if (instance.IsForeign) return null;

			if (instance.StaticCtor != null)
				return instance.StaticCtor;

			var ctor = type.GetStaticCtor();
			if (ctor != null)
				return instance.StaticCtor = _generator.MethodBuilder.BuildAbcMethod(ctor);

			if (type.HasInitFields(true))
			{
				string name = type.GetStaticCtorName();
				instance.StaticCtor = instance.DefineMethod(
					Sig.@static(name, AvmTypeCode.Void),
					code =>
					{
						code.PushThisScope();
						code.InitFields(type, true, false);
						code.ReturnVoid();
					});
				return instance.StaticCtor;
			}

			return null;
		}

		public void EnsureStaticCtor(AbcInstance instance)
		{
			DefineStaticCtor(instance, instance.Type);
		}

		public void DelayCalls(AbcCode code, IList<AbcInstance> list, int arr)
		{
			int vf = arr + 1;
			code.GetLocal(arr);
			int n = list.Count;
			for (int i = 0; i < n; ++i)
			{
				var instance = list[i];

				code.PushNativeBool(false); //delayed
				code.SetLocal(vf);

				GetCalledFlag(code, instance);
				var called = code.IfTrue();

				SetCalledFlag(code, instance, true);

				code.PushNativeBool(true);
				code.SetLocal(vf);

				called.BranchTarget = code.Label();

				code.GetLocal(arr);
				code.GetLocal(vf);
				code.CallAS3("push", 1);
			}
		}

		public void UndelayCalls(AbcCode code, IList<AbcInstance> list, int arr)
		{
			int n = list.Count;
			for (int i = 0; i < n; ++i)
			{
				var instance = list[i];

				code.GetLocal(arr);
				code.PushInt(i);
				code.GetNativeArrayItem();
				var br = code.IfFalse();

				SetCalledFlag(code, instance, false);

				br.BranchTarget = code.Label();
			}
		}
    }
}