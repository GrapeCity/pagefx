using DataDynamics.PageFX.Common.Extensions;
using DataDynamics.PageFX.Common.Services;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.FlashLand.Abc;
using DataDynamics.PageFX.FlashLand.Core.ByteCodeGeneration.CorlibTypes;

namespace DataDynamics.PageFX.FlashLand.Core.ByteCodeGeneration
{
    partial class AbcGenerator
    {
        const string DebugPropertyPrefix = "pfx$debug$";

        public void DefineDebugInfo(IType type, AbcInstance instance)
        {
			if (!GlobalSettings.EmitDebugInfo) return;
			if (!GlobalSettings.EmitDebugDisplay) return;

            DefineDebuggerDisplay(type, instance);
            
            if (IsCollection(type))
            {
                DefineDebugCollectionView(instance);
            }
        }

        #region DefineDebuggerDisplay
        void DefineDebuggerDisplay(IType type, AbcInstance instance)
        {
			if (type.IsInterface) return;
            if (type.Is(SystemTypeCode.Object)) return;
            if (type.Is(SystemTypeCode.String)) return;
            if (type.Is(SystemTypeCode.Array)) return;

            //MSDN: DebuggerDisplay attribute takes precedence over the ToString() override
            var attr = type.FindAttribute(Attrs.DebuggerDisplay);
            if (attr != null)
            {
                if (DefineDebuggerDisplay(type, instance, attr))
                    return;
            }

            //Use ToString
            var toString = type.Methods.Find(Const.Object.MethodToString, 0);
            if (toString != null)
            {
                var tostr = DefineAbcMethod(toString);

                var name = _abc.DefineGlobalQName(DebugPropertyPrefix + "display");

				var m = instance.DefineInstanceGetter(
                    name, AvmTypeCode.String,
                    code =>
                        {
                            code.LoadThis();	// this might be redundant --olegz
                            code.Call(tostr);
                            code.ReturnValue();
                        });

				// as soon the method will be overridden in descendants we have to mark it as virtual (case 147084)
            	m.Trait.IsVirtual = !type.IsSealed;
                m.Trait.IsOverride = instance.FindSuperTrait(name, AbcTraitKind.Getter) != null;
            }
        }

        #region DebuggerDisplay attribute
        bool DefineDebuggerDisplay(IType type, AbcInstance instance, ICustomAttribute attr)
        {
            if (attr.Arguments.Count != 1) return false;
            var display = attr.Arguments[0].Value as string;
            if (string.IsNullOrEmpty(display)) return false;
            if (!display.CheckFormatBraceBalance())
            {
                CompilerReport.Add(Warnings.InvalidDebuggerDisplayString, display);
                return false;
            }

            var name = _abc.DefineGlobalQName(DebugPropertyPrefix + "display$exp");

            //TODO: Parse display string to build string
            var m = instance.DefineInstanceGetter(
                name, AvmTypeCode.String,
                code =>
                {
                    code.PushString(display);
                    code.ReturnValue();
                });

			m.Trait.IsVirtual = !type.IsSealed;
			m.Trait.IsOverride = instance.FindSuperTrait(name, AbcTraitKind.Getter) != null;

            return true;
        }
        #endregion
        #endregion

        #region DefineDebugCollectionView
        void DefineDebugCollectionView(AbcInstance instance)
        {
            var name = _abc.DefineGlobalQName(DebugPropertyPrefix + "collection$view");

            if (instance.FindSuperTrait(name, AbcTraitKind.Getter) != null)
                return;

            if (IsDictionary(instance.Type))
            {
                instance.DefineInstanceGetter(
                    DebugPropertyPrefix + "dictionary$marker", AvmTypeCode.Boolean,
                    code =>
                        {
                            code.PushNativeBool(true);
                            code.ReturnValue();
                        });
            }

            instance.DefineInstanceGetter(
                name, AvmTypeCode.Array,
                code =>
                    {
                        var m = GetMethod(CompilerMethodId.ToArray);
                        code.Getlex(m);
                        code.LoadThis();
                        code.Call(m);
                        code.ReturnValue();
                    });
        }
        #endregion

        #region Utils
        private bool IsCollection(IType type)
        {
            if (type == null) return false;
            if (type.TypeKind != TypeKind.Class) return false;
            if (type.Is(SystemTypeCode.String)) return false;
            if (type.Is(SystemTypeCode.Array)) return false;
            if (type.Implements(GetType(CorlibTypeId.IEnumerable)))
            {
                //TODO: Do extra filter
                return true;
            }
            return false;
        }

        private bool IsDictionary(IType type)
        {
            if (type == null) return false;
            if (type.TypeKind != TypeKind.Class) return false;
            return type.Implements(GetType(CorlibTypeId.IDictionary));
        }
        #endregion
    }
}