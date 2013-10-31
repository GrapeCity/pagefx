namespace demo.scenes
{
    using starling.display;
    using starling.events;
    using starling.text;

    public class BlendModeScene : Scene
    {
        private Button mButton;
        private Image mImage;
        private TextField mInfoText;

	    private Avm.Array mBlendModes = new Avm.Array(
		    BlendMode.NORMAL,
		    BlendMode.MULTIPLY,
		    BlendMode.SCREEN,
		    BlendMode.ADD,
		    BlendMode.ERASE,
		    BlendMode.NONE
		    );
        
        public BlendModeScene()
        {
            mButton = new Button(Game.assets.getTexture("button_normal"), "Switch Mode");
            mButton.x = (Constants.CenterX - mButton.width / 2);
            mButton.y = 15;
            mButton.triggered += onButtonTriggered;
            addChild(mButton);
            
            mImage = new Image(Game.assets.getTexture("starling_rocket"));
            mImage.x = (Constants.CenterX - mImage.width / 2);
            mImage.y = 170;
            addChild(mImage);
            
            mInfoText = new TextField(300, 32, "", "Verdana", 19);
            mInfoText.x = 10;
            mInfoText.y = 330;
            addChild(mInfoText);
            
            onButtonTriggered(null);
        }
        
        private void onButtonTriggered(Event e)
        {
            var blendMode = mBlendModes.shift() as string;
            mBlendModes.push(blendMode);
            
            mInfoText.text = blendMode;
            mImage.blendMode = blendMode;
        }
    }
}