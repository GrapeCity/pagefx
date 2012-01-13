using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using Graphics = System.Drawing.Graphics;
using Bitmap = System.Drawing.Bitmap;
using flash.display;
using flash.events;
using flash.ui;
using mx.core;
using mx.controls;
using mx.containers;
using mx.collections;

[Root]
class App : Application
{
    public App()
    {
        Init();
    }

    void Init()
    {
        var vbox = new VBox();
        vbox.percentWidth = vbox.percentHeight = 100;
        vbox.setStyle("verticalGap", 2);
        vbox.setStyle("verticalAlign", "top");
        //vbox.setStyle("verticalCenter", 0);

        var test = new Test();

        var hbox = new HBox();
        hbox.percentWidth = 100;
        hbox.setStyle("horizontalGap", 2);

        AddLabel(hbox, "Width: ");
        AddInput(hbox, test.Width, num => 
        { 
            test.Width = (float)num;
            vbox.invalidateDisplayList();
        });
        AddLabel(hbox, "Height: ");
        AddInput(hbox, test.Height, num => 
        {
            test.Height = (float)num;
            vbox.invalidateDisplayList();
        });
        AddCheckBox(hbox, "Vertical", test.Vertical, f => { test.Vertical = f; });
        AddCheckBox(hbox, "RTL", test.RTL, f => { test.RTL = f; });
        AddComboBox(hbox, "Brush: ", 0, v => { test.Brush = v; }, "Solid", "LG", "Texture");
        vbox.addChild(hbox);

        hbox = new HBox();
        hbox.percentWidth = 100;
        hbox.setStyle("horizontalGap", 2);

        AddComboBox(hbox, "Align: ", 0, v => { test.Align = (StringAlignment)v; }, "near", "center", "far");
        AddComboBox(hbox, "Line Align: ", 0, v => { test.LineAlign = (StringAlignment)v; }, "near", "center", "far");

        AddComboBox(hbox, "Trimming: ", (int)test.Trimming, 
            v => { test.Trimming = (StringTrimming)v; },
            "None", "Char", "Word", "EllipsisChar", "EllipsisWord", "EllipsisPath");

        vbox.addChild(hbox);

        hbox = new HBox();
        hbox.percentWidth = 100;
        hbox.setStyle("horizontalGap", 2);
        AddLabel(hbox, "Font: ");
        AddCheckBox(hbox, "Bold", test.Bold, f => { test.Bold = f; });
        vbox.addChild(hbox);

        vbox.addChild(test);
        addChild(vbox);
    }

    static void AddInput(DisplayObjectContainer container, double defval, Action<double> action)
    {
        var input = new TextInput();
        input.text = defval.ToString();
        input.width = 50;
        input.keyDown += e =>
            {
                if (e.keyCode == Keyboard.ENTER)
                {
                    var v = double.Parse(input.text);
                    action(v);
                }
            };
        container.addChild(input);
    }

    static void AddCheckBox(DisplayObjectContainer container, string label, bool defval, Action<bool> action)
    {
        var cb = new CheckBox();
        cb.selected = defval;
        cb.label = label;
        cb.change += e =>
            {
                action(cb.selected);
            };
        container.addChild(cb);
    }

    static void AddComboBox(DisplayObjectContainer container, string label, int defval, Action<int> action, params string[] values)
    {
        AddLabel(container, label);

        var col = new ArrayCollection();
        for (int i = 0; i < values.Length; ++i)
        {
            var item = avm.NewObject("label", values[i], "data", i);
            col.addItem(item);
        }
        
        var cb = new ComboBox();
        cb.dataProvider = col;
        cb.selectedIndex = defval;
        cb.OnClose += e =>
            {
                action(cb.selectedIndex);
            };
        container.addChild(cb);
    }

    static void AddLabel(DisplayObjectContainer container, string label)
    {
        if (string.IsNullOrEmpty(label)) return;
        var c = new Label();
        c.text = label;
        container.addChild(c);
    }
}

class BaseComp : UIComponent
{
    public BaseComp()
    {
        Render();
    }

    bool _redraw = true;

    void Render()
    {
        if (!_redraw) return;
        _redraw = false;
        graphics.clear();
        while (numChildren > 0)
            removeChildAt(0);
        RenderCore();
    }

    protected virtual void RenderCore()
    {
    }

    public void Invalidate()
    {
        _redraw = true;
        Render();
    }
}

class Test : BaseComp
{
    public Test()
    {
        Invalidate();
    }

    float _w = 350, _h = 250;

    public float Width
    {
        get { return _w; }
        set
        {
            if (value != _w)
            {
                _w = value;
                width = value;
                Invalidate();
            }
        }
    }

    public float Height
    {
        get { return _h; }
        set
        {
            if (value != _h)
            {
                _h = value;
                height = value;
                Invalidate();
            }
        }
    }

    public bool Bold
    {
        get { return _bold; }
        set 
        {
            if (value != _bold)
            {
                _bold = value;
                Invalidate();
            }
        }
    }
    bool _bold = true;

    public bool Vertical
    {
        get { return _vert; }
        set
        {
            if (value != _vert)
            {
                _vert = value;
                Invalidate();
            }
        }
    }
    bool _vert;

    public bool RTL
    {
        get { return _rtl; }
        set 
        {
            if (value != _rtl)
            {
                _rtl = value;
                Invalidate();
            }
        }
    }
    bool _rtl;

    public float FontSize
    {
        get { return _fontSize; }
        set 
        {
            if (value != _fontSize)
            {
                _fontSize = value;
                Invalidate();
            }
        }
    }
    float _fontSize = 14;

    public StringAlignment Align
    {
        get { return _align; }
        set 
        {
            if (value != _align)
            {
                _align = value;
                Invalidate();
            }
        }
    }
    StringAlignment _align;

    public StringAlignment LineAlign
    {
        get { return _lalign; }
        set
        {
            if (value != _lalign)
            {
                _lalign = value;
                Invalidate();
            }
        }
    }
    StringAlignment _lalign;

    public StringTrimming Trimming
    {
        get { return _trim; }
        set 
        {
            if (value != _trim)
            {
                _trim = value;
                Invalidate();
            }
        }
    }
    StringTrimming _trim = StringTrimming.EllipsisCharacter;

    public int Brush
    {
        get { return _brush; }
        set 
        {
            if (value != _brush)
            {
                _brush = value;
                Invalidate();
            }
        }
    }
    int _brush;

    protected override void measure()
    {
        width = measuredWidth = _w;
        height = measuredHeight = _h;
    }

    const string LoremIpsum =
        "Lorem ipsum dolor sit amet, consectetuer adipiscing elit,"
        + " sed diam nonummy nibh euismod tincidunt ut laoreet dolore magna aliquam erat volutpat."
        + " Ut wisi enim ad minim veniam, quis nostrud exerci tation ullamcorper suscipit lobortis"
        + " nisl ut aliquip ex ea commodo consequat. Duis autem vel eum iriure dolor in hendrerit"
        + " in vulputate velit esse molestie consequat, vel illum dolore eu feugiat nulla facilisis"
        + " at vero eros et accumsan et iusto odio dignissim qui blandit praesent luptatum zzril"
        + " delenit augue duis dolore te feugait nulla facilisi.";

    Brush CreateBrush()
    {
        switch (_brush)
        {
            case 1:
                {

                    return new LinearGradientBrush(
                        GetTextRect(),
                        Color.Red,
                        Color.Blue,
                        LinearGradientMode.Horizontal);
                }

            default:
                return Brushes.Black;
        }
    }

    RectangleF GetTextRect()
    {
        return new RectangleF(5, 5, _w - 10, _h - 10);
    }

    protected override void RenderCore()
    {
        var g = new Graphics(this);

        var style = FontStyle.Regular;
        if (_bold) style |= FontStyle.Bold;
        //style |= FontStyle.Strikeout;
        //style |= FontStyle.Underline;

        var font = new Font("Arial", _fontSize, style);

        var sf = new StringFormat();
        if (_vert)
            sf.FormatFlags |= StringFormatFlags.DirectionVertical;
        if (_rtl)
            sf.FormatFlags |= StringFormatFlags.DirectionRightToLeft;
        sf.Alignment = _align;
        sf.LineAlignment = _lalign;
        sf.Trimming = _trim;

        var rect = GetTextRect();
        
        g.DrawString(LoremIpsum, font, CreateBrush(), rect, sf);

        g.DrawRectangle(Pens.Red, rect);
        g.DrawRectangle(Pens.Black, 0, 0, _w, _h);
    }
}