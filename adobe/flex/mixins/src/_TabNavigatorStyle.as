
package 
{

import flash.display.Sprite;
import mx.core.IFlexModuleFactory;
import mx.core.mx_internal;
import mx.styles.CSSStyleDeclaration;
import mx.styles.StyleManager;

[ExcludeClass]

public class _TabNavigatorStyle
{

    public static function init(fbs:IFlexModuleFactory):void
    {
        var style:CSSStyleDeclaration = StyleManager.getStyleDeclaration("TabNavigator");
    
        if (!style)
        {
            style = new CSSStyleDeclaration();
            StyleManager.setStyleDeclaration("TabNavigator", style, false);
        }
    
        if (style.defaultFactory == null)
        {
            style.defaultFactory = function():void
            {
                this.paddingTop = 10;
                this.borderColor = 0xaab3b3;
                this.horizontalAlign = "left";
                this.tabOffset = 0;
                this.horizontalGap = -1;
                this.borderStyle = "solid";
                this.backgroundColor = 0xffffff;
            };
        }
    }
}

}
