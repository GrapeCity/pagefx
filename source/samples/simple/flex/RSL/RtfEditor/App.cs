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
        box.setStyle("top", 10);
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
    }
}