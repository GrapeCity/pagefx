using System;
using System.Collections.Generic;
using System.IO;
using DataDynamics.PageFX.FLI.ABC;
using DataDynamics.PageFX.FLI.SWC;

namespace DataDynamics.PageFX.FLI
{
	internal partial class SwfCompiler
	{
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

		//private HashedList<string, AbcInstance> _styleClients;

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

		private void ImportStyleMixins(AbcFile app)
		{			
#if PERF
            int start = Environment.TickCount;
            Console.WriteLine("ImportStyleMixins");
#endif
			string styleMixins = Options.StyleMixins;
			if (!string.IsNullOrEmpty(styleMixins)
				&& File.Exists(styleMixins))
			{
				var swc = new SwcFile(styleMixins);
				swc.ResolveDependencies(new SimpleSwcLinker(_assembly), null);
				ImportStyleMixins(app, swc, true);
				return;
			}

			ImportEmbeddedStyleMixins(app);

#if PERF
            Console.WriteLine("ImportStyleMixins: {0}", Environment.TickCount - start);
#endif
		}

		private void ImportEmbeddedStyleMixins(AbcFile app)
		{
			var swcResource = ResourceHelper.GetStream(GetType(), "mixins.swc");
			if (swcResource == null)
				throw new InvalidOperationException("Unable to load mixins");

			var depsResource = ResourceHelper.GetStream(GetType(), "mixins.dep");
			var deps = new SwcDepFile(depsResource);

			var swc = new SwcFile(swcResource);

			swc.ResolveDependencies(new SimpleSwcLinker(_assembly), deps);

			ImportStyleMixins(app, swc, false);
		}

		private void ImportStyleMixins(AbcFile app, SwcFile swc, bool strict)
		{
			var mixins = new List<AbcInstance>(GetStyleMixins(swc, strict));
			foreach (var mixin in mixins)
			{
				string name = mixin.FullName;
				AddMixin(name);
				app.Import(mixin.ABC);
			}
		}

		private static IEnumerable<AbcInstance> GetStyleMixins(SwcFile swc, bool strict)
		{
			foreach (var abc in swc.GetAbcFiles())
			{
				foreach (var instance in GetStyleMixins(abc, strict))
				{
					yield return instance;
				}
			}
		}

		private static IEnumerable<AbcInstance> GetStyleMixins(AbcFile abc, bool strict)
		{
			foreach (var script in abc.Scripts)
			{
				foreach (var trait in script.Traits)
				{
					var klass = trait.Class;
					if (klass == null) continue;

					var instance = klass.Instance;
					if (!IsStyleMixin(instance, strict)) continue;

					instance.IsMixin = true;
					instance.IsStyleMixin = true;

					yield return instance;
				}
			}
		}

		private static bool IsStyleMixin(AbcInstance instance, bool strict)
		{
			if (instance.Traits.Count > 0) return false;
			var klass = instance.Class;
			if (klass.Traits.Count == 0) return false;
			var trait = klass.Traits.FindMethod("init");
			if (trait == null) return false;
			if (trait.Method.ParamCount != 1) return false;
			if (!strict) return true;
			var type = trait.Method.Parameters[0].Type;
			if (type == null) return false;
			return type.FullName == MX.IFlexModuleFactory;
		}
	}
}