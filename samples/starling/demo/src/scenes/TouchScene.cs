namespace demo.scenes
{
    using starling.display;
    using starling.text;
    using utils;

    public class TouchScene : Scene
    {
        public TouchScene()
        {
            var description = "[use Ctrl/Cmd & Shift to simulate multi-touch]";
            
            var infoText = new TextField(300, 25, description);
            infoText.x = infoText.y = 10;
            addChild(infoText);
            
            // to find out how to react to touch events have a look at the TouchSheet class! 
            // It's part of the demo.
            
            var sheet = new TouchSheet(new Image(Game.assets.getTexture("starling_sheet")));
            sheet.x = Constants.CenterX;
            sheet.y = Constants.CenterY;
            sheet.rotation = starling.utils.Global.deg2rad(10);
            addChild(sheet);
        }
    }
}