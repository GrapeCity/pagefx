using System;
using flash.display;
using flash.text;

[Root]
class App : Sprite
{
    public App()
    {
        Console.WriteLine("Hello Flash Trace!!!");
        Console.WriteLine("Trace Message 1");
        Console.WriteLine("Trace Message 2");
        Console.WriteLine("Trace Message 3");
        TextField tf = new TextField();
        tf.x = 100;
        tf.y = 100;
        tf.text = "Hello World!!!";
        addChild(tf);
    }
}