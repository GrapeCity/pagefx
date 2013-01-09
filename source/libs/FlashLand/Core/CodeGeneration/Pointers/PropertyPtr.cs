using DataDynamics.PageFX.FlashLand.Abc;

namespace DataDynamics.PageFX.FlashLand.Core.CodeGeneration.Pointers
{
	internal sealed class PropertyPtr
	{
		private readonly AbcFile _abc;
		private AbcInstance _instance;

		public PropertyPtr(AbcFile abc)
		{
			_abc = abc;
		}

		public AbcInstance Instance
		{
			get { return _instance ?? (_instance = Build()); }
		}

		private AbcInstance Build()
		{
			var instanceName = _abc.DefineName(new PfxQName("prop_ptr"));
			var instance = _abc.DefineEmptyInstance(instanceName, false);

			var obj = instance.CreatePrivateSlot("_obj", AvmTypeCode.Object);
			var ns = instance.CreatePrivateSlot("_ns", AvmTypeCode.Namespace);
			var name = instance.CreatePrivateSlot("_name", AvmTypeCode.String);

			instance.Initializer = _abc.DefineTraitsInitializer(obj, ns, name);

			instance.DefineMethod(
				Sig.ptr_get,
				code =>
					{
						code.LoadThis();
						code.GetProperty(obj);
						code.LoadThis();
						code.GetProperty(ns);
						code.LoadThis();
						code.GetProperty(name);
						code.GetRuntimeProperty();
						code.ReturnValue();
					});

			instance.DefineMethod(
				Sig.ptr_set,
				code =>
					{
						code.LoadThis();
						code.GetProperty(obj);
						code.LoadThis();
						code.GetProperty(ns);
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
