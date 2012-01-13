using flash.display;
using flash.filters;

[Root]
class DropShadowExample : Sprite
{
    public DropShadowExample()
    {
        graphics.beginFill(0xFFCC00);
        graphics.drawRect(50, 50, 100, 100);
        graphics.endFill();
        //add drop shadow
        filters = new Avm.Array(CreateShadow());
    }

    private BitmapFilter CreateShadow()
    {
        uint color = 0x000000;
        double angle = 45;
        double alpha = 0.8;
        double blurX = 8;
        double blurY = 8;
        double distance = 15;
        double strength = 0.65;
        bool inner = false;
        bool knockout = false;
        int quality = BitmapFilterQuality.HIGH;
        return new DropShadowFilter(distance, angle, color, alpha, blurX, blurY,
            strength, quality, inner, knockout);
    }
}
