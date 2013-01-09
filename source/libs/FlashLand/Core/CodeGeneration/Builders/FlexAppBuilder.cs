using System.Diagnostics;
using DataDynamics.PageFX.FlashLand.Abc;
using DataDynamics.PageFX.FlashLand.IL;

namespace DataDynamics.PageFX.FlashLand.Core.CodeGeneration.Builders
{
    internal sealed class FlexAppBuilder
    {
	    private readonly AbcGenerator _generator;

	    public FlexAppBuilder(AbcGenerator generator)
		{
			_generator = generator;
		}

	    private AbcFile Abc
	    {
			get { return _generator.Abc; }
	    }

	    private static class FlexAppEvents
		{
			public const string Initialize = "initialize";
		}

    	public void DefineMembers(AbcInstance instance)
        {
            if (!_generator.IsFlexApplication) return;
			if (!ReferenceEquals(instance.Type, _generator.SwfCompiler.TypeFlexApp)) return;

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

        	instance.Initializer = Abc.DefineInitializer(
        		code =>
        			{
        				code.PushThisScope();
        				code.ConstructSuper();

						code.Trace("PFC: calling App.initializer");

        				CtorAfterSuperCall(code);

						if (!IsFlex4)
						{
							var ctor = instance.FindParameterlessConstructor();
							if (ctor != null)
							{
								var ctorMethod = _generator.DefineAbcMethod(ctor);
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
        public void CtorAfterSuperCall(AbcCode code)
        {
			Debug.Assert(_generator.IsFlexApplication);

			var instance = _generator.MainInstance;
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

        private void OverrideFlexAppInitialize(AbcInstance instance)
        {
			var name = Abc.DefineGlobalQName("initialize");
    		instance.DefineMethod(
    			Sig.@virtual(name, AvmTypeCode.Void).@override(),
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
			var flexModuleFactoryInterface = _generator.ImportType(MX.IFlexModuleFactory);

			var propertyName = Abc.DefineGlobalQName("moduleFactory");
    		instance.DefineMethod(
    			Sig.set(propertyName, flexModuleFactoryInterface).@virtual().@override(),
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
    					var ctor = instance.FindParameterlessConstructor();
    					if (ctor != null)
    					{
							var ctorMethod = _generator.DefineAbcMethod(ctor);
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

	    private AbcMethod DefineInitFlexAppStyles(AbcInstance instance)
        {
            var done = instance.DefineSlot("_init_styles_done", AvmTypeCode.Boolean);

			return instance.DefineMethod(
				Sig.@this("$init_flex_styles$", AvmTypeCode.Void),
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
							var styleMgr = _generator.ImportType("mx.styles.StyleManager");
							code.Getlex(styleMgr);
							code.CallVoid(Abc.DefineMxInternalName("initProtoChainRoots"), 0);
						}

						code.ReturnVoid();
					});
        }

	    public AbcInstance FlexEventType()
        {
			return _generator.ImportType(MX.FlexEvent, ref _typeFlexEvent);
        }
        private AbcInstance _typeFlexEvent;

	    private bool IsFlex4
    	{
    		get
    		{
    			if (!_isFlex4.HasValue)
					_isFlex4 = _generator.ImportType(MX.IStyleManager2, true) != null;
    			return _isFlex4.Value;
    		}
    	}
    	private bool? _isFlex4;
    }
}