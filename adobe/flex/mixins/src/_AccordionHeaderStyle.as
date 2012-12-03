
package 
{

import flash.display.Sprite;
import mx.core.IFlexModuleFactory;
import mx.core.mx_internal;
import mx.styles.CSSStyleDeclaration;
import mx.styles.StyleManager;
import mx.skins.halo.AccordionHeaderSkin;

[ExcludeClass]

public class _AccordionHeaderStyle
{

    public static function init(fbs:IFlexModuleFactory):void
    {
        var style:CSSStyleDeclaration = StyleManager.getStyleDeclaration("AccordionHeader");
    
        if (!style)
        {
            style = new CSSStyleDeclaration();
            StyleManager.setStyleDeclaration("AccordionHeader", style, false);
        }
    
        if (style.defaultFactory == null)
        {
            style.defaultFactory = function():void
            {
                this.skin = mx.skins.halo.AccordionHeaderSkin;
                this.paddingLeft = 5;
                this.fontWeight = "bold";
                this.upSkin = null;
                this.fontSize = "10";
                this.overSkin = null;
                this.paddingBottom = 0;
                this.selectedDisabledSkin = null;
                this.paddingTop = 0;
                this.textAlign = "left";
                this.selectedDownSkin = null;
                this.selectedUpSkin = null;
                this.paddingRight = 5;
                this.selectedOverSkin = null;
                this.downSkin = null;
                this.horizontalGap = 2;
                this.disabledSkin = null;
            };
        }
    }
}

}
