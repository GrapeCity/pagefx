using System.Diagnostics;
using DataDynamics.PageFX.FLI.ABC;
using DataDynamics.PageFX.FLI.IL;

namespace DataDynamics.PageFX.FLI
{
    internal partial class AbcGenerator
    {
		private static class FlexAppEvents
		{
			public const string Initialize = "initialize";
		}

    	private void DefineFlexAppMembers(AbcInstance instance)
        {
            if (!IsMxApplication) return;
            if (instance.Type != sfc.TypeFlexApp) return;

            if (AbcGenConfig.FlexAppCtorAsHandler)
                DefineFlexAppInitializer(instance);

    		OverrideFlexAppInitialize(instance);

			if (IsFlex4)
			{
				OverrideFlexAppModuleFactorySetter(instance);
			}
        }

        private void DefineFlexAppInitializer(AbcInstance instance)
        {
            Debug.Assert(instance.Initializer == null);

        	instance.Initializer = _abc.DefineInitializer(
        		code =>
        			{
        				code.PushThisScope();
        				code.ConstructSuper();

						code.Trace("PFC: calling App.initializer");

        				FlexAppCtorAfterSuperCall(code);

						if (!IsFlex4)
						{
							var ctor = TypeExtensions.FindParameterlessConstructor(instance);
							if (ctor != null)
							{
								var ctorMethod = DefineAbcMethod(ctor);
								AddEventListener(code, ctorMethod, FlexAppEvents.Initialize);
							}
						}

        				code.ReturnVoid();
        			});
        }

		/// <summary>
		/// Adds some initialization after app.ctor super.ctor call.
		/// </summary>
		/// <param name="code"></param>
        public void FlexAppCtorAfterSuperCall(AbcCode code)
        {
            Debug.Assert(IsMxApplication);

            var instance = MainInstance;
            Debug.Assert(instance != null);

			code.Trace("PFC: init flex _document property");

            code.LoadThis();
            code.LoadThis();
            code.SetMxInternalProperty("_document");

			//init styles
			if (!IsFlex4)
			{
				CallInitStyles(code, instance);
			}

        	//properties
            code.LoadThis();
            code.PushString("absolute");
            code.InitProperty("layout");
        }

    	private void CallInitStyles(AbcCode code, AbcInstance instance)
    	{
			code.Trace("PFC: calling App.initStyles");
    		var initStyles = DefineInitFlexAppStyles(instance);
    		code.LoadThis();
    		code.Call(initStyles);
    	}

    	private static void AddEventListener(AbcCode code, AbcMethod method, string eventName)
        {
            code.LoadThis();
            code.PushString(eventName);
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

    	private void OverrideFlexAppInitialize(AbcInstance instance)
        {
            var name = _abc.DefineGlobalQName("initialize");
    		instance.DefineVirtualOverrideMethod(
    			name, AvmTypeCode.Void,
    			code =>
    				{
    					code.LoadThis();
    					code.Add(InstructionCode.Callsupervoid, name, 0);

    					code.Trace("PFC: calling App.initialize");

    					//Alert(code, "App Initialize!!!");

    					code.ReturnVoid();
    				});
        }

    	private void OverrideFlexAppModuleFactorySetter(AbcInstance instance)
		{
			Debug.Assert(IsFlex4);

			var moduleFactoryInitialized = instance.DefineSlot("__moduleFactoryInitialized", AvmTypeCode.Boolean);
    		var flexModuleFactoryInterface = ImportType(MX.IFlexModuleFactory);

			var propertyName = _abc.DefineGlobalQName("moduleFactory");
    		instance.DefineSetter(
				propertyName, flexModuleFactoryInterface,
				AbcMethodSemantics.Virtual | AbcMethodSemantics.Override, 
    			code =>
    				{
						code.Trace("PFC: setting FlexModuleFactory to application");

    					// super.moduleFactory = value
    					code.LoadThis();
    					code.GetLocal(1);
    					code.SetSuper(propertyName);

    					code.LoadThis();
    					code.GetProperty(moduleFactoryInitialized);
    					var br = code.IfFalse();
    					code.ReturnVoid();

    					br.BranchTarget = code.Label();

    					code.LoadThis();
    					code.Add(InstructionCode.Pushtrue);
    					code.SetProperty(moduleFactoryInitialized);

    					CallInitStyles(code, instance);

						// init application after styles are initialized
						var ctor = TypeExtensions.FindParameterlessConstructor(instance);
						if (ctor != null)
						{
							var ctorMethod = DefineAbcMethod(ctor);
							code.LoadThis();
							if (AbcGenConfig.FlexAppCtorAsHandler)
							{
								code.PushNull();
							}
							code.Call(ctorMethod);
						}

    					code.ReturnVoid();
    				});
		}

		#region DefineInitFlexAppStyles
		private AbcMethod DefineInitFlexAppStyles(AbcInstance instance)
        {
            var done = instance.DefineSlot("_init_styles_done", AvmTypeCode.Boolean);

			return instance.DefineInstanceMethod(
				"$init_flex_styles$", AvmTypeCode.Void,
				code =>
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

						if (IsFlex4)
						{
							code.LoadThis();
							code.GetProperty("styleManager");
							code.CallVoid("initProtoChainRoots", 0);
						}
						else
						{
							var styleMgr = ImportType("mx.styles.StyleManager");
							code.Getlex(styleMgr);
							code.CallVoid(_abc.DefineMxInternalName("initProtoChainRoots"), 0);
						}

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

		private AbcInstance ImportFlexEventType()
        {
            return ImportType(MX.FlexEvent, ref _typeFlexEvent);
        }
        private AbcInstance _typeFlexEvent;

        private AbcInstance ImportAlertControl()
        {
			return ImportType("mx.controls.Alert", ref _typeAlert);
        }
        private AbcInstance _typeAlert;

    	private bool IsFlex4
    	{
    		get
    		{
    			if (!_isFlex4.HasValue)
    				_isFlex4 = ImportType(MX.IStyleManager2, true) != null;
    			return _isFlex4.Value;
    		}
    	}
    	private bool? _isFlex4;
        #endregion
    }
}