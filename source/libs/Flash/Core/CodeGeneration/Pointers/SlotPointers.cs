using System;
using System.Collections.Generic;
using DataDynamics.PageFX.Flash.Abc;
using DataDynamics.PageFX.Flash.Avm;

namespace DataDynamics.PageFX.Flash.Core.CodeGeneration.Pointers
{
	internal sealed class SlotPointers
	{
		private readonly AbcGenerator _generator;
		private readonly Dictionary<string, AbcInstance> _cache = new Dictionary<string, AbcInstance>();

		public SlotPointers(AbcGenerator generator)
		{
			_generator = generator;
		}

		public AbcInstance Define(AbcTrait slot)
		{
			if (slot == null)
				throw new ArgumentNullException("slot");

			//NOTE: VerifyError: Error #1026: Slot 1 exceeds slotCount=0 of Object
			//therefore we can not use Get/Set slots by slot_id.

			string name = "slot_ptr$" + slot.NameString;

			AbcInstance instance;
			if (_cache.TryGetValue(name, out instance))
				return instance;

			var abc = _generator.Abc;

			var instanceName = abc.DefineName(QName.PfxPackage(name));
			instance = abc.DefineEmptyInstance(instanceName, false);
			_cache[name] = instance;

			var obj = instance.CreatePrivateSlot("_obj", AvmTypeCode.Object);

			instance.Initializer = abc.DefineTraitsInitializer(obj);

			instance.DefineMethod(
				Sig.ptr_get,
				code =>
					{
						code.LoadThis();
						code.GetProperty(obj);
						code.GetProperty(slot);
						code.ReturnValue();
					});

			instance.DefineMethod(
				Sig.ptr_set,
				code =>
					{
						code.LoadThis();
						code.GetProperty(obj);
						code.GetLocal(1); //value
						code.SetProperty(slot);
						code.ReturnVoid();
					});

			return instance;
		}
	}
}
