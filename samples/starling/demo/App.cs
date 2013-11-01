using System;

namespace demo 
{
	using Avm;
    using flash.display;
    using flash.system;
    
    using starling.core;
    using starling.events;
    using starling.textures;
    using starling.utils;
    
    // If you set this class as your 'default application', it will run without a preloader.
    // To use a preloader, see 'Demo_Web_Preloader.as'.
    
	[Root]
    [Swf(Width=320, Height=480, FrameRate=60, BackgroundColor="#222222")]
    public class App : Sprite
    {
        [Embed("startup.jpg")]
        private static Class Background;
        
        private Starling mStarling;
        
        public App()
        {
	        if (stage != null)
	        {
		        start();
	        }
	        else
	        {
		        addedToStage += onAddedToStage;
	        }
        }
        
        private void start()
        {
            Starling.multitouchEnabled = true; // for Multitouch Scene
            Starling.handleLostContext = true; // required on Windows, needs more memory

			mStarling = new Starling(typeof(Game), stage);
            mStarling.simulateMultitouch = true;
            mStarling.enableErrorChecking = Capabilities.isDebugger;
            mStarling.start();
            
            // this event is dispatched when stage3D is set up
            mStarling.addEventListener(Event.ROOT_CREATED, (Action<Event,Game>)onRootCreated);
        }
        
        private void onAddedToStage(flash.events.Event e)
        {
	        addedToStage -= onAddedToStage;
            start();
        }
        
        private void onRootCreated(Event e, Game game)
        {
            // set framerate to 30 in software mode
            if (mStarling.context.driverInfo.toLowerCase().indexOf("software") != -1)
                mStarling.nativeStage.frameRate = 30;
            
            // define which resources to load
            var assets = new AssetManager();
            assets.verbose = Capabilities.isDebugger;
            assets.enqueue((Avm.Class)typeof(EmbeddedAssets));
            
            // background texture is embedded, because we need it right away!
            var bgTexture = Texture.fromEmbeddedAsset(Background, false);
            
            // game will first load resources, then start menu
            game.start(bgTexture, assets);
        }
    }
}