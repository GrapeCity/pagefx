
package 
{

import flash.display.Sprite;
import mx.core.IFlexModuleFactory;
import mx.core.mx_internal;
import mx.styles.CSSStyleDeclaration;
import mx.styles.StyleManager;

[ExcludeClass]

public class _DividedBoxStyle
{
    [Embed(source='../Assets.swf', symbol='mx.skins.BoxDividerSkin')]
    private static var _embed_css_Assets_swf_mx_skins_BoxDividerSkin_1544025422:Class;
    [Embed(source='../Assets.swf', symbol='mx.skins.cursor.VBoxDivider')]
    private static var _embed_css_Assets_swf_mx_skins_cursor_VBoxDivider_660504365:Class;
    [Embed(source='../Assets.swf', symbol='mx.skins.cursor.HBoxDivider')]
    private static var _embed_css_Assets_swf_mx_skins_cursor_HBoxDivider_1792811235:Class;

    public static function init(fbs:IFlexModuleFactory):void
    {
        var style:CSSStyleDeclaration = StyleManager.getStyleDeclaration("DividedBox");
    
        if (!style)
        {
            style = new CSSStyleDeclaration();
            StyleManager.setStyleDeclaration("DividedBox", style, false);
        }
    
        if (style.defaultFactory == null)
        {
            style.defaultFactory = function():void
            {
                this.verticalDividerCursor = _embed_css_Assets_swf_mx_skins_cursor_VBoxDivider_660504365;
                this.dividerAffordance = 6;
                this.verticalGap = 10;
                this.horizontalGap = 10;
                this.dividerThickness = 3;
                this.dividerAlpha = 0.75;
                this.horizontalDividerCursor = _embed_css_Assets_swf_mx_skins_cursor_HBoxDivider_1792811235;
                this.dividerSkin = _embed_css_Assets_swf_mx_skins_BoxDividerSkin_1544025422;
                this.dividerColor = 0x6f7777;
            };
        }
    }
}

}
