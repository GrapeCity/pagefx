
package 
{

import flash.display.Sprite;
import mx.core.IFlexModuleFactory;
import mx.core.mx_internal;
import mx.styles.CSSStyleDeclaration;
import mx.styles.StyleManager;

[ExcludeClass]

public class _TreeStyle
{
    [Embed(source='../Assets.swf', symbol='TreeFolderClosed')]
    private static var _embed_css_Assets_swf_TreeFolderClosed_1392035692:Class;
    [Embed(source='../Assets.swf', symbol='TreeNodeIcon')]
    private static var _embed_css_Assets_swf_TreeNodeIcon_1608160323:Class;
    [Embed(source='../Assets.swf', symbol='TreeFolderOpen')]
    private static var _embed_css_Assets_swf_TreeFolderOpen_1126279298:Class;
    [Embed(source='../Assets.swf', symbol='TreeDisclosureOpen')]
    private static var _embed_css_Assets_swf_TreeDisclosureOpen_2043381121:Class;
    [Embed(source='../Assets.swf', symbol='TreeDisclosureClosed')]
    private static var _embed_css_Assets_swf_TreeDisclosureClosed_1116295395:Class;

    public static function init(fbs:IFlexModuleFactory):void
    {
        var style:CSSStyleDeclaration = StyleManager.getStyleDeclaration("Tree");
    
        if (!style)
        {
            style = new CSSStyleDeclaration();
            StyleManager.setStyleDeclaration("Tree", style, false);
        }
    
        if (style.defaultFactory == null)
        {
            style.defaultFactory = function():void
            {
                this.paddingLeft = 2;
                this.folderClosedIcon = _embed_css_Assets_swf_TreeFolderClosed_1392035692;
                this.defaultLeafIcon = _embed_css_Assets_swf_TreeNodeIcon_1608160323;
                this.paddingRight = 0;
                this.verticalAlign = "middle";
                this.disclosureClosedIcon = _embed_css_Assets_swf_TreeDisclosureClosed_1116295395;
                this.folderOpenIcon = _embed_css_Assets_swf_TreeFolderOpen_1126279298;
                this.disclosureOpenIcon = _embed_css_Assets_swf_TreeDisclosureOpen_2043381121;
            };
        }
    }
}

}
