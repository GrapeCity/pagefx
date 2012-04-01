using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DataDynamics.PageFX.FLI.ABC;
using DataDynamics.PageFX.FLI.SWC;

namespace DataDynamics.PageFX.FLI
{
	internal partial class SwfCompiler
	{
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
			var swcResource = GetType().GetResourceStream("mixins.swc");
			if (swcResource == null)
				throw new InvalidOperationException("Unable to load mixins");

			var depsResource = GetType().GetResourceStream("mixins.dep");
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
			return swc.GetAbcFiles().SelectMany(abc => GetStyleMixins(abc, strict));
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