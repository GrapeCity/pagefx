using mx.core;
using mx.controls;

class MyApp : Application
{
    ComboBox ctrl;

    public MyApp()
    {
    	ctrl = new ComboBox();
    	ctrl.x = 100;
    	ctrl.y = 100;
    	addChild(ctrl);
    }
}