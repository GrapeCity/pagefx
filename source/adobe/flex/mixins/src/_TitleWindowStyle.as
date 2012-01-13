
package 
{

import flash.display.Sprite;
import mx.core.IFlexModuleFactory;
import mx.core.mx_internal;
import mx.styles.CSSStyleDeclaration;
import mx.styles.StyleManager;

[ExcludeClass]

public class _TitleWindowStyle
{
    [Embed(source='../Assets.swf', symbol='CloseButtonDisabled')]
    private static var _embed_css_Assets_swf_CloseButtonDisabled_1347420978:Class;
    [Embed(source='../Assets.swf', symbol='CloseButtonOver')]
    private static var _embed_css_Assets_swf_CloseButtonOver_1585554598:Class;
    [Embed(source='../Assets.swf', symbol='CloseButtonDown')]
    private static var _embed_css_Assets_swf_CloseButtonDown_1153995696:Class;
    [Embed(source='../Assets.swf', symbol='CloseButtonUp')]
    private static var _embed_css_Assets_swf_CloseButtonUp_512526383:Class;

    public static function init(fbs:IFlexModuleFactory):void
    {
        var style:CSSStyleDeclaration = StyleManager.getStyleDeclaration("TitleWindow");
    
        if (!style)
        {
            style = new CSSStyleDeclaration();
            StyleManager.setStyleDeclaration("TitleWindow", style, false);
        }
    
        if (style.defaultFactory == null)
        {
            style.defaultFactory = function():void
            {
                this.paddingTop = 4;
                this.paddingLeft = 4;
                this.cornerRadius = 8;
                this.paddingRight = 4;
                this.dropShadowEnabled = true;
                this.closeButtonDownSkin = _embed_css_Assets_swf_CloseButtonDown_1153995696;
                this.closeButtonOverSkin = _embed_css_Assets_swf_CloseButtonOver_1585554598;
                this.closeButtonUpSkin = _embed_css_Assets_swf_CloseButtonUp_512526383;
                this.closeButtonDisabledSkin = _embed_css_Assets_swf_CloseButtonDisabled_1347420978;
                this.paddingBottom = 4;
                this.backgroundColor = 0xffffff;
            };
        }
    }
}

}
