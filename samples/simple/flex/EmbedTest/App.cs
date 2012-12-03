using flash.display;
using mx.core;

[Root]
class EmbedTest : Sprite
{
    [Embed("bg.png")]
    private static Avm.Class BgClass = null;

    [Embed("bg.png")]
    private static Avm.Class BgClass2 = null;

    public EmbedTest()
    {
        BitmapAsset img = (BitmapAsset)BgClass.CreateInstance();
        Graphics g = graphics;
        g.beginBitmapFill(img.bitmapData);
        g.drawCircle(100, 100, 100);
        g.endFill();

        img = (BitmapAsset)BgClass2.CreateInstance();
        g.beginBitmapFill(img.bitmapData);
        g.drawRect(220, 10, 100, 100);
        g.endFill();
    }
}