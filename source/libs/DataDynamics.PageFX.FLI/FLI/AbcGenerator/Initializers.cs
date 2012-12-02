using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DataDynamics.PageFX.CodeModel;
using DataDynamics.PageFX.CodeModel.TypeSystem;
using DataDynamics.PageFX.FLI.ABC;
using DataDynamics.PageFX.FLI.IL;

namespace DataDynamics.PageFX.FLI
{
    internal partial class AbcGenerator
    {
        /// <summary>
        /// Enshures iinit and cinit methods of given instance.
        /// </summary>
        /// <param name="instance"></param>
        public void EnshureInitializers(AbcInstance instance)
        {
            if (instance.IsForeign) return;
            EnshureInstanceInitializer(instance);
            EnshureClassInitializer(instance.Class);
        }

        #region EnshureInstanceInitializer
        void EnshureInstanceInitializer(AbcInstance instance)
        {
            if (instance.Initializer != null)
                return;

#if DEBUG
            DebugService.DoCancel();
#endif

            if (instance.IsInterface)
            {
                instance.Initializer = _abc.DefineEmptyAbstractMethod();
                return;
            }

            if (!GlobalSettings.ReflectionSupport)
            {
                var ctor = InternalTypeExtensions.FindInitializer(instance);
                if (ctor != null)
                {
                    DefineAbcMethod(ctor);

                    if (instance.Initializer == null)
                        throw new InvalidOperationException();

                    return;
                }
            }

            var type = instance.Type;
            instance.Initializer = DefineDefaultInitializer(type);
            
            if (instance.Initializer == null)
                throw new InvalidOperationException();
        }

        AbcMethod DefineDefaultInitializer(IType type)
        {
            return _abc.DefineInitializer(
	            code =>
		            {
			            code.PushThisScope();
			            code.InitFields(type, false, false);
			            code.ConstructSuper();
			            code.ReturnVoid();
		            });
        }
        #endregion

        #region EnshureClassInitializer
        void EnshureClassInitializer(AbcClass klass)
        {
            if (klass.Initializer != null) return;

#if DEBUG
            DebugService.DoCancel();
#endif

            var type = klass.Type;
            if (type != null && type.IsEnum)
            {
                if (AbcGenConfig.InitEnumFields && !AbcGenConfig.ExludeEnumConstants)
                    klass.Initializer = DefineEnumCinit(type);
                else
                    klass.Initializer = _abc.DefineEmptyMethod();
                return;
            }

            var instance = klass.Instance;
            if (instance.IsInterface)
            {
                klass.Initializer = _abc.DefineEmptyMethod();
            }
            else
            {
                EnsureStaticCtor(instance);

                var info = GetClassInitInfo(instance);
                if (info != null && info.MustDefine)
                {
                    klass.Initializer = DefineClassInitializer(klass, info);
                }
                else
                {
                    klass.Initializer = _abc.DefineEmptyMethod();
                }
            }

            if (klass.Initializer == null)
                throw new InvalidOperationException();
        }
        #endregion

        #region DefineClassInitializer
        class InitInfo
        {
            public readonly List<IField> Assets = new List<IField>();

            public bool MustDefine
            {
                get { return Assets.Count > 0; }
            }
        }

        AbcMethod DefineClassInitializer(AbcClass klass, InitInfo info)
        {
            var assetTraits = new List<AbcTrait>();
            foreach (var f in info.Assets)
            {
                DefineField(f);
                var t = f.Tag as AbcTrait;
                if (t == null)
                    throw new InvalidOperationException();
                Debug.Assert(t.AssetInstance != null);
                assetTraits.Add(t);
            }

            var type = klass.Type;

            return _abc.DefineInitializer(
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

        static InitInfo GetClassInitInfo(AbcInstance instance)
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
        #endregion

        #region DefineEnumCinit
        private AbcMethod DefineEnumCinit(IType type)
        {
            if (type == null) return null;
            if (!type.IsEnum) return null;

        	var method = new AbcMethod {ReturnType = _abc.BuiltinTypes.Void};
        	var body = new AbcMethodBody(method);

            AddMethod(method);

            var code = new AbcCode(_abc);

            foreach (var field in type.Fields.Where(field => field.IsConstant))
            {
            	code.LoadThis();
            	code.LoadConstant(field.Value);
            	code.InitProperty(GetFieldName(field));
            }

            code.ReturnVoid();

            body.Finish(code);

            return method;
        }
        #endregion

        #region DefineStaticCtor
        AbcMethod DefineStaticCtor(AbcInstance instance)
        {
            if (instance == null) return null;
            var type = instance.Type;
            if (type == null) return null;
            return DefineStaticCtor(instance, type);
        }

        AbcMethod DefineStaticCtor(AbcInstance instance, IType type)
        {
            if (type == null) return null;
            if (instance.IsForeign) return null;

            if (instance.StaticCtor != null)
                return instance.StaticCtor;

            var ctor = type.GetStaticCtor();
            if (ctor != null)
                return instance.StaticCtor = DefineAbcMethod(ctor);

            if (InternalTypeExtensions.HasInitFields(type, true))
            {
                string name = type.GetStaticCtorName();
                instance.StaticCtor = instance.DefineStaticMethod(
                    name, AvmTypeCode.Void,
                    code =>
	                    {
		                    code.PushThisScope();
		                    code.InitFields(type, true, false);
		                    code.ReturnVoid();
	                    });
                return instance.StaticCtor;
            }
            return null;
        }

        void EnsureStaticCtor(AbcInstance instance)
        {
            DefineStaticCtor(instance, instance.Type);
        }
        #endregion
    }
}