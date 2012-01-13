using flash.display;
using mx.core;
using mx.controls;
using mx.containers;

class MyApp : Application
{
    public MyApp()
    {
        VDividedBox box = new VDividedBox();
        box.setStyle("left", 10);
        box.setStyle("right", 10);
        box.setStyle("top", 30);
        box.setStyle("bottom", 10);
        addChild(box);

        RichTextEditor ed = new RichTextEditor();
        ed.percentWidth = 100;
        ed.percentHeight = 100;
    	box.addChild(ed);

        ed = new RichTextEditor();
        ed.percentWidth = 100;
        ed.percentHeight = 100;
        box.addChild(ed);

        Button btn = ed.boldButton;
        Avm.Class klass = btn.getStyle("icon") as Avm.Class;
        if (klass != null)
        {
            BitmapAsset bmp = avm.CreateInstance(klass) as BitmapAsset;
            if (bmp != null)
            {
                if (bmp.bitmapData == null)
                {
                    Alert.show("Bad bitmapData");
                    return;
                }

                Sprite sprite = new Sprite();
                Graphics g = sprite.graphics;
                g.beginBitmapFill(bmp.bitmapData);
                g.drawRect(0, 0, 16, 16);
                g.endFill();
                addChild(sprite);
            }
            else
            {
                Alert.show("Icon class is not BitmapAsset");
            }
        }
    }
}