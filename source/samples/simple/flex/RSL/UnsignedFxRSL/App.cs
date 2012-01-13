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
    	btn.click += SayHello;
    	addChild(btn);
    }

    private void SayHello(MouseEvent e)
    {
    	Alert.show("Hello World!!!");
    }
}