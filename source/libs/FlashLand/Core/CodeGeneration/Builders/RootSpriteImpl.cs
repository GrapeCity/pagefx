using DataDynamics.PageFX.FlashLand.Abc;

namespace DataDynamics.PageFX.FlashLand.Core.CodeGeneration.Builders
{
	internal sealed class RootSpriteImpl
	{
		private readonly AbcGenerator _generator;

		public RootSpriteImpl(AbcGenerator generator)
		{
			_generator = generator;
		}

		public void BuildTimeline()
		{
			DefineRootSprite();
			_generator.Scripts.BuildMainScript();
		}

		public bool IsGenerated { get; private set; }

		public AbcInstance Instance { get; set; }

		private AbcFile Abc
		{
			get { return _generator.Abc; }
		}

		private void DefineRootSprite()
		{
			if (!_generator.IsSwf) return;
			if (_generator.IsSwc) return;

			//NOTE: In Flex Root sprite is SystemManager.
			if (_generator.IsFlexApplication) return;
			if (Instance != null) return;

			var rootStageField = DefineRootStageHolder();

			IsGenerated = true;
			const string rootName = "$ROOTSPRITE$";
			Instance = new AbcInstance(true)
			              	{
			              		Name = Abc.DefineName(QName.Package(_generator.RootNamespace, rootName)),
			              		Flags = (AbcClassFlags.Sealed | AbcClassFlags.ProtectedNamespace),
			              		ProtectedNamespace = Abc.DefineProtectedNamespace(rootName)
			              	};

			const string superTypeName = "flash.display.Sprite";
			var superType = _generator.FindInstanceDefOrRef(superTypeName);
			if (superType == null)
				throw Errors.Type.UnableToFind.CreateException(superTypeName);

			Instance.BaseTypeName = Abc.ImportConst(superType.Name);
			Instance.BaseInstance = superType;

			Instance.Class.Initializer = Abc.DefineEmptyMethod();

			Abc.AddInstance(Instance);

			Instance.Initializer = Abc.DefineInitializer(
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

						_generator.NUnit.Main(code);

						_generator.Scripts.CallEntryPoint(code);
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