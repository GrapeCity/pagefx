
package 
{

import flash.display.Sprite;
import mx.core.IFlexModuleFactory;
import mx.core.mx_internal;
import mx.styles.CSSStyleDeclaration;
import mx.styles.StyleManager;

[ExcludeClass]

public class _DateFieldStyle
{
    [Embed(source='../Assets.swf', symbol='openDateOver')]
    private static var _embed_css_Assets_swf_openDateOver_1569852304:Class;

    public static function init(fbs:IFlexModuleFactory):void
    {
        var style:CSSStyleDeclaration = StyleManager.getStyleDeclaration("DateField");
    
        if (!style)
        {
            style = new CSSStyleDeclaration();
            StyleManager.setStyleDeclaration("DateField", style, false);
        }
    
        if (style.defaultFactory == null)
        {
            style.defaultFactory = function():void
            {
                this.upSkin = _embed_css_Assets_swf_openDateOver_1569852304;
                this.overSkin = _embed_css_Assets_swf_openDateOver_1569852304;
                this.downSkin = _embed_css_Assets_swf_openDateOver_1569852304;
                this.dateChooserStyleName = "dateFieldPopup";
                this.disabledSkin = _embed_css_Assets_swf_openDateOver_1569852304;
            };
        }
    }
}

}
