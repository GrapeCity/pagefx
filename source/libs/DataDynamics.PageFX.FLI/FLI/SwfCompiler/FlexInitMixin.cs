using System;
using System.Collections.Generic;
using System.Diagnostics;
using DataDynamics.PageFX.FLI.ABC;
using DataDynamics.PageFX.FLI.IL;

namespace DataDynamics.PageFX.FLI
{
    internal partial class SwfCompiler
    {
        private readonly List<KeyValuePair<string, AbcMultiname>> _remoteClasses = new List<KeyValuePair<string, AbcMultiname>>();

        public void RegisterRemoteClass(string alias, AbcMultiname name)
        {
            if (name == null)
                throw new ArgumentNullException("name");
            _remoteClasses.Add(new KeyValuePair<string, AbcMultiname>(alias, name));
        }

        public void RegisterRemoteClass(string alias, AbcInstance instance)
        {
            RegisterRemoteClass(alias, instance.Name);
        }

        private readonly List<KeyValuePair<string, string>> _effects = new List<KeyValuePair<string, string>>();

        public void RegisterEffectTrigger(string name, string eventName)
        {
            _effects.Add(new KeyValuePair<string, string>(name, eventName));
        }

        //TODO: Get inheriting styles handling Style tag
        private static readonly string[] InheritStyleNames =
            {
                "fontWeight", "modalTransparencyBlur", "rollOverColor",
                "textRollOverColor", "verticalGridLineColor",
                "backgroundDisabledColor", "textIndent", "barColor",
                "fontSize", "kerning", "footerColors", "textAlign",
                "disabledIconColor", "fontStyle",
                "modalTransparencyDuration", "textSelectedColor",
                "horizontalGridLineColor", "selectionColor",
                "modalTransparency", "fontGridFitType",
                "selectionDisabledColor", "disabledColor",
                "fontAntiAliasType", "modalTransparencyColor",
                "alternatingItemColors", "leading", "iconColor",
                "dropShadowColor", "themeColor", "letterSpacing",
                "fontFamily", "color", "fontThickness", "errorColor",
                "headerColors", "fontSharpness", "textDecoration"
            };

        private AbcInstance DefineFlexInitMixin(AbcFile app)
        {
            Debug.Assert(IsMxApplication);

            var IFlexModuleFactory = ImportType(app, "mx.core.IFlexModuleFactory");
            var StyleManager = ImportType(app, "mx.styles.StyleManager");

            var instance = new AbcInstance(true);
            string name = "_FlexInit_" + MxAppPrefix;
            string ns = RootNamespace;
            instance.Name = app.DefineQName(ns, name);
            instance.SuperName = app.BuiltinTypes.Object;
            instance.IsMixin = true;
            instance.IsFlexInitMixin = true;

            instance.Initializer = app.DefineEmptyConstructor();
            instance.Class.Initializer = app.DefineEmptyMethod();

            app.AddInstance(instance);

            instance.DefineStaticMethod(
                "init", AvmTypeCode.Void,
                code =>
                    {
                        code.PushThisScope();

                        //effect triggers
                        if (_effects.Count > 0)
                        {
                            var EffectManager = ImportType(app, "mx.effects.EffectManager");
                            var mn = app.DefineMxInternalName("registerEffectTrigger");
                            foreach (var pair in _effects)
                            {
                                code.Getlex(EffectManager);
                                code.PushString(pair.Key);
                                code.PushString(pair.Value);
                                code.CallVoid(mn, 2);
                            }
                        }

                        //remote classes
                        if (_remoteClasses.Count > 0)
                        {
                            var registerClassAlias = app.DefinePackageQName("flash.net", "registerClassAlias");
                            foreach (var pair in _remoteClasses)
                            {
                                code.FindPropertyStrict(registerClassAlias);
                                code.PushString(pair.Key);
                                code.Getlex(pair.Value);
                                code.CallVoid(registerClassAlias, 2);
                            }
                        }

                        //register inheriting styles
                        var registerInheritingStyle = app.DefineGlobalQName("registerInheritingStyle");
                        foreach (var styleName in InheritStyleNames)
                        {
                            code.Getlex(StyleManager);
                            code.PushString(styleName);
                            code.CallVoid(registerInheritingStyle, 1);
                        }

                        //NOTE: Uncomment to add forward refernce to Application
                        //code.Getlex(app.generator.MainInstance);
                        //code.Pop();

                        code.ReturnVoid();
                    },
                IFlexModuleFactory, "f");

            return instance;
        }
    }
}