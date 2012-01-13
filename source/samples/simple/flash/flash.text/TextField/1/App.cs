using flash.text;
using flash.text.engine;
using flash.display;
using flash.events;
using flash.system;
using Avm;

[Root]
class App : Sprite
{
    const string p1 = "\u5185\u95A3\u5E9C\u304C\u300C\u653F\u5E9C\u30A4"
            + "\u30F3\u30BF\u30FC\u30CD\u30C3\u30C8\u30C6\u30EC"
            + "\u30D3\u300D\u306E\u52D5\u753B\u914D\u4FE1\u5411"
            + "\u3051\u306B\u30A2\u30C9\u30D3\u30B7\u30B9\u30C6"
            + "\u30E0\u30BA\u793E\u306E"
            + "FMS 2"
            + "\u3092\u63A1\u7528\u3059\u308B\u3068"
            + "\u767a\u8868\u3057\u307e\u3057\u305F\u3002";

    const string p2 = "\u30AF\u30ED\u30B9\u30D7\u30E9\u30C3\u30C8\u30D5"
        + "\u30A9\u30FC\u30E0\u4E0A\u3067\u518D\u751F\u53EF"
        + "\u80FD\u306A"
        + "Flash Video"
        + "\u3092\u914D\u4FE1\u3001\u653F\u5E9C\u6700\u65B0"
        + "\u60C5\u5831\u3092\u3088\u308A\u591A\u304F\u306E"
        + "\u56FD\u6C11\u306B\u9AD8\u54C1\u8CEA\u306A\u753B"
        + "\u50CF\u3067\u7C21\u5358\u304B\u3064\u30EA\u30A2"
        + "\u30EB\u30BF\u30A4\u30E0\u306B\u63D0\u4F9B\u3059"
        + "\u308B\u3053\u3068\u304C\u53EF\u80FD\u306B\u306A"
        + "\u308A\u307e\u3057\u305F\u3002";

    const string p3 = "\u3010" + "2007" + "\u5E74" + "2" + "\u6708" + "21" + "\u65E5\u3011";

    const string p = p1 + p2 + p3;

    private static string GetText()
    {
        string s = "";
        for (int i = 0; i < 5; ++i)
            s += p;
        return s;
    }

    TextField tf;

    public App()
    {
        tf = new TextField();
        tf.x = 5;
        tf.y = 5;
        tf.width = stage.stageWidth - 10;
        tf.height = stage.stageHeight - 10;
        tf.defaultTextFormat = CreateFormat();
        tf.type = TextFieldType.INPUT;
        tf.background = true;
        tf.border = true;
        tf.multiline = true;
        tf.wordWrap = true;
        tf.gridFitType = GridFitType.SUBPIXEL;
        tf.text = GetText();
        addChild(tf);

        stage.resize += OnResize;
    }

    private static bool IsMacOS()
    {
        return Capabilities.os.search("Mac OS") > -1;
    }

    private TextFormat CreateFormat()
    {
        string fontName;

        if (IsMacOS())
            fontName = "\u5C0F\u585A\u660E\u671D" + " Pro R"; // "Kozuka Mincho Pro R"                    
            //fontName = "Adobe " + "\uBA85\uC870" + " Std M"; // "Adobe Myungjo Std M"
        else
            fontName = "Kozuka Mincho Pro R";

        TextFormat format = new TextFormat();
        format.font = fontName;
        format.size = 20;
        
        return format;
    }

    private void OnResize(Event e)
    {
        tf.width = stage.stageWidth - 10;
        tf.height = stage.stageHeight - 10;
    }
}
