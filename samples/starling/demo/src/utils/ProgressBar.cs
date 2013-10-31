namespace demo.utils
{
	using System;
    using flash.display;
    
    using starling.core;
    using starling.display;
    using starling.textures;

    public class ProgressBar : starling.display.Sprite
    {
        private Quad mBar;
        private Image mBackground;
        
        public ProgressBar(int width, int height)
        {
            init(width, height);
        }
        
        private void init(int width, int height)
        {
            var scale = Starling.__contentScaleFactor;
            var padding = height * 0.2;
            var cornerRadius = padding * scale * 2;
            
            // create black rounded box for background
            
            var bgShape = new Shape();
            bgShape.graphics.beginFill(0x0, 0.5);
            bgShape.graphics.drawRoundRect(0, 0, width*scale, height*scale, cornerRadius, cornerRadius);
            bgShape.graphics.endFill();
            
            var bgBitmapData = new BitmapData((int)(width*scale), (int)(height*scale), true, 0x0);
            bgBitmapData.draw(bgShape);
            var bgTexture = Texture.fromBitmapData(bgBitmapData, false, false, scale);
            
            mBackground = new Image(bgTexture);
            addChild(mBackground);
            
            // create progress bar quad
            
            mBar = new Quad(width - 2*padding, height - 2*padding, 0xeeeeee);
            mBar.setVertexColor(2, 0xaaaaaa);
            mBar.setVertexColor(3, 0xaaaaaa);
            mBar.x = padding;
            mBar.y = padding;
            mBar.scaleX = 0;
            addChild(mBar);
        }

	    public double ratio
	    {
		    get { return mBar.scaleX; }
		    set
		    {
			    mBar.scaleX = Math.Max(0.0, Math.Min(1.0, value));
		    }
	    }
    }
}