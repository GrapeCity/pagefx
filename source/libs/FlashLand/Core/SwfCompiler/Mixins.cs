using System;
using System.Collections.Generic;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.FlashLand.Abc;

namespace DataDynamics.PageFX.FlashLand.Core.SwfCompiler
{
    internal partial class SwfCompilerImpl
    {
    	private const string NsMxSystemClasses = "mx.managers.systemClasses";
    	private const string NsMxMarshalClasses = "mx.managers.marshalClasses";

    	private static readonly Dictionary<string, bool> ExcludedMixinClasses =
    		new Dictionary<string, bool>
    			{
					{NsMxSystemClasses + ".MarshallingSupport", true},
					{NsMxMarshalClasses + ".CursorManagerMarshalMixin", true},
					{NsMxMarshalClasses + ".DragManagerMarshalMixin", true},
					{NsMxMarshalClasses + ".FocusManagerMarshalMixin", true},
					{NsMxMarshalClasses + ".PopUpManagerMarshalMixin", true},
					{NsMxMarshalClasses + ".ToolTipManagerMarshalMixin", true},
    			};

        private readonly List<string> _mixinNames = new List<string>();
        private readonly Dictionary<string, bool> _mixinCache = new Dictionary<string, bool>();
        private readonly List<string> _mixinClasses = new List<string>();
        private bool _mixinsImported;

        public void ImportMixins()
        {
            if (!IsFlexApplication) return;

            if (_mixinsImported) return;
            _mixinsImported = true;

            var app = FrameApp;
            if (app == null)
                throw new InvalidOperationException("invalid context");

            ImportAppMixins();
            ImportStyleMixins(app);

			// flush app mixins to mixins names which will be defined in SystemManager.info.
            AddAppMixins();

            //FlexInit mixin. Should be the first in list.
            var flexInit = DefineFlexInitMixin(app);
            string name = flexInit.FullName;
            _mixinNames.Insert(0, name);
            _mixinCache.Add(name, true);

        }

        private void ImportAppMixins()
        {
            _assembly.ProcessReferences(true, ImportMixins);
        }

        private void ImportMixins(IAssembly ar)
        {
            var swc = AssemblyTag.Instance(ar).SWC;
            if (swc == null) return;

            foreach (var mixin in swc.AbcCache.Mixins)
                FrameApp.ImportInstance(mixin);
        }

        public void RegisterMixin(AbcInstance instance)
        {
            if (instance == null) throw new ArgumentNullException("instance");
            _mixinClasses.Add(instance.FullName);
        }
        
        private void AddMixin(string name)
        {
            if (_mixinCache.ContainsKey(name)) return;
            _mixinNames.Add(name);
            _mixinCache.Add(name, true);
        }
                
        private void AddAppMixins()
        {
			foreach (var name in _mixinClasses)
			{
				if (ExcludedMixinClasses.ContainsKey(name))
				{
					continue;
				}
				AddMixin(name);
			}
        }
    }
}