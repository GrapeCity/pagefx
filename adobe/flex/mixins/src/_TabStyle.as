
package 
{

import flash.display.Sprite;
import mx.core.IFlexModuleFactory;
import mx.core.mx_internal;
import mx.styles.CSSStyleDeclaration;
import mx.styles.StyleManager;
import mx.skins.halo.TabSkin;

[ExcludeClass]

public class _TabStyle
{

    public static function init(fbs:IFlexModuleFactory):void
    {
        var style:CSSStyleDeclaration = StyleManager.getStyleDeclaration("Tab");
    
        if (!style)
        {
            style = new CSSStyleDeclaration();
            StyleManager.setStyleDeclaration("Tab", style, false);
        }
    
        if (style.defaultFactory == null)
        {
            style.defaultFactory = function():void
            {
                this.paddingTop = 1;
                this.skin = mx.skins.halo.TabSkin;
                this.selectedDownSkin = null;
                this.selectedUpSkin = null;
                this.selectedOverSkin = null;
                this.upSkin = null;
                this.overSkin = null;
                this.downSkin = null;
                this.selectedDisabledSkin = null;
                this.paddingBottom = 1;
                this.disabledSkin = null;
            };
        }
    }
}

}
