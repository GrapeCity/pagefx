
package 
{

import flash.display.Sprite;
import mx.core.IFlexModuleFactory;
import mx.core.mx_internal;
import mx.styles.CSSStyleDeclaration;
import mx.styles.StyleManager;
import mx.skins.halo.SliderHighlightSkin;
import mx.skins.halo.SliderTrackSkin;
import mx.skins.halo.SliderThumbSkin;

[ExcludeClass]

public class _HSliderStyle
{

    public static function init(fbs:IFlexModuleFactory):void
    {
        var style:CSSStyleDeclaration = StyleManager.getStyleDeclaration("HSlider");
    
        if (!style)
        {
            style = new CSSStyleDeclaration();
            StyleManager.setStyleDeclaration("HSlider", style, false);
        }
    
        if (style.defaultFactory == null)
        {
            style.defaultFactory = function():void
            {
                this.thumbOffset = 0;
                this.borderColor = 0x919999;
                this.trackHighlightSkin = mx.skins.halo.SliderHighlightSkin;
                this.tickColor = 0x6f7777;
                this.tickOffset = -6;
                this.labelOffset = -10;
                this.tickLength = 4;
                this.trackColors = [0xe7e7e7, 0xe7e7e7];
                this.dataTipPlacement = "top";
                this.dataTipOffset = 16;
                this.trackSkin = mx.skins.halo.SliderTrackSkin;
                this.showTrackHighlight = false;
                this.thumbSkin = mx.skins.halo.SliderThumbSkin;
                this.tickThickness = 1;
                this.dataTipPrecision = 2;
                this.slideDuration = 300;
            };
        }
    }
}

}
