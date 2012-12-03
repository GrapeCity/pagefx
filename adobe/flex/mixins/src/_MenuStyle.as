
package 
{

import flash.display.Sprite;
import mx.core.IFlexModuleFactory;
import mx.core.mx_internal;
import mx.styles.CSSStyleDeclaration;
import mx.styles.StyleManager;
import mx.skins.halo.ListDropIndicator;

[ExcludeClass]

public class _MenuStyle
{
    [Embed(source='../Assets.swf', symbol='MenuRadioDisabled')]
    private static var _embed_css_Assets_swf_MenuRadioDisabled_370096492:Class;
    [Embed(source='../Assets.swf', symbol='MenuCheckDisabled')]
    private static var _embed_css_Assets_swf_MenuCheckDisabled_1422173487:Class;
    [Embed(source='../Assets.swf', symbol='MenuSeparator')]
    private static var _embed_css_Assets_swf_MenuSeparator_1275587218:Class;
    [Embed(source='../Assets.swf', symbol='MenuRadioEnabled')]
    private static var _embed_css_Assets_swf_MenuRadioEnabled_1948756655:Class;
    [Embed(source='../Assets.swf', symbol='MenuBranchDisabled')]
    private static var _embed_css_Assets_swf_MenuBranchDisabled_845700313:Class;
    [Embed(source='../Assets.swf', symbol='MenuBranchEnabled')]
    private static var _embed_css_Assets_swf_MenuBranchEnabled_1198128532:Class;
    [Embed(source='../Assets.swf', symbol='MenuCheckEnabled')]
    private static var _embed_css_Assets_swf_MenuCheckEnabled_165056116:Class;

    public static function init(fbs:IFlexModuleFactory):void
    {
        var style:CSSStyleDeclaration = StyleManager.getStyleDeclaration("Menu");
    
        if (!style)
        {
            style = new CSSStyleDeclaration();
            StyleManager.setStyleDeclaration("Menu", style, false);
        }
    
        if (style.defaultFactory == null)
        {
            style.defaultFactory = function():void
            {
                this.branchDisabledIcon = _embed_css_Assets_swf_MenuBranchDisabled_845700313;
                this.paddingLeft = 1;
                this.checkIcon = _embed_css_Assets_swf_MenuCheckEnabled_165056116;
                this.dropShadowEnabled = true;
                this.checkDisabledIcon = _embed_css_Assets_swf_MenuCheckDisabled_1422173487;
                this.radioIcon = _embed_css_Assets_swf_MenuRadioEnabled_1948756655;
                this.radioDisabledIcon = _embed_css_Assets_swf_MenuRadioDisabled_370096492;
                this.borderStyle = "menuBorder";
                this.paddingBottom = 1;
                this.rightIconGap = 15;
                this.paddingTop = 1;
                this.dropIndicatorSkin = mx.skins.halo.ListDropIndicator;
                this.paddingRight = 0;
                this.verticalAlign = "middle";
                this.separatorSkin = _embed_css_Assets_swf_MenuSeparator_1275587218;
                this.branchIcon = _embed_css_Assets_swf_MenuBranchEnabled_1198128532;
                this.leftIconGap = 18;
                this.horizontalGap = 6;
            };
        }
    }
}

}
