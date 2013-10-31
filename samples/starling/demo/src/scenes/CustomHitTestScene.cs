namespace demo.scenes
{
    using starling.text;
    using starling.utils;
    using utils;

    public class CustomHitTestScene : Scene
    {
        public CustomHitTestScene()
        {
            var description = 
                "Pushing the bird only works when the touch occurs within a circle." + 
                "This can be accomplished by overriding the method 'hitTest'.";
            
            var infoText = new TextField(300, 100, description);
            infoText.x = infoText.y = 10;
            infoText.vAlign = VAlign.TOP;
            infoText.hAlign = HAlign.CENTER;
            addChild(infoText);
            
            // 'RoundButton' is a helper class of the Demo, not a part of Starling!
            // Have a look at its code to understand this sample.
            
            var button = new RoundButton(Game.assets.getTexture("starling_round"), "", null);
            button.x = Constants.CenterX - (button.width / 2);
            button.y = 150;
            addChild(button);
        }
    }
}