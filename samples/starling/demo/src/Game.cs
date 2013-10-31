using System;

namespace demo 
{
	using Avm;
	using scenes;
	using utils;
	using flash.ui;

	using starling.core;
	using starling.display;
	using starling.events;
	using starling.textures;
	using starling.utils;

    public class Game : Sprite
    {
        // Embed the Ubuntu Font. Beware: the 'embedAsCFF'-part IS REQUIRED!!!
        //[Embed("../../demo/assets/fonts/Ubuntu-R.ttf", fontFamily="Ubuntu")]
        //private static const UbuntuRegular:Class;
        
        private ProgressBar mLoadingProgress;
        private MainMenu mMainMenu;
        private Scene mCurrentScene;
        private Sprite _container;
        
        private static AssetManager sAssets;
        
        public Game()
        {
            // nothing to do here -- Startup will call "start" immediately.
        }
        
        public void start(Texture background, AssetManager assets)
        {
            sAssets = assets;
            
            // The background is passed into this method for two reasons:
            // 
            // 1) we need it right away, otherwise we have an empty frame
            // 2) the Startup class can decide on the right image, depending on the device.
            
            addChild(new Image(background));
            
            // The AssetManager contains all the raw asset data, but has not created the textures
            // yet. This takes some time (the assets might be loaded from disk or even via the
            // network), during which we display a progress indicator. 
            
            mLoadingProgress = new ProgressBar(175, 20);
            mLoadingProgress.x = (background.width  - mLoadingProgress.width) / 2;
            mLoadingProgress.y = background.height * 0.7;
            addChild(mLoadingProgress);

	        assets.loadQueue(
		        (Action<double>)(ratio =>
		        {
			        mLoadingProgress.ratio = ratio;

			        // a progress bar should always show the 100% for a while,
			        // so we show the main menu only after a short delay. 

			        if (ratio == 1)
				        Starling.__juggler.delayCall(
					        (Action)(() =>
					        {
						        mLoadingProgress.removeFromParent(true);
						        mLoadingProgress = null;
						        showMainMenu();
					        }), 0.15);
		        }));

	        addEventListener(Event.TRIGGERED, new Action<Event>(onButtonTriggered));
            stage.keyDown += onKey;
        }
        
        private void showMainMenu()
        {
            // now would be a good time for a clean-up 
            flash.system.System.pauseForGCIfCollectionImminent(0);
            flash.system.System.gc();
            
            if (mMainMenu == null)
                mMainMenu = new MainMenu();
            
            addChild(mMainMenu);
        }
        
        private void onKey(KeyboardEvent e)
        {
            if (e.keyCode == Keyboard.SPACE)
                Starling.current.showStats = !Starling.current.showStats;
            else if (e.keyCode == Keyboard.X)
                Starling.__context.dispose();
        }
        
        private void onButtonTriggered(Event e)
        {
            var button = e.target as Button;
            
            if (button.name == "backButton")
                closeScene();
            else
                showScene(button.name);
        }
        
        private void closeScene()
        {
            mCurrentScene.removeFromParent(true);
            mCurrentScene = null;
            showMainMenu();
        }
        
        private void showScene(string name)
        {
            if (mCurrentScene != null) return;
            
            var sceneClass = flash.utils.Global.getDefinitionByName(name) as Class;
            mCurrentScene = sceneClass.CreateInstance() as Scene;
            mMainMenu.removeFromParent();
            addChild(mCurrentScene);
        }
        
        public static AssetManager assets { get { return sAssets; } }
    }
}