using flash.display;
using Avm;

[Root]
class App : Sprite
{
    public App()
    {
        Graphics g = graphics;
        g.beginFill(0xFF8000);
        Vector<double> v = new Vector<double>();
        v.push(10, 10, 100, 10, 10, 100, 100, 100);
        Vector<int> ind = new Vector<int>();
        ind.push(0, 1, 2, 1, 3, 2);
        g.drawTriangles(v, ind);
    }
}