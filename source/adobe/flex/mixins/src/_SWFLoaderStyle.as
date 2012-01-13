
package 
{

import flash.display.Sprite;
import mx.core.IFlexModuleFactory;
import mx.core.mx_internal;
import mx.styles.CSSStyleDeclaration;
import mx.styles.StyleManager;
import mx.skins.halo.BrokenImageBorderSkin;

[ExcludeClass]

public class _SWFLoaderStyle
{
    [Embed(source='../Assets.swf', symbol='__brokenImage')]
    private static var _embed_css_Assets_swf___brokenImage_542830610:Class;

    public static function init(fbs:IFlexModuleFactory):void
    {
        var style:CSSStyleDeclaration = StyleManager.getStyleDeclaration("SWFLoader");
    
        if (!style)
        {
            style = new CSSStyleDeclaration();
            StyleManager.setStyleDeclaration("SWFLoader", style, false);
        }
    
        if (style.defaultFactory == null)
        {
            style.defaultFactory = function():void
            {
                this.brokenImageSkin = _embed_css_Assets_swf___brokenImage_542830610;
                this.borderStyle = "none";
                this.brokenImageBorderSkin = mx.skins.halo.BrokenImageBorderSkin;
            };
        }
    }
}

}
