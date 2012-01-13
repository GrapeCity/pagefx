using System;
using mx.core;
using mx.controls;

class App : Application
{
    Label label;

    public App()
    {
    	label = new Label();
    	label.text = "Test";
    	label.x = 100;
    	label.y = 100; 
    	addChild(label);

    	label.setStyle("fontWeight", "bold");
    	//label.setStyle("right", 10);
    	label.setConstraintValue("right", 10);
    }
}