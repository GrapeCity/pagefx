
package 
{

import flash.display.Sprite;
import mx.core.IFlexModuleFactory;
import mx.core.mx_internal;
import mx.styles.CSSStyleDeclaration;
import mx.styles.StyleManager;
import mx.skins.halo.DateChooserIndicator;
import mx.skins.halo.DateChooserYearArrowSkin;
import mx.skins.halo.DateChooserMonthArrowSkin;

[ExcludeClass]

public class _DateChooserStyle
{

    public static function init(fbs:IFlexModuleFactory):void
    {
        var style:CSSStyleDeclaration = StyleManager.getStyleDeclaration("DateChooser");
    
        if (!style)
        {
            style = new CSSStyleDeclaration();
            StyleManager.setStyleDeclaration("DateChooser", style, false);
        }
    
        if (style.defaultFactory == null)
        {
            style.defaultFactory = function():void
            {
                this.todayStyleName = "todayStyle";
                this.todayColor = 0x818181;
                this.weekDayStyleName = "weekDayStyle";
                this.prevYearSkin = mx.skins.halo.DateChooserYearArrowSkin;
                this.todayIndicatorSkin = mx.skins.halo.DateChooserIndicator;
                this.headerStyleName = "headerDateText";
                this.nextMonthSkin = mx.skins.halo.DateChooserMonthArrowSkin;
                this.cornerRadius = 4;
                this.prevMonthSkin = mx.skins.halo.DateChooserMonthArrowSkin;
                this.headerColors = [0xe1e5eb, 0xf4f5f7];
                this.selectionIndicatorSkin = mx.skins.halo.DateChooserIndicator;
                this.nextYearSkin = mx.skins.halo.DateChooserYearArrowSkin;
                this.rollOverIndicatorSkin = mx.skins.halo.DateChooserIndicator;
                this.backgroundColor = 0xffffff;
            };
        }
    }
}

}
