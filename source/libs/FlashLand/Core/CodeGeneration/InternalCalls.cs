using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.FlashLand.Abc;

namespace DataDynamics.PageFX.FlashLand.Core.CodeGeneration
{
    partial class AbcGenerator
    {
        #region DefineCopyMethod
        public AbcMethod DefineStaticCopyMethod(IType type)
        {
            if (!InternalTypeExtensions.HasCopy(type)) return null;
            var instance = DefineAbcInstance(type);
            return DefineStaticCopyMethod(instance);
        }

        public AbcMethod DefineStaticCopyMethod(AbcInstance instance)
        {
            var copy = DefineCopyMethod(instance);
            if (copy == null) return null;

            var name = Abc.DefinePfxName("__static_copy__");

            return instance.DefineStaticMethod(
                name, instance.Name,
                code =>
                    {
                        code.GetLocal(1); //value
                        code.Call(copy);
                        code.ReturnValue();
                    },
                instance.Name, "value");
        }

        public AbcMethod DefineCopyMethod(IType type)
        {
            if (!InternalTypeExtensions.HasCopy(type)) return null;
            var instance = DefineAbcInstance(type);
            return DefineCopyMethod(instance);
        }

        /// <summary>
        /// Signature: Instance Instance.__copy__()
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public AbcMethod DefineCopyMethod(AbcInstance instance)
        {
            var type = instance.Type;
            if (type == null) return null;
            if (!InternalTypeExtensions.HasCopy(type)) return null;

            var name = Abc.DefinePfxName("__copy__");

            return instance.DefineInstanceMethod(
                name, instance.Name,
                code =>
                    {
                        //SUPER BUG:
                        //For some times like System.Int64 DefineCopyMethod method can be called before DefineFields
                        //so we should define type fields
                        DefineFields(type);

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
        public AbcMethod DefineCopyFromMethod(AbcInstance instance)
        {
            var type = instance.Type;
            if (!InternalTypeExtensions.HasCopy(type)) return null;

            var name = Abc.DefinePfxName("__copy_from__");

            return instance.DefineInstanceMethod(
                name, AvmTypeCode.Void,
                code =>
                    {
                        code.CopySlots(instance, 1, 0);
                        code.ReturnVoid();
                    },
                instance.Name, "value");
        }
        #endregion

        #region BeginMethod
        private AbcMethod BeginMethod(IMethod method, AbcInstance instance)
        {
            var abcMethod = new AbcMethod(method);

	        SetData(method, abcMethod);

            Abc.Methods.Add(abcMethod);

            var trait = DefineMethodTrait(abcMethod, method);
            instance.AddTrait(trait, method.IsStatic);

            abcMethod.ReturnType = DefineReturnType(method.Type);
            DefineParameters(abcMethod, method);

            var body = new AbcMethodBody(abcMethod);
            Abc.MethodBodies.Add(body);
            
            return abcMethod;
        }
        #endregion
    }
}