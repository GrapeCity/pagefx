
package 
{

import flash.display.Sprite;
import mx.core.IFlexModuleFactory;
import mx.core.mx_internal;
import mx.styles.CSSStyleDeclaration;
import mx.styles.StyleManager;
import mx.skins.halo.RadioButtonIcon;

[ExcludeClass]

public class _RadioButtonStyle
{

    public static function init(fbs:IFlexModuleFactory):void
    {
        var style:CSSStyleDeclaration = StyleManager.getStyleDeclaration("RadioButton");
    
        if (!style)
        {
            style = new CSSStyleDeclaration();
            StyleManager.setStyleDeclaration("RadioButton", style, false);
        }
    
        if (style.defaultFactory == null)
        {
            style.defaultFactory = function():void
            {
                this.selectedDisabledIcon = null;
                this.fontWeight = "normal";
                this.overIcon = null;
                this.upSkin = null;
                this.overSkin = null;
                this.selectedDisabledSkin = null;
                this.selectedDownIcon = null;
                this.textAlign = "left";
                this.cornerRadius = 7;
                this.icon = mx.skins.halo.RadioButtonIcon;
                this.upIcon = null;
                this.horizontalGap = 5;
                this.downSkin = null;
                this.selectedUpIcon = null;
                this.downIcon = null;
                this.paddingLeft = 0;
                this.skin = null;
                this.selectedOverIcon = null;
                this.iconColor = 0x2b333c;
                this.disabledIcon = null;
                this.selectedDownSkin = null;
                this.paddingRight = 0;
                this.selectedUpSkin = null;
                this.selectedOverSkin = null;
                this.disabledSkin = null;
            };
        }
    }
}

}
