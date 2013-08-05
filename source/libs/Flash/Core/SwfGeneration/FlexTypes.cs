using DataDynamics.PageFX.Flash.Abc;

namespace DataDynamics.PageFX.Flash.Core.SwfGeneration
{
	internal sealed class FlexTypes
	{
		private readonly SwfCompiler _compiler;
		private AbcInstance _flexModuleFactoryInterface;
		private AbcInstance _childManagerInstance;
		private AbcInstance _styleManagerInstance;
		private AbcInstance _styleManager2Interface;
		private AbcInstance _styleManagerImpl;

		public FlexTypes(SwfCompiler compiler)
		{
			_compiler = compiler;
		}

		public AbcInstance GetFlexModuleFactoryInterface(AbcFile app)
		{
			return _flexModuleFactoryInterface ?? (_flexModuleFactoryInterface = _compiler.ImportType(app, MX.IFlexModuleFactory));
		}

		public AbcInstance GetChildManagerInstance(AbcFile app)
		{
			return _childManagerInstance ?? (_childManagerInstance = _compiler.ImportType(app, MX.ChildManager, true));
		}

		public AbcInstance GetStyleManagerInstance(AbcFile app)
		{
			return _styleManagerInstance ?? (_styleManagerInstance = _compiler.ImportType(app, MX.StyleManager));
		}

		public AbcInstance GetStyleManager2Interface(AbcFile app)
		{
			return _styleManager2Interface ?? (_styleManager2Interface = _compiler.ImportType(app, MX.IStyleManager2));
		}

		public AbcInstance GetStyleManagerImpl(AbcFile app)
		{
			return _styleManagerImpl ?? (_styleManagerImpl = _compiler.ImportType(app, MX.StyleManagerImpl));
		}
	}
}
