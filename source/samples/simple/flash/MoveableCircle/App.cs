using flash.display;
using flash.events;
using flash.text;
using System.Diagnostics;

[Root]
class App : Sprite
{
    private Rect border;
    private Ellipse circle;
    private TextField status;

    private double cx, cy;
    private double ccx, ccy;
    private bool startMove;

    public App()
    {
        Button btn = new Button();
        addChild(btn);

        btn.click += OnBreak;

        border = new Rect();
        border.visible = false;
        addChild(border);

        circle = new Ellipse(100, 100, 100, 100);
        addChild(circle);

        status = new TextField();
        status.x = 2;
        status.y = 550;
        status.width = 700;
        addChild(status);

        circle.doubleClickEnabled = true;
        circle.mouseOver += circle_MouseOver;
        circle.mouseOut += circle_MouseOut;
        circle.mouseMove += circle_MouseMove;
        circle.mouseDown += circle_MouseDown;
        circle.mouseUp += circle_MouseUp;
        circle.doubleClick += circle_DoubleClick;

        stage.mouseMove += stage_MouseMove;
        stage.mouseUp += stage_MouseUp;
        stage.invalidate();
    }

    private static void Foo(ref int iPtr)
    {
        int off = 2;
        iPtr = iPtr + off;
    }

    private void OnBreak(MouseEvent e)
    {
        int a = 10;
        int b = 20;
        int c = 30;
        int d = 40;
        int answer = 42;

        Foo(ref answer);

        circle.x = 300 + a;
        circle.y = 300 + b;

        a = add(a, b) - answer;
        b = sub(a, b) + answer;

        circle.x += a;
        circle.y += b;
    }

    private static int add(int a, int b)
    {
        return (int)(a + b);
    }

    private static int sub(int a, int b)
    {
        return a - b;
    }

    private void UpdateStatus(MouseEvent e)
    {
        string type = e.type;
        status.text = string.Format("status: {0}(lx = {1}, ly = {2}, gx = {3}, gy = {4}, cx = {5}, cy = {6})",
            type, e.localX, e.localY, e.stageX, e.stageY, circle.x, circle.y);
    }

    private void ShowBorder(bool show)
    {
        if (show)
        {
            border.x = circle.x;
            border.y = circle.y;
            border.Width = circle.Width;
            border.Height = circle.Height;
        }
        border.visible = show;
    }

    private void circle_MouseOver(MouseEvent e)
    {
        if (!startMove)
        {
            ShowBorder(true);
            UpdateStatus(e);
            stage.invalidate();
        }
    }

    private void circle_MouseOut(MouseEvent e)
    {
        if (!startMove)
        {
            ShowBorder(false);
            UpdateStatus(e);
            stage.invalidate();
        }
    }

    private void circle_MouseMove(MouseEvent e)
    {
        if (!startMove)
        {
            ShowBorder(true);
            UpdateStatus(e);
            stage.invalidate();
        }
    }

    private void circle_MouseDown(MouseEvent e)
    {
        if (!startMove)
        {
            cx = e.stageX;
            cy = e.stageY;
            ccx = circle.x;
            ccy = circle.y;
            UpdateStatus(e);
            startMove = true;
            stage.invalidate();
        }
    }

    private void circle_MouseUp(MouseEvent e)
    {
        if (!startMove)
        {
            ShowBorder(true);
            UpdateStatus(e);
            stage.invalidate();
        }
    }

    private bool red;

    private void circle_DoubleClick(MouseEvent e)
    {
        circle.Color = red ? 0xFF0000 : 0x00FF00;
        red = !red;
        startMove = false;
        UpdateStatus(e);
        stage.invalidate();
    }

    private void stage_MouseMove(MouseEvent e)
    {
        if (startMove)
        {
            double dx = e.stageX - cx;
            double dy = e.stageY - cy;
            circle.x = ccx + dx;
            circle.y = ccy + dy;
            ShowBorder(true);
        }

        UpdateStatus(e);
        stage.invalidate();
    }

    private void stage_MouseUp(MouseEvent e)
    {
        if (startMove)
        {
            UpdateStatus(e);
            startMove = false;
            stage.invalidate();
        }
    }
}

class Button : Sprite
{
    public Button()
    {
        render += OnRender;
    }

    private void OnRender(Event e)
    {
        Graphics g = graphics;
        g.clear();
        g.lineStyle(1);
        g.beginFill((uint)0x33FF33);
        g.drawRect(0, 0, 20, 20);
        g.endFill();
    }
}

class Ellipse : Sprite
{
    public Ellipse()
    {
        render += OnRender;
    }

    public Ellipse(int x, int y, int w, int h) : this()
    {
        this.x = x;
        this.y = y;
        _width = w;
        _height = h;
    }

    public int Color
    {
        get { return _color; }
        set { _color = value; }
    }
    private int _color = 0xFF0000;

    public int Width
    {
        get { return _width; }
        set { _width = value; }
    }
    private int _width;

    public int Height
    {
        get { return _height; }
        set { _height = value; }
    }
    private int _height;

    public void Render()
    {
        Graphics g = graphics;
        g.clear();
        g.beginFill((uint)_color);
        g.drawEllipse(0, 0, _width, _height);
        g.endFill();
    }

    private void OnRender(Event e)
    {
        Render();
    }
}

class Rect : Sprite
{
    public Rect()
    {
        render += OnRender;
    }

    public int Width
    {
        get { return _width; }
        set { _width = value; }
    }
    private int _width;

    public int Height
    {
        get { return _height; }
        set { _height = value; }
    }
    private int _height;

    public void Render()
    {
        Graphics g = graphics;
        g.clear();
        g.lineStyle(1);
        g.drawRect(0, 0, _width, _height);
    }

    private void OnRender(Event e)
    {
        Render();
    }
}