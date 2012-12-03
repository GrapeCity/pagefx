
package 
{

import flash.display.Sprite;
import mx.core.IFlexModuleFactory;
import mx.core.mx_internal;
import mx.styles.CSSStyleDeclaration;
import mx.styles.StyleManager;
import mx.skins.halo.ColorPickerSkin;

[ExcludeClass]

public class _ColorPickerStyle
{

    public static function init(fbs:IFlexModuleFactory):void
    {
        var style:CSSStyleDeclaration = StyleManager.getStyleDeclaration("ColorPicker");
    
        if (!style)
        {
            style = new CSSStyleDeclaration();
            StyleManager.setStyleDeclaration("ColorPicker", style, false);
        }
    
        if (style.defaultFactory == null)
        {
            style.defaultFactory = function():void
            {
                this.skin = mx.skins.halo.ColorPickerSkin;
                this.verticalGap = 0;
                this.shadowColor = 0x4d555e;
                this.fontSize = 11;
                this.swatchBorderSize = 0;
                this.iconColor = 0x000000;
            };
        }
    }
}

}
