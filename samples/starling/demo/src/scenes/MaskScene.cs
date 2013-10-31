namespace demo.scenes
{
    using flash.geom;
    using starling.core;
    using starling.display;
    using starling.events;
    using starling.filters;
    using starling.text;

    public class MaskScene : Scene
    {
        private Sprite mContents;
        private Quad mClipQuad;
        
        public MaskScene()
        {
            mContents = new Sprite();
            addChild(mContents);
            
            var stageWidth  = Starling.current.stage.stageWidth;
            var stageHeight = Starling.current.stage.stageHeight;
            
            var touchQuad = new Quad(stageWidth, stageHeight);
            touchQuad.alpha = 0; // only used to get touch events
            addChildAt(touchQuad, 0);
            
            var image = new Image(Game.assets.getTexture("flight_00"));
            image.x = (stageWidth - image.width) / 2;
            image.y = 80;
            mContents.addChild(image);
            
            // just to prove it works, use a filter on the image.
            var cm = new ColorMatrixFilter();
            cm.adjustHue(-0.5);
            image.filter = cm;
            
            var scissorText = new TextField(256, 128, 
                "Move the mouse (or a finger) over the screen to move the clipping rectangle.");
            scissorText.x = (stageWidth - scissorText.width) / 2;
            scissorText.y = 240;
            mContents.addChild(scissorText);
            
            var maskText = new TextField(256, 128, 
                "Currently, Starling supports only stage-aligned clipping; more complex masks " +
                "will be supported in future versions.");
            maskText.x = scissorText.x;
            maskText.y = 290;
            mContents.addChild(maskText);
            
            var scissorRect = new Rectangle(0, 0, 150, 150); 
            scissorRect.x = (stageWidth  - scissorRect.width)  / 2;
            scissorRect.y = (stageHeight - scissorRect.height) / 2 + 5;
            mContents.clipRect = scissorRect;
            
            mClipQuad = new Quad(scissorRect.width, scissorRect.height, 0xff0000);
            mClipQuad.x = scissorRect.x;
            mClipQuad.y = scissorRect.y;
            mClipQuad.alpha = 0.1;
            mClipQuad.touchable = false;
            addChild(mClipQuad);
            
            touch += onTouch;
        }

	    public override void dispose()
	    {
			touch -= onTouch;
		    base.dispose();
	    }

	    private void onTouch(TouchEvent e)
        {
			var touch = e.getTouch(this, TouchPhase.HOVER);
	        if (touch == null)
	        {
		        touch = e.getTouch(this, TouchPhase.BEGAN);
		        if (touch == null)
		        {
			        touch = e.getTouch(this, TouchPhase.MOVED);
		        }
	        }

            if (touch != null)
            {
                var localPos = touch.getLocation(this);
                var clipRect = mContents.clipRect;
                clipRect.x = localPos.x - clipRect.width  / 2;
                clipRect.y = localPos.y - clipRect.height / 2;
                
                mClipQuad.x = clipRect.x;
                mClipQuad.y = clipRect.y;
            }
        }
    }
}