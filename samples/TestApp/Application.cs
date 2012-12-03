using System;
using System.Collections;
using System.Collections.Generic;
using flash.display;
using flash.events;
using flash.text;
using System.Diagnostics;

[DebuggerDisplay("[Name = {Name}, Age = {Age}]")]
class Person
{
    public string Name { get; set; }
    public int Age { get; set; }

    public override string ToString()
    {
        return string.Format("{{Person: Name = {0}, Age = {1}}}", Name, Age);
    }
}

class Point
{
    public int X, Y;

    public override string ToString()
    {
        return string.Format("{{X = {0}, Y = {1}}}", X, Y);
    }
}

[Root]
class App : Sprite
{
    Rect border;
    Ellipse circle;
    TextField status;

    double cx, cy;
    double ccx, ccy;
    bool startMove;

    protected string error;

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

    static void Foo(ref int iPtr)
    {
        int off = 2;
        iPtr = iPtr + off;
    }

    static void Fail()
    {
        throw new Exception("FAIL!");
    }

    static void CatchFail()
    {
        try
        {
            Fail();
        }
        catch (Exception exc)
        {
            Debug.WriteLine(exc.Message);
        }
    }

    static void Print(IEnumerable set)
    {
        foreach (var o in set)
            Console.WriteLine(o);
    }

    int bn;

    void OnBreak(MouseEvent e)
    {
        ++bn;
        Debug.WriteLine("OnBreak");

        int a = 10;
        int b = 20;
        int c = 30;
        int d = 40;
        int answer = 42;
        string str = "aaa";
        Console.WriteLine(str);

        var arr = new[] {10, 20, 30};
        int sum = Utils.Sum(arr);
        Console.WriteLine("Sum: {0}", sum);

        var pt = new Point { X = 10, Y = 10 };
        Console.WriteLine(pt);

        var p1 = new Person { Name = "Petr Cech", Age = 28 };

        var list1 = new List<int> {10, 20, 30};
        var list2 = new List<string> {"apple", "lemon", "pear"};
        var dic1 = new Dictionary<string, string> {{"A", "apple"}, {"B", "banana"}};
        var dic2 = new Dictionary<int, string> {{10, "apple"}, {20, "banana"}};
        var dic3 = new Dictionary<string, Person> 
        { 
            { "Vasya", new Person{Name = "Vasya", Age = 20}}, 
            { "Petr", p1 } 
        };
        Print(list1);
        Print(list2);
        Print(dic1);
        Print(dic2);
        Print(dic3);

        string mls = "aaa\nbbb\nccc\uABBA\u8812";
        Console.WriteLine(mls);

        error = "kaharman";

        //Debugger.Break();

        Utils.SayHello();

        //CatchFail();
        //Fail();

        Util.Foo();

        int i = SharedLib.Algorithms.IndexOf(arr, v => v == 20);
        Console.WriteLine(i);

        Foo(ref answer);

        circle.x = 300 + a;
        circle.y = 300 + b;

        a = add(a, b) - answer;
        b = sub(a, b) + answer;

        circle.x += a;
        circle.y += b;
    }

    static int add(int a, int b)
    {
        return (int)(a + b);
    }

    static int sub(int a, int b)
    {
        return a - b;
    }

    void UpdateStatus(MouseEvent e)
    {
        string type = e.type;
        status.text = string.Format("status: {0}(lx = {1}, ly = {2}, gx = {3}, gy = {4}, cx = {5}, cy = {6})",
            type, e.localX, e.localY, e.stageX, e.stageY, circle.x, circle.y);
    }

    void ShowBorder(bool show)
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

    void circle_MouseOver(MouseEvent e)
    {
        if (!startMove)
        {
            ShowBorder(true);
            UpdateStatus(e);
            stage.invalidate();
        }
    }

    void circle_MouseOut(MouseEvent e)
    {
        if (!startMove)
        {
            ShowBorder(false);
            UpdateStatus(e);
            stage.invalidate();
        }
    }

    void circle_MouseMove(MouseEvent e)
    {
        if (!startMove)
        {
            ShowBorder(true);
            UpdateStatus(e);
            stage.invalidate();
        }
    }

    void circle_MouseDown(MouseEvent e)
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

    void circle_MouseUp(MouseEvent e)
    {
        if (!startMove)
        {
            ShowBorder(true);
            UpdateStatus(e);
            stage.invalidate();
        }
    }

    bool red;

    void circle_DoubleClick(MouseEvent e)
    {
        circle.Color = red ? 0xFF0000 : 0x00FF00;
        red = !red;
        startMove = false;
        UpdateStatus(e);
        stage.invalidate();
    }

    void stage_MouseMove(MouseEvent e)
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

    void stage_MouseUp(MouseEvent e)
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

    void OnRender(Event e)
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

    public Ellipse(int x, int y, int w, int h)
        : this()
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
    int _color = 0xFF0000;

    public int Width
    {
        get { return _width; }
        set { _width = value; }
    }
    int _width;

    public int Height
    {
        get { return _height; }
        set { _height = value; }
    }
    int _height;

    public void Render()
    {
        Graphics g = graphics;
        g.clear();
        g.beginFill((uint)_color);
        g.drawEllipse(0, 0, _width, _height);
        g.endFill();
    }

    void OnRender(Event e)
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
    int _width;

    public int Height
    {
        get { return _height; }
        set { _height = value; }
    }
    int _height;

    public void Render()
    {
        Graphics g = graphics;
        g.clear();
        g.lineStyle(1);
        g.drawRect(0, 0, _width, _height);
    }

    void OnRender(Event e)
    {
        Render();
    }
}