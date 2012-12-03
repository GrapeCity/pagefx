
package 
{

import flash.display.Sprite;
import mx.core.IFlexModuleFactory;
import mx.core.mx_internal;
import mx.styles.CSSStyleDeclaration;
import mx.styles.StyleManager;
import mx.skins.halo.NumericStepperUpSkin;
import mx.skins.halo.NumericStepperDownSkin;

[ExcludeClass]

public class _NumericStepperStyle
{

    public static function init(fbs:IFlexModuleFactory):void
    {
        var style:CSSStyleDeclaration = StyleManager.getStyleDeclaration("NumericStepper");
    
        if (!style)
        {
            style = new CSSStyleDeclaration();
            StyleManager.setStyleDeclaration("NumericStepper", style, false);
        }
    
        if (style.defaultFactory == null)
        {
            style.defaultFactory = function():void
            {
                this.downArrowSkin = mx.skins.halo.NumericStepperDownSkin;
                this.cornerRadius = 5;
                this.focusRoundedCorners = "tr br";
                this.upArrowSkin = mx.skins.halo.NumericStepperUpSkin;
            };
        }
    }
}

}
