
package 
{

import flash.display.Sprite;
import mx.core.IFlexModuleFactory;
import mx.core.mx_internal;
import mx.styles.CSSStyleDeclaration;
import mx.styles.StyleManager;

[ExcludeClass]

public class _TabBarStyle
{

    public static function init(fbs:IFlexModuleFactory):void
    {
        var style:CSSStyleDeclaration = StyleManager.getStyleDeclaration("TabBar");
    
        if (!style)
        {
            style = new CSSStyleDeclaration();
            StyleManager.setStyleDeclaration("TabBar", style, false);
        }
    
        if (style.defaultFactory == null)
        {
            style.defaultFactory = function():void
            {
                this.textAlign = "center";
                this.horizontalAlign = "left";
                this.verticalAlign = "top";
                this.verticalGap = -1;
                this.horizontalGap = -1;
                this.selectedTabTextStyleName = "activeTabStyle";
            };
        }
    }
}

}
