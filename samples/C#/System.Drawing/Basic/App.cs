using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using Graphics = System.Drawing.Graphics;
using Bitmap = System.Drawing.Bitmap;
using flash.display;
using flash.events;

[Root]
class App : Sprite
{
    Bitmap[] Images;
    int _imageCount;
    Action[] _drawMethods;
    int _drawIndex;

    static readonly flash.text.TextFormat TitleFormat = new flash.text.TextFormat
        {
            size = 20,
            bold = true,
        };

    public App()
    {
        stage.click += e => 
        {
            DrawNext();
        };

        _drawMethods = new Action[] { DrawShapes, DrawImages, TestTextureBrush, DrawText };

        LoadImages();
    }

    #region LoadImages
    void LoadImages()
    {
        System.EventHandler loaderHandler = (sender, e) =>
            {
                ++_imageCount;
                if (_imageCount == Images.Length)
                    DrawNext();
            };

        Images = new Bitmap[4];
        for (int i = 0; i < Images.Length; ++i)
        {
            var bmp = new Bitmap((i + 1) + ".jpg");
            bmp.Loaded += loaderHandler;
            Images[i] = bmp;
        }
    }

    bool ImagesLoaded { get { return _imageCount == Images.Length; } }
    #endregion

    #region DrawNext
    void DrawNext()
    {
        if (!ImagesLoaded) return;

        while (this.numChildren > 0)
            removeChildAt(0);

        var draw = _drawMethods[_drawIndex];

        _drawIndex++;
        if (_drawIndex >= _drawMethods.Length)
            _drawIndex = 0;

        graphics.clear();
        draw();
        stage.invalidate();
    }
    #endregion

    #region SetTitle
    void SetTitle(string title)
    {
        var tf = new flash.text.TextField();
        tf.x = 200;
        tf.y = 5;
        tf.width = 300;
        tf.defaultTextFormat = TitleFormat;
        tf.text = title;
        addChild(tf);
        stage.invalidate();
    }
    #endregion

    #region Shapes & Brushes
    void DrawShapes()
    {
        SetTitle("Shapes & Brushes");

        var g = new Graphics(this);
        g.FillEllipse(Brushes.Red, 50, 50, 100, 100);
        g.DrawEllipse(Pens.Black, 50, 50, 100, 100);

        g.TranslateTransform(150, 150);
        g.RotateTransform(45);
        g.FillRectangle(Brushes.Yellow, 0, 0, 100, 100);
        g.ResetTransform();

        var rect = new RectangleF(200, 100, 200, 200);
        var lg = new LinearGradientBrush(rect, Color.Red, Color.Blue, LinearGradientMode.Horizontal);
        g.FillEllipse(lg, rect);

        rect = new RectangleF(500, 100, 200, 200);
        lg = new LinearGradientBrush(rect, Color.Red, Color.Blue, LinearGradientMode.Horizontal);
        lg.InterpolationColors = ColorBlends.Rainbow;
        g.FillRectangle(lg, rect);
    }
    #endregion

    #region Images
    void DrawImages()
    {
        SetTitle("Image");

        var g = new Graphics(this);

        float x = 50;
        float y = 50;

        g.DrawImage(Images[0], x, y);
        g.DrawImage(Images[1], x, y + Images[0].Height + 5);

        g.DrawImage(Images[0], 160, 50, 200, 200);

        g.DrawImage(Images[3], 50, 260, new RectangleF(150, 50, 280, 280), GraphicsUnit.Pixel);

        g.DrawImage(Images[3], new RectangleF(370, 50, 100, 100), new RectangleF(150, 50, 250, 280), GraphicsUnit.Pixel);
    }
    #endregion

    #region TextureBrush
    void TestTextureBrush()
    {
        SetTitle("TextureBrush");

        var g = new Graphics(this);

        var rect = new RectangleF(50, 50, 500, 500);
        var tb = new TextureBrush(Images[1], rect);
        g.FillRectangle(tb, rect);
    }
    #endregion

    #region Text

    const string LoremIpsum =
        "Lorem ipsum dolor sit amet, consectetuer adipiscing elit,"
        + " sed diam nonummy nibh euismod tincidunt ut laoreet dolore magna aliquam erat volutpat."
        + " Ut wisi enim ad minim veniam, quis nostrud exerci tation ullamcorper suscipit lobortis"
        + " nisl ut aliquip ex ea commodo consequat. Duis autem vel eum iriure dolor in hendrerit"
        + " in vulputate velit esse molestie consequat, vel illum dolore eu feugiat nulla facilisis"
        + " at vero eros et accumsan et iusto odio dignissim qui blandit praesent luptatum zzril"
        + " delenit augue duis dolore te feugait nulla facilisi.";

    void DrawText()
    {
        SetTitle("Text");

        var g = new Graphics(this);

        var font = new Font("Arial", 50);

        g.DrawString("Hello, World!!!", font, Brushes.Red, 10, 30);

        var style = FontStyle.Bold;
        style |= FontStyle.Strikeout;
        style |= FontStyle.Underline;

        font = new Font("Verdana", 14, style);

        var sf = new StringFormat();
        sf.Alignment = StringAlignment.Center;

        var rect = new RectangleF(100, 100, 200, 200);
        g.DrawRectangle(Pens.Red, rect);

        g.DrawString(LoremIpsum, font, Brushes.Blue, rect, sf);
    }
    #endregion
}

static class ColorBlends
{
    public static float[] CreatePositions(int n)
    {
        float d = 1f / ((float)n - 1);
        float[] pos = new float[n];
        float v = 0;
        for (int i = 0; i < n; ++i)
        {
            pos[i] = v;
            v += d;
        }
        pos[n - 1] = 1;
        return pos;
    }

    public static ColorBlend CreatePalette(Color[] colors)
    {
        int n = colors.Length;
        ColorBlend res = new ColorBlend(n);
        res.Positions = CreatePositions(n);
        res.Colors = colors;
        return res;
    }

    public static ColorBlend Rainbow
    {
        get
        {
            if (_rainbowPalette == null)
                _rainbowPalette = CreatePalette(
                    new Color[]
                            {
                                Color.Purple,
                                Color.Blue,
                                Color.Cyan,
                                Color.Green,
                                Color.Yellow,
                                Color.Orange,
                                Color.Red
                            });
            return _rainbowPalette;
        }
    }
    static ColorBlend _rainbowPalette;
}