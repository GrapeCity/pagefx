
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

public class _VSliderStyle
{

    public static function init(fbs:IFlexModuleFactory):void
    {
        var style:CSSStyleDeclaration = StyleManager.getStyleDeclaration("VSlider");
    
        if (!style)
        {
            style = new CSSStyleDeclaration();
            StyleManager.setStyleDeclaration("VSlider", style, false);
        }
    
        if (style.defaultFactory == null)
        {
            style.defaultFactory = function():void
            {
                this.thumbOffset = 0;
                this.borderColor = 0x919999;
                this.trackHighlightSkin = mx.skins.halo.SliderHighlightSkin;
                this.tickColor = 0x6f7777;
                this.thumbDownSkin = null;
                this.tickOffset = -6;
                this.labelOffset = -10;
                this.thumbOverSkin = null;
                this.tickLength = 3;
                this.thumbDisabledSkin = null;
                this.trackColors = [0xe7e7e7, 0xe7e7e7];
                this.thumbUpSkin = null;
                this.dataTipPlacement = "left";
                this.dataTipOffset = 16;
                this.trackSkin = mx.skins.halo.SliderTrackSkin;
                this.tickThickness = 1;
                this.showTrackHighlight = false;
                this.thumbSkin = mx.skins.halo.SliderThumbSkin;
                this.dataTipPrecision = 2;
                this.slideDuration = 300;
            };
        }
    }
}

}
