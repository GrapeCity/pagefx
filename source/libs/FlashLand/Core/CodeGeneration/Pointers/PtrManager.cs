using System;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.FlashLand.Abc;

namespace DataDynamics.PageFX.FlashLand.Core.CodeGeneration.Pointers
{
	internal sealed class PtrManager
	{
		private readonly AbcGenerator _generator;
		private FuncPtr _funcPtr;
		private PropertyPtr _propertyPtr;
		private GlobalPropertyPtr _globalPropertyPtr;
		private ElemPtr _elemPtr;
		private readonly SlotPointers _slotPtrs;

		public PtrManager(AbcGenerator generator)
		{
			_generator = generator;
			_slotPtrs = new SlotPointers(generator);
		}

		private AbcFile Abc
		{
			get { return _generator.Abc; }
		}

		internal FuncPtr FuncPtr
		{
			get { return _funcPtr ?? (_funcPtr = new FuncPtr(Abc)); }
		}

		private PropertyPtr PropPtr
		{
			get { return _propertyPtr ?? (_propertyPtr = new PropertyPtr(Abc)); }
		}

		private GlobalPropertyPtr GlobalPropertyPtr
		{
			get { return _globalPropertyPtr ?? (_globalPropertyPtr = new GlobalPropertyPtr(Abc)); }
		}

		internal ElemPtr ElemPtr
		{
			get { return _elemPtr ?? (_elemPtr = new ElemPtr(_generator)); }
		}

		public AbcInstance FieldPtr(IField field)
		{
			return field.IsStatic
				       ? StaticFieldPtr.Define(_generator, field)
				       : PropertyPtr(_generator.GetFieldName(field));
		}

		public AbcInstance PropertyPtr(AbcMultiname name)
		{
			return name.Namespace.IsGlobalPackage
				       ? GlobalPropertyPtr.Instance
				       : PropPtr.Instance;
		}

		public AbcInstance SlotPtr(AbcTrait slot)
		{
			if (slot == null)
				throw new ArgumentNullException("slot");

			return _slotPtrs.Define(slot);
		}

		public AbcMethod GetElemPtr()
		{
			var instance = _generator.GetArrayInstance();

			var name = _generator.DefinePfxName("GetElemPtr");
			return instance.DefineMethod(
				Sig.@this(name, AvmTypeCode.Object, AvmTypeCode.Int32, "index"),
				code =>
					{
						var ptr = ElemPtr.Instance;
						code.Getlex(ptr);
						code.LoadThis();
						code.GetLocal(1); //index
						code.Construct(2);
						code.ReturnValue();
					});
		}
	}
}