using DataDynamics.PageFX.Common.CompilerServices;
using DataDynamics.PageFX.Common.Extensions;
using DataDynamics.PageFX.Common.Services;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.FlashLand.Abc;
using DataDynamics.PageFX.FlashLand.Avm;
using DataDynamics.PageFX.FlashLand.Core.CodeGeneration.Corlib;

namespace DataDynamics.PageFX.FlashLand.Core.CodeGeneration.Builders
{
    internal static class DebugInfoBuilder
    {
        const string DebugPropertyPrefix = "pfx$debug$";

        public static void Build(AbcGenerator generator, IType type, AbcInstance instance)
        {
			if (!GlobalSettings.EmitDebugInfo) return;
			if (!GlobalSettings.EmitDebugDisplay) return;

			DefineDebuggerDisplay(generator, type, instance);
            
            if (IsCollection(generator, type))
            {
                DefineDebugCollectionView(generator, instance);
            }
        }

	    private static void DefineDebuggerDisplay(AbcGenerator generator, IType type, AbcInstance instance)
        {
			if (type.IsInterface) return;
            if (type.Is(SystemTypeCode.Object)) return;
            if (type.Is(SystemTypeCode.String)) return;
            if (type.Is(SystemTypeCode.Array)) return;

            //MSDN: DebuggerDisplay attribute takes precedence over the ToString() override
            var attr = type.FindAttribute(Attrs.DebuggerDisplay);
            if (attr != null)
            {
                if (DefineDebuggerDisplay(generator, type, instance, attr))
                    return;
            }

            //Use ToString
            var toString = type.Methods.Find(Const.Object.MethodToString, 0);
            if (toString != null)
            {
				var tostr = generator.MethodBuilder.BuildAbcMethod(toString);

                var name = generator.Abc.DefineName(QName.Global(DebugPropertyPrefix + "display"));

	            var m = instance.DefineMethod(
					Sig.get(name, AvmTypeCode.String),
		            code =>
			            {
				            code.LoadThis(); // this might be redundant --olegz
				            code.Call(tostr);
				            code.ReturnValue();
			            });

				// as soon the method will be overridden in descendants we have to mark it as virtual (case 147084)
            	m.Trait.IsVirtual = !type.IsSealed;
                m.Trait.IsOverride = instance.FindSuperTrait(name, AbcTraitKind.Getter) != null;
            }
        }

	    private static bool DefineDebuggerDisplay(AbcGenerator generator, IType type, AbcInstance instance, ICustomAttribute attr)
        {
            if (attr.Arguments.Count != 1) return false;
            var display = attr.Arguments[0].Value as string;
            if (string.IsNullOrEmpty(display)) return false;
            if (!display.CheckFormatBraceBalance())
            {
                CompilerReport.Add(Warnings.InvalidDebuggerDisplayString, display);
                return false;
            }

            var name = generator.Abc.DefineName(QName.Global(DebugPropertyPrefix + "display$exp"));

            //TODO: Parse display string to build string
	        var m = instance.DefineMethod(
				Sig.get(name, AvmTypeCode.String),
		        code =>
			        {
				        code.PushString(display);
				        code.ReturnValue();
			        });

			m.Trait.IsVirtual = !type.IsSealed;
			m.Trait.IsOverride = instance.FindSuperTrait(name, AbcTraitKind.Getter) != null;

            return true;
        }

	    private static void DefineDebugCollectionView(AbcGenerator generator, AbcInstance instance)
        {
            var name = generator.Abc.DefineName(QName.Global(DebugPropertyPrefix + "collection$view"));

            if (instance.FindSuperTrait(name, AbcTraitKind.Getter) != null)
                return;

            if (IsDictionary(generator, instance.Type))
            {
	            instance.DefineMethod(
					Sig.get(DebugPropertyPrefix + "dictionary$marker", AvmTypeCode.Boolean),
		            code =>
			            {
				            code.PushNativeBool(true);
				            code.ReturnValue();
			            });
            }

	        instance.DefineMethod(
				Sig.get(name, AvmTypeCode.Array),
		        code =>
			        {
				        var m = generator.Corlib.GetMethod(CompilerMethodId.ToArray);
				        code.Getlex(m);
				        code.LoadThis();
				        code.Call(m);
				        code.ReturnValue();
			        });
        }

	    private static bool IsCollection(AbcGenerator generator, IType type)
        {
            if (type == null) return false;
            if (type.TypeKind != TypeKind.Class) return false;
            if (type.Is(SystemTypeCode.String)) return false;
            if (type.Is(SystemTypeCode.Array)) return false;
			if (type.Implements(generator.Corlib.GetType(CorlibTypeId.IEnumerable)))
            {
                //TODO: Do extra filter
                return true;
            }
            return false;
        }

        private static bool IsDictionary(AbcGenerator generator, IType type)
        {
            if (type == null) return false;
            if (type.TypeKind != TypeKind.Class) return false;
			return type.Implements(generator.Corlib.GetType(CorlibTypeId.IDictionary));
        }
    }
}