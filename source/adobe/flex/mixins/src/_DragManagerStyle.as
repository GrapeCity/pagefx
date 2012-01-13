
package 
{

import flash.display.Sprite;
import mx.core.IFlexModuleFactory;
import mx.core.mx_internal;
import mx.styles.CSSStyleDeclaration;
import mx.styles.StyleManager;
import mx.skins.halo.DefaultDragImage;

[ExcludeClass]

public class _DragManagerStyle
{
    [Embed(source='../Assets.swf', symbol='mx.skins.cursor.DragReject')]
    private static var _embed_css_Assets_swf_mx_skins_cursor_DragReject_792450908:Class;
    [Embed(source='../Assets.swf', symbol='mx.skins.cursor.DragLink')]
    private static var _embed_css_Assets_swf_mx_skins_cursor_DragLink_929989873:Class;
    [Embed(source='../Assets.swf', symbol='mx.skins.cursor.DragMove')]
    private static var _embed_css_Assets_swf_mx_skins_cursor_DragMove_929962250:Class;
    [Embed(source='../Assets.swf', symbol='mx.skins.cursor.DragCopy')]
    private static var _embed_css_Assets_swf_mx_skins_cursor_DragCopy_929727598:Class;

    public static function init(fbs:IFlexModuleFactory):void
    {
        var style:CSSStyleDeclaration = StyleManager.getStyleDeclaration("DragManager");
    
        if (!style)
        {
            style = new CSSStyleDeclaration();
            StyleManager.setStyleDeclaration("DragManager", style, false);
        }
    
        if (style.defaultFactory == null)
        {
            style.defaultFactory = function():void
            {
                this.rejectCursor = _embed_css_Assets_swf_mx_skins_cursor_DragReject_792450908;
                this.defaultDragImageSkin = mx.skins.halo.DefaultDragImage;
                this.moveCursor = _embed_css_Assets_swf_mx_skins_cursor_DragMove_929962250;
                this.copyCursor = _embed_css_Assets_swf_mx_skins_cursor_DragCopy_929727598;
                this.linkCursor = _embed_css_Assets_swf_mx_skins_cursor_DragLink_929989873;
            };
        }
    }
}

}
