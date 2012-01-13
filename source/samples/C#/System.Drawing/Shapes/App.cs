using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using Graphics = System.Drawing.Graphics;
using Bitmap = System.Drawing.Bitmap;
using flash.display;
using flash.text;
using flash.events;

[Root]
class App : Sprite
{
    Bitmap B1;
    Bitmap B2;
    int _imageCount;
    int _shapeIndex;

    static readonly TextFormat TitleFormat = new TextFormat
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

        LoadImages();
    }

    #region LoadImages
    void LoadImages()
    {
        System.EventHandler loaderHandler = (sender, e) =>
            {
                ++_imageCount;
                if (_imageCount == 2)
                    DrawNext();
            };

        B1 = new Bitmap("1.jpg");
        B2 = new Bitmap("2.jpg");

        B1.Loaded += loaderHandler;
        B2.Loaded += loaderHandler;
    }

    bool ImagesLoaded { get { return _imageCount == 2; } }
    #endregion

    void DrawNext()
    {
        if (!ImagesLoaded) return;

        while (this.numChildren > 0)
            removeChildAt(0);

        graphics.clear();

        int index = _shapeIndex;

        SetTitle(Shapes.Names[index]);

        _shapeIndex++;
        if (_shapeIndex >= Shapes.ShapeCount)
            _shapeIndex = 0;

        var shape = Shapes.GetShape(index);

        if (shape != null)
        {
            var g = new Graphics(this);
            g.FillPath(Brushes.Red, shape);
            g.DrawPath(Pens.Black, shape);
        }

        stage.invalidate();
    }

    void SetTitle(string title)
    {
        var tf = new TextField();
        tf.x = 200;
        tf.y = 500;
        tf.width = 300;
        tf.defaultTextFormat = TitleFormat;
        tf.text = title;
        addChild(tf);
        stage.invalidate();
    }
}