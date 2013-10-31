using Avm;

namespace demo.scenes
{
    using flash.display;
    
    using starling.core;
    using starling.display;
    using starling.events;
    using starling.filters;
    using starling.text;
    using starling.textures;

    public class FilterScene : Scene
    {
        private Button mButton;
        private Image mImage;
        private TextField mInfoText;
        private Array mFilterInfos;
        
        public FilterScene()
        {
            mButton = new Button(Game.assets.getTexture("button_normal"), "Switch Filter");
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
            
            initFilters();
            onButtonTriggered(null);
        }
        
        private void onButtonTriggered(Event e)
        {
            var filterInfo = mFilterInfos.shift() as Avm.Array;
            mFilterInfos.push(filterInfo);
            
            mInfoText.text = filterInfo[0] as string;
			mImage.filter = filterInfo[1] as FragmentFilter;
        }
        
        private void initFilters()
        {
	        mFilterInfos = new Array(
		        new Array("Identity", new ColorMatrixFilter()),
		        new Array("Blur", new BlurFilter()),
		        new Array("Drop Shadow", BlurFilter.createDropShadow()),
		        new Array("Glow", BlurFilter.createGlow())
		        );
            
            var displacementFilter = new DisplacementMapFilter(
                createDisplacementMap(mImage.width, mImage.height), null,
                BitmapDataChannel.RED, BitmapDataChannel.GREEN, 25, 25);
            mFilterInfos.push(new Array("Displacement Map", displacementFilter));
            
            var invertFilter = new ColorMatrixFilter();
            invertFilter.invert();
            mFilterInfos.push(new Array("Invert", invertFilter));
            
            var grayscaleFilter = new ColorMatrixFilter();
            grayscaleFilter.adjustSaturation(-1);
            mFilterInfos.push(new Array("Grayscale", grayscaleFilter));
            
            var saturationFilter = new ColorMatrixFilter();
            saturationFilter.adjustSaturation(1);
            mFilterInfos.push(new Array("Saturation", saturationFilter));
            
            var contrastFilter = new ColorMatrixFilter();
            contrastFilter.adjustContrast(0.75);
            mFilterInfos.push(new Array("Contrast", contrastFilter));

            var brightnessFilter = new ColorMatrixFilter();
            brightnessFilter.adjustBrightness(-0.25);
            mFilterInfos.push(new Array("Brightness", brightnessFilter));

            var hueFilter = new ColorMatrixFilter();
            hueFilter.adjustHue(1);
            mFilterInfos.push(new Array("Hue", hueFilter));
        }
        
        private Texture createDisplacementMap(double width, double height)
        {
            var scale = Starling.__contentScaleFactor;
            var map = new BitmapData((int)(width*scale), (int)(height*scale), false);
            map.perlinNoise(20*scale, 20*scale, 3, 5, false, true);
            var texture = Texture.fromBitmapData(map, false, false, scale);
            return texture;
        }
    }
}