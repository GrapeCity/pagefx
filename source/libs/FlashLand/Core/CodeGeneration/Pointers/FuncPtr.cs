using System;
using DataDynamics.PageFX.FlashLand.Abc;
using DataDynamics.PageFX.FlashLand.Avm;

namespace DataDynamics.PageFX.FlashLand.Core.CodeGeneration.Pointers
{
	internal sealed class FuncPtr
	{
		private readonly AbcFile _abc;
		private AbcInstance _instance;

		public FuncPtr(AbcFile abc)
		{
			if (abc == null)
				throw new ArgumentNullException("abc");

			_abc = abc;
		}

		public AbcInstance Instance
		{
			get { return _instance ?? (_instance = Build()); }
		}

		private AbcInstance Build()
		{
			var name = _abc.DefineName(QName.PfxPackage("func_ptr"));
			var instance = _abc.DefineEmptyInstance(name, false);

			var getter = instance.CreatePrivateSlot("_getter", AvmTypeCode.Function);
			var setter = instance.CreatePrivateSlot("_setter", AvmTypeCode.Function);

			instance.Initializer = _abc.DefineTraitsInitializer(getter, setter);

			instance.DefineMethod(
				Sig.ptr_get,
				code =>
					{
						code.LoadThis();
						code.GetProperty(getter);
						code.PushNull();
						code.CallFunction(1); //this
						code.ReturnValue();
					});

			instance.DefineMethod(
				Sig.ptr_set,
				code =>
					{
						code.LoadThis();
						code.GetProperty(setter);
						code.PushNull();
						code.GetLocal(1); //value
						code.CallFunction(2); //this + value
						code.Pop();
						code.ReturnVoid();
					});

			return instance;
		}

		public AbcMethod GetProperty(AbcMultiname prop)
		{
			if (prop == null)
				throw new ArgumentNullException("prop");

			var m = _abc.DefineMethod(
				AvmTypeCode.Object,
				code =>
					{
						code.Getlex(prop);
						code.ReturnValue();
					});

#if DEBUG
			m.Name = _abc.DefineString("get_" + prop.NameString);
#endif

			return m;
		}

		public AbcMethod SetProperty(AbcMultiname prop)
		{
			if (prop == null)
				throw new ArgumentNullException("prop");

			var m = _abc.DefineMethod(
				AvmTypeCode.Void,
				code =>
					{
						code.FindProperty(prop);
						code.GetLocal(1); //value
						code.SetProperty(prop);
						code.ReturnVoid();
					},
				AvmTypeCode.Object, "value");

#if DEBUG
			m.Name = _abc.DefineString("set_" + prop.NameString);
#endif

			return m;
		}
	}
}
