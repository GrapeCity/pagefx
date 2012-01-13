
package 
{

import flash.display.Sprite;
import mx.core.IFlexModuleFactory;
import mx.core.mx_internal;
import mx.styles.CSSStyleDeclaration;
import mx.styles.StyleManager;
import mx.skins.halo.PopUpIcon;
import mx.skins.halo.PopUpButtonSkin;

[ExcludeClass]

public class _PopUpButtonStyle
{

    public static function init(fbs:IFlexModuleFactory):void
    {
        var style:CSSStyleDeclaration = StyleManager.getStyleDeclaration("PopUpButton");
    
        if (!style)
        {
            style = new CSSStyleDeclaration();
            StyleManager.setStyleDeclaration("PopUpButton", style, false);
        }
    
        if (style.defaultFactory == null)
        {
            style.defaultFactory = function():void
            {
                this.popUpIcon = mx.skins.halo.PopUpIcon;
                this.skin = mx.skins.halo.PopUpButtonSkin;
                this.paddingLeft = 3;
                this.arrowButtonWidth = 18;
                this.paddingRight = 3;
                this.popUpGap = 0;
            };
        }
    }
}

}
