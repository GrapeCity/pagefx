
package 
{

import flash.display.Sprite;
import mx.core.IFlexModuleFactory;
import mx.core.mx_internal;
import mx.styles.CSSStyleDeclaration;
import mx.styles.StyleManager;

[ExcludeClass]

public class _SwatchPanelStyle
{

    public static function init(fbs:IFlexModuleFactory):void
    {
        var style:CSSStyleDeclaration = StyleManager.getStyleDeclaration("SwatchPanel");
    
        if (!style)
        {
            style = new CSSStyleDeclaration();
            StyleManager.setStyleDeclaration("SwatchPanel", style, false);
        }
    
        if (style.defaultFactory == null)
        {
            style.defaultFactory = function():void
            {
                this.columnCount = 20;
                this.borderColor = 0xa5a9ae;
                this.swatchHighlightColor = 0xffffff;
                this.paddingLeft = 5;
                this.verticalGap = 0;
                this.fontSize = 11;
                this.shadowColor = 0x4d555e;
                this.swatchBorderSize = 1;
                this.swatchHighlightSize = 1;
                this.paddingBottom = 5;
                this.textFieldStyleName = "swatchPanelTextField";
                this.swatchHeight = 12;
                this.paddingTop = 4;
                this.highlightColor = 0xffffff;
                this.textFieldWidth = 72;
                this.paddingRight = 5;
                this.swatchGridBackgroundColor = 0x000000;
                this.swatchGridBorderSize = 0;
                this.horizontalGap = 0;
                this.swatchWidth = 12;
                this.backgroundColor = 0xe5e6e7;
                this.previewHeight = 22;
                this.swatchBorderColor = 0x000000;
                this.previewWidth = 45;
            };
        }
    }
}

}
