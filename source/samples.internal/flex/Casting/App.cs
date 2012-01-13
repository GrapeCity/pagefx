using System;
using mx.core;
using mx.controls;
using Avm;

delegate void Func();

class MyApp : Application
{
    public MyApp()
    {
    	Function f = (Func)delegate
    	{
    		Alert.show("Hello World!!!"); 
    	};
    	f.call(null);

    	Type type = typeof(Application);
    	Avm.Class klass = type;
    }
}