using System;
using System.Collections.Generic;
using System.Diagnostics;
using DataDynamics.PageFX.FlashLand.Abc;
using DataDynamics.PageFX.FlashLand.IL;

namespace DataDynamics.PageFX.FlashLand.Core.SwfGeneration
{
	// This module builds flex mixin to initialize flex infrastructure
    internal partial class SwfCompiler
	{
    	#region Inherit Style Names
		//TODO: Get inheriting styles handling Style tag
    	private static readonly string[] CommonInheritStyles =
    		{
    			"alternatingItemColors",
    			"backgroundDisabledColor",
    			"barColor",
    			"color",
    			"disabledColor",
    			"disabledIconColor",
    			"dropShadowColor",
    			"errorColor",
    			"fontAntiAliasType",
    			"fontFamily",
    			"fontGridFitType",
    			"fontSharpness",
    			"fontSize",
    			"fontStyle",
    			"fontThickness",
    			"fontWeight",
    			"footerColors",
    			"headerColors",
    			"iconColor",
    			"kerning",
    			"leading",
    			"letterSpacing",
    			"modalTransparency",
    			"modalTransparencyBlur",
    			"modalTransparencyColor",
    			"modalTransparencyDuration",
    			"rollOverColor",
    			"selectionColor",
    			"selectionDisabledColor",
    			"textAlign",
    			"textDecoration",
    			"textIndent",
    			"textRollOverColor",
    			"textSelectedColor",
    			"themeColor"
    		};

		// inherit style names defined only in flex 3 framework
    	private static readonly string[] Flex3InheritStyles =
    		{
    			"horizontalGridLineColor",
    			"verticalGridLineColor"
    		};

		// inherit style names defined only in flex 4 framework
    	private static readonly string[] Flex4InheritStyles =
    		{
    			"accentColor",
    			"chromeColor",
    			"contentBackgroundAlpha",
    			"contentBackgroundColor",
    			"depthColors",
    			"direction",
    			"dividerColor",
    			"dropdownBorderColor",
    			"focusColor",
    			"interactionMode",
    			"layoutDirection",
    			"locale",
    			"shadowColor",
    			"showErrorSkin",
    			"showErrorTip",
    			"strokeColor",
    			"strokeWidth",
    			"symbolColor"
    		};
		#endregion

		private readonly List<KeyValuePair<string, AbcMultiname>> _remoteClasses = new List<KeyValuePair<string, AbcMultiname>>();
		private readonly List<KeyValuePair<string, string>> _effects = new List<KeyValuePair<string, string>>();

		private AbcInstance _flexModuleFactoryInterface;
		private AbcInstance _childManagerInstance;
    	private AbcInstance _styleManagerInstance;
    	private AbcInstance _styleManager2Interface;
		private AbcInstance _styleManagerImpl;

    	public void RegisterRemoteClass(string alias, AbcMultiname name)
        {
            if (name == null) throw new ArgumentNullException("name");
            _remoteClasses.Add(new KeyValuePair<string, AbcMultiname>(alias, name));
        }

        public void RegisterRemoteClass(string alias, AbcInstance instance)
        {
            RegisterRemoteClass(alias, instance.Name);
        }

        public void RegisterEffectTrigger(string name, string eventName)
        {
            _effects.Add(new KeyValuePair<string, string>(name, eventName));
        }

		private AbcInstance GetFlexModuleFactoryInterface(AbcFile app)
		{
			return ImportType(app, MX.IFlexModuleFactory, ref _flexModuleFactoryInterface);
		}

    	private AbcInstance GetChildManagerInstance(AbcFile app)
    	{
			return ImportType(app, MX.ChildManager, ref _childManagerInstance, true);
    	}

		private AbcInstance GetStyleManagerInstance(AbcFile app)
		{
			return ImportType(app, MX.StyleManager, ref _styleManagerInstance);
		}

		private AbcInstance GetStyleManager2Interface(AbcFile app)
		{
			return ImportType(app, MX.IStyleManager2, ref _styleManager2Interface);
		}

		private AbcInstance GetStyleManagerImpl(AbcFile app)
		{
			return ImportType(app, MX.StyleManagerImpl, ref _styleManagerImpl);
		}

    	private AbcInstance DefineFlexInitMixin(AbcFile app)
        {
            Debug.Assert(IsFlexApplication);

            var flexModuleFactoryInterface = GetFlexModuleFactoryInterface(app);
			var childManagerInstance = GetChildManagerInstance(app);
    		var flex4 = childManagerInstance != null;

			string name = "_FlexInit_" + MxAppPrefix;
			string ns = RootNamespace;

    		var instance = new AbcInstance(true)
    		               	{
    		               		Name = app.DefineQName(ns, name),
    		               		SuperName = app.BuiltinTypes.Object,
    		               		IsMixin = true,
    		               		IsFlexInitMixin = true,
    		               		Initializer = app.DefineEmptyConstructor(),
    		               		Class = {Initializer = app.DefineEmptyMethod()}
    		               	};

    		app.AddInstance(instance);

            instance.DefineStaticMethod(
                "init", AvmTypeCode.Void,
                code =>
                    {
                        code.PushThisScope();

                    	const int moduleFactoryArg = 1;
						const int styleManagerVar = 2;

						if (flex4)
						{
							CreateInstance(code, childManagerInstance, moduleFactoryArg);
							code.Pop();

							var styleManager2 = GetStyleManager2Interface(app);
							var styleManagerImpl = GetStyleManagerImpl(app);
							CreateInstance(code, styleManagerImpl, moduleFactoryArg);
							code.Coerce(styleManager2);
							code.SetLocal(styleManagerVar);
						}

                    	RegisterEffectTriggers(app, code);
                        RegisterRemoteClasses(app, code);

                    	Action pushStyleManager;
						if (flex4)
						{
							pushStyleManager = () => code.GetLocal(styleManagerVar);
						}
						else
						{
							pushStyleManager = () => code.Getlex(GetStyleManagerInstance(app));
						}

						RegisterInheritStyles(app, code, pushStyleManager, flex4);

                    	//NOTE: Uncomment to add forward refernce to Flex Application
						//var appInstance = app.generator.MainInstance;
						//code.Trace(string.Format("PFC: forward reference to FlexApp class {0}", appInstance.FullName));
						//code.Getlex(appInstance);
                    	//code.Pop();

                        code.ReturnVoid();
                    },
                flexModuleFactoryInterface, "f");

            return instance;
        }

    	private static void CreateInstance(AbcCode code, AbcInstance instance, int argLocal)
		{
			code.CreateInstance(instance,
			                    () =>
			                    	{
			                    		code.GetLocal(argLocal);
			                    		return 1;
			                    	});
		}

    	private void RegisterEffectTriggers(AbcFile app, AbcCode code)
    	{
    		if (_effects.Count == 0) return;

    		var effectManager = ImportType(app, "mx.effects.EffectManager");
    		var mn = app.DefineMxInternalName("registerEffectTrigger");
    		foreach (var pair in _effects)
    		{
    			code.Getlex(effectManager);
    			code.PushString(pair.Key);
    			code.PushString(pair.Value);
    			code.CallVoid(mn, 2);
    		}
    	}

    	private void RegisterRemoteClasses(AbcFile app, AbcCode code)
    	{
    		if (_remoteClasses.Count == 0) return;

    		var registerClassAlias = app.DefinePackageQName("flash.net", "registerClassAlias");
    		foreach (var pair in _remoteClasses)
    		{
    			code.FindPropertyStrict(registerClassAlias);
    			code.PushString(pair.Key);
    			code.Getlex(pair.Value);
    			code.CallVoid(registerClassAlias, 2);
    		}
    	}

    	private static void RegisterInheritStyles(AbcFile app, AbcCode code, Action pushStyleManager, bool flex4)
    	{
    		var registerInheritingStyle = app.DefineGlobalQName("registerInheritingStyle");

    		var styles = new List<string>(CommonInheritStyles);
    		styles.AddRange(flex4 ? Flex4InheritStyles : Flex3InheritStyles);
			styles.Sort(StringComparer.Ordinal);

    		foreach (var style in styles)
    		{
    			pushStyleManager();
    			code.PushString(style);
    			code.CallVoid(registerInheritingStyle, 1);
    		}
    	}
    }
}