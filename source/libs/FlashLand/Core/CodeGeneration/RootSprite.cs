using DataDynamics.PageFX.FlashLand.Abc;

namespace DataDynamics.PageFX.FlashLand.Core.CodeGeneration
{
	internal partial class AbcGenerator
	{
		private void BuildRootTimeline()
		{
			DefineRootSprite();
			BuildMainScript();
		}

		public AbcInstance RootSprite
		{
			get { return _rootSprite; }
		}

		private AbcInstance _rootSprite;
		private bool _rootSpriteGenerated;

		private void DefineRootSprite()
		{
			if (!IsSwf) return;
			if (IsSwc) return;
			//NOTE: In Flex Root sprite is MX SystemManager.
			if (IsFlexApplication) return;
			if (_rootSprite != null) return;

			var rootStageField = DefineRootStageHolder();

			_rootSpriteGenerated = true;
			const string rootName = "$ROOTSPRITE$";
			_rootSprite = new AbcInstance(true)
			              	{
			              		Name = Abc.DefineQName(RootAbcNamespace, rootName),
			              		Flags = (AbcClassFlags.Sealed | AbcClassFlags.ProtectedNamespace),
			              		ProtectedNamespace = Abc.DefineProtectedNamespace(rootName)
			              	};

			const string superTypeName = "flash.display.Sprite";
			var superType = FindInstanceDefOrRef(superTypeName);
			if (superType == null)
				throw Errors.Type.UnableToFind.CreateException(superTypeName);

			_rootSprite.SuperName = Abc.ImportConst(superType.Name);
			_rootSprite.SuperType = superType;

			_rootSprite.Class.Initializer = Abc.DefineEmptyMethod();

			Abc.AddInstance(_rootSprite);

			_rootSprite.Initializer = Abc.DefineInitializer(
				code =>
					{
						code.PushThisScope();
						code.ConstructSuper();

						if (rootStageField != null)
						{
							code.SetStaticProperty(rootStageField,
							                       () =>
							                       	{
							                       		code.LoadThis();
							                       		code.GetProperty("stage");
							                       	});
						}

						NUnitMain(code);

						CallEntryPoint(code);
					});
		}

		private AbcTrait DefineRootStageHolder()
		{
			// TODO: Stage holder should be generated and exposed via some internal call like flash.getStage();
//			var instance = ImportType("System.RootStage", true);
//			if (instance == null) return null;
//			var type = instance.Type;
//			var field = type.Fields["Value"];
//			DefineField(field);
//			return field.Data as AbcTrait;
			return null;
		}
	}
}