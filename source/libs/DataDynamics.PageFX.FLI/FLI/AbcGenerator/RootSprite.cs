using DataDynamics.PageFX.FlashLand.Abc;

namespace DataDynamics.PageFX.FLI
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
			if (IsMxApplication) return;
			if (_rootSprite != null) return;

			var rootStageField = DefineRootStageHolder();

			_rootSpriteGenerated = true;
			const string rootName = "$ROOTSPRITE$";
			_rootSprite = new AbcInstance(true)
			              	{
			              		Name = _abc.DefineQName(RootAbcNamespace, rootName),
			              		Flags = (AbcClassFlags.Sealed | AbcClassFlags.ProtectedNamespace),
			              		ProtectedNamespace = _abc.DefineProtectedNamespace(rootName)
			              	};

			const string superTypeName = "flash.display.Sprite";
			var superType = FindInstanceDefOrRef(superTypeName);
			if (superType == null)
				throw Errors.Type.UnableToFind.CreateException(superTypeName);

			_rootSprite.SuperName = _abc.ImportConst(superType.Name);
			_rootSprite.SuperType = superType;

			_rootSprite.Class.Initializer = _abc.DefineEmptyMethod();

			_abc.AddInstance(_rootSprite);

			_rootSprite.Initializer = _abc.DefineInitializer(
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
			var instance = ImportType("System.RootStage", true);
			if (instance == null) return null;
			var type = instance.Type;
			var field = type.Fields["Value"];
			DefineField(field);
			return field.Tag as AbcTrait;
		}
	}
}