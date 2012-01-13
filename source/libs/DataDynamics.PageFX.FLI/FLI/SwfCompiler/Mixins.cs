using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using DataDynamics.PageFX.CodeModel;
using DataDynamics.PageFX.FLI.ABC;
using DataDynamics.PageFX.FLI.SWC;

namespace DataDynamics.PageFX.FLI
{
    partial class SwfCompiler
    {
        readonly List<string> _mixinNames = new List<string>();
        readonly Hashtable _mixinCache = new Hashtable();
        readonly List<string> _mixinClasses = new List<string>();

        bool _mixinsImported;

        #region StyleMixinNames
        string[] StyleMixinNames =
            {
                "_alertButtonStyleStyle",
                "_HorizontalListStyle",
                "_ScrollBarStyle",
                "_MenuStyle",
                "_TabStyle",
                "_ToolTipStyle",
                "_HDividedBoxStyle",
                "_ComboBoxStyle",
                "_comboDropdownStyle",
                "_CheckBoxStyle",
                "_ListBaseStyle",
                "_DateChooserStyle",
                "_ProgressBarStyle",
                "_VDividedBoxStyle",
                "_globalStyle",
                "_PanelStyle",
                "_MenuBarStyle",
                "_windowStylesStyle",
                "_activeButtonStyleStyle",
                "_DividedBoxStyle",
                "_HSliderStyle",
                "_LinkBarStyle",
                "_ApplicationControlBarStyle",
                "_errorTipStyle",
                "_CursorManagerStyle",
                "_dateFieldPopupStyle",
                "_ButtonBarButtonStyle",
                "_FormHeadingStyle",
                "_HRuleStyle",
                "_dataGridStylesStyle",
                "_LinkButtonStyle",
                "_TitleWindowStyle",
                "_PopUpButtonStyle",
                "_RadioButtonStyle",
                "_VRuleStyle",
                "_DataGridItemRendererStyle",
                "_ColorPickerStyle",
                "_ControlBarStyle",
                "_activeTabStyleStyle",
                "_TabBarStyle",
                "_textAreaHScrollBarStyleStyle",
                "_PopUpMenuButtonStyle",
                "_TreeStyle",
                "_VideoDisplayStyle",
                "_DragManagerStyle",
                "_TextAreaStyle",
                "_DateFieldStyle",
                "_RichTextEditorStyle",
                "_textAreaVScrollBarStyleStyle",
                "_ContainerStyle",
                "_linkButtonStyleStyle",
                "_windowStatusStyle",
                "_AccordionHeaderStyle",
                "_NumericStepperStyle",
                "_richTextEditorTextAreaStyleStyle",
                "_FormItemStyle",
                "_todayStyleStyle",
                "_TextInputStyle",
                "_ButtonBarStyle",
                "_TabNavigatorStyle",
                "_SwatchPanelStyle",
                "_plainStyle",
                "_AccordionStyle",
                "_FormStyle",
                "_ApplicationStyle",
                "_SWFLoaderStyle",
                "_FormItemLabelStyle",
                "_headerDateTextStyle",
                "_ButtonStyle",
                "_DataGridStyle",
                "_CalendarLayoutStyle",
                "_popUpMenuStyle",
                "_VSliderStyle",
                "_swatchPanelTextFieldStyle",
                "_opaquePanelStyle",
                "_weekDayStyleStyle",
                "_headerDragProxyStyleStyle",
                "_TileListStyle",
            };
        #endregion

        public void ImportMixins()
        {
            if (!IsMxApplication) return;
            if (_mixinsImported) return;
            _mixinsImported = true;

            var app = FrameApp;
            if (app == null)
                throw new InvalidOperationException("invalid context");

            ImportAppMixins();
            ImportStyleMixins(app);

            AddAppMixins();

            //FlexInit mixin. Should be the first in list.
            var flexInit = DefineFlexInitMixin(app);
            string name = flexInit.FullName;
            _mixinNames.Insert(0, name);
            _mixinCache[name] = name;
        }

        void ImportAppMixins()
        {
            AssemblyHelper.ProcessReferences(_assembly, true, ImportMixins);
        }

        void ImportMixins(IAssembly ar)
        {
            var swc = AssemblyTag.Instance(ar).SWC;
            if (swc == null) return;
            foreach (var mixin in swc.AbcCache.Mixins)
                FrameApp.ImportInstance(mixin);
        }

        public void RegisterMixin(AbcInstance instance)
        {
            if (instance == null)
                throw new ArgumentNullException("instance");
            _mixinClasses.Add(instance.FullName);
        }
        
        void AddMixin(string name)
        {
            if (_mixinCache.Contains(name)) return;
            _mixinNames.Add(name);
            _mixinCache[name] = name;
        }
                
        void AddAppMixins()
        {
            foreach (var name in _mixinClasses)
                AddMixin(name);

            //foreach (AbcFile abc in AbcFrames)
            //{
            //    foreach (AbcInstance instance in abc.Instances)
            //    {
            //        if (instance.IsMixin)
            //            RegisterMixin(instance.FullName);
            //    }
            //}
        }

        public void AddStyleClient(AbcInstance instance)
        {
            //if (_styleClients == null)
            //    _styleClients = new HashedList<string, AbcInstance>(
            //        delegate(AbcInstance i)
            //            {
            //                return i.FullName;
            //            });
            //if (!_styleClients.Contains(instance))
            //    _styleClients.Add(instance);
        }

        //HashedList<string, AbcInstance> _styleClients;

        void ImportStyleMixins(AbcFile app)
        {
            var rs = ResourceHelper.GetStream(GetType(), "mixins.swc");
            if (rs == null)
                throw new InvalidOperationException("Unable to load mixins");

#if PERF
            int start = Environment.TickCount;
            Console.WriteLine("ImportStyleMixins");
#endif

            var swc = new SwcFile(rs);

            rs = ResourceHelper.GetStream(GetType(), "mixins.dep");
            var deps = new SwcDepFile(rs);

            swc.ResolveDependencies(new SimpleSwcLinker(_assembly), deps);

            var mixins = GetStyleMixins(swc);

            //Hashtable mixins = new Hashtable();

            foreach (var mixin in mixins)
            {
                string name = mixin.FullName;
                //mixins[name] = instance;
                AddMixin(name);
                app.Import(mixin.ABC);
            }

#if PERF
            Console.WriteLine("ImportStyleMixins: {0}", Environment.TickCount - start);
#endif

            //ImportStyleMixin(app, mixins, "global");

            //for (int i = 0; i < _styleClients.Count; ++i)
            //{
            //    AbcInstance styleClient = _styleClients[i];
            //    ImportStyleMixin(app, mixins, styleClient.NameString);
            //}
        }

        void ImportStyleMixin(AbcFile app, IDictionary mixins, string name)
        {
            string key = "_" + name + "Style";
            var instance = mixins[key] as AbcInstance;
            if (instance == null) return;
            AddMixin(name);
            app.Import(instance.ABC);
        }

        static List<AbcInstance> GetStyleMixins(SwcFile swc)
        {
            var list = new List<AbcInstance>();
            foreach (var abc in swc.GetAbcFiles())
            {
                var instance = GetMixinInstance(abc);
                if (instance != null)
                {
                    instance.IsMixin = true;
                    instance.IsStyleMixin = true;
                    list.Add(instance);
                }
            }
            return list;
        }

        static AbcInstance GetMixinInstance(AbcFile abc)
        {
            foreach (var script in abc.Scripts)
            {
                foreach (var trait in script.Traits)
                {
                    var klass = trait.Class;
                    if (klass != null)
                    {
                        var instance = klass.Instance;
                        if (IsMixin(instance))
                            return instance;
                    }
                }
            }
            return null;
        }

        static bool IsMixin(AbcInstance instance)
        {
            string name = instance.FullName;
            if (name[0] == '_')
            {
                if (instance.Traits.Count > 0)
                    return false;
                var klass = instance.Class;
                if (klass.Traits.Count == 0)
                    return false;
                var t = klass.Traits.FindMethod("init");
                if (t == null)
                    return false;
                return true;
            }
            return false;
        }
    }
}