
package 
{

import flash.display.Sprite;
import mx.core.IFlexModuleFactory;
import mx.core.mx_internal;
import mx.styles.CSSStyleDeclaration;
import mx.styles.StyleManager;

[ExcludeClass]

public class _TextAreaStyle
{

    public static function init(fbs:IFlexModuleFactory):void
    {
        var style:CSSStyleDeclaration = StyleManager.getStyleDeclaration("TextArea");
    
        if (!style)
        {
            style = new CSSStyleDeclaration();
            StyleManager.setStyleDeclaration("TextArea", style, false);
        }
    
        if (style.defaultFactory == null)
        {
            style.defaultFactory = function():void
            {
                this.horizontalScrollBarStyleName = "textAreaHScrollBarStyle";
                this.backgroundDisabledColor = 0xdddddd;
                this.borderStyle = "solid";
                this.backgroundColor = 0xffffff;
                this.verticalScrollBarStyleName = "textAreaVScrollBarStyle";
            };
        }
    }
}

}
