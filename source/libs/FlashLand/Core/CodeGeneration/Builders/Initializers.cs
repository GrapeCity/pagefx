using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.FlashLand.Abc;
using DataDynamics.PageFX.FlashLand.Core.Tools;
using DataDynamics.PageFX.FlashLand.IL;

namespace DataDynamics.PageFX.FlashLand.Core.CodeGeneration.Builders
{
    internal static class Initializers
    {
		/// <summary>
        /// Enshures iinit and cinit methods of given instance.
        /// </summary>
        /// <param name="instance"></param>
        public static void EnshureInitializers(AbcInstance instance)
        {
            if (instance.IsForeign) return;
            EnshureInstanceInitializer(instance);
            EnshureClassInitializer(instance.Class);
        }

	    private static void EnshureInstanceInitializer(AbcInstance instance)
        {
            if (instance.Initializer != null)
                return;

#if DEBUG
            DebugService.DoCancel();
#endif

            if (instance.IsInterface)
            {
                instance.Initializer = instance.Abc.DefineEmptyAbstractMethod();
                return;
            }

	        var generator = instance.Generator;

            if (!GlobalSettings.ReflectionSupport)
            {
                var ctor = InternalTypeExtensions.FindInitializer(instance);
                if (ctor != null)
                {
					generator.MethodBuilder.BuildAbcMethod(ctor);

                    if (instance.Initializer == null)
                        throw new InvalidOperationException();

                    return;
                }
            }

            var type = instance.Type;
            instance.Initializer = BuildDefaultInitializer(instance.Abc, type);
            
            if (instance.Initializer == null)
                throw new InvalidOperationException();
        }

        public static AbcMethod BuildDefaultInitializer(AbcFile abc, IType type)
        {
            return abc.DefineInitializer(
	            code =>
		            {
			            code.PushThisScope();
			            code.InitFields(type, false, false);
			            code.ConstructSuper();
			            code.ReturnVoid();
		            });
        }

	    private static void EnshureClassInitializer(AbcClass klass)
        {
            if (klass.Initializer != null) return;

#if DEBUG
            DebugService.DoCancel();
#endif
	        var abc = klass.Abc;
	        var type = klass.Type;
            if (type != null && type.IsEnum)
            {
                if (AbcGenConfig.InitEnumFields && !AbcGenConfig.ExludeEnumConstants)
                    klass.Initializer = BuildEnumCinit(abc, type);
                else
                    klass.Initializer = abc.DefineEmptyMethod();
                return;
            }

            var instance = klass.Instance;
			var generator = instance.Generator;
            if (instance.IsInterface)
            {
                klass.Initializer = abc.DefineEmptyMethod();
            }
            else
            {
				generator.StaticCtors.EnsureStaticCtor(instance);

                var info = GetClassInitInfo(instance);
                if (info != null && info.MustDefine)
                {
                    klass.Initializer = BuildClassInitializer(klass, info);
                }
                else
                {
                    klass.Initializer = abc.DefineEmptyMethod();
                }
            }

            if (klass.Initializer == null)
                throw new InvalidOperationException();
        }

	    private class InitInfo
        {
            public readonly List<IField> Assets = new List<IField>();

            public bool MustDefine
            {
                get { return Assets.Count > 0; }
            }
        }

        private static AbcMethod BuildClassInitializer(AbcClass klass, InitInfo info)
        {
	        var abc = klass.Abc;
	        var generator = abc.Generator;

            var assetTraits = new List<AbcTrait>();
            foreach (var f in info.Assets)
            {
				generator.FieldBuilder.Build(f);
                var t = f.Data as AbcTrait;
                if (t == null)
                    throw new InvalidOperationException();
                Debug.Assert(t.AssetInstance != null);
                assetTraits.Add(t);
            }

            var type = klass.Type;

            return abc.DefineInitializer(
	            code =>
		            {
			            code.PushThisScope();

			            code.InitFields(type, true, false);

			            foreach (var t in assetTraits)
			            {
				            code.LoadThis();
				            code.Getlex(t.AssetInstance.Name);
				            code.SetProperty(t.Name);
			            }

			            code.ReturnVoid();
		            });
        }

        private static InitInfo GetClassInitInfo(AbcInstance instance)
        {
            var type = instance.Type;
            if (type == null) return null;
            var info = new InitInfo();
            foreach (var field in type.Fields)
            {
                if (field.HasEmbedAttribute())
                    info.Assets.Add(field);
            }
            return info;
        }

	    private static AbcMethod BuildEnumCinit(AbcFile abc, IType type)
        {
            if (type == null) return null;
            if (!type.IsEnum) return null;

        	var method = new AbcMethod {ReturnType = abc.BuiltinTypes.Void};
        	var body = new AbcMethodBody(method);

	        abc.AddMethod(method);

	        var code = new AbcCode(abc);

	        var generator = abc.Generator;
            foreach (var field in type.Fields.Where(field => field.IsConstant))
            {
            	code.LoadThis();
            	code.LoadConstant(field.Value);
				code.InitProperty(generator.FieldBuilder.GetFieldName(field));
            }

            code.ReturnVoid();

            body.Finish(code);

            return method;
        }
    }
}