using System;
using DataDynamics.PageFX.Common.Extensions;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.FlashLand.Abc;

namespace DataDynamics.PageFX.FlashLand.Core.CodeGeneration.Pointers
{
	internal static class StaticFieldPtr
	{
		public static AbcInstance Define(AbcGenerator generator, IField field)
		{
			if (field == null)
				throw new ArgumentNullException("field");
			if (!field.IsStatic)
				throw new ArgumentException("field is not static");

			var abc = generator.Abc;

			var type = field.DeclaringType;
			string typeName = type.FullName.MakeFullName(field.Name);
			var name = abc.DefineName(QName.PfxPublic("sfld_ptr$" + typeName));

			var instance = abc.Instances[name];
			if (instance != null)
				return instance;

			var fieldInstance = generator.TypeBuilder.BuildInstance(type);

			instance = abc.DefineEmptyInstance(name, true);

			instance.DefineMethod(
				Sig.ptr_get,
				code =>
					{
						code.Getlex(fieldInstance);
						code.GetField(field);
						code.ReturnValue();
					});

			instance.DefineMethod(
				Sig.ptr_set,
				code =>
					{
						code.Getlex(fieldInstance);
						code.GetLocal(1); //value
						code.SetField(field);
						code.ReturnVoid();
					});

			var ti = instance.CreatePrivateStaticSlot("_instance", instance);

			instance.DefineMethod(
				Sig.get(Const.Instance, instance.Name).@static(),
				code =>
					{
						code.LoadThis();
						code.GetProperty(ti);
						var br = code.IfNotNull(true);

						code.LoadThis();
						code.LoadThis();
						code.Construct(0);
						code.SetProperty(ti);

						br.BranchTarget = code.Label();
						code.LoadThis();
						code.GetProperty(ti);
						code.ReturnValue();
					});

			return instance;
		}
	}
}
