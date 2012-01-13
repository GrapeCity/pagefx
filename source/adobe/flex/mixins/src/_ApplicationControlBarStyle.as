
package 
{

import flash.display.Sprite;
import mx.core.IFlexModuleFactory;
import mx.core.mx_internal;
import mx.styles.CSSStyleDeclaration;
import mx.styles.StyleManager;

[ExcludeClass]

public class _ApplicationControlBarStyle
{

    public static function init(fbs:IFlexModuleFactory):void
    {
        var style:CSSStyleDeclaration = StyleManager.getStyleDeclaration("ApplicationControlBar");
    
        if (!style)
        {
            style = new CSSStyleDeclaration();
            StyleManager.setStyleDeclaration("ApplicationControlBar", style, false);
        }
    
        if (style.defaultFactory == null)
        {
            style.defaultFactory = function():void
            {
                this.paddingTop = 5;
                this.paddingLeft = 8;
                this.fillAlphas = [0, 0];
                this.cornerRadius = 5;
                this.paddingRight = 8;
                this.fillColors = [0xffffff, 0xffffff];
                this.dropShadowEnabled = true;
                this.docked = false;
                this.paddingBottom = 4;
                this.borderStyle = "applicationControlBar";
                this.shadowDistance = 5;
            };
        }
    }
}

}
