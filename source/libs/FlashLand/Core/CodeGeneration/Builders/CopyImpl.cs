using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.FlashLand.Abc;

namespace DataDynamics.PageFX.FlashLand.Core.CodeGeneration.Builders
{
    internal sealed class CopyImpl
    {
	    private readonly AbcGenerator _generator;

		public static CopyImpl With(AbcGenerator generator)
		{
			return new CopyImpl(generator);
		}

	    private CopyImpl(AbcGenerator generator)
		{
			_generator = generator;
		}

	    public AbcMethod StaticCopy(IType type)
        {
            if (!InternalTypeExtensions.HasCopy(type)) return null;
			var instance = _generator.TypeBuilder.BuildInstance(type);
            return StaticCopy(instance);
        }

        public AbcMethod StaticCopy(AbcInstance instance)
        {
            var copy = Copy(instance);
            if (copy == null) return null;

			var name = _generator.Abc.DefineName(QName.PfxPublic("__static_copy__"));

	        return instance.DefineMethod(
		        Sig.@static(name, instance.Name, instance.Name, "value"),
		        code =>
			        {
				        code.GetLocal(1); //value
				        code.Call(copy);
				        code.ReturnValue();
			        });
        }

        public AbcMethod Copy(IType type)
        {
            if (!InternalTypeExtensions.HasCopy(type)) return null;
			var instance = _generator.TypeBuilder.BuildInstance(type);
            return Copy(instance);
        }

        /// <summary>
        /// Signature: Instance Instance.__copy__()
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public AbcMethod Copy(AbcInstance instance)
        {
            var type = instance.Type;
            if (type == null) return null;
            if (!InternalTypeExtensions.HasCopy(type)) return null;

			var name = _generator.Abc.DefineName(QName.PfxPublic("__copy__"));

            return instance.DefineMethod(
                Sig.@this(name, instance.Name),
                code =>
                    {
                        //SUPER BUG:
                        //For some times like System.Int64 DefineCopyMethod method can be called before DefineFields
                        //so we should define type fields
						_generator.TypeBuilder.DefineFields(type);

                        const int obj = 1;
                        code.CreateInstance(instance);

                        code.SetLocal(obj);

                        code.CopySlots(instance, 0, obj);

                        code.GetLocal(obj);
                        code.ReturnValue();
                    });
        }

        /// <summary>
        /// Signature: void Instance.__copy_from__(Instance value)
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public AbcMethod CopyFrom(AbcInstance instance)
        {
            var type = instance.Type;
            if (!InternalTypeExtensions.HasCopy(type)) return null;

			var name = _generator.Abc.DefineName(QName.PfxPublic("__copy_from__"));

	        return instance.DefineMethod(
		        Sig.@this(name, AvmTypeCode.Void, instance.Name, "value"),
		        code =>
			        {
				        code.CopySlots(instance, 1, 0);
				        code.ReturnVoid();
			        });
        }
    }
}