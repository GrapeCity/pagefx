using DataDynamics.PageFX.Flash.Abc;
using DataDynamics.PageFX.Flash.Avm;

namespace DataDynamics.PageFX.Flash.Core.CodeGeneration.Pointers
{
	internal sealed class ElemPtr
	{
		private readonly AbcGenerator _generator;
		private AbcInstance _instance;

		public ElemPtr(AbcGenerator generator)
		{
			_generator = generator;
		}

		public AbcInstance Instance
		{
			get { return _instance ?? (_instance = Build()); }
		}

		private AbcInstance Build()
		{
			var abc = _generator.Abc;
			var instanceName = abc.DefineName(QName.PfxPackage("elem_ptr"));
			var instance = abc.DefineEmptyInstance(instanceName, false);

			var arrType = _generator.Corlib.Array.Instance;
			var arr = instance.CreatePrivateSlot("_arr", arrType);
			var index = instance.CreatePrivateSlot("_index", AvmTypeCode.Int32);

			instance.Initializer = abc.DefineTraitsInitializer(arr, index);

			instance.DefineMethod(
				Sig.ptr_get,
				code =>
					{
						code.LoadThis();
						code.GetProperty(arr);
						code.LoadThis();
						code.GetProperty(index);
						code.GetArrayElem(false);
						code.ReturnValue();
					});

			instance.DefineMethod(
				Sig.ptr_set,
				code =>
					{
						code.LoadThis();
						code.GetProperty(arr);
						code.LoadThis();
						code.GetProperty(index);
						code.GetLocal(1); //value
						code.SetArrayElem(false);
						code.ReturnVoid();
					});

			return instance;
		}
	}
}
