using flash.display;

[Root]
class App : Sprite
{
    public App()
    {
        Graphics g = graphics;
        g.beginFill(0xFF0000, 1.0);
        g.drawCircle(200, 200, 100);

        g.beginFill(0x00FF00, 1.0);
        g.drawCircle(100, 100, 50);
    }
}