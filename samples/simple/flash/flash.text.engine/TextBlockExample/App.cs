using flash.text;
using flash.text.engine;
using flash.display;
using flash.events;
using flash.system;
using Avm;

[Root]
class TextBlockExample : Sprite
{
    bool vertical;
    Sprite container;
    Vector<TextBlock> textBlocks;
    TextField status;

    public TextBlockExample()
    {
        TextField tf = new TextField();
        tf.htmlText = "<b>CLICK</b>";
        tf.textColor = 0xFF0000;
        tf.x = 10;
        tf.y = 10;
        tf.selectable = false;
        tf.click += clickHandler;
        addChild(tf);

        status = new TextField();
        status.text = "layout: horizontal";
        status.x = 10;
        status.y = 25;
        addChild(status);

        createContent();
        createLines();
    }

    private TextBlock createEmptyBlock()
    {
        TextBlock textBlock = new TextBlock();
        textBlock.baselineZero = TextBaseline.IDEOGRAPHIC_CENTER;
        textBlock.textJustifier = new EastAsianJustifier("ja", LineJustification.ALL_BUT_LAST);
        textBlock.lineRotation = vertical ? TextRotation.ROTATE_90 : TextRotation.ROTATE_0;
        return textBlock;
    }

    private TextBlock paragraph1(ElementFormat format)
    {
        string p = "\u5185\u95A3\u5E9C\u304C\u300C\u653F\u5E9C\u30A4"
            + "\u30F3\u30BF\u30FC\u30CD\u30C3\u30C8\u30C6\u30EC"
            + "\u30D3\u300D\u306E\u52D5\u753B\u914D\u4FE1\u5411"
            + "\u3051\u306B\u30A2\u30C9\u30D3\u30B7\u30B9\u30C6"
            + "\u30E0\u30BA\u793E\u306E"
            + "FMS 2"
            + "\u3092\u63A1\u7528\u3059\u308B\u3068"
            + "\u767a\u8868\u3057\u307e\u3057\u305F\u3002";

        TextBlock textBlock = createEmptyBlock();
        textBlock.content = new TextElement(p, format);
        return textBlock;
    }

    private TextBlock paragraph2(ElementFormat format)
    {
        string p = "\u30AF\u30ED\u30B9\u30D7\u30E9\u30C3\u30C8\u30D5"
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

        TextBlock textBlock = createEmptyBlock();
        textBlock.content = new TextElement(p, format);
        return textBlock;
    }

    private TextBlock paragraph3(ElementFormat format)
    {
        string p = "\u3010" + "2007"
            + "\u5E74" + "2" + "\u6708" + "21"
            + "\u65E5\u3011";
        TextBlock textBlock = createEmptyBlock();
        textBlock.content = new TextElement(p, format);
        return textBlock;
    }

    private void createContent()
    {
        FontDescription font = new FontDescription();

        if (Capabilities.os.search("Mac OS") > -1)
            font.fontName = "\u5C0F\u585A\u660E\u671D" + " Pro R"; // "Kozuka Mincho Pro R"                    
        //koFont.fontName = "Adobe " + "\uBA85\uC870" + " Std M"; // "Adobe Myungjo Std M"
        else
            font.fontName = "Kozuka Mincho Pro R";

        ElementFormat format = new ElementFormat();
        format.fontDescription = font;
        format.fontSize = 12;
        format.locale = "ja";
        format.color = 0x000000;
        if (!vertical)
            format.textRotation = TextRotation.ROTATE_0;

        textBlocks = new Vector<TextBlock>();
        textBlocks.push(
            paragraph1(format),
            paragraph2(format),
            paragraph3(format)
        );
    }

    private void createLines()
    {
        if (container != null)
            removeChild(container);

        container = new Sprite();
        container.y = 45;
        container.x = 40;
        addChild(container);

        double linePosition = vertical ? this.stage.stageWidth - 120 : 12;

        for (int i = 0; i < (int)textBlocks.length; i++)
        {
            TextBlock textBlock = textBlocks[i];
            TextLine previousLine = null;

            while (true)
            {
                TextLine textLine = textBlock.createTextLine(previousLine, 300);
                if (textLine == null)
                    break;
                if (vertical)
                {
                    textLine.x = linePosition;
                    linePosition -= 24;
                }
                else
                {
                    textLine.y = linePosition + 50;
                    linePosition += 24;
                }
                container.addChild(textLine);
                previousLine = textLine;
            }
            if (vertical)
                linePosition -= 16;
            else
                linePosition += 16;
        }
    }

    private void clickHandler(MouseEvent e)
    {
        vertical = !vertical;
        if (vertical)
            status.text = "layout: vecrtical";
        else
            status.text = "layout: horitontal";
        createContent();
        createLines();
    }
}
