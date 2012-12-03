
package 
{

import flash.display.Sprite;
import mx.core.IFlexModuleFactory;
import mx.core.mx_internal;
import mx.styles.CSSStyleDeclaration;
import mx.styles.StyleManager;

[ExcludeClass]

public class _FormItemStyle
{
    [Embed(source='../Assets.swf', symbol='mx.containers.FormItem.Required')]
    private static var _embed_css_Assets_swf_mx_containers_FormItem_Required_1851820665:Class;

    public static function init(fbs:IFlexModuleFactory):void
    {
        var style:CSSStyleDeclaration = StyleManager.getStyleDeclaration("FormItem");
    
        if (!style)
        {
            style = new CSSStyleDeclaration();
            StyleManager.setStyleDeclaration("FormItem", style, false);
        }
    
        if (style.defaultFactory == null)
        {
            style.defaultFactory = function():void
            {
                this.indicatorSkin = _embed_css_Assets_swf_mx_containers_FormItem_Required_1851820665;
            };
        }
    }
}

}
