using flash.display;
using flash.geom;
using Avm;

[Root]
class App : Sprite
{
    public App()
    {
        Graphics g = graphics;

        // stroke object
        GraphicsStroke stroke = new GraphicsStroke(3);
        stroke.joints = JointStyle.MITER;
        stroke.fill = new GraphicsSolidFill(0x102020);

        // fill object
        GraphicsGradientFill fill = new GraphicsGradientFill();
        fill.colors = new Avm.Array(0x0000FF, 0xEEFFEE);
        fill.matrix = new Matrix();
        fill.matrix.createGradientBox(70, 70, Avm.Math.PI / 2);

        Vector<int> commands = new Vector<int>();
        commands.push(1, 2, 2);

        Vector<double> data = new Vector<double>();
        data.push(125, 0, 50, 100, 175, 0);

        GraphicsPath path = new GraphicsPath(commands, data);

        // combine objects for complete drawing
        Vector<IGraphicsData> drawing = new Vector<IGraphicsData>();
        drawing.push(stroke, fill, path);

        // draw the drawing multiple times
        // change one value to modify each variation
        g.drawGraphicsData(drawing);
        data[2] += 200;
        g.drawGraphicsData(drawing);
        data[2] -= 150;
        g.drawGraphicsData(drawing);
        data[2] += 100;
        g.drawGraphicsData(drawing);
        data[2] -= 50;
        g.drawGraphicsData(drawing);
    }
}