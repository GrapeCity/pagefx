using flash.display;
using flash.text;
using flash.events;
using System;

[Root]
class App : Sprite
{
    Sprite circle;
    bool minus;

    public App()
    {
    	circle = new Sprite();
    	circle.graphics.beginFill(0x990000);
    	circle.graphics.drawCircle(50, 50, 50);
    	circle.graphics.endFill();
    	minus = true;

        addChild(circle);

        circle.enterFrame += FadeCircle;
    }

    private void FadeCircle(Event e)
    {
    	if (minus) circle.alpha -= 0.05;
    	else circle.alpha += 0.05;

    	if (circle.alpha <= 0)
    	{
    		minus = false;
        }
        else if (circle.alpha >= 1)
        {
        	minus = true;
        }
    }
}