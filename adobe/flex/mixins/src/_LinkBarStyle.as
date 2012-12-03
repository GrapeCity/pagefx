
package 
{

import flash.display.Sprite;
import mx.core.IFlexModuleFactory;
import mx.core.mx_internal;
import mx.styles.CSSStyleDeclaration;
import mx.styles.StyleManager;
import mx.skins.halo.LinkSeparator;

[ExcludeClass]

public class _LinkBarStyle
{

    public static function init(fbs:IFlexModuleFactory):void
    {
        var style:CSSStyleDeclaration = StyleManager.getStyleDeclaration("LinkBar");
    
        if (!style)
        {
            style = new CSSStyleDeclaration();
            StyleManager.setStyleDeclaration("LinkBar", style, false);
        }
    
        if (style.defaultFactory == null)
        {
            style.defaultFactory = function():void
            {
                this.paddingTop = 2;
                this.paddingLeft = 2;
                this.separatorWidth = 1;
                this.paddingRight = 2;
                this.separatorSkin = mx.skins.halo.LinkSeparator;
                this.verticalGap = 8;
                this.linkButtonStyleName = "linkButtonStyle";
                this.horizontalGap = 8;
                this.paddingBottom = 2;
                this.separatorColor = 0xc4cccc;
            };
        }
    }
}

}
