
package 
{

import flash.display.Sprite;
import mx.core.IFlexModuleFactory;
import mx.core.mx_internal;
import mx.styles.CSSStyleDeclaration;
import mx.styles.StyleManager;
import mx.skins.halo.ComboBoxArrowSkin;

[ExcludeClass]

public class _ComboBoxStyle
{

    public static function init(fbs:IFlexModuleFactory):void
    {
        var style:CSSStyleDeclaration = StyleManager.getStyleDeclaration("ComboBox");
    
        if (!style)
        {
            style = new CSSStyleDeclaration();
            StyleManager.setStyleDeclaration("ComboBox", style, false);
        }
    
        if (style.defaultFactory == null)
        {
            style.defaultFactory = function():void
            {
                this.skin = mx.skins.halo.ComboBoxArrowSkin;
                this.paddingLeft = 5;
                this.fontWeight = "bold";
                this.disabledIconColor = 0x919999;
                this.cornerRadius = 5;
                this.arrowButtonWidth = 22;
                this.paddingRight = 5;
                this.leading = 0;
                this.dropdownStyleName = "comboDropdown";
            };
        }
    }
}

}
