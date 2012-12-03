using mx.core;
using mx.controls;
using flash.events;

class MyApp : Application
{
    Button btnTest;
    Button btnAdd;
    Button btnRemove;

    public MyApp()
    {
    	btnTest= new Button();
    	btnTest.label = "Test";
    	btnTest.x = 10;
    	btnTest.y = 10;
    	btnTest.click += btnTest_click;
    	addChild(btnTest);

    	btnAdd= new Button();
    	btnAdd.label = "Add";
    	btnAdd.x = 10;
    	btnAdd.y = 35;
    	btnAdd.click += btnAdd_click;
    	addChild(btnAdd);

    	btnRemove= new Button();
    	btnRemove.label = "Remove";
    	btnRemove.x = 10;
    	btnRemove.y = 60;
    	btnRemove.click += btnRemove_click;
    	addChild(btnRemove);
    }

    private void btnTest_click(MouseEvent e)
    {
    	Alert.show("Hello World!!!");
    }

    private void btnAdd_click(MouseEvent e)
    {
    	btnTest.click += btnTest_click;

    	MouseEventHandler h = delegate(MouseEvent e2)
    	{
    	  Alert.show("a1");
    	};
    	h += delegate(MouseEvent e3)
    	{
    	  Alert.show("a2");
    	};
    	btnTest.click += h;
    }

    private void btnRemove_click(MouseEvent e)
    {
    	btnTest.click -= btnTest_click;
    }
}