
package 
{

import flash.display.Sprite;
import mx.core.IFlexModuleFactory;
import mx.core.mx_internal;
import mx.styles.CSSStyleDeclaration;
import mx.styles.StyleManager;
import mx.skins.halo.ProgressTrackSkin;
import mx.skins.halo.ProgressBarSkin;
import mx.skins.halo.ProgressIndeterminateSkin;
import mx.skins.halo.ProgressMaskSkin;

[ExcludeClass]

public class _ProgressBarStyle
{

    public static function init(fbs:IFlexModuleFactory):void
    {
        var style:CSSStyleDeclaration = StyleManager.getStyleDeclaration("ProgressBar");
    
        if (!style)
        {
            style = new CSSStyleDeclaration();
            StyleManager.setStyleDeclaration("ProgressBar", style, false);
        }
    
        if (style.defaultFactory == null)
        {
            style.defaultFactory = function():void
            {
                this.trackColors = [0xe7e7e7, 0xffffff];
                this.fontWeight = "bold";
                this.maskSkin = mx.skins.halo.ProgressMaskSkin;
                this.leading = 0;
                this.trackSkin = mx.skins.halo.ProgressTrackSkin;
                this.indeterminateMoveInterval = 28;
                this.barSkin = mx.skins.halo.ProgressBarSkin;
                this.indeterminateSkin = mx.skins.halo.ProgressIndeterminateSkin;
            };
        }
    }
}

}
