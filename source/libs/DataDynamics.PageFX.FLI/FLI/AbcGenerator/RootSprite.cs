using DataDynamics.PageFX.FLI.ABC;
using DataDynamics.PageFX.FLI.IL;

namespace DataDynamics.PageFX.FLI
{
    partial class AbcGenerator
    {
        void BuildRootTimeline()
        {
            DefineRootSprite();
            BuildMainScript();
        }

        public AbcInstance RootSprite
        {
            get { return _rootSprite; }
        }
        AbcInstance _rootSprite;
        bool _rootSpriteGenerated;

        void DefineRootSprite()
        {
            if (!IsSwf) return;
            if (IsSwc) return;
            //NOTE: In Flex Root sprite is MX SystemManager.
            if (IsMxApplication) return;
            if (_rootSprite != null) return;

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
                delegate(AbcCode code)
                {
                    code.PushThisScope();
                    code.ConstructSuper();
                    NUnitMain(code);
                    CallEntryPoint(code);
                });
        }
    }
}