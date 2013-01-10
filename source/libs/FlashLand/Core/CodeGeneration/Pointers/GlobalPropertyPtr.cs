using DataDynamics.PageFX.FlashLand.Abc;
using DataDynamics.PageFX.FlashLand.Avm;

namespace DataDynamics.PageFX.FlashLand.Core.CodeGeneration.Pointers
{
	internal sealed class GlobalPropertyPtr
	{
		private readonly AbcFile _abc;
		private AbcInstance _instance;

		public GlobalPropertyPtr(AbcFile abc)
		{
			_abc = abc;
		}

		public AbcInstance Instance
		{
			get { return _instance ?? (_instance = Build()); }
		}

		private AbcInstance Build()
		{
			var instanceName = _abc.DefineName(QName.PfxPackage("gprop_ptr"));
			var instance = _abc.DefineEmptyInstance(instanceName, false);

			var obj = instance.CreatePrivateSlot("_obj", AvmTypeCode.Object);
			var name = instance.CreatePrivateSlot("_name", AvmTypeCode.String);

			instance.Initializer = _abc.DefineTraitsInitializer(obj, name);

			instance.DefineMethod(
				Sig.ptr_get,
				code =>
					{
						code.LoadThis();
						code.GetProperty(obj);
						code.PushGlobalPackage();
						code.LoadThis();
						code.GetProperty(name);
						code.GetRuntimeProperty();
						code.ReturnValue();
					});

			instance.DefineMethod(Sig.ptr_set, code =>
				{
					code.LoadThis();
					code.GetProperty(obj);
					code.PushGlobalPackage();
					code.LoadThis();
					code.GetProperty(name);
					code.GetLocal(1); //value
					code.SetRuntimeProperty();
					code.ReturnVoid();
				});

			return instance;
		}
	}
}
