namespace demo
{
	using scenes;
    using starling.core;
    using starling.display;
    using starling.events;
    using starling.text;
    using starling.utils;

    public class MainMenu : Sprite
    {
        public MainMenu()
        {
            init();
        }
        
        private void init()
        {
            var logo = new Image(Game.assets.getTexture("logo"));
            addChild(logo);

	        var scenesToCreate = new[]
	        {
		        new {title = "Textures", type = typeof(TextureScene)},
		        new {title = "Multitouch", type = typeof(TouchScene)},
//		        new {title = "TextFields", type = typeof(TextScene)},
		        new {title = "Animations", type = typeof(AnimationScene)},
		        new {title = "Custom hit-test", type = typeof(CustomHitTestScene)},
//		        new {title = "Movie Clip", type = typeof(MovieScene)},
		        new {title = "Filters", type = typeof(FilterScene)},
		        new {title = "Blend Modes", type = typeof(BlendModeScene)},
//		        new {title = "Render Texture", type = typeof(RenderTextureScene)},
		        new {title = "Benchmark", type = typeof(BenchmarkScene)},
		        new {title = "Clipping", type = typeof(MaskScene)}
	        };
            
            var buttonTexture = Game.assets.getTexture("button_medium");
            var count = 0;
            
            foreach (var sceneToCreate in scenesToCreate)
            {
                var sceneTitle = sceneToCreate.title;
                var sceneClass = sceneToCreate.type;
                
                var button = new Button(buttonTexture, sceneTitle);
                button.x = count % 2 == 0 ? 28 : 167;
                button.y = 155 + (count / 2) * 46;
                button.name = flash.utils.Global.getQualifiedClassName(sceneClass);
                addChild(button);
                
                if (scenesToCreate.Length % 2 != 0 && count % 2 == 1)
                    button.y += 24;
                
                ++count;
            }
            
            // show information about rendering method (hardware/software)
            
            var driverInfo = Starling.__context.driverInfo;
            var infoText = new TextField(310, 64, driverInfo, "Verdana", 10);
            infoText.x = 5;
            infoText.y = 475 - infoText.height;
            infoText.vAlign = VAlign.BOTTOM;
            infoText.touch += onInfoTextTouched;
            addChildAt(infoText, 0);
        }
        
        private void onInfoTextTouched(TouchEvent e)
        {
            if (e.getTouch(this, TouchPhase.ENDED) != null)
                Starling.current.showStats = !Starling.current.showStats;
        }
    }
}