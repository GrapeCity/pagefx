
package 
{

import flash.display.Sprite;
import mx.core.IFlexModuleFactory;
import mx.core.mx_internal;
import mx.styles.CSSStyleDeclaration;
import mx.styles.StyleManager;
import mx.skins.halo.ActivatorSkin;
import mx.skins.halo.MenuBarBackgroundSkin;

[ExcludeClass]

public class _MenuBarStyle
{

    public static function init(fbs:IFlexModuleFactory):void
    {
        var style:CSSStyleDeclaration = StyleManager.getStyleDeclaration("MenuBar");
    
        if (!style)
        {
            style = new CSSStyleDeclaration();
            StyleManager.setStyleDeclaration("MenuBar", style, false);
        }
    
        if (style.defaultFactory == null)
        {
            style.defaultFactory = function():void
            {
                this.itemSkin = mx.skins.halo.ActivatorSkin;
                this.backgroundSkin = mx.skins.halo.MenuBarBackgroundSkin;
                this.translucent = false;
            };
        }
    }
}

}
