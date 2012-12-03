using mx.core;
using mx.controls;

class MyApp : Application
{
    CheckBox ctrl;

    public MyApp()
    {
    	ctrl = new CheckBox();
    	ctrl.label = "Test";
    	ctrl.x = 100;
    	ctrl.y = 100;
    	addChild(ctrl);
    }
}