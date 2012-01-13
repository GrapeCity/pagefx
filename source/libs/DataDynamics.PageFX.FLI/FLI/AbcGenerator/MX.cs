using System.Diagnostics;
using DataDynamics.PageFX.FLI.ABC;
using DataDynamics.PageFX.FLI.IL;

namespace DataDynamics.PageFX.FLI
{
    internal partial class AbcGenerator
    {
        const string Event_Initialize = "initialize";

        void MxApp_DefineMembers(AbcInstance instance)
        {
            if (!IsMxApplication) return;
            if (instance.Type != sfc.TypeMxApp) return;
            if (AbcGenConfig.MxAppCtorAsHandler)
                MxApp_DefineInitializer(instance);
            //MxApp_DefineInitialize(instance);
        }

        void MxApp_DefineInitializer(AbcInstance instance)
        {
            Debug.Assert(instance.Initializer == null);

            instance.Initializer = _abc.DefineInitializer(
                delegate(AbcCode code)
                    {
                        code.PushThisScope();
                        code.ConstructSuper();

                        MxCtorAfterSuperCall(code);

                        var ctor = TypeHelper.FindParameterlessConstructor(instance);
                        if (ctor != null)
                        {
                            var ctorMethod = DefineAbcMethod(ctor);
                            AddEventListener(code, ctorMethod);

                            //AbcMethod m = MxApp_ShowAssets(instance);
                            //AddEventListener(code, m);
                        }

                        code.ReturnVoid();
                    });
        }

        public void MxCtorAfterSuperCall(AbcCode code)
        {
            Debug.Assert(IsMxApplication);

            var instance = MainInstance;
            Debug.Assert(instance != null);

            code.LoadThis();
            code.LoadThis();
            code.SetMxInternalProperty("_document");

            //our style settings

            //ambient styles
            var initStyles = MxApp_DefineInitStyles(instance);
            code.Getlex(instance);
            code.Call(initStyles);

            //properties
            code.LoadThis();
            code.PushString("absolute");
            code.InitProperty("layout");
        }

        static void AddEventListener(AbcCode code, AbcMethod method)
        {
            code.LoadThis();
            code.PushString(Event_Initialize);
            code.LoadThis();
            code.GetProperty(method.TraitName);
            code.CallVoid("addEventListener", 2);
        }

        #region MxApp_ShowAssets
        //AbcMethod MxApp_ShowAssets(AbcInstance instance)
        //{
        //    var flexEvent = ImportFlexEventType();
        //    var method = instance.DefineInstanceMethod("__show_assets__", AvmTypeCode.Void, null,
        //                                                     flexEvent, "e");
        //    AddLateMethod(method, MxApp_ShowAssetsCode);
        //    return method;
        //}

        //void MxApp_ShowAssetsCode(AbcCode code)
        //{
        //    Debug.Assert(sfc != null);

        //    foreach (var instance in sfc.GetAppAssets())
        //    {
        //        if (instance != null)
        //        {
        //            code.Getlex(instance);
        //            var br = code.IfNull();

        //            Alert(code, string.Format("Asset instance {0} is linked", instance.FullName));

        //            br.BranchTarget = code.Label();
        //        }
        //        else
        //        {
        //            throw new InvalidOperationException();
        //        }
        //    }

        //    code.ReturnVoid();
        //}
        #endregion

        #region MxApp_DefineInitialize
        AbcMethod MxApp_DefineInitialize(AbcInstance instance)
        {
            var name = _abc.DefineGlobalQName("initialize");
            return instance.DefineVirtualOverrideMethod(
                name, AvmTypeCode.Void,
                delegate(AbcCode code)
                    {
                        code.LoadThis();
                        code.Add(InstructionCode.Callsupervoid, name, 0);

                        //Alert(code, "App Initialize!!!");

                        code.ReturnVoid();
                    });
        }
        #endregion

        #region MxApp_DefineInitStyles
        AbcMethod MxApp_DefineInitStyles(AbcInstance instance)
        {
            var done = instance.DefineStaticSlot("_init_styles_done", AvmTypeCode.Boolean);

            return instance.DefineStaticMethod(
                "$init_styles$", AvmTypeCode.Void,
                delegate(AbcCode code)
                {
                    code.LoadThis();
                    code.GetProperty(done);

                    var br = code.IfFalse();
                    code.ReturnVoid();

                    br.BranchTarget = code.Label();

                    code.LoadThis();
                    code.Add(InstructionCode.Pushtrue);
                    code.SetProperty(done);

                    //TODO: init app CSSStyleDeclaration
                    //TODO: init app effects

                    var styleMgr = ImportType("mx.styles.StyleManager");
                    code.Getlex(styleMgr);
                    code.CallVoid(_abc.DefineMxInternalName("initProtoChainRoots"), 0);

                    code.ReturnVoid();
                });
        }
        #endregion

        #region Utils
        void Alert(AbcCode code, string value)
        {
            var alert = ImportAlertControl();
            code.Getlex(alert);
            code.PushString(value);
            code.CallVoid("show", 1);
        }

        AbcInstance ImportFlexEventType()
        {
            if (_typeFlexEvent == null)
                _typeFlexEvent = ImportType("mx.events.FlexEvent");
            return _typeFlexEvent;
        }
        AbcInstance _typeFlexEvent;

        AbcInstance ImportAlertControl()
        {
            if (_typeAlert == null)
                _typeAlert = ImportType("mx.controls.Alert");
            return _typeAlert;
        }
        AbcInstance _typeAlert;
        #endregion
    }
}