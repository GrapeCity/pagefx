using mx.core;
using mx.controls;
using flash.events;

class MyApp : Application
{
    public MyApp()
    {
        Button btn = new Button();
    	btn.label = "Test";
    	btn.x = 10;
    	btn.y = 10;
    	btn.click += OnClick;
    	addChild(btn);
    }

    private void OnClick(MouseEvent e)
    {
    	Alert.show("Hello World!!!");

        dd.Test t = new dd.Test();
        t.Run();
    }
}