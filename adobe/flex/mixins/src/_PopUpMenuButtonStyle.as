
package 
{

import flash.display.Sprite;
import mx.core.IFlexModuleFactory;
import mx.core.mx_internal;
import mx.styles.CSSStyleDeclaration;
import mx.styles.StyleManager;
import mx.skins.halo.PopUpMenuIcon;
import mx.skins.halo.PopUpButtonSkin;

[ExcludeClass]

public class _PopUpMenuButtonStyle
{

    public static function init(fbs:IFlexModuleFactory):void
    {
        var style:CSSStyleDeclaration = StyleManager.getStyleDeclaration("PopUpMenuButton");
    
        if (!style)
        {
            style = new CSSStyleDeclaration();
            StyleManager.setStyleDeclaration("PopUpMenuButton", style, false);
        }
    
        if (style.defaultFactory == null)
        {
            style.defaultFactory = function():void
            {
                this.popUpIcon = mx.skins.halo.PopUpMenuIcon;
                this.skin = mx.skins.halo.PopUpButtonSkin;
                this.popUpStyleName = "popUpMenu";
            };
        }
    }
}

}
