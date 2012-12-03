
package 
{

import flash.display.Sprite;
import mx.core.IFlexModuleFactory;
import mx.core.mx_internal;
import mx.styles.CSSStyleDeclaration;
import mx.styles.StyleManager;
import mx.skins.halo.LinkButtonSkin;

[ExcludeClass]

public class _LinkButtonStyle
{

    public static function init(fbs:IFlexModuleFactory):void
    {
        var style:CSSStyleDeclaration = StyleManager.getStyleDeclaration("LinkButton");
    
        if (!style)
        {
            style = new CSSStyleDeclaration();
            StyleManager.setStyleDeclaration("LinkButton", style, false);
        }
    
        if (style.defaultFactory == null)
        {
            style.defaultFactory = function():void
            {
                this.skin = mx.skins.halo.LinkButtonSkin;
                this.paddingLeft = 7;
                this.selectedDownSkin = null;
                this.selectedUpSkin = null;
                this.paddingRight = 7;
                this.selectedOverSkin = null;
                this.upSkin = null;
                this.overSkin = null;
                this.downSkin = null;
                this.selectedDisabledSkin = null;
                this.disabledSkin = null;
            };
        }
    }
}

}
